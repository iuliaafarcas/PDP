using MPI;
using System;
using System.Collections.Generic;

namespace MPI_convultion
{
    class Program
    {

        static List<int> computePartialConvolution(List<int> a, List<int> b, int start, int end)
        {
            List<int> result = new List<int>();
            for (int i = start; i < end; i++)
            {
                int element = 0;
                for (int j = 0; j < a.Count; j ++)
                {
                    element += a[j] * b[(i - j) >= 0 ? (i-j) : (a.Count + i -j)];
                }
                result.Add(element);
            }
            return result;
        }

        static void worker()
        {
            List<int> a;
            List<int> b;


            a = Communicator.world.Receive<List<int>>(0, 0);
            b = Communicator.world.Receive<List<int>>(0, 1);

            int numbersPerProcess = (a.Count + Communicator.world.Size - 1) / Communicator.world.Size;
            int start = Communicator.world.Rank * numbersPerProcess;
            int end = start + numbersPerProcess;
            if (end > a.Count)
            {
                end = a.Count;
            }

            List<int> result = computePartialConvolution(a, b, start, end);

            Communicator.world.Send<List<int>>(result, 0, 3);
        }
        static void controller()
        {
            List<int> a = new List<int>();
            List<int> b = new List<int>();
            int n = 5;
            for (int i = 0; i < n; i++)
                a.Add(i);
            for (int i = n - 1; i >= 0; i--)
                b.Add(i);

            List<int> result = new List<int>();

            int startsPerProcess = (a.Count + Communicator.world.Size - 1) / Communicator.world.Size;

            for(int i = 1; i < Communicator.world.Size; i++)
            {
                Communicator.world.Send<List<int>>(a, i, 0);
                Communicator.world.Send<List<int>>(b, i, 1);
            }
            result.AddRange(computePartialConvolution(a, b, 0, startsPerProcess > a.Count ? a.Count : startsPerProcess));

            for (int i = 1; i < Communicator.world.Size; i++)
            {
                List<int> partialResult = Communicator.world.Receive<List<int>>(i, 3);
                result.AddRange(partialResult);
            }

            Console.WriteLine(String.Concat(result));


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
