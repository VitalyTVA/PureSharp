using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureSharp.F;

namespace PureSharp.ReaderMonad {
    public static partial class ReaderExtensions {
        public static Func<A, A> Ask<A>() {
            return Id<A>();
        }
        public static Func<E, A> Local<E, A>(Func<E, E> f, Func<E, A> reader) {
            return f.Pipe(reader);
        }
        public static Func<E, A> AsReader<E, A>(this A source) {
            return source.Unit<E, A>();
        }

        static Func<E, A> Unit<E, A>(this A source) {
            return x => source;
        }
        static Func<E, B> SelectMany<E, A, B>(this Func<E, A> source, Func<A, Func<E, B>> f) {
            return e => f(source(e))(e);
        }
    }
}
