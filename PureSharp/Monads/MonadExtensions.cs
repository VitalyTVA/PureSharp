﻿using System;

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
