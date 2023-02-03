using MPI;
using System;
using System.Collections.Generic;

namespace Mpi__prime_numbers
{
    class Program
    {
        static List<int> isPrime(int start, int end, List<int> primeNumbers)
        {
            List<int> result = new List<int>();
            for (int j = start; j < end; j++)
            {
                int x = j;
                bool prime = true;
                for (int i = 0; i < primeNumbers.Count; i++)
                    if (x % primeNumbers[i] == 0) prime = false;
                if (prime)
                {
                    result.Add(x);
                }
            }
            return result;
        }
        static void worker()
        {
            int start, end,n;
            List<int> primeNumbers;
            List<int> numbersLeft;

            primeNumbers = Communicator.world.Receive<List<int>>(0, 0);
            numbersLeft = Communicator.world.Receive<List<int>>(0, 1);
            n= Communicator.world.Receive<int>(0, 2);

            int noPerThread = (numbersLeft.Count + Communicator.world.Size - 1) / Communicator.world.Size;
            start = Communicator.world.Rank * noPerThread;
            end = start + noPerThread;
            if (end > numbersLeft.Count)
                end = numbersLeft.Count;

            List<int> result = new List<int>();
            result.AddRange(isPrime(numbersLeft[start], numbersLeft[end - 1], primeNumbers));

            Communicator.world.Send(result, 0, 4);
        }

        static void controller()
        {
            int n = 24;
            List<int> primeNumbers = new List<int>();
            List<int> numbersLeft= new List<int>();

            primeNumbers.Add(2);
            primeNumbers.Add(3);

            for (int i = Convert.ToInt32(Math.Floor(Math.Sqrt(n)))+1; i <= n; i++)
                numbersLeft.Add(i);

            int noPerThread = (numbersLeft.Count + Communicator.world.Size - 1) / Communicator.world.Size;

            for (int i = 1; i < Communicator.world.Size; i++)
            {
              
                Communicator.world.Send<List<int>>(primeNumbers, i, 0);
                Communicator.world.Send<List<int>>(numbersLeft, i, 1);
                Communicator.world.Send<int>(n, i, 2);


            }

            List<int> result = new List<int>();
            int start = Convert.ToInt32(Math.Floor(Math.Sqrt(n))) + 1;
            int end = start + noPerThread;
            result.AddRange(isPrime(start, end, primeNumbers));

            for (int i = 1; i < Communicator.world.Size; i++)
            {
                List<int> partialResult = Communicator.world.Receive<List<int>>(i, 4);
                result.AddRange(partialResult);
            }

            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine(String.Concat(result[i]));
            }

        }

        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                if (Communicator.world.Rank == 0)
                {
                    controller();
                }
                else
                {
                    worker();
                }

            }
        }
    }
}
