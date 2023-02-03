using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace T_numere_prime
{

    class PrimeNumbers
    {
        private int x;
        private int noThreads;
        private int currentThread;
        private List<int> list_;

        public PrimeNumbers(int x, int noThreads, int currentThread, List<int> list_)
        {
            this.x = x;
            this.noThreads = noThreads;
            this.currentThread = currentThread;
            this.list_ = list_;
        }

        static bool isPrime(int n)
        {
            bool ok = true;
            if (n < 2) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0) ok = false;
            }
            return ok;
        }
        public void primeNumbers()
        {
            int perThread = x / noThreads;
            int start = currentThread * perThread;
            int end = this.currentThread == this.noThreads - 1 ? x : start + perThread - 1;


            for (int i = start; i <= end; i++)
            {
                if (isPrime(i) == true)
                {
                    lock(list_)
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
            int  x = Convert.ToInt32(Console.ReadLine());
            var list_ = new List<int>();
            var threadList = new List<Thread>();
            int noThreads = 3;

            for(int i = 0; i < noThreads;i++)
            {
                PrimeNumbers prime = new PrimeNumbers(x, noThreads, i, list_);
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
