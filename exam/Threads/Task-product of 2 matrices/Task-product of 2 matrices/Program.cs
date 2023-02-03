using System;
using System.Collections.Generic;
using System.Threading;

namespace Task_product_of_2_matrices
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
                int i = position / result[0].Count;
                int j = position % result[0].Count;
                Console.Write(i + " "+ j+ " ");

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
        static void Main(string[] args)
        {
            List<List<int>> matrix1 = new List<List<int>>();
            for (int i = 0; i < 4; i++)
            {
                matrix1.Add(new List<int>());
                for (int j = 0; j < 5; j++)
                {
                    matrix1[i].Add(1);
                }
            }
            List<List<int>> matrix2 = new List<List<int>>();
            for (int i = 0; i < 5; i++)
            {
                matrix2.Add(new List<int>());
                for (int j = 0; j < 6; j++)
                {
                    matrix2[i].Add(1);
                }
            }
            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < 4; i++)
            {
                result.Add(new List<int>());
                for (int j = 0; j < 6; j++)
                {
                    result[i].Add(0);
                }
            }

            result = multiplyMatrices(matrix1, matrix2);

            foreach (List<int> resultPart in result)
            {
                foreach (int resultMember in resultPart)
                {
                    Console.Write(resultMember + " ");
                }
                Console.WriteLine();
            }


        }
    }
}
