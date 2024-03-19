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

    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int matrixSize = random.Next(3, 5);
            SquareMatrix myMatrix1 = new SquareMatrix(matrixSize);
            SquareMatrix myMatrix2 = new SquareMatrix(matrixSize);
            DiagonalizeMatrixDelegate diagonalizeMatrixDelegate = delegate (SquareMatrix matrix)
            {
                for (int rowIndex = 0; rowIndex < matrix.size; ++rowIndex)
                {
                    for (int columnIndex = 0; columnIndex < matrix.size; ++columnIndex)
                    {
                        if (rowIndex != columnIndex)
                        {
                            matrix.matrix[rowIndex, columnIndex] = 0;
                        }
                    }
                }
                return matrix;
            };

            Console.WriteLine("\n Матрица 1:");
            Console.WriteLine(myMatrix1);
            Console.WriteLine(" Матрица 2:");
            Console.WriteLine(myMatrix2);
            Console.WriteLine(" Сумма матриц:");
            Console.WriteLine(myMatrix1 + myMatrix2);
            Console.WriteLine(" Произведение матриц:");
            Console.WriteLine(myMatrix1 * myMatrix2);
            Console.WriteLine(" Определитель 1-ой матрицы: " + myMatrix1.Determinant() + "\n");
            Console.WriteLine(" Определитель 2-ой матрицы: " + myMatrix2.Determinant() + "\n");
            Console.WriteLine(" След 1-ой матрицы: " + myMatrix1.Trace() + "\n");
            Console.WriteLine(" След 2-ой матрицы: " + myMatrix2.Trace() + "\n");
            Console.WriteLine(" Матрица, обратная 1-ой:");
            Console.WriteLine(myMatrix1.Inverse());
            Console.WriteLine(" Матрица, обратная 2-ой:");
            Console.WriteLine(myMatrix2.Inverse());
            Console.WriteLine(" Диагональная матрица 1:");
            Console.WriteLine(diagonalizeMatrixDelegate(myMatrix1));
            Console.WriteLine(" Диагональная матрица 2:");
            Console.WriteLine(diagonalizeMatrixDelegate(myMatrix2));
            Console.WriteLine(" 1-ая матрица больше 2-ой: " + (myMatrix1 > myMatrix2) + "\n");
            Console.WriteLine(" 1-ая матрица меньше 2-ой: " + (myMatrix1 < myMatrix2) + "\n");
            Console.WriteLine(" 1-ая матрица больше или равна 2-ой: " + (myMatrix1 >= myMatrix2) + "\n");
            Console.WriteLine(" 1-ая матрица меньше или равна 2-ой: " + (myMatrix1 <= myMatrix2) + "\n");
            Console.WriteLine(" Матрицы равны: " + (myMatrix1 == myMatrix2) + "\n");
            Console.WriteLine(" Матрицы не равны: " + (myMatrix1 != myMatrix2) + "\n");
            Console.WriteLine(" 1-ая матрица нулевая: " + myMatrix1.IsMatrixNull() + "\n");
            Console.WriteLine(" 2-ая матрица нулевая: " + myMatrix2.IsMatrixNull() + "\n");
            Console.WriteLine(" Hash code 1-ой матрицы : " + myMatrix1.GetHashCode() + "\n");
            Console.WriteLine(" Hash code 2-ой матрицы : " + myMatrix2.GetHashCode() + "\n");
        }
    }
}
