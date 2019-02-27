namespace PerfAnalyzers.Benchmarks
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Collections.ObjectModel;
    using System.Linq;
    using BenchmarkDotNet.Attributes;

    [Config(typeof(DefaultConfig))]
    public class ForLoop
    {
        private const int Iterations = 10000;

        private static int[] _array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private static List<int> _list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private static IList<int> _iList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private static IEnumerable<int> _iEnumerable = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private static HashSet<int> _hashSet = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private static IReadOnlyList<int> _iReadonlyList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private static ImmutableList<int> _immutableList = ImmutableList.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        private static ImmutableArray<int> _immutableArray = ImmutableArray.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        private static ImmutableHashSet<int> _immutableHashSet = ImmutableHashSet.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        private static ReadOnlyCollection<int> _readonlyCollection = new ReadOnlyCollection<int>(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });

        private static Dictionary<int, int> _dictionary = new Dictionary<int, int>
        {
            [1] = 1,
            [2] = 2,
            [3] = 2,
            [4] = 2,
            [5] = 2,
            [6] = 2,
            [7] = 2,
            [8] = 2,
            [9] = 2,
            [10] = 2,
            [11] = 2,
            [12] = 2,
        };

        [Benchmark(OperationsPerInvoke = Iterations, Baseline = true)]
        public int ArrayForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _array)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int ListForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _list)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int IListForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _iList)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int IEnumerableForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _iEnumerable)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int HashSetForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _hashSet)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int IReadonlyListForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _iReadonlyList)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int ImmutableListForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _immutableList)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int ImmutableArrayForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _immutableArray)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int ImmutableHashSetForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _immutableHashSet)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int ReadonlyCollectionForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _readonlyCollection)
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int DictionaryForeach()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (KeyValuePair<int, int> e in _dictionary)
                {
                    res += e.Value;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int IDictionaryForeach()
        {
            int res = 0;
            IDictionary<int, int> iDictionary = _dictionary;
            for (int i = 0; i < Iterations; i++)
            {
                foreach (KeyValuePair<int, int> e in iDictionary)
                {
                    res += e.Value;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int ArrayForeachWhere()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _array.Where(x => x % 2 == 1))
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int ListForeachWhere()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _list.Where(x => x % 2 == 1))
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int IListForeachWhere()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _iList.Where(x => x % 2 == 1))
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int IEnumerableForeachWhere()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _iEnumerable.Where(x => x % 2 == 1))
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int HashSetForeachWhere()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _hashSet.Where(x => x % 2 == 1))
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int IReadonlyListForeachWhere()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _iReadonlyList.Where(x => x % 2 == 1))
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int ImmutableListForEachWhere()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _immutableList.Where(x => x % 2 == 1))
                {
                    res += e;
                }
            }

            return res;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public int ImmutableArrayForeachWhere()
        {
            int res = 0;

            for (int i = 0; i < Iterations; i++)
            {
                foreach (int e in _immutableArray.Where(x => x % 2 == 1))
                {
                    res += e;
                }
            }

            return res;
        }
    }
}