using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
namespace T_permutari
{
    class Permutari
    {
        private List<int> n;
        private int noThreads;
        private int currentThread;
        private List<int> list_;
        private List<int> startingSymbols;

        public Permutari(List<int> n, int noThreads, int currentThread, List<int> list_, List<int> startingSymbols)
        {
            this.n = n;
            this.noThreads = noThreads;
            this.currentThread = currentThread;
            this.list_ = list_;
            this.startingSymbols = startingSymbols;
        }

        static bool satisfyCondition(List<int> list_)
        {
            if (list_[list_.Count - 1] % 2 == 0) return true;
            return false;
        }

        public List<List<int>> generatePermutations(List<int> list_)
        {
            List<List<int>> result = new List<List<int>>();
            for(int i = 0; i < this.n.Count; i++)
            {
                if (!list_.Contains(this.n[i]))
                {
                    list_.Add(n[i]);
                    if (list_.Count == n.Count)
                    {
                        List<int> resultList = new List<int>();
                        for (int j = 0; j < list_.Count; j++)
                        {
                            resultList.Add(list_[j]);
                        }
                        result.Add(resultList);
                    }
                    else
                    {
                        List<List<int>> results = generatePermutations(list_);
                        result.AddRange(results);
                    }
                    list_.RemoveAt(list_.Count - 1);
                }
            }
            return result;
        }
        public void findPermutation()
        {
            for(int i = 0; i < this.startingSymbols.Count; i++)
            {
                List<int> permutationList = new List<int>();
                permutationList.Add(this.startingSymbols[i]);

                List<List<int>> permutations = generatePermutations(permutationList);

                for(int j = 0; j < permutations.Count; j++)
                {
                    if (satisfyCondition(permutations[i]))
                    {
                        lock(list_)
                        {
                            if (list_.Count == 0)
                            {
                                list_.AddRange(permutations[i]);
                            }
                        }
                        return;
                    }
                }

            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Give a number: ");
            int x = Convert.ToInt32(Console.ReadLine());
            List<int> n= new List<int>();
            List<int> result = new List<int>();


            for (int i = 0; i < x; i++)
            {
                int number = Convert.ToInt32(Console.ReadLine());
                n.Add(number);

            }

            int permutationLength = n.Count;
            var threadList = new List<Thread>();
            int noThreads = 3;
            if (noThreads >= permutationLength) {
                
                for (int i = 0; i < noThreads; i++)
                {
                    List<int> startingSymbol = new List<int>();
                    startingSymbol.Add(n[i]);
                    Permutari permutari = new Permutari(n, noThreads, i, result, startingSymbol);
                    ThreadStart threadStart = new ThreadStart(permutari.findPermutation);
                    Thread thread = new Thread(threadStart);
                    threadList.Add(thread);
                    thread.Start();
                }
            }
            else
            {
                int numberPerThread = permutationLength / noThreads;
                for (int i = 0; i < noThreads-1; i++)
                {
                    List<int> startingSymbol = new List<int>();

                    for(int j=i*numberPerThread; j< i * numberPerThread+numberPerThread;j++)
                        startingSymbol.Add(n[j]);
                    Permutari permutari = new Permutari(n, noThreads, i, result, startingSymbol);
                    ThreadStart threadStart = new ThreadStart(permutari.findPermutation);
                    Thread thread = new Thread(threadStart);
                    threadList.Add(thread);
                    thread.Start();
                }

                List<int> startingSymbolFinal = new List<int>();

                for (int j =( noThreads-1) * numberPerThread; j < permutationLength; j++)
                    startingSymbolFinal.Add(n[j]);
                Permutari permutariFinal = new Permutari(n, noThreads, noThreads, result, startingSymbolFinal);
                ThreadStart threadStartFinal = new ThreadStart(permutariFinal.findPermutation);
                Thread threadFinal = new Thread(threadStartFinal);
                threadList.Add(threadFinal);
                threadFinal.Start();

            }

            for (int i = 0; i < noThreads; i++)
            {
                threadList[i].Join();
            }

            for (int i = 0; i < result.Count; i++)
            {
                Console.Write(result[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
