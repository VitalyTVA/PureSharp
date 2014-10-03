using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSharp.WriterMonad {
    public struct Writer<W, A>(A value, W log, Monoid<W> monoid) {
        public readonly W Log = log;
        public readonly A Value = value;
        public readonly Monoid<W> Monoid = monoid;
    }
    public static class WriterExtensions {
        public static Writer<W, Zero> Apppend<W, A>(this Writer<W, Zero> source, W log) {
            return source.Value.AsWriter(source.Monoid.Sum(source.Log, log), source.Monoid);
        }
        public static Writer<W, Zero> Tell<W, A>(W log, Monoid<W> monoid) {
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


        //should be auto generated
        public static Writer<W, C> SelectMany<W, A, B, C>(this Writer<W, A> source, Func<A, Writer<W, B>> f, Func<A, B, C> resultSelector) {
            return source.SelectMany(
                outer => f(outer).SelectMany(
                inner => resultSelector(outer, inner).Unit<W, C>(source.Monoid)));
        }
        public static Writer<W, B> Select<W, A, B>(this Writer<W, A> source, Func<A, B> f) {
            return source.SelectMany(x => f(x).Unit<W, B>(source.Monoid));
        }
    }
}
