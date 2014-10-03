using System;

namespace PureSharp {
    public class Monoid<W>(W @null, Func<W, W, W> sum) {
        public readonly W Null = @null;
        public readonly Func<W, W, W> Sum = sum;
    }
    public static partial class Monoid {
        public static Monoid<W> CreateMonoid<W>(W @null, Func<W, W, W> sum) {
            return new Monoid<W>(@null, sum);
        }
    }
    public sealed class Zero {
        public static readonly Zero Instance = new Zero();
        Zero() { }
    }
}