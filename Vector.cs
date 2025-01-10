namespace Vector
{
    public class Vector
    {

        public double[] array;
        public Vector(double[] array)
        {
            this.array = array;
        }


        public int Length
        {
            get
            {
                return array.Length;
            }
        }

        public double this[int index]
        {
            get
            {
                if (index < Length)
                {
                    return array[index];
                }
                else
                {
                    throw new System.ArgumentException("Index out of range");
                }
            }
            set
            {
                if (index < Length)
                {
                    array[index] = value;
                }
                else
                {
                    throw new System.ArgumentException("Index out of range");
                }
            }
        }

        public Vector this[bool[] index,int T=1]
        {
            get
            {
                List<double> result = [];
                if (T > 1 || Length >= 1000)
                {
                    T = T == 1 ? Environment.ProcessorCount : T;
                    Action<int, int> func = (t, T) =>
                    {
                        for (int i = t; i < Length; i += T)
                        {
                            if (index[i])
                            {
                                result.Add(array[i]);
                            }
                        }
                    };
                    List<Thread> threads = new();
                    for (int t = 0; t < T; t++)
                    {
                        Thread thread = new(() => func(t, T));
                        thread.Start();
                        threads.Add(thread);
                    }
                    Parallel.ForEach(threads, thread => thread.Join());
                }
                else
                {
                    for (int i = 0; i < Length; i++)
                    {
                        if (index[i])
                        {
                            result.Add(array[i]);
                        }
                    }
                }
                return new Vector(result);
            }
        }

        public Vector this[int start, int end, int T = 1]
        {
            get
            {
                List<double> result = [];
                if (T > 1 || end-start>=1000)
                {
                    T = T == 1 ? Environment.ProcessorCount : T;
                    Action<int, int> func = (t, T) =>
                    {
                        for (int i = t; i < end; i += T)
                        {
                            if (i < Length)
                            {
                                result.Add(array[i]);
                            }
                            else
                            {
                                throw new System.ArgumentException("Index out of range");
                            }
                        }
                    };
                    List<Thread> threads = new();
                    for (int t = 0; t < T; t++)
                    {
                        Thread thread = new(() => func(t, T));
                        thread.Start();
                        threads.Add(thread);
                    }
                    Parallel.ForEach(threads, thread => thread.Join());
                }
                else
                {
                    for (int i = start; i < end; i++)
                    {
                        if (i < Length)
                        {
                            result.Add(array[i]);
                        }
                        else
                        {
                            throw new System.ArgumentException("Index out of range");
                        }
                    }
                }
                return new Vector(result);
            }

            set
            {
                if (value.Length > start + end && value.Length<=array.Length)
                {
                    if (T > 1)
                    {
                        Action<int, int> func = (t, T) =>
                        {
                            for (int i = t + start; i < end; i += T)
                            {
                                if (i < Length)
                                {
                                    array[i] = value[i - start];
                                }
                                else
                                {
                                    throw new System.ArgumentException("Index out of range");
                                }
                            }
                        };
                        List<Thread> threads = new();
                        for (int t = 0; t < T; t++)
                        {
                            Thread thread = new(() => func(t, T));
                            thread.Start();
                            threads.Add(thread);
                        }
                        Parallel.ForEach(threads, thread => thread.Join());
                    }
                    else
                    {
                        for (int i = start; i < end; i++)
                        {
                            if (i < Length)
                            {
                                array[i] = value[i - start];
                            }
                            else
                            {
                                throw new System.ArgumentException("Index out of range");
                            }
                        }
                    }
                }
                else
                {
                    throw new System.ArgumentException("Value must have the same length as the range");
                }
            }
        }

        public Vector this[int[] index, int T = 1]
        {
            get
            {
                List<double> result = [];
                if (T > 1)
                {
                    Action<int, int> func = (t, T) =>
                    {
                        for (int i = t; i < index.Length; i += T)
                        {
                            if (index[i] < Length)
                            {
                                result.Add(array[index[i]]);
                            }
                            else
                            {
                                throw new System.ArgumentException("Index out of range");
                            }
                        }
                    };
                    List<Thread> threads = new();
                    for (int t = 0; t < T; t++)
                    {
                        Thread thread = new(() => func(t, T));
                        thread.Start();
                        threads.Add(thread);
                    }
                    Parallel.ForEach(threads, thread => thread.Join());
                }
                else
                {
                    for (int i = 0; i < index.Length; i++)
                    {
                        if (index[i] < Length)
                        {
                            result.Add(array[index[i]]);
                        }
                        else
                        {
                            throw new System.ArgumentException("Index out of range");
                        }
                    }
                }
                return new Vector(result);
            }
        }


        public static Vector operator +(Vector v1, double scalar)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] + scalar;
            }
            return new Vector(result);
        }

        public static Vector operator +(double scalar, Vector v1)
        {
            return v1 + scalar;
        }

        public static Vector operator -(Vector v1, double scalar)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] - scalar;
            }
            return new Vector(result);
        }

        public static Vector operator -(double scalar, Vector v1)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = scalar - v1[i];
            }
            return new Vector(result);
        }

        public static Vector Ones(int n,int T=1)
        {
            double[] result = new double[n];
            if (T>1 || n>=1000)
            {
                T = T == 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < n; i += T)
                    {
                        result[i] = 1;
                    }
                };
                List<Thread> threads = new List<Thread>();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new Thread(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = 1;
                }
            }
            return new Vector(result);
        }

        public static Vector Zeros(int n)
        {
            double[] result = new double[n];
            return new Vector(result);
        }

        public Vector Copy()
        {
            return new Vector(array);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
            {
                throw new System.ArgumentException("Vectors must have the same length");
            }
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] + v2[i];
            }
            return new Vector(result);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
            {
                throw new System.ArgumentException("Vectors must have the same length");
            }
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] - v2[i];
            }
            return new Vector(result);
        }

        public static Vector operator *(Vector v1, double scalar)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] * scalar;
            }
            return new Vector(result);
        }

        public Vector(List<double> list)
        {
            array = [.. list];
        }

        public static Vector operator *(double scalar, Vector v1)
        {
            return v1 * scalar;
        }

        public static double operator *(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
            {
                throw new System.ArgumentException("Vectors must have the same length");
            }
            double result = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                result += v1[i] * v2[i];
            }
            return result;
        }

        public static Vector operator /(Vector v1, double scalar)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] / scalar;
            }
            return new Vector(result);
        }

        public static bool[] operator >(Vector v1, double scalar)
        {
            bool[] result = new bool[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] > scalar;
            }
            return result;
        }

        public static bool[] operator <(Vector v1, double scalar)
        {
            bool[] result = new bool[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] < scalar;
            }
            return result;
        }

        public static bool[] operator >=(Vector v1, double scalar)
        {
            bool[] result = new bool[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] >= scalar;
            }
            return result;
        }

        public static bool[] operator <=(Vector v1, double scalar)
        {
            bool[] result = new bool[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] <= scalar;
            }
            return result;
        }

        public static Vector operator /(double scalar, Vector v1)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = scalar / v1[i];
            }
            return new Vector(result);
        }

        public static Vector operator -(Vector v1)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = -v1[i];
            }
            return new Vector(result);
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
            {
                return false;
            }
            for (int i = 0; i < v1.Length; i++)
            {
                if (v1[i] != v2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this == (Vector)obj;
        }

        public override int GetHashCode()
        {
            return array.GetHashCode();
        }

        public override string ToString()
        {
            string result = "[";
            for (int i = 0; i < Length; i++)
            {
                result += array[i];
                if (i != Length - 1)
                {
                    result += ", ";
                }
            }
            result.Remove(result.Length - 1,1);
            result += "]";
            return result;
        }

        public double Norm(int T=1)
        {
            Mutex m = new();
            double result = 0;
            if (T > 1 || Length >= 1000)
            {
                T = T == 1 ? Environment.ProcessorCount : T;
                Action<int,int> func = (t,T) => {
                    for (int i = t; i < Length; i+=T)
                    {
                        m.WaitOne();
                        result += array[i] * array[i];
                        m.ReleaseMutex();
                    }
                };
                List<Thread> threads = new List<Thread>();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new Thread(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    result += array[i] * array[i];
                }
            }
            return System.Math.Sqrt(result);
        }

        public Vector Normalize(int T=1)
        {
            return this / Norm(T);
        }

        public static double Dot(Vector v1, Vector v2)
        {
            return v1 * v2;
        }

        public Vector ArgWhere(bool[] index,int T=1)
        {
            List<double> result = new List<double>();
            if (T > 1 || Length >= 1000)
            {
                T = T == 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < Length; i += T)
                    {
                        if (index[i])
                        {
                            result.Add(i);
                        }
                    }
                };
                List<Thread> threads = new();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    if (index[i])
                    {
                        result.Add(i);
                    }
                }
            }
            return new Vector(result);
        }

        public Vector Unique() { return new Vector(array.Distinct().ToArray()); }

        public Vector Sort() { return new Vector(array.OrderBy(x => x).ToArray()); }

        public Vector Reverse() { return new Vector(array.Reverse().ToArray()); }

        public Vector Shuffle()
        {
            Random random = new();
            return new Vector(array.OrderBy(x => random.Next()%Length).ToArray());
        }

        public Vector Append(Vector v1)
        {
            return new Vector(array.Concat(v1.array).ToArray());
        }
        public Vector Append(double scalar)
        {
            return new Vector(array.Concat([scalar]).ToArray());
        }

        public Vector Insert(int index, double scalar)
        {
            List<double> result = [];
            for (int i = 0; i < index; i++)
            {
                result.Add(array[i]);
            }
            result.Add(scalar);
            for (int i = index; i < Length; i++)
            {
                result.Add(array[i]);
            }
            return new Vector(result);
        }

        public Vector Insert(int index, Vector v1)
        {
            List<double> result = [];
            for (int i = 0; i < index; i++)
            {
                result.Add(array[i]);
            }
            result.AddRange(v1.array);
            for (int i = index; i < Length; i++)
            {
                result.Add(array[i]);
            }
            return new Vector(result);
        }

        public Vector Remove(int index)
        {
            List<double> result = [];
            for (int i = 0; i < index; i++)
            {
                result.Add(array[i]);
            }
            for (int i = index + 1; i < Length; i++)
            {
                result.Add(array[i]);
            }
            return new Vector(result);
        }

        public Vector Remove(Vector v1)
        {
            List<double> result = [];
            for (int i = 0; i < Length; i++)
            {
                if (!v1.array.Contains(array[i]))
                {
                    result.Add(array[i]);
                }
            }
            return new Vector(result);
        }

        public double Sum(int T = 1)
        {
            Mutex m = new();
            double result = 0;
            if (T > 1 || Length>=1000)
            {
                T= T == 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < Length; i += T)
                    {
                        m.WaitOne();
                        result += array[i];
                        m.ReleaseMutex();
                    }
                };
                List<Thread> threads = new List<Thread>();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new Thread(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    result += array[i];
                }
            }
            return result;
        }

        public double Mean(int T = 1)
        {
            return Sum(T) / Length;
        }

        public double Min(int T = 1)
        {
            Mutex m = new();
            double result = double.MaxValue;
            if (T > 1)
            {
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < Length; i += T)
                    {
                        m.WaitOne();
                        if (array[i] < result)
                        {
                            result = array[i];
                        }
                        m.ReleaseMutex();
                    }
                };
                List<Thread> threads = new List<Thread>();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new Thread(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    if (array[i] < result)
                    {
                        result = array[i];
                    }
                }
            }
            return result;
        }

        public double Max(int T = 1)
        {
            Mutex m = new();
            double result = double.MinValue;
            if (T > 1)
            {
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < Length; i += T)
                    {
                        m.WaitOne();
                        if (array[i] > result)
                        {
                            result = array[i];
                        }
                        m.ReleaseMutex();
                    }
                };
                List<Thread> threads = new List<Thread>();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new Thread(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    if (array[i] > result)
                    {
                        result = array[i];
                    }
                }
            }
            return result;
        }

        public double Median()
        {
            double[] sorted = [.. array.OrderBy(x => x)];
            if (Length % 2 == 0)
            {
                return (sorted[Length / 2] + sorted[Length / 2 - 1]) / 2;
            }
            else
            {
                return sorted[Length / 2];
            }
        }

        public Vector Pow(double scalar, int T = 1)
        {
            double[] result = new double[Length];
            if (T > 1 || Length>=1000)
            {
                T = T == 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < Length; i += T)
                    {
                        result[i] = System.Math.Pow(array[i], scalar);
                    }
                };
                List<Thread> threads = new List<Thread>();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new Thread(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    result[i] = System.Math.Pow(array[i], scalar);
                }
            }
            return new Vector(result);
        }

        public double Var(int T = 1)
        {
            return ((this - Mean(T)).Pow(2, T).Sum(T)) / Length;
        }

        public double Sd(int T = 1)
        {
            return System.Math.Sqrt(Var(T));
        }

        public double Cov(Vector v1, int T = 1)
        {
            if (Length != v1.Length)
            {
                throw new System.ArgumentException("Vectors must have the same length");
            }
            return (this-Mean(T))*(v1-v1.Mean(T))/Length-1;
        }

        public double Corr(Vector v1, int T = 1)
        {
            return Cov(v1, T) / (Sd(T) * v1.Sd(T));
        }

        public Vector Apply(Func<double, double> func, int T = 1)
        {
            double[] result = new double[Length];
            if (T > 1)
            {
                Action<int, int> func1 = (t, T) =>
                {
                    for (int i = t; i < Length; i += T)
                    {
                        result[i] = func(array[i]);
                    }
                };
                List<Thread> threads = new List<Thread>();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new Thread(() => func1(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    result[i] = func(array[i]);
                }
            }
            return new Vector(result);
        }

        public Vector Apply(Func<Vector,int,Vector> func, int T = 1)
        {
            return func(this,T);
        }

        public static Vector Random(int n,int T)
        {
            Random random = new();
            double[] result = new double[n];
            if (T > 1 || n >= 1000)
            {
                T = T == 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < n; i += T)
                    {
                        result[i] = random.NextDouble();
                    }
                };
                List<Thread> threads = new List<Thread>();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new Thread(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = random.NextDouble();
                }
            }
            return new Vector(result);
        }

        public static Vector Random(int n)
        {
            return Random(n, 1);
        }

        public static Vector Random(int n, double min, double max, int T)
        {
            Random random = new();
            double[] result = new double[n];
            if (T > 1 || n >= 1000)
            {
                T= T == 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < n; i += T)
                    {
                        result[i] = random.NextDouble() * (max - min) + min;
                    }
                };
                List<Thread> threads = new List<Thread>();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new Thread(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = random.NextDouble() * (max - min) + min;
                }
            }
            return new Vector(result);
        }

        public static Vector Random(int n, double min, double max)
        {
            return Random(n, min, max, 1);
        }

        public static Vector Random(int n, double max)
        {
            return Random(n, 0, max, 1);
        }

        public static Vector Random(int n, double max, int T)
        {
            return Random(n, 0, max, T);
        }

        public static Vector Linspace(double start, double end, int n, int T)
        {
            double[] result = new double[n];
            if (T > 1 || n>=1000)
            {
                T = T == 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < n; i += T)
                    {
                        result[i] = start + i * (end - start) / (n - 1);
                    }
                };
                List<Thread> threads = [];
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = start + i * (end - start) / (n - 1);
                }
            }
            return new Vector(result);
        }

        public static Vector Linspace(double start, double end, int n)
        {
            return Linspace(start, end, n, 1);
        }

        public static Vector Arange(double start, double end, double step, int T)
        {
            int n = (int)((end - start) / step);
            double[] result = new double[n];
            if (T > 1 || n>=1000)
            {
                T = T == 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < result.Length; i += T)
                    {
                        result[i] = start + i * step;
                    }
                };
                List<Thread> threads = new();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = start + i * step;
                }
            }
            return new Vector(result);
        }

        public static Vector Arange(double start, double end, double step)
        {
            return Arange(start, end, step, 1);
        }

        public static Vector Arange(double start, double end)
        {
            return Arange(start, end, 1, 1);
        }

        public static Vector Arange(double end)
        {
            return Arange(0, end, 1, 1);
        }

        public static Vector Concat(List<Vector> vectors)
        {
            List<double> result = new List<double>();
            foreach (Vector v in vectors)
            {
                result.AddRange(v.array);
            }
            return new Vector(result);
        }

        public static Vector Concat(Vector v1, Vector v2)
        {
            return new Vector(v1.array.Concat(v2.array).ToArray());
        }

        public static Vector Concat(Vector v1, double scalar)
        {
            return new Vector(v1.array.Concat([scalar]).ToArray());
        }

        public Vector Scale(int T=1)
        {
            var sd = Sd(T);
            var mean = Mean(T);
            double[] result = new double[Length];

            if (T > 1 || Length>=1000)
            {
                T=T== 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < Length; i += T)
                    {
                        result[i] = (array[i] - mean) / sd;
                    }
                };
                List<Thread> threads = [];
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    result[i] = (array[i] - mean) / sd;
                }
            }
            return new Vector(result);
        }

        public double First()
        {
            return array[0];
        }

        public double Last() { 
            return array[Length-1];
        }

        public Vector Where(bool[] index,int T=1)
        {
            List<double> result =[];
            if (index.Length!=Length)
            {
                throw new ArgumentException("index must be the same size as the current vector");
            }
            if (T > 1 || Length >= 1000)
            {
                T = T == 1 ? Environment.ProcessorCount : T;
                Action<int, int> func = (t, T) =>
                {
                    for (int i = t; i < Length; i += T)
                    {
                        if (index[i])
                        {
                            result.Add(array[i]);
                        }
                    }
                };
                List<Thread> threads = new();
                for (int t = 0; t < T; t++)
                {
                    Thread thread = new(() => func(t, T));
                    thread.Start();
                    threads.Add(thread);
                }
                Parallel.ForEach(threads, thread => thread.Join());
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    if (index[i])
                    {
                        result.Add(array[i]);
                    }
                }
            }
            return new Vector(result);

        }
    }
}
