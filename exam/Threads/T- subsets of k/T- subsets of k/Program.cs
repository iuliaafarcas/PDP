using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace T__subsets_of_k
{
    class Program
    {

        static bool checkProperty(List<int> subset)
        {
            int sum = 5;
            for (int i = 0; i < subset.Count; i++)
            {
                sum -= subset[i];
            }
            return sum >= 0;
        }

        static int countSubsetsWithProperty(List<int> elements, List<int> currentSubset, int size)
        {
            int count = 0;
            for (int i = 0;  i < elements.Count; i++)
            {
                if (currentSubset[currentSubset.Count - 1] < elements[i])
                {
                    currentSubset.Add(elements[i]);
                    if (currentSubset.Count == size)
                    {
                        if (checkProperty(currentSubset))
                        {
                            count++;
                        }
                    } else
                    {
                        count += countSubsetsWithProperty(elements, currentSubset, size);
                    }
                    currentSubset.RemoveAt(currentSubset.Count - 1);
                }
            }
            return count;
        }

        static void Main(string[] args)
        {
            int n=8;
            int k = 3;
            List<int> multime = new List<int>();
            for (int i = 0; i < n; i++)
                multime.Add(i);
            List<Task<int>> taskList = new List<Task<int>>();
            int noThreads = n - k + 1;
            for(int i = 0;  i <  noThreads; i++)
            {
                List<int> elements = multime.GetRange(i + 1, multime.Count - i - 1);
                List<int> currentSubset = new List<int> { multime[i] };
                Task<int> task = Task.Factory.StartNew(() => countSubsetsWithProperty(elements, currentSubset, k));
                taskList.Add(task);
            }

            int count = 0;
            foreach (Task<int> task in taskList)
            {
                count += task.Result;
            }
            Console.WriteLine(count);
        }
    }
}
