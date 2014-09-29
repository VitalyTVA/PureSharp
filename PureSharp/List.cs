using System;
using PureSharp.F;
using PureSharp.LazyExtensions;
using PureSharp.Int32Functions;
using System.Collections.Generic;

namespace PureSharp {
    public interface IList<out T> {
        T Head { get; }
        IList<T> Tail { get; }
        bool IsEmpty { get; }
    }

    public static class List {
        class EmptyList<T> : IList<T> {
            public static readonly IList<T> Instanse = new EmptyList<T>();
            EmptyList() { }
            T IList<T>.Head { get { throw new InvalidOperationException(StringResources.ListIsEmpty); } }
            IList<T> IList<T>.Tail { get { throw new InvalidOperationException(StringResources.ListIsEmpty); } }
            bool IList<T>.IsEmpty { get { return true; } }
        }

        class SimpleList<T>(T head, IList<T> tail) : IList<T> {
            public T Head { get; } = head;
            public IList<T> Tail { get; } = tail;
            public bool IsEmpty { get { return false; } }
        }

        class LazyList<T>(Lazy<T> head_, Lazy<IList<T>> tail_) : IList<T> {
            readonly Lazy<T> head = head_;
            readonly Lazy<IList<T>> tail = tail_;

            public T Head {
                get {
                    CheckIsEmpty();
                    return this.head.Value;
                }
            }
            public IList<T> Tail {
                get {
                    CheckIsEmpty();
                    return this.tail.Value;
                }
            }
            public bool IsEmpty { get { return this.tail.Value == null; } }

            public LazyList(Func<T> head, Func<IList<T>> tail)
               : this(Lazy(head), Lazy(tail)) { }

            void CheckIsEmpty() {
                if(IsEmpty)
                    throw new InvalidOperationException(StringResources.ListIsEmpty);
            }
        }

        public static IList<T> Empty<T>() {
            return EmptyList<T>.Instanse;
        }

        public static IList<T> Cons<T>(this IList<T> list, T item) {
            return new SimpleList<T>(item, list);
        }
        public static IList<T> Cons<T>(this T item, IList<T> list) {
            return list.Cons(item);
        }
        public static IList<T> ConsLazy<T>(this T item, Func<IList<T>> list) {
            return new LazyList<T>(() => item, list);
        }

        public static IList<T> Reverse<T>(this IList<T> list) {
            return Reverse<T>()(list);
        }

        public static IList<TResult> Map<T, TResult>(this Func<T, TResult> mapping, IList<T> list) {
            return Map<T, TResult>()(mapping, list);
        }

        public static Func<IList<T>, IList<T>> Reverse<T>() {
            return MapReverse<T, T>()
                .Partial(Indentity<T>());
        }
        public static Func<Func<T, TResult>, IList<T>, IList<TResult>> Map<T, TResult>() {
            return MapReverse<T, TResult>()
                .Pipe(Reverse<TResult>());
        }
        static Func<Func<T, TResult>, IList<T>, IList<TResult>> MapReverse<T, TResult>() {
            return (mapping, list) => {
                IList<TResult> accum = Empty<TResult>();
                while(!list.IsEmpty) {
                    accum = accum.Cons(mapping(list.Head));
                    list = list.Tail;
                }
                return accum;
            };
        }

        public static IList<int> Infinite(int startIndex) {
            return new LazyList<int>(() => startIndex, () => Infinite(startIndex + 1));
        }

        public static IList<TResult> MapLazy<T, TResult>(this Func<T, TResult> mapping, IList<T> list) {
            if(list.IsEmpty)
                return Empty<TResult>();
            return new LazyList<TResult>(
                () => mapping(list.Head), 
                () => MapLazy(mapping, list.Tail)
            );
        }
        public static Func<Func<T, TResult>, IList<T>, IList<TResult>> MapLazy<T, TResult>() {
            return (mapping, list) => MapLazy(mapping, list);
        }

        public static IList<TResult> ZipLazy<T1, T2, TResult>(this Func<T1, T2, TResult> zipper, IList<T1> list1, IList<T2> list2) {
            if(list1.IsEmpty && list2.IsEmpty)
                return Empty<TResult>();
            return new LazyList<TResult>(
                () => zipper(list1.Head, list2.Head), 
                () => ZipLazy(zipper, list1.Tail, list2.Tail)
            );
        }

        public static IList<T> ConcatLazy<T>(this IList<T> left, IList<T> right) {
            if(left.IsEmpty)
                return right;
            if(right.IsEmpty)
                return left;
            return new LazyList<T>(() => left.Head, () => ConcatLazy(left.Tail, right));
        }

        public static IList<T> FilterLazy<T>(this Func<T, bool> filter, IList<T> list) {
            Lazy<IList<T>> firstElement = Lazy(() => {
                var first = list;
                while(!first.IsEmpty) {
                    if(filter(first.Head))
                        return first;
                    first = first.Tail;
                }
                return null;
            });
            return new LazyList<T>(() => firstElement.Value.Head, () => firstElement.Value != null ? FilterLazy(filter, firstElement.Value.Tail) : null);
        }

        public static Func<Func<T, bool>, IList<T>, IList<T>> FilterLazy<T>() {
            return (filter, list) => FilterLazy(filter, list);
        }

        public static IList<T> Force<T>(this IList<T> list) {
            var iterator = list;
            while(!iterator.IsEmpty) {
                var ignore = iterator.Head;
                iterator = iterator.Tail;
            }
            return list;
        }

        public static IList<T> QuickSort<T>(this IList<T> list) where T : IComparable<T> {
            if(list.IsEmpty)
                return Empty<T>();
            Func<T, bool> lessThan = x => Comparer<T>.Default.Compare(x, list.Head) < 0;
            Func<T, bool> greaterThanOrEqual = x => Comparer<T>.Default.Compare(x, list.Head) >= 0;

            return QuickSort(lessThan.FilterLazy(list.Tail))
                .ConcatLazy(Empty<T>().Cons(list.Head))
                .ConcatLazy(QuickSort(greaterThanOrEqual.FilterLazy(list.Tail)));
        }

        public static readonly IList<int> Fibonacci = 0.Cons(1.ConsLazy(() => Sum.ZipLazy(Fibonacci, Fibonacci.Tail)));
    }
}