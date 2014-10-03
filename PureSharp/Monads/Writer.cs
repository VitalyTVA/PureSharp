using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSharp.WriterMonad {
    public struct Writer<A, W>(A value, W log, Monoid<W> monoid) {
        public readonly W Log = log;
        public readonly A Value = value;
        public readonly Monoid<W> Monoid = monoid;
    }
    public static class WriterExtensions {
        public static Writer<A, W> AsWriter<A, W>(this A source, Monoid<W> monoid) {
            return source.Unit(monoid);
        }
        static Writer<A, W> Unit<A, W>(this A source, Monoid<W> monoid) {
            return new Writer<A, W>(source, monoid.Null, monoid);
        }
        public static Writer<B, W> SelectMany<A, B, W>(this Writer<A, W> source, Func<A, Writer<B, W>> f) {
            return new Writer<B, W>(f(source.Value).Value, source.Log, source.Monoid);
        }
    }
}
