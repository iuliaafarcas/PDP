using MPI;
using System;
using System.Collections.Generic;

namespace MPI_hamiltonian
{
    class Program
    {
        static bool satisfyCondition(List<int> list_, List<List<int>> edges)
        {
            for (int i = 0; i < list_.Count - 1; i++)
            {
                bool found = false;
                for (int j = 0; j < edges.Count; j++)
                {
                    if (edges[j][0] == list_[0] && edges[j][1] == list_[1])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }
            return true;
        }
       static public List<int> generatePermutations(List<int> partialPermutation, List<int> n, List<List<int>> edges)
        {
            for (int i = 0; i < n.Count; i++)
            {
                if (!partialPermutation.Contains(n[i]))
                {
                    partialPermutation.Add(n[i]);
                    if (partialPermutation.Count == n.Count)
                    {
                        if (satisfyCondition(partialPermutation, edges))
                        {
                            return partialPermutation.GetRange(0, partialPermutation.Count);
                        }
                    }
                    else
                    {
                        List<int> possibleResult = generatePermutations(partialPermutation,n,edges);
                        if (possibleResult.Count > 0)
                        {
                            return possibleResult;
                        }
                    }
                    partialPermutation.RemoveAt(partialPermutation.Count - 1);
                }
            }
            return new List<int>();
        }
        static void worker()
        {
            int start, end, givenVertex;
            List<int> vertices;
            List<List<int>> edges;

            vertices = Communicator.world.Receive<List<int>>(0, 0);
            edges = Communicator.world.Receive<List<List<int>>>(0, 1);
            start = Communicator.world.Receive<int>(0, 2);
            end = Communicator.world.Receive<int>(0, 3);
            givenVertex = Communicator.world.Receive<int>(0, 4);

            List<int> result = new List<int>();

            for (int i = start; i < end; i++)
            {
                
                List<int> currentPermutation = new List<int>();
                currentPermutation.Add(givenVertex);
                currentPermutation.Add(vertices[i]);

                List<int> possibleResult = generatePermutations(currentPermutation, vertices, edges);
                if(possibleResult.Count>0 && result.Count == 0)
                {
                    result.AddRange(possibleResult);
                }

            }

            Communicator.world.Send<List<int>>(result, 0, 5);
        }
        static void controller()
        {
            int n = 5;
            int givenVertex = 0;
            List<List<int>> edges = new List<List<int>>();
            List<int> vertices = new List<int>();
            List<int> result = new List<int>();

            for (int i = 0; i < n; i++)
            {
                vertices.Add(i);
            }
            edges.Add(new List<int> { 0, 1 });
            edges.Add(new List<int> { 1, 2 });
            edges.Add(new List<int> { 2, 3 });
            edges.Add(new List<int> { 3, 4 });
            edges.Add(new List<int> { 4, 0 });
            edges.Add(new List<int> { 1, 3 });
            edges.Add(new List<int> { 2, 4 });

            
            int startsPerProcess = (n + Communicator.world.Size - 2) / (Communicator.world.Size - 1);

            for (int i = 1; i < Communicator.world.Size; i++)
            {
                int start = (i-1) * startsPerProcess;
                int end = start + startsPerProcess;
                if (end > n)
                    end = n;
                
                Communicator.world.Send<List<int>>(vertices, i, 0);
                Communicator.world.Send<List<List<int>>>(edges, i, 1);
                Communicator.world.Send<int>(start, i, 2);
                Communicator.world.Send<int>(end, i, 3);
                Communicator.world.Send<int>(givenVertex, i, 4);

            }

            for (int i = 1; i < Communicator.world.Size; i++)
            {
                List<int> partialResult = Communicator.world.Receive<List<int>>(i, 5);

                if (result.Count == 0 && partialResult.Count > 0)
                {
                    result.AddRange(partialResult);
                }
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
