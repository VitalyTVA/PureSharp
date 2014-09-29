using System;

namespace PureSharp {
    public static class F {
        public static Func<T, T> Id<T>() {
            return x => x;
        }
        public static Func<T1, TResult> Pipe<T1, T2, TResult>(this Func<T1, T2> f1, Func<T2, TResult> f2) {
            return (T1 arg1) => f2(f1(arg1));
        }
        public static Func<T1, T2, TResult> Pipe<T1, T2, T3, TResult>(this Func<T1, T2, T3> f1, Func<T3, TResult> f2) {
            return (T1 arg1, T2 arg2) => f2(f1(arg1, arg2));
        }
    }
    public static class LazyExtensions {
        public static Lazy<T> Lazy<T>(Func<T> func) {
            return new Lazy<T>(func);
        }
        //public static Lazy<T> Lazy<T>(T val) {
        //    return new Lazy<T>(() => val);
        //}
    }
    public static class Int32Functions {
        public static readonly Func<int, int, int> Sum = (x, y) => x + y;
    }
}
