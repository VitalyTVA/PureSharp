using NUnit.Framework;
using PureSharp.Tests.Utils;
using PureSharp.MayBeMonad;

namespace PureSharp.Tests {
    [TestFixture]
    public class MayBeTest {
        class Street {
            public string Name;
        }
        class Person {
            public Address Address;
        }
        class Address {
            public Street Street;
        }
        [Test]
        public void Test() {
            (from a in "a"
             from b in 1
             select a + b).IsEqual<string>("a1");

            (from a in "a"
             from b in (int?)1
             select a + b).IsEqual<string>("a1");

            (from a in "a"
             from b in (int?)null
             select a + b).IsEqual<string>(null);

            (from a in 0
             select a + 1).IsEqual<int>(1);

            (from a in "a"
             from b in 1
             select a + b).IsEqual<string>("a1");

            (from a in "a"
             from b in 0
             select a + b).IsEqual<string>("a0");

            (from a in default(string)
             from b in (int)1
             select a + b).IsEqual<string>(null);

            GetStreetName(default(Person)).IsNull();
            GetStreetName(new Person { }).IsNull();
            GetStreetName(new Person { Address = new Address { } }).IsNull();
            GetStreetName(new Person { Address = new Address { Street = new Street { } } }).IsNull();
            GetStreetName(new Person { Address = new Address { Street = new Street { Name = "name" } } }).IsEqual("name");

            GetStreetName(default(Address)).IsNull();
            GetStreetName(new Address { }).IsNull();
            GetStreetName(new Address { Street = new Street { } }).IsNull();
            GetStreetName(new Address { Street = new Street { Name = "name" } }).IsEqual("name");

            GetStreetName(default(Street)).IsNull();
            GetStreetName(new Street { Name = "name" }).IsEqual("name");

            //GetStreetNameUnsafe(default(Street)).IsNull();
            GetStreetNameUnsafe(new Street { Name = "name" }).IsEqual("name");

            GetStreetNameCorrect(default(Street)).IsNull();
            GetStreetNameCorrect(new Street { Name = "name" }).IsEqual("name");


            (from a in (int?)1
             from b in (byte?)2
             select (long)a + b).IsEqual<long?>(3);

            (from a in 1
             from b in (byte?)2
             select (long)a + b).IsEqual<long?>(3);

            (from a in (int?)null
             from b in (byte?)2
             select (long)a + b).IsEqual<long?>(null);

            (from a in (int?)1
             from b in "test"
             select a.ToString() + b).IsEqual<string>("1test");

            (from a in (int?)null
             from b in "test"
             select a.ToString() + b).IsEqual<string>(null);

            (from a in (int?)1
             from b in (string)null
             select a.ToString() + b).IsEqual<string>(null);

            (var x = from a in (int?)1
             from b in (byte?)2
             let c = a * 2
             select (long)c + b).IsEqual<long?>(4);

            SumIfGreater(2, 1).IsEqual<int?>(3);
            SumIfGreater(1, 2).IsEqual<int?>(null);
            SumIfGreater(2, null).IsEqual<int?>(null);
            SumIfGreater(null, 1).IsEqual<int?>(null);

            SumIfGreater2(2, 1).IsEqual<int>(3);
            SumIfGreater2(1, 2).IsEqual<int>(0);
        }
        int SumIfGreater2(int a, int b) {
            return from x1 in a
                   from x2 in b
                   where x1 > x2
                   select x1 + x2;
        }
        int? SumIfGreater(int? a, int? b) {
            return from x1 in a
                   from x2 in b
                   where x1 > x2
                   select x1 + x2;
        }
        string GetStreetName(Person person) {
            return from p in person
                   from address in p.Address
                   from street in address.Street
                   select street.Name;
        }
        string GetStreetName(Address address) {
            return from a in address
                   from street in a.Street
                   select street.Name;
        }
        string GetStreetName(Street street) {
            return from s in street
                   from name in s.Name
                   select name;
        }
        string GetStreetNameUnsafe(Street street) {
            return from name in street.Name
                   select name;
        }
        string GetStreetNameCorrect(Street street) {
            return from s in street
                   select s.Name;
        }
    }

    //public abstract class Monad<T, TMonad> where TMonad : Monad<T, TMonad> {
    //    public abstract TResultMonad Bind<TResult, TResultMonad>(Func<T, TResultMonad> f) where TResultMonad : Monad<T, TResultMonad> ;
    //}

    //public struct Identity<T> {
    //    readonly T val;
    //    internal Identity(T value) {
    //        this.val = value;
    //    }
    //    public Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> f) {
    //        return f(val);
    //    }
    //}
    //public static class IdentityExtensions {
    //    public static Identity<T> Identity<T>(T value){
    //        return new Identity<T>(value);
    //    }
    //    public static Identity<TResult> Bind<T, TResult>(this Identity<T> identity, Func<T, TResult> f) {
    //        return identity.Bind(x => Identity(f(x)));
    //    }
    //    public static Func<Identity<T>, Identity<TResult>> Lift<T, TResult>(Func<T, TResult> f){
    //        return x => x.Bind(f);
    //    }
    //}

    //public struct Maybe<T> where T : struct {
    //    internal static readonly Maybe<T> Null = new Maybe<T>();
    //    readonly T? val = null;
    //    internal Maybe(T value) {
    //        this.val = value;
    //    }
    //    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> f) where TResult : struct {
    //        if(val == null)
    //            return Maybe<TResult>.Null;
    //        return f(val.Value);
    //    }
    //}
    //public static class MaybeExtensions {
    //    public static Maybe<T> Maybe<T>(T value) where T : struct {
    //        return new Maybe<T>(value);
    //    }
    //    public static Maybe<T> Null<T>() where T : struct {
    //        return PureSharp.Tests.Maybe<T>.Null;
    //    }
    //    public static Maybe<TResult> Bind<T, TResult>(this Maybe<T> maybe, Func<T, TResult> f) where T : struct where TResult : struct {
    //        return maybe.Bind(x => Maybe(f(x)));
    //    }
    //    public static Func<Maybe<T>, Maybe<TResult>> Lift<T, TResult>(Func<T, TResult> f) where T : struct where TResult : struct {
    //        return x => x.Bind(f);
    //    }
    //}

    //public struct L<T> {
    //    readonly Func<T> func;
    //    public L(Func<T> func) {
    //        this.func = func;
    //    }
    //    public L<TResult> Bind<TResult>(Func<Func<T>, L<TResult>> f) {
    //        return f(func);
    //    }
    //}
    //public static class LExtensions {
    //    public static L<T> L<T>(Func<T> value) {
    //        return new L<T>(value);
    //    }
    //    public static L<TResult> Bind<T, TResult>(this L<T> l, Func<Func<T>, Func<TResult>> f) {
    //        return l.Bind(x => L<TResult>(f(x)));
    //    }

    //    public static L<T> L<T>(T value) {
    //        return L(() => value);
    //    }
    //    public static L<TResult> Bind<T, TResult>(this L<T> l, Func<T, TResult> f) {
    //        return l.Bind<T, TResult>(x => () => f(x()));
    //    }
    //    public static Func<L<T>, L<TResult>> Lift<T, TResult>(Func<Func<T>, Func<TResult>> f) {
    //        return x => x.Bind(f);
    //    }
    //}
}
