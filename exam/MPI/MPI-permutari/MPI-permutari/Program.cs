using MPI;
using System;
using System.Collections.Generic;

namespace MPI_permutari
{
    class Program
    {
        
        static List<List<int>> generatePermutations(List<int> currentPermutation, List<int> multime)
        {
            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < multime.Count; i++)
            {
                if (!currentPermutation.Contains(multime[i]))
                {
                    currentPermutation.Add(multime[i]);
                    if (currentPermutation.Count == multime.Count)
                    {
                        result.Add(currentPermutation.GetRange(0, currentPermutation.Count));
                    } else
                    {
                        result.AddRange(generatePermutations(currentPermutation, multime));
                    }
                    currentPermutation.RemoveAt(currentPermutation.Count - 1);
                }
            }
            return result;
        }

        static void worker()
        {
            int start, end;
            List<int> multime;

            multime = Communicator.world.Receive<List<int>>(0, 0);
            start = Communicator.world.Receive<int>(0, 1);
            end = Communicator.world.Receive<int>(0, 2);

            List<List<int>> result = new List<List<int>>();
            for (int i = start; i < end; i++)
            {
                List<int> currentPermutation = new List<int>();
                currentPermutation.Add(multime[i]);

                result.AddRange(generatePermutations(currentPermutation, multime));
            }

            Communicator.world.Send<List<List<int>>>(result, 0, 3);
        }

        static void controller()
        {
            int n = 5;
            List<int> multime = new List<int>();
            for (int i = 0; i < n; i++)
            {
                multime.Add(i);
            }

            int startsPerProcess = (multime.Count + Communicator.world.Size - 2) / (Communicator.world.Size - 1);
            
            for (int i = 1; i < Communicator.world.Size; i++)
            {
                int start = (i - 1) * startsPerProcess;
                int end = start + startsPerProcess;
                if (end > multime.Count)
                    end = multime.Count;
                Communicator.world.Send<List<int>>(multime, i, 0);
                Communicator.world.Send<int>(start, i, 1);
                Communicator.world.Send<int>(end, i, 2);
            }

            List<List<int>> permutations = new List<List<int>>();
            for (int i = 1; i < Communicator.world.Size; i++)
            {
                List<List<int>> partialResult = Communicator.world.Receive<List<List<int>>>(i, 3);
                permutations.AddRange(partialResult);
            }

            for(int i=0;i<permutations.Count;i++)
            {
                Console.WriteLine(String.Concat(permutations[i]));
            }

        }

        static void Main(string[] args)
        {

            using (new MPI.Environment(ref args))
            {
                if (Communicator.world.Rank == 0)
                {
                    controller();
                } else
                {
                    worker();
                }


            }
        }
    }
}
