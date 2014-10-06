using System;
using PureSharp.MayBeMonad2;
using PureSharp.MayBeMonad;

namespace PureSharp.LazyMonad {
    partial class LazyExtensions {
        public static Lazy<C> SelectMany<A, B, C>(this Lazy<A> source, Func<A, Lazy<B>> f, Func<A, B, C> resultSelector) {
            return source.SelectMany(
                outer => f(outer).SelectMany(
                inner => resultSelector(outer, inner).Unit<C>()));
        }
        public static Lazy<B> Select<A, B>(this Lazy<A> source, Func<A, B> f) {
            return source.SelectMany(x => f(x).Unit<B>());
        }
    }
}
namespace PureSharp.MayBeMonad {
    partial class MayBeExtensions {
        public static C SelectMany<A, B, C>(this A source, Func<A, B> f, Func<A, B, C> resultSelector) {
            return source.SelectMany(
                outer => f(outer).SelectMany(
                inner => resultSelector(outer, inner).Unit<C>()));
        }
        public static B Select<A, B>(this A source, Func<A, B> f) {
            return source.SelectMany(x => f(x).Unit<B>());
        }
        public static A Where<A>(this A source, Func<A, bool> f) {
            return source.SelectMany(x => f(x) ? x.Unit<A>() : Empty<A>());
        }
    }
}
namespace PureSharp.MayBeMonad2 {
    partial class MayBe2Extensions {
        public static MayBe<C> SelectMany<A, B, C>(this MayBe<A> source, Func<A, MayBe<B>> f, Func<A, B, C> resultSelector) {
            return source.SelectMany(
                outer => f(outer).SelectMany(
                inner => resultSelector(outer, inner).Unit<C>()));
        }
        public static MayBe<B> Select<A, B>(this MayBe<A> source, Func<A, B> f) {
            return source.SelectMany(x => f(x).Unit<B>());
        }
        public static MayBe<A> Where<A>(this MayBe<A> source, Func<A, bool> f) {
            return source.SelectMany(x => f(x) ? x.Unit<A>() : Empty<A>());
        }
    }
}
namespace PureSharp.ReaderMonad {
    partial class ReaderExtensions {
        public static Func<E, C> SelectMany<E, A, B, C>(this Func<E, A> source, Func<A, Func<E, B>> f, Func<A, B, C> resultSelector) {
            return source.SelectMany(
                outer => f(outer).SelectMany(
                inner => resultSelector(outer, inner).Unit<E, C>()));
        }
        public static Func<E, B> Select<E, A, B>(this Func<E, A> source, Func<A, B> f) {
            return source.SelectMany(x => f(x).Unit<E, B>());
        }
    }
}
namespace PureSharp.WriterMonad {
    partial class WriterExtensions {
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
namespace PureSharp.LazyMayBeMonad2 {
    partial class LazyMayBe2Extensions {
        public static LazyMayBe<C> SelectMany<A, B, C>(this LazyMayBe<A> source, Func<A, LazyMayBe<B>> f, Func<A, B, C> resultSelector) {
            return source.SelectMany(
                outer => f(outer).SelectMany(
                inner => resultSelector(outer, inner).Unit<C>()));
        }
        public static LazyMayBe<B> Select<A, B>(this LazyMayBe<A> source, Func<A, B> f) {
            return source.SelectMany(x => f(x).Unit<B>());
        }
        public static LazyMayBe<A> Where<A>(this LazyMayBe<A> source, Func<A, bool> f) {
            return source.SelectMany(x => f(x) ? x.Unit<A>() : Empty<A>());
        }
    }
}
namespace PureSharp.LazyMayBeMonad2 {
    public struct LazyMayBe<A>(Lazy<MayBe<A>> value) {
        public readonly Lazy<MayBe<A>> Value = value;
    }
    public static partial class LazyMayBe2Extensions {
        public static LazyMayBe<A> AsLazyMayBe<A>(this Lazy<MayBe<A>> source) {
            return new LazyMayBe<A>(source);
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this A source) {
            return source.Unit<A>();
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this MayBe<A> source) {
            return LazyMonad.LazyExtensions.Unit<MayBe<A>>(source).AsLazyMayBe();
        }

        static LazyMayBe<A> Unit<A>(this A source) {
            return LazyMonad.LazyExtensions.Unit<MayBe<A>>(source.AsMayBe()).AsLazyMayBe();
        }
        static LazyMayBe<A> Empty<A>() {
            return LazyMonad.LazyExtensions.Unit<MayBe<A>>(MayBe2Extensions.Empty<A>()).AsLazyMayBe();
        }
        static LazyMayBe<B> SelectMany<A, B>(this LazyMayBe<A> source, Func<A, LazyMayBe<B>> f) {
            return LazyMonad.LazyExtensions.SelectMany<MayBe<A>, MayBe<B>>(
                source.Value,
                x => (x.Value != null ? f(x.Value).Value : LazyMonad.LazyExtensions.Unit<MayBe<B>>(MayBe2Extensions.Empty<B>()))
            ).AsLazyMayBe();
        }
    }
}
namespace PureSharp.LazyMayBeMonad {
    partial class LazyMayBeExtensions {
        public static LazyMayBe<C> SelectMany<A, B, C>(this LazyMayBe<A> source, Func<A, LazyMayBe<B>> f, Func<A, B, C> resultSelector) {
            return source.SelectMany(
                outer => f(outer).SelectMany(
                inner => resultSelector(outer, inner).Unit<C>()));
        }
        public static LazyMayBe<B> Select<A, B>(this LazyMayBe<A> source, Func<A, B> f) {
            return source.SelectMany(x => f(x).Unit<B>());
        }
        public static LazyMayBe<A> Where<A>(this LazyMayBe<A> source, Func<A, bool> f) {
            return source.SelectMany(x => f(x) ? x.Unit<A>() : Empty<A>());
        }
    }
}
namespace PureSharp.LazyMayBeMonad {
    public struct LazyMayBe<A>(Lazy<A> value) {
        public readonly Lazy<A> Value = value;
    }
    public static partial class LazyMayBeExtensions {
        public static LazyMayBe<A> AsLazyMayBe<A>(this Lazy<A> source) {
            return new LazyMayBe<A>(source);
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this A source) {
            return source.Unit();
        }

        static LazyMayBe<A> Unit<A>(this A source) {
            return LazyMonad.LazyExtensions.Unit(source).AsLazyMayBe();
        }
        static LazyMayBe<A> Empty<A>() {
            return LazyMonad.LazyExtensions.Unit(MayBeExtensions.Empty<A>()).AsLazyMayBe();
        }
        static LazyMayBe<B> SelectMany<A, B>(this LazyMayBe<A> source, Func<A, LazyMayBe<B>> f) {
            return LazyMonad.LazyExtensions.SelectMany<A, B>(
                source.Value,
                x => (x != null ? f(x).Value : LazyMonad.LazyExtensions.Unit(MayBeExtensions.Empty<B>()))
            ).AsLazyMayBe();
        }
    }
}

