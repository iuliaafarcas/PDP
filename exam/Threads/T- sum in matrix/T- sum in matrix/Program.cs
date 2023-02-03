using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace T__sum_in_matrix
{
    class Program
    {

        static int computeSum(List<int> elements, int start, int end)
        {
            if (end - start == 1)
            {
                return elements[start];
            }
            if (end - start == 2)
            {
                return elements[start] + elements[start+1];
            }
            int middle = (end + start) / 2;

            Task<int> task = Task<int>.Factory.StartNew(() => computeSum(elements, middle, end));
            int firstPartSum = computeSum(elements, start, middle);
            int secondPartSum = task.Result;
            return firstPartSum + secondPartSum;
        }


        static void Main(string[] args)
        {
            List<List<int>> matrix = new List<List<int>>();
            int no = 0;
            for (int i = 0; i < 4; i++)
            {
                matrix.Add(new List<int>());
                for(int j=0  ;j<5;j++)
                {
                    matrix[i].Add(no++);
                }
            }

            List<int> list = new List<int>();
            for (int i = 0; i < matrix.Count; i++)
            {
                list.AddRange(matrix[i]);
            }

            int sum = computeSum(list, 0, list.Count);
            Console.WriteLine(sum);

        } 
    }
}
