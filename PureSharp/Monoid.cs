using System;

namespace PureSharp {
    public struct Monoid<W> {
        public readonly W Null;
        public readonly Func<W, W, W> Sum;
        public Monoid(W @null, Func<W, W, W> sum) {
            Null = @null;
            Sum = sum;
        }
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