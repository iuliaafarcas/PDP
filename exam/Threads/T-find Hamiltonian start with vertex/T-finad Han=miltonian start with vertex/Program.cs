using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace T_finad_Han_miltonian_start_with_vertex
{
    class Permutari
    {
        private List<int> n;
        private int noThreads;
        private int currentThread;
        private List<int> list_;
        private int startingSymbol;
        private List<int> secondSymbols;
        private List<List<int>> edges;

        public Permutari(List<int> n, int noThreads, int currentThread, List<int> list_, int startingSymbol, List<int> secondSymbols, List<List<int>> edges)
        {
            this.n = n;
            this.noThreads = noThreads;
            this.currentThread = currentThread;
            this.list_ = list_;
            this.startingSymbol = startingSymbol;
            this.secondSymbols = secondSymbols;
            this.edges = edges;
        }

        public bool satisfyCondition(List<int> list_)
        {
          for(int i = 0; i < list_.Count - 1; i++)
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

        public List<int> generatePermutations(List<int> partialPermutation)
        {
            for (int i = 0; i < this.n.Count; i++)
            {
                if (!partialPermutation.Contains(this.n[i]))
                {
                    partialPermutation.Add(n[i]);
                    if (partialPermutation.Count == n.Count)
                    {
                        if (satisfyCondition(partialPermutation))
                        {
                            return partialPermutation;
                        }
                    }
                    else
                    {
                        List<int> possibleResult = generatePermutations(partialPermutation);
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

        public List<int> findPermutation()
        {
            for (int i = 0; i < this.secondSymbols.Count; i++)
            {
                List<int> permutationList = new List<int>();
                permutationList.Add(this.startingSymbol);
                permutationList.Add(this.secondSymbols[i]);

                List<int> permutation = generatePermutations(permutationList);
                if (permutation.Count > 0)
                {
                    return permutation;
                }
            }
            return new List<int>();
        }


    }
    class Program
    {
        static void Main(string[] args)
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

            List<Task<List<int>>> taskList = new List<Task<List<int>>>();
            int noThreads = 3;
            int digitsPerTask = n / noThreads;
            for (int i = 0; i < noThreads; i++)
            {
                int start = i * digitsPerTask;
                int end = i == noThreads - 1 ? n : start + digitsPerTask;
                List<int> secondDigits = new List<int>();

                for (int j = start; j < end; j++)
                {
                    secondDigits.Add(j);
                }

                Permutari permutari = new Permutari(vertices, noThreads, i, result, givenVertex, secondDigits, edges);

                Task<List<int>> task = Task.Factory.StartNew(() => permutari.findPermutation());
                taskList.Add(task);
            }

            for (int i = 0; i < noThreads; i++)
            {
                if ( result.Count == 0 && taskList[i].Result.Count > 0)
                {
                    result.AddRange(taskList[i].Result);
                }
            }

            Console.WriteLine(String.Concat(result));
        }
    }
}
