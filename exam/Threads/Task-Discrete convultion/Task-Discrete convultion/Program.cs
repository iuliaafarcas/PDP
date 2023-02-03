using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Task_Discrete_convultion
{
    class Program
    {
        static int computeSum(List<int> a, List<int> b, int pos)
        {
            Console.WriteLine(pos);
            int element = 0;
            for (int j = 0; j < a.Count; j++)
            {
                element += a[j] * b[(pos - j) >= 0 ? (pos - j) : (a.Count + pos - j)];
            }
            return element;
        }

   
        static void Main(string[] args)
        {
            List<int> a = new List<int>();
            List<int> b = new List<int>();
            int n = 5;
            for (int i = 0; i < n; i++)
                a.Add(i);
            for (int i = n-1; i >=0; i--)
                b.Add(i);

            List<int> result = new List<int>();
            List<Task<int>> taskList = new List<Task<int>>();
          
              for (int i = 0; i < n; i++)
            {
                int pos = i;
                Task<int> task = Task.Factory.StartNew(() => computeSum(a,b,pos));
                taskList.Add(task);
            }

            for (int i = 0; i < n; i++)
            {
                
            result.Add(taskList[i].Result);
                
            }

            Console.WriteLine(String.Concat(result));
        }
    }
    }

