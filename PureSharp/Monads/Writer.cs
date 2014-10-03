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

        //public static MayBe<A> AsMayBe<A>(this A source) {
        //    return source.Unit();
        //}
        //static MayBe<A> Unit<A>(this A source) {
        //    return new MayBe<A>(source);
        //}
        //public static MayBe<A> Empty<A>() {
        //    return default(A).Unit();
        //}
        //public static MayBe<B> SelectMany<A, B>(this MayBe<A> source, Func<A, MayBe<B>> f) {
        //    if(source.Value == null)
        //        return Empty<B>();
        //    return f(source.Value);
        //}
    }
}
