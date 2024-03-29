using System;
using System.Text;

namespace DelegatesAndEvents
{
    public class SquareMatrix : ICloneable, IComparable<SquareMatrix>
    {
        public int[,] Matrix;
        public int size;

        public SquareMatrix(int size)
        {
            if (size <= 1)
            {
                throw new InvalidMatrixSizeException("Матрица не может быть такого размера!");
            }

            this.size = size;
            Matrix = new int[size, size];
            Random random = new Random((int)DateTime.Now.Ticks);

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    Matrix[rowIndex, columnIndex] = random.Next(-10, 10);
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
                    result.Matrix[rowIndex, columnIndex] += matrix2.Matrix[rowIndex, columnIndex];
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
                        result.Matrix[rowIndex, columnIndex] += matrix1.Matrix[rowIndex, positionIndex] *
                                                                matrix2.Matrix[positionIndex, columnIndex];
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
                    result[rowIndex, columnIndex] = matrix.Matrix[rowIndex, columnIndex];
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
                    result += Matrix[rowIndex, columnIndex];
                }
            }
            
            return result == 0;
        }

        public int Determinant()
        {
            if (size == 2)
            {
                return (Matrix[0, 0] * Matrix[1, 1]) - (Matrix[0, 1] * Matrix[1, 0]);
            }
            int result = 0;

            for (int columnIndex = 0; columnIndex < size; ++columnIndex)
            {
                result += (columnIndex % 2 == 1 ? 1 : -1) * Matrix[1, columnIndex] *
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
                    result.Matrix[rowIndex, columnIndex] = columnIndex < column ?
                      rowIndex < row ?
                      Matrix[rowIndex, columnIndex] :
                      Matrix[rowIndex + 1, columnIndex] :
                      rowIndex < row ?
                      Matrix[rowIndex, columnIndex + 1] :
                      Matrix[rowIndex + 1, columnIndex + 1];
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
                    result.Matrix[rowIndex, columnIndex] = (columnIndex + rowIndex) % 2 == 0 ? 1 : -1 *
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
                    stringBuilder.Append(" " + Matrix[rowIndex, columnIndex].ToString("0\t"));
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
                        if (this.Matrix[rowIndex, columnIndex] != other.Matrix[rowIndex, columnIndex])
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
                        hash = hash * primeNumber + Matrix[rowIndex, columnIndex].GetHashCode();
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
                    clone.Matrix[rowIndex, columnIndex] = this.Matrix[rowIndex, columnIndex];
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
                    result.Matrix[rowIndex, columnIndex] = matrixForTransposition.Matrix[columnIndex, rowIndex];
                }
            }

            return result;
        }

        public static int Trace(this SquareMatrix traceMatrix)
        {
            int trace = 0;

            for (int rowIndex = 0; rowIndex < traceMatrix.size; ++rowIndex)
            {
                trace += traceMatrix.Matrix[rowIndex, rowIndex];
            }

            return trace;
        }
    }

    public delegate SquareMatrix DiagonalizeMatrixDelegate(SquareMatrix matrix);

    public abstract class Operation
    {
        public string OperationType;
    }

    class Add : Operation
    {
        public Add()
        {
            OperationType = "1";
        }
    }

    class Multiplication : Operation
    {
        public Multiplication()
        {
            OperationType = "2";
        }
    }

    class Determinant : Operation
    {
        public Determinant()
        {
            OperationType = "3";
        }
    }

    class Inverse : Operation
    {
        public Inverse()
        {
            OperationType = "4";
        }
    }

    class Transposition : Operation
    {
        public Transposition()
        {
            OperationType = "5";
        }
    }

    class Trace : Operation
    {
        public Trace()
        {
            OperationType = "6";
        }
    }

    class Diagonal : Operation
    {
        public Diagonal()
        {
            OperationType = "7";
        }
    }

    public abstract class BaseHandler
    {
        protected BaseHandler Next;
        protected Operation Operation;
        protected delegate void RunFunction();
        protected RunFunction TargetFunction;
        public SquareMatrix Matrix1;
        public SquareMatrix Matrix2;

        public BaseHandler()
        {
            Next = null;
        }

        public virtual void Handle(Operation operation, SquareMatrix matrixOne, SquareMatrix matrixTwo)
        {
            Matrix1 = matrixOne;
            Matrix2 = matrixTwo;

            if (Operation.OperationType == operation.OperationType)
            {
                Console.WriteLine("\n Операция успешно обработана, номер обработчика: " + Operation.OperationType);
                TargetFunction();
            }
            else
            {
                Console.WriteLine(" Не могу обработать, отправляю следующему обработчику...");

                if (Next != null)
                {
                    Next.Handle(operation, matrixOne, matrixTwo);
                }
                else
                {
                    Console.WriteLine(" Неизвестная операция, не могу обработать.");
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
            TargetFunction = delegate ()
            {
                Console.WriteLine("\n Матрица 1:");
                Console.WriteLine(Matrix1);
                Console.WriteLine(" Матрица 2:");
                Console.WriteLine(Matrix2);
                Console.WriteLine(" Сумма матриц:");
                Console.WriteLine(Matrix1 + Matrix2);
            };
        }
    }

    class MultiplicationHandler : BaseHandler
    {
        public MultiplicationHandler()
        {
            Operation = new Multiplication();
            Next = new DeterminantHandler();
            TargetFunction = delegate ()
            {
                Console.WriteLine("\n Матрица 1:");
                Console.WriteLine(Matrix1);
                Console.WriteLine(" Матрица 2:");
                Console.WriteLine(Matrix2);
                Console.WriteLine(" Произведение матриц:");
                Console.WriteLine(Matrix1 * Matrix2);
            };
        }
    }

    class DeterminantHandler : BaseHandler
    {
        public DeterminantHandler()
        {
            Operation = new Determinant();
            Next = new InverseHandler();
            TargetFunction = delegate ()
            {
                Console.WriteLine("\n Матрица:");
                Console.WriteLine(Matrix1);
                Console.WriteLine(" Определитель матрицы: " + Matrix1.Determinant() + "\n");
            };
        }
    }

    class InverseHandler : BaseHandler
    {
        public InverseHandler()
        {
            Operation = new Inverse();
            Next = new TranspositionHandler();
            TargetFunction = delegate ()
            {
                Console.WriteLine("\n Матрица:");
                Console.WriteLine(Matrix1);
                Console.WriteLine(" Обратная матрица:");
                Console.WriteLine(Matrix1.Inverse());
            };
        }
    }

    class TranspositionHandler : BaseHandler
    {
        public TranspositionHandler()
        {
            Operation = new Transposition();
            Next = new TraceHandler();
            TargetFunction = delegate ()
            {
                Console.WriteLine("\n Матрица:");
                Console.WriteLine(Matrix1);
                Console.WriteLine(" Транспонированная матрица:");
                Console.WriteLine(Matrix1.Transposed());
            };
        }
    }

    class TraceHandler : BaseHandler
    {
        public TraceHandler()
        {
            Operation = new Trace();
            Next = new DiagonalHandler();
            TargetFunction = delegate ()
            {
                Console.WriteLine("\n Матрица:");
                Console.WriteLine(Matrix1);
                Console.WriteLine(" След матрицы: " + Matrix1.Trace() + "\n");
            };
        }
    }

    class DiagonalHandler : BaseHandler
    {
        public DiagonalHandler()
        {
            Operation = new Diagonal();
            Next = null;
            DiagonalizeMatrixDelegate diagonalizeMatrixDelegate = delegate (SquareMatrix matrixForDiagonalize)
            {
                for (int rowIndex = 0; rowIndex < matrixForDiagonalize.size; ++rowIndex)
                {
                    for (int columnIndex = 0; columnIndex < matrixForDiagonalize.size; ++columnIndex)
                    {
                        if (rowIndex != columnIndex)
                        {
                            matrixForDiagonalize.Matrix[rowIndex, columnIndex] = 0;
                        }
                    }
                }
                return matrixForDiagonalize;
            };

            TargetFunction = delegate ()
            {
                Console.WriteLine("\n Матрица:");
                Console.WriteLine(Matrix1);
                Console.WriteLine(" Диагонализированная матрица:");
                Console.WriteLine(diagonalizeMatrixDelegate(Matrix1));
            };
        }
    }

    public class ChainApplication
    {
        private BaseHandler _operationHandler;

        public ChainApplication()
        {
            _operationHandler = new AddHandler();
        }

        public void Run(Operation operation, SquareMatrix matrix1, SquareMatrix matrix2)
        {
            _operationHandler.Handle(operation, matrix1, matrix2);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix myMatrix1 = new SquareMatrix(matrixSize);
            SquareMatrix myMatrix2 = new SquareMatrix(matrixSize);

            Console.WriteLine(" Какаю операцию вы хотите выполнить? (напишите только цифру)\n" +
                              " 1. Сложить две случайные матрицы\n" +
                              " 2. Умножить две случайные матрицы\n" +
                              " 3. Посчитать определитель случайной матрицы\n" +
                              " 4. Найти матрицу, обратную случайной матрице\n" +
                              " 5. Транспонировать случайную матрицу\n" +
                              " 6. Найти след случайной матрицы\n" +
                              " 7. Привести матрицу к диагональному виду\n");
            Console.Write(" Ваш выбор: ");
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    ChainApplication chainApplication1 = new ChainApplication();
                    chainApplication1.Run(new Add(), myMatrix1, myMatrix2);
                    break;
                case "2":
                    ChainApplication chainApplication2 = new ChainApplication();
                    chainApplication2.Run(new Multiplication(), myMatrix1, myMatrix2);
                    break;
                case "3":
                    ChainApplication chainApplication3 = new ChainApplication();
                    chainApplication3.Run(new Determinant(), myMatrix1, myMatrix2);
                    break;
                case "4":
                    ChainApplication chainApplication4 = new ChainApplication();
                    chainApplication4.Run(new Inverse(), myMatrix1, myMatrix2);
                    break;
                case "5":
                    ChainApplication chainApplication5 = new ChainApplication();
                    chainApplication5.Run(new Transposition(), myMatrix1, myMatrix2);
                    break;
                case "6":
                    ChainApplication chainApplication6 = new ChainApplication();
                    chainApplication6.Run(new Trace(), myMatrix1, myMatrix2);
                    break;
                case "7":
                    ChainApplication chainApplication7 = new ChainApplication();
                    chainApplication7.Run(new Diagonal(), myMatrix1, myMatrix2);
                    break;
            }

            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
