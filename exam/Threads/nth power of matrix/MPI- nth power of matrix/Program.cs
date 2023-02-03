using System;
using System.Collections.Generic;
using System.Threading;

namespace MPI__nth_power_of_matrix
{
    class Program
    {
        static int computeMultiplicationElement(List<List<int>> firstMatrix, List<List<int>> secondMatrix, int i, int j)
        {
            int result = 0;
            for (int k = 0; k < secondMatrix.Count; k++)
            {
                result += firstMatrix[i][k] * secondMatrix[k][j];
            }
            return result;
        }

        static void partiallyMultiplyMatrix(List<List<int>> firstMatrix, List<List<int>> secondMatrix, List<List<int>> result, int threadNumber, int threadCount)
        {
            int resultSize = result.Count * result[0].Count;
            int position = threadNumber;
            while (position < resultSize)
            {
                int i = position / result.Count;
                int j = position % result.Count;
                result[i][j] = computeMultiplicationElement(firstMatrix, secondMatrix, i, j);
                position += threadCount;
            }
        }

        static List<List<int>> multiplyMatrices(List<List<int>> firstMatrix, List<List<int>> secondMatrix)
        {
            int noThreads = 5;
            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < firstMatrix.Count; i++)
            {
                result.Add(new List<int>());
                for (int j = 0; j < secondMatrix[0].Count; j++)
                {
                    result[i].Add(0);
                }
            }

            List<Thread> threadList = new List<Thread>();
            for (int i = 0; i < noThreads; i++)
            {
                int pos = i;
                Thread thread = new Thread(() => partiallyMultiplyMatrix(firstMatrix, secondMatrix, result, pos, noThreads));
                thread.Start();
                threadList.Add(thread);
            }

            foreach (Thread thread in threadList)
            {
                thread.Join();
            }

            return result;
        }

        static List<bool> convertToPowersOfTwo(int n)
        {
            List<bool> result = new List<bool>();
            while (n > 0)
            {
                result.Add(n % 2 == 1);
                n /= 2;
            }
            return result;
        }

        static void Main(string[] args)
        {
            List<List<int>> matrix = new List<List<int>>();
            int n = 5;
            for (int i = 0; i < 4; i++)
            {
                matrix.Add(new List<int>());
                for (int j = 0; j < 4; j++)
                {
                    matrix[i].Add(1);
                }
            }

            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < 4; i++)
            {
                result.Add(new List<int>());
                for (int j = 0; j < 4; j++)
                {
                    result[i].Add(i == j ?  1 :  0);
                }
            }

            List<bool> powerAsPowersOfTwo = convertToPowersOfTwo(n);
            for (int i = 0; i < powerAsPowersOfTwo.Count; i++)
            {
                if (powerAsPowersOfTwo[i])
                {
                   result = multiplyMatrices(result, matrix);
                }
                matrix = multiplyMatrices(matrix, matrix);
            }

            foreach (List<int> resultPart in result)
            {
                foreach(int resultMember in resultPart)
                {
                    Console.Write(resultMember + " ");
                }
                Console.WriteLine();
            }

        }
    }
}
