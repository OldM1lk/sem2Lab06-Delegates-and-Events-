using System;
using System.Text;

namespace DelegatesAndEvents
{
    public class SquareMatrix : ICloneable, IComparable<SquareMatrix>
    {
        public int[,] matrix;
        public int size;

        public SquareMatrix(int size)
        {
            if (size <= 1)
            {
                throw new InvalidMatrixSizeException("Матрица не может быть такого размера!");
            }

            this.size = size;
            matrix = new int[size, size];
            Random random = new Random((int)DateTime.Now.Ticks);

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    matrix[rowIndex, columnIndex] = random.Next(-10, 10);
                }
            }
        }

        public static SquareMatrix operator +(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.size != matrix2.size)
            {
                throw new DiffrentMatrixSizeException("Матрицы должны быть одного размера!");
            }

            SquareMatrix result = (SquareMatrix)matrix1.Clone();

            for (int rowIndex = 0; rowIndex < matrix1.size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrix1.size; ++columnIndex)
                {
                    result.matrix[rowIndex, columnIndex] += matrix2.matrix[rowIndex, columnIndex];
                }
            }

            return result;
        }

        public static SquareMatrix operator *(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.size != matrix2.size)
            {
                throw new DiffrentMatrixSizeException("Матрицы должны быть одного размера!");
            }

            SquareMatrix result = (SquareMatrix)matrix1.Clone();

            for (int rowIndex = 0; rowIndex < matrix1.size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrix1.size; ++columnIndex)
                {
                    for (int positionIndex = 0; positionIndex < matrix1.size; ++positionIndex)
                    {
                        result.matrix[rowIndex, columnIndex] += matrix1.matrix[rowIndex, positionIndex] *
                                                                matrix2.matrix[positionIndex, columnIndex];
                    }
                }
            }

            return result;
        }

        public static bool operator >(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1 is null || matrix2 is null)
            {
                throw new CustomArgumentNullException("Обе матрицы должны быть ненулевыми!");
            }

            return matrix1.CompareTo(matrix2) > 0;
        }

        public static bool operator <(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.size != matrix2.size)
            {
                throw new DiffrentMatrixSizeException("Матрицы должны быть одного размера!");
            }

            return matrix1.CompareTo(matrix2) < 0;
        }

        public static bool operator >=(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.size != matrix2.size)
            {
                throw new DiffrentMatrixSizeException("Матрицы должны быть одного размера!");
            }

            return matrix1.CompareTo(matrix2) >= 0;
        }

        public static bool operator <=(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.size != matrix2.size)
            {
                throw new DiffrentMatrixSizeException("Матрицы должны быть одного размера!");
            }

            return matrix1.CompareTo(matrix2) <= 0;
        }

        public static bool operator ==(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.size != matrix2.size)
            {
                throw new DiffrentMatrixSizeException("Матрицы должны быть одного размера!");
            }

            return matrix1.Equals(matrix2);
        }

        public static bool operator !=(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.size != matrix2.size)
            {
                throw new DiffrentMatrixSizeException("Матрицы должны быть одного размера!");
            }

            return !matrix1.Equals(matrix2);
        }

        public static explicit operator int[,](SquareMatrix matrix)
        {
            int[,] result = new int[matrix.size, matrix.size];

            for (int rowIndex = 0; rowIndex < matrix.size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrix.size; ++columnIndex)
                {
                    result[rowIndex, columnIndex] = matrix.matrix[rowIndex, columnIndex];
                }
            }

            return result;
        }

        public static bool operator true(SquareMatrix matrix)
        {
            return !matrix.IsMatrixNull();
        }

        public static bool operator false(SquareMatrix matrix)
        {
            return matrix.IsMatrixNull();
        }

        public bool IsMatrixNull()
        {
            int result = 0;

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    result += matrix[rowIndex, columnIndex];
                }
            }
            if (result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Determinant()
        {
            if (size == 2)
            {
                return (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);
            }
            int result = 0;

            for (int columnIndex = 0; columnIndex < size; ++columnIndex)
            {
                result += (columnIndex % 2 == 1 ? 1 : -1) * matrix[1, columnIndex] *
                           GetMatrixMinor(1, columnIndex).Determinant();
            }

            return result;
        }

        private SquareMatrix GetMatrixMinor(int row, int column)
        {
            SquareMatrix result = new SquareMatrix(size - 1);

            for (int rowIndex = 0; rowIndex < size - 1; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size - 1; ++columnIndex)
                {
                    result.matrix[rowIndex, columnIndex] = columnIndex < column ?
                      rowIndex < row ?
                      matrix[rowIndex, columnIndex] :
                      matrix[rowIndex + 1, columnIndex] :
                      rowIndex < row ?
                      matrix[rowIndex, columnIndex + 1] :
                      matrix[rowIndex + 1, columnIndex + 1];
                }
            }

            return result;
        }

        public SquareMatrix Inverse()
        {
            SquareMatrix result = new SquareMatrix(size);

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    result.matrix[rowIndex, columnIndex] = (columnIndex + rowIndex) % 2 == 0 ? 1 : -1 *
                                                           GetMatrixMinor(rowIndex, columnIndex).Determinant();
                }
            }

            return result.Transposed();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    stringBuilder.Append(" " + matrix[rowIndex, columnIndex].ToString("0\t"));
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public int CompareTo(SquareMatrix other)
        {
            if (other is SquareMatrix)
            {
                int determinantThis = this.Determinant();
                int determinantOther = other.Determinant();

                if (determinantThis < determinantOther)
                {
                    return -1;
                }
                else if (determinantThis > determinantOther)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }

        public override bool Equals(object activeObject)
        {
            if (activeObject is SquareMatrix other)
            {
                if (this.size != other.size)
                {
                    return false;
                }
                for (int rowIndex = 0; rowIndex < size; ++rowIndex)
                {
                    for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                    {
                        if (this.matrix[rowIndex, columnIndex] != other.matrix[rowIndex, columnIndex])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                int primeNumber = 23;
                hash = hash * primeNumber + size.GetHashCode();

                for (int rowIndex = 0; rowIndex < size; ++rowIndex)
                {
                    for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                    {
                        hash = hash * primeNumber + matrix[rowIndex, columnIndex].GetHashCode();
                    }
                }

                return hash;
            }
        }

        public object Clone()
        {
            SquareMatrix clone = new SquareMatrix(size);
            clone = (SquareMatrix)this.MemberwiseClone();

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    clone.matrix[rowIndex, columnIndex] = this.matrix[rowIndex, columnIndex];
                }
            }

            return clone;
        }
    }

    class InvalidMatrixSizeException : Exception
    {
        public InvalidMatrixSizeException(string message) : base(message) { }
    }

    class DiffrentMatrixSizeException : Exception
    {
        public DiffrentMatrixSizeException(string message) : base(message) { }
    }

    class NonInvertibleMatrixException : Exception
    {
        public NonInvertibleMatrixException(string message) : base(message) { }
    }

    class CustomArgumentNullException : Exception
    {
        public CustomArgumentNullException(string message) : base(message) { }
    }

    public static class ExtendingMetods
    {
        public static SquareMatrix Transposed(this SquareMatrix matrixForTransposition)
        {
            SquareMatrix result = new SquareMatrix(matrixForTransposition.size);

            for (int rowIndex = 0; rowIndex < matrixForTransposition.size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrixForTransposition.size; ++columnIndex)
                {
                    result.matrix[rowIndex, columnIndex] = matrixForTransposition.matrix[columnIndex, rowIndex];
                }
            }

            return result;
        }

        public static int Trace(this SquareMatrix traceMatrix)
        {
            int trace = 0;

            for (int rowIndex = 0; rowIndex < traceMatrix.size; ++rowIndex)
            {
                trace += traceMatrix.matrix[rowIndex, rowIndex];
            }

            return trace;
        }
    }

    public delegate SquareMatrix DiagonalizeMatrixDelegate(SquareMatrix matrix);

    public abstract class IOperation
    {
        public string OperationType;
    }

    class Add : IOperation
    {
        public Add()
        {
            OperationType = "+";
        }
    }

    class Multiplication : IOperation
    {
        public Multiplication()
        {
            OperationType = "*";
        }
    }

    class More : IOperation
    {
        public More()
        {
            OperationType = ">";
        }
    }

    class Less : IOperation
    {
        public Less()
        {
            OperationType = "<";
        }
    }

    class MoreOrEqual : IOperation
    {
        public MoreOrEqual()
        {
            OperationType = ">=";
        }
    }

    class LessOrEqual : IOperation
    {
        public LessOrEqual()
        {
            OperationType = "<=";
        }
    }

    class Equal : IOperation
    {
        public Equal()
        {
            OperationType = "==";
        }
    }

    class NotEqual : IOperation
    {
        public NotEqual()
        {
            OperationType = "!=";
        }
    }

    class Determinant : IOperation
    {
        public Determinant()
        {
            OperationType = "determinant";
        }
    }

    class Inverse : IOperation
    {
        public Inverse()
        {
            OperationType = "inverse";
        }
    }

    class Transposition : IOperation
    {
        public Transposition()
        {
            OperationType = "transposition";
        }
    }

    class Trace : IOperation
    {
        public Trace()
        {
            OperationType = "trace";
        }
    }

    class Diagonal : IOperation
    {
        public Diagonal()
        {
            OperationType = "diagonal";
        }
    }

    public abstract class BaseHandler
    {
        protected BaseHandler Next;
        protected IOperation Operation;

        public BaseHandler()
        {
            Next = null;
        }

        public virtual void Handle(IOperation operation)
        {
            if (Operation.OperationType == operation.OperationType)
            {
                Console.WriteLine("Операция успешно обработана");
            }
            else
            {
                Console.WriteLine("Не могу обработать, отправляю следующему обработчику...");
                
                if (Next != null)
                {
                    Next.Handle(operation);
                }
                else
                {
                    Console.WriteLine("Неизвестная операция, не могу обработать.");
                }
            }
        }

        protected void SetNextHandler(BaseHandler newHandler)
        {
            Next = newHandler;
        }
    }

    class AddHandler : BaseHandler
    {
        public AddHandler()
        {
            Operation = new Add();
            Next = new MultiplicationHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix1 = new SquareMatrix(matrixSize);
            SquareMatrix matrix2 = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine(" Матрица 2:");
            Console.WriteLine(matrix2);
            Console.WriteLine(" Сумма матриц:");
            Console.WriteLine(matrix1 + matrix2);
        }
    }

    class MultiplicationHandler : BaseHandler
    {
        public MultiplicationHandler()
        {
            Operation = new Multiplication();
            Next = new MoreHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix1 = new SquareMatrix(matrixSize);
            SquareMatrix matrix2 = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine(" Матрица 2:");
            Console.WriteLine(matrix2);
            Console.WriteLine(" Произведение матриц:");
            Console.WriteLine(matrix1 * matrix2);
        }
    }

    class MoreHandler : BaseHandler
    {
        public MoreHandler()
        {
            Operation = new More();
            Next = new LessHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix1 = new SquareMatrix(matrixSize);
            SquareMatrix matrix2 = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine(" Матрица 2:");
            Console.WriteLine(matrix2);
            Console.WriteLine(" 1-ая матрица больше 2-ой: " + (matrix1 > matrix2) + "\n");
        }
    }

    class LessHandler : BaseHandler
    {
        public LessHandler()
        {
            Operation = new Less();
            Next = new MoreOrEqualHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix1 = new SquareMatrix(matrixSize);
            SquareMatrix matrix2 = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine(" Матрица 2:");
            Console.WriteLine(matrix2);
            Console.WriteLine(" 1-ая матрица меньше 2-ой: " + (matrix1 < matrix2) + "\n");
        }
    }

    class MoreOrEqualHandler : BaseHandler
    {
        public MoreOrEqualHandler()
        {
            Operation = new MoreOrEqual();
            Next = new LessOrEqualHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix1 = new SquareMatrix(matrixSize);
            SquareMatrix matrix2 = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine(" Матрица 2:");
            Console.WriteLine(matrix2);
            Console.WriteLine(" 1-ая матрица больше или равна 2-ой: " + (matrix1 >= matrix2) + "\n");
        }
    }

    class LessOrEqualHandler : BaseHandler
    {
        public LessOrEqualHandler()
        {
            Operation = new LessOrEqual();
            Next = new EqualHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix1 = new SquareMatrix(matrixSize);
            SquareMatrix matrix2 = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine(" Матрица 2:");
            Console.WriteLine(matrix2);
            Console.WriteLine(" 1-ая матрица меньше или равна 2-ой: " + (matrix1 <= matrix2) + "\n");
        }
    }

    class EqualHandler : BaseHandler
    {
        public EqualHandler()
        {
            Operation = new Equal();
            Next = new NotEqualHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix1 = new SquareMatrix(matrixSize);
            SquareMatrix matrix2 = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine(" Матрица 2:");
            Console.WriteLine(matrix2);
            Console.WriteLine(" Матрицы равны: " + (matrix1 == matrix2) + "\n");
        }
    }

    class NotEqualHandler : BaseHandler
    {
        public NotEqualHandler()
        {
            Operation = new NotEqual();
            Next = new DeterminantHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix1 = new SquareMatrix(matrixSize);
            SquareMatrix matrix2 = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица 1:");
            Console.WriteLine(matrix1);
            Console.WriteLine(" Матрица 2:");
            Console.WriteLine(matrix2);
            Console.WriteLine(" Матрицы не равны: " + (matrix1 != matrix2) + "\n");
        }
    }

    class DeterminantHandler : BaseHandler
    {
        public DeterminantHandler()
        {
            Operation = new Determinant();
            Next = new InverseHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица:");
            Console.WriteLine(matrix);
            Console.WriteLine(" Определитель матрицы: " + matrix.Determinant() + "\n");
        }
    }

    class InverseHandler : BaseHandler
    {
        public InverseHandler()
        {
            Operation = new Inverse();
            Next = new TranspositionHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица:");
            Console.WriteLine(matrix);
            Console.WriteLine(" Обратная матрица:");
            Console.WriteLine(matrix.Inverse());
        }
    }

    class TranspositionHandler : BaseHandler
    {
        public TranspositionHandler()
        {
            Operation = new Transposition();
            Next = new TraceHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица:");
            Console.WriteLine(matrix);
            Console.WriteLine(" Транспонированная матрица:");
            Console.WriteLine(matrix.Transposed());
        }
    }

    class TraceHandler : BaseHandler
    {
        public TraceHandler()
        {
            Operation = new Trace();
            Next = new DiagonalHandler();
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix = new SquareMatrix(matrixSize);

            Console.WriteLine("\n Матрица:");
            Console.WriteLine(matrix);
            Console.WriteLine(" След матрицы: " + matrix.Trace() + "\n");
        }
    }

    class DiagonalHandler : BaseHandler
    {
        public DiagonalHandler()
        {
            Operation = new Diagonal();
            Next = null;
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix matrix = new SquareMatrix(matrixSize);

            DiagonalizeMatrixDelegate diagonalizeMatrixDelegate = delegate (SquareMatrix matrixForDiagonalize)
            {
                for (int rowIndex = 0; rowIndex < matrix.size; ++rowIndex)
                {
                    for (int columnIndex = 0; columnIndex < matrix.size; ++columnIndex)
                    {
                        if (rowIndex != columnIndex)
                        {
                            matrixForDiagonalize.matrix[rowIndex, columnIndex] = 0;
                        }
                    }
                }
                return matrixForDiagonalize;
            };

            Console.WriteLine("\n Матрица:");
            Console.WriteLine(matrix);
            Console.WriteLine(" Диагонализированная матрица:");
            Console.WriteLine(diagonalizeMatrixDelegate(matrix));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int userChoice;
            BaseHandler operationHandler;
            operationHandler = new AddHandler();

            Console.WriteLine(" Какаю операцию вы хотите выполнить? (напишите только цифру)\n" +
                              " 1. Сложить две случайные матрицы\n" +
                              " 2. Умножить две случайные матрицы\n" +
                              " 3. Посчитать определитель случайной матрицы\n" +
                              " 4. Найти матрицу, обратную случайной матрице\n" +
                              " 5. Транспонировать случайную матрицу\n" +
                              " 6. Найти след случайной матрицы\n" +
                              " 7. Привести матрицу к диагональному виду\n");
            Console.Write("Ваш выбор: ");
            userChoice = Convert.ToInt32(Console.ReadLine());

            switch (userChoice)
            {
                case 1:
                    //operationHandler.Handle(new AddHandler());
                    break;
                case 2:
                    operationHandler = new MultiplicationHandler();
                    break;
                case 3:
                    operationHandler = new DeterminantHandler();
                    break;
                case 4:
                    operationHandler = new InverseHandler();
                    break;
                case 5:
                    operationHandler = new TranspositionHandler();
                    break;
                case 6:
                    operationHandler = new TraceHandler();
                    break;
                case 7:
                    operationHandler = new DiagonalHandler();
                    break;
            }
        }
    }
}
