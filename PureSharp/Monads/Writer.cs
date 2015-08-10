using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSharp.WriterMonad {
    public struct Writer<W, A> {
        public readonly W Log;
        public readonly A Value;
        public readonly Monoid<W> Monoid;
        public Writer(A value, W log, Monoid<W> monoid) {
            Log = log;
            Value = value;
            Monoid = monoid;
        }
    }
    public static partial class WriterExtensions {
        public static Writer<W, A> Append<W, A>(this Writer<W, A> source, W log) {
            return source.Value.AsWriter(source.Monoid.Sum(source.Log, log), source.Monoid);
        }
        public static Writer<W, Zero> Tell<W>(W log, Monoid<W> monoid) {
            return Zero.Instance.AsWriter(log, monoid);
        }
        public static Writer<W, A> AsWriter<W, A>(this A source, Monoid<W> monoid) {
            return source.Unit(monoid);
        }
        public static Writer<W, A> AsWriter<W, A>(this A source, W log, Monoid<W> monoid) {
            return new Writer<W, A>(source, log, monoid);
        }

        static Writer<W, A> Unit<W, A>(this A source, Monoid<W> monoid) {
            return source.AsWriter(monoid.Null, monoid);
        }
        public static Writer<W, B> SelectMany<W, A, B>(this Writer<W, A> source, Func<A, Writer<W, B>> f) {
            var result = f(source.Value);
            return result.Value.AsWriter(source.Monoid.Sum(source.Log, result.Log), source.Monoid);
        }
    }
}
