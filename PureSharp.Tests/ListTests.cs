using System;
using System.Linq;
using NUnit.Framework;
using PureSharp.Extensions;
using static PureSharp.List;
using PureSharp.Tests.Utils;

namespace PureSharp.Tests {
    [TestFixture]
    public class ListTests {
        const int BlowStackCount = 100000;
        static readonly Func<int, int> SumFunc = x => x + x;

        [Test]
        public void BasicOperationsTest() {
            Empty<int>()
                .IsTrue(x => x.IsEmpty);
            Assert.Throws<InvalidOperationException>(() => { var ignore = Empty<int>().Head; });
            Assert.Throws<InvalidOperationException>(() => { var ignore = Empty<int>().Tail; });

            var oneItem = Empty<int>().Cons(1)
                .IsEqual(x => x.Head, 1)
                .IsFalse(x => x.IsEmpty)
                .IsTrue(x => x.Tail.IsEmpty)
                .IsSame(x => x.Tail, Empty<int>());

            oneItem.Cons(2)
                .IsEqual(x => x.Head, 2)
                .IsFalse(x => x.IsEmpty)
                .IsSame(x => x.Tail, oneItem);
        }
        [Test]
        public void AsEnumerableTest() {
            CollectionAssert.IsEmpty(Empty<int>().AsEnumerable());
            CollectionAssert.AreEqual(new[] { 1 }, Empty<int>().Cons(1).AsEnumerable());
            CollectionAssert.AreEqual(new[] { 2, 1 }, Empty<int>().Cons(1).Cons(2).AsEnumerable());
        }
        [Test]
        public void AsListTest() {
            Enumerable.Empty<int>().AsList().IsTrue(x => x.IsEmpty);
            CollectionAssert.AreEqual(new[] { 1 }, new[] { 1 }.AsList().AsEnumerable());
            CollectionAssert.AreEqual(new[] { 1, 2 }, new[] { 1, 2 }.AsList().AsEnumerable());
        }
        [Test]
        public void ReverseTest() {
            CollectionAssert.IsEmpty(Empty<int>().Reverse().AsEnumerable());
            CollectionAssert.AreEqual(new[] { 1 }, Empty<int>().Cons(1).Reverse().AsEnumerable());
            CollectionAssert.AreEqual(new[] { 1, 2 }, Empty<int>().Cons(1).Cons(2).Reverse().AsEnumerable());
        }
        [Test]
        public void ReverseLargeListTest() {
            CollectionAssert.AreEqual(Enumerable.Range(0, BlowStackCount).Reverse(), Enumerable.Range(0, BlowStackCount).AsList().Reverse().AsEnumerable());
        }
        [Test]
        public void MapTest() {
            CollectionAssert.IsEmpty(SumFunc.Map(Empty<int>()).AsEnumerable());
            CollectionAssert.AreEqual(new[] { 2 }, SumFunc.Map(Empty<int>().Cons(1)).AsEnumerable());
            CollectionAssert.AreEqual(new[] { 2, 4 }, SumFunc.Map(Empty<int>().Cons(2).Cons(1)).AsEnumerable());
        }
        [Test]
        public void LazyMapTest() {
            int callCount = 0;
            Func<int, int> doubleFunc = x => {
                callCount++;
                return 2 * x;
            };

            doubleFunc.MapLazy(Empty<int>()).IsTrue(x => x.IsEmpty);
            callCount.IsEqual(0);

            var list = Empty<int>().Cons(1).Cons(2);

            var mapped1 = doubleFunc.MapLazy(list.Tail);
            callCount.IsEqual(0);
            mapped1.Head.IsEqual(2);
            callCount.IsEqual(1);
            mapped1.Head.IsEqual(2);
            callCount.IsEqual(1);

            var mapped2 = doubleFunc.MapLazy(list);
            callCount.IsEqual(1);
            mapped2.Head.IsEqual(4);
            callCount.IsEqual(2);
            mapped2.Tail.Head.IsEqual(2);
            callCount.IsEqual(3);
        }
        [Test]
        public void MapMemoryTest() {
            IList<WeakReference> references;
            var mappedList = MapMemoryCore(Map<int, int>(), out references);
            references.AssertAllCollected();
        }
        [Test]
        public void LazyMapMemoryTest() {
            IList<WeakReference> references;
            var mappedList = MapMemoryCore(MapLazy<int, int>(), out references);
            references.AssertAllAlive();
            mappedList.Force();
            references.AssertAllCollected();
        }
        IList<int> MapMemoryCore(Func<Func<int, int>, IList<int>, IList<int>> mapFunction, out IList<WeakReference> references) {
            return CollectReferencesAndModifyList(x => mapFunction(SumFunc, x), out references);
        }
        IList<int> CollectReferencesAndModifyList(Func<IList<int>, IList<int>> modifyFunction, out IList<WeakReference> references) {
            var list = Empty<int>().Cons(1).Cons(2);
            references = Empty<WeakReference>().CollectReference(list).CollectReference(list.Tail);
            return modifyFunction(list);
        }

        [Test]
        public void InfiniteTest() {
            CollectionAssert.AreEqual(Enumerable.Range(1000, BlowStackCount), Infinite(1000).AsEnumerable().Take(BlowStackCount));
            CollectionAssert.AreEqual(Enumerable.Range(1000, BlowStackCount).Select(SumFunc), SumFunc.MapLazy(Infinite(1000)).AsEnumerable().Take(BlowStackCount));
        }

        abstract class A { }
        class B :  A { }
        class C : A { }
        [Test]
        public void CovariantList() {
            IList<A> list = new B().Cons(Empty<B>());
            IList<A> list2 = list.Cons(new C());
        }

        [Test]
        public void LazyZipTest() {
            int callCount = 0;
            Func<int, int, int> sumFunc = (x, y) => {
                callCount++;
                return x + y;
            };

            sumFunc.ZipLazy(Empty<int>(), Empty<int>()).IsTrue(x => x.IsEmpty);
            callCount.IsEqual(0);

            var list1 = Empty<int>().Cons(1).Cons(2);
            var list2 = Empty<int>().Cons(10).Cons(20);

            var zipped1 = sumFunc.ZipLazy(list1.Tail, list2.Tail);
            callCount.IsEqual(0);
            zipped1.Head.IsEqual(11);
            callCount.IsEqual(1);
            zipped1.Head.IsEqual(11);
            callCount.IsEqual(1);

            var zipped2 = sumFunc.ZipLazy(list1, list2);
            callCount.IsEqual(1);
            zipped2.Head.IsEqual(22);
            callCount.IsEqual(2);
            zipped2.Tail.Head.IsEqual(11);
            callCount.IsEqual(3);
        }

        [Test]
        public void LazyConcatTest() {
            var list1 = 1.Cons(2.Cons(Empty<int>()));
            var list2 = 3.Cons(4.Cons(Empty<int>()));
            list1.ConcatLazy(Empty<int>()).IsSame(list1);
            Empty<int>().ConcatLazy(list1).IsSame(list1);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, list1.ConcatLazy(list2).AsEnumerable());

            int callCount = 0;
            Func<int, int> doubleFunc = x => {
                callCount++;
                return 2 * x;
            };
            var concatenated = doubleFunc.MapLazy(list1).ConcatLazy(doubleFunc.MapLazy(list2));
            callCount.IsEqual(0);
            CollectionAssert.AreEqual(new[] { 2, 4, 6, 8 }, concatenated.AsEnumerable());
            callCount.IsEqual(4);
        }
        [Test]
        public void LazyConcatMemoryTest1() {
            IList<WeakReference> references;
            var concatenated = CollectReferencesAndModifyList(x => x.ConcatLazy(3.Cons(4.Cons(Empty<int>()))), out references);
            references.AssertAllAlive();
            concatenated.Force();
            references.AssertAllCollected();
        }
        [Test]
        public void LazyConcatMemoryTest2() {
            IList<WeakReference> references;
            var concatenated = CollectReferencesAndModifyList(x => 3.Cons(4.Cons(Empty<int>())).ConcatLazy(x), out references);
            references.AssertAllAlive();
            concatenated.Force();
            references.AssertAllAlive();
        }
        [Test]
        public void ForceTest() {
            int callCount = 0;
            Func<int, int> doubleFunc = x => {
                callCount++;
                return 2 * x;
            };
            var list = Empty<int>().Cons(1).Cons(2);
            var mapped = doubleFunc.MapLazy(list);
            callCount.IsEqual(0);
            mapped.Force().IsSame(mapped);
            callCount.IsEqual(2);
        }
        [Test]
        public void FibonacciTest() {
            var fibs = Fibonacci.AsEnumerable().Take(10);
            CollectionAssert.AreEqual(new[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 }, fibs);
        }
        [Test]
        public void LazyFilterTest1() {
            Func<int, bool> isOddFunc = x => x % 2 == 1;
            isOddFunc.FilterLazy(Empty<int>()).IsTrue(x => x.IsEmpty);
            CollectionAssert.AreEqual(new[] { 1 }, isOddFunc.FilterLazy(new[] { 1 }.AsList()).AsEnumerable());
            CollectionAssert.AreEqual(new[] { 1 }, isOddFunc.FilterLazy(new[] { 0, 1, 2 }.AsList()).AsEnumerable());
            CollectionAssert.AreEqual(new int[] { }, isOddFunc.FilterLazy(new[] { 0, 2, 2 }.AsList()).AsEnumerable());
            CollectionAssert.AreEqual(new [] { 1, 3 }, isOddFunc.FilterLazy(new[] { 1, 2, 2, 3 }.AsList()).AsEnumerable());
        }
        [Test]
        public void LazyFilterTest2() {
            int incCallCount = 0;
            Func<int, int> incFunc = x => {
                incCallCount++;
                return x + 1;
            };
            int isOddCallCount = 0;
            Func<int, bool> isOddFunc = x => {
                isOddCallCount++;
                return x % 2 == 1;
            };

            var initialList = Empty<int>().Cons(3).Cons(2).Cons(1).Cons(0);
            var oddList = MapLazy<int, int>().Partial(incFunc).Pipe(FilterLazy<int>().Partial(isOddFunc))(initialList);
            incCallCount.IsEqual(0);
            isOddCallCount.IsEqual(0);

            oddList.IsEqual(x => x.Head, 1);
            incCallCount.IsEqual(1);
            isOddCallCount.IsEqual(1);

            oddList = oddList.Tail;
            oddList.IsEqual(x => x.Head, 3);
            incCallCount.IsEqual(3);
            isOddCallCount.IsEqual(3);

            oddList.IsTrue(x => x.Tail.IsEmpty);
            incCallCount.IsEqual(4);
            isOddCallCount.IsEqual(4);

            Assert.Throws<InvalidOperationException>(() => { var ignore = oddList.Tail.Head; });
            Assert.Throws<InvalidOperationException>(() => { var ignore = oddList.Tail.Tail; });
            incCallCount.IsEqual(4);
            isOddCallCount.IsEqual(4);
        }

        [Test, Ignore]
        public void LazyFilterTest3() {
            int isOddCallCount = 0;
            Func<int, bool> isOddFunc = x => {
                isOddCallCount++;
                return x % 2 == 1;
            };

            var initialList = Empty<int>().Cons(3).Cons(2).Cons(1).Cons(0);
            var oddList = isOddFunc.FilterLazy(initialList);
            isOddCallCount.IsEqual(0);

            var mappedList = F.Id<int>().MapLazy(oddList);
            isOddCallCount.IsEqual(0); //OOPS!
        }

        [Test,]
        public void LazyFilterTest4() {
            int isOddCallCount = 0;
            Func<int, bool> isOddFunc = x => {
                isOddCallCount++;
                return x % 2 == 1;
            };

            var initialList = Empty<int>().Cons(3).Cons(2).Cons(1).Cons(0);
            var oddList = isOddFunc.FilterLazy(initialList);
            isOddCallCount.IsEqual(0);

            var mappedList = isOddFunc.FilterLazy(oddList);
            isOddCallCount.IsEqual(0);
        }

        [Test]
        public void QuickSortTest() {
            CollectionAssert.AreEqual(new int[] { }, new int[] { }.AsList().QuickSort().AsEnumerable());
            CollectionAssert.AreEqual(new int[] { 1 }, new int[] { 1 }.AsList().QuickSort().AsEnumerable());
            CollectionAssert.AreEqual(new int[] { 1, 1 }, new int[] { 1, 1 }.AsList().QuickSort().AsEnumerable());
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, new[] { 3, 1, 2 }.AsList().QuickSort().AsEnumerable());
            CollectionAssert.AreEqual(new[] { 1, 1, 2, 2, 3, 3 }, new[] { 1, 3, 2, 1, 3, 2 }.AsList().QuickSort().AsEnumerable());
        }
    }
    //public static class TestFunctionExtensions {
    //    [Test]
    //    public void Test1() {
    //        Func<Func<int, string>, IList<int>, IList<string>> f2 = (Func<int, string> x, IList<int> y) => Map<int, string>(x, y);
    //        Func<IList<int>, IList<string>> f3 = f2.Partial(x => x.ToString());

    //        var f4 = Map<int, string>().Partial(ToString<int>());
    //        var f5 = Map<int, string>().Partial(x => x.ToString());

    //    }
    //    public static Func<T, string> ToString<T>() {
    //        return (T x) => x.ToString();
    //    }
    //}
}
