using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
namespace T__number_is_div_with_a_prime_number
{

    class PrimeNumber
    {
        private int x;
        private int noThreads;
        private int currentThread;
        private List<int> list_;
        private int noElems;
        private List<int> primeElems;
        private int start;

        public PrimeNumber(int x, int noThreads, int currentThread, List<int> list_, int noElems, List<int> primeElems, int start)
        {
            this.x = x;
            this.noThreads = noThreads;
            this.currentThread = currentThread;
            this.list_ = list_;
            this.noElems = noElems;
            this.primeElems = primeElems;
            this.start = start;
        }

        public bool isPrime(int n)
        {
            for(int i = 0; i < this.primeElems.Count; i++)
            {
                if (n % this.primeElems[i] == 0) return false;
            }
            return true;

        }

        public void primeNumbers()
        {
            int perThread = this.noElems / this.noThreads;
            int startFrom = start + currentThread * perThread;

            int end = this.currentThread == this.noThreads - 1 ? x : startFrom + perThread - 1;

            for (int i = startFrom; i <= end; i++)
            {
                if (isPrime(i) == true)
                {
                    lock (list_)
                    {
                        list_.Add(i);
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

            Console.WriteLine("Give the number of elements: ");
            int noPrimeNumbers = Convert.ToInt32(Console.ReadLine());

            List<int> primeList = new List<int>();
            List<int> result = new List<int>();
            List<int> list_ = new List<int>();



            for (int i = 0; i < noPrimeNumbers; i++)
            {
                int number = Convert.ToInt32(Console.ReadLine());
                primeList.Add(number);

            }

            int noElems=0;
            for(int i = Convert.ToInt32(Math.Sqrt(x)); i < x; i++) {
                noElems += 1 ;
            }
            var threadList = new List<Thread>();
            int noThreads = 3;
            for (int i = 0; i < noThreads; i++)
            {
                PrimeNumber prime = new PrimeNumber(x, noThreads, i, list_,noElems, primeList, Convert.ToInt32(Math.Sqrt(x)));
                ThreadStart threadStart = new ThreadStart(prime.primeNumbers);
                Thread thread = new Thread(threadStart);
                threadList.Add(thread);
                thread.Start();
            }

            for (int i = 0; i < noThreads; i++)
            {
                threadList[i].Join();
            }

            for (int i = 0; i < list_.Count; i++)
            {
                Console.Write(list_[i] + " ");
            }
            Console.WriteLine();
        }
    }
    }

