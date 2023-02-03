using MPI;
using System;
using System.Collections.Generic;

namespace MPI_permutation_with_property
{


    class Program
    {
        static bool satisfyCondition(List<int> multime)
        {
            if (multime[multime.Count - 1] == 2) return true;
            return false;
        }
        
        
        static int countPermutations(List<int> currentPermutation, List<int> multime)
        {
            int count = 0;
            for (int i = 0; i < multime.Count; i++)
            {
                if (!currentPermutation.Contains(multime[i]))
                {
                    currentPermutation.Add(multime[i]);
                    if (currentPermutation.Count == multime.Count)
                    {
                        if (satisfyCondition(currentPermutation)) { count += 1; }
                    } else
                    {
                        count+= countPermutations(currentPermutation, multime);
                    }
                    currentPermutation.RemoveAt(currentPermutation.Count - 1);
                }
            }
            return count;
        }
        static void worker()
        {
            int start, end;
            List<int> multime;

            multime = Communicator.world.Receive<List<int>>(0, 0);
            start = Communicator.world.Receive<int>(0, 1);
            end = Communicator.world.Receive<int>(0, 2);

            int count=0;
            for (int i = start; i < end; i++)
            {
                List<int> currentPermutation = new List<int>();
                currentPermutation.Add(multime[i]);
                int partialCount= countPermutations(currentPermutation, multime);
                count += partialCount;
            }

            Communicator.world.Send<int>(count, 0, 3);
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

            int count = 0;
            for (int i = 1; i < Communicator.world.Size; i++)
            {
                int partialCount = Communicator.world.Receive<int>(i, 3);
                count += partialCount;

            }

            Console.WriteLine(count);
            

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
