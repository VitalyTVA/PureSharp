using NUnit.Framework;
using PureSharp.Tests.Utils;
using PureSharp.MayBeMonad2;

namespace PureSharp.Tests {
    [TestFixture]
    public class MayBe2Test {
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
            (from a in "a".AsMayBe()
             from b in 1.AsMayBe()
             select a + b).Value.IsEqual<string>("a1");

            (from a in "a".AsMayBe()
             from b in ((int?)1).AsMayBe()
             select a + b).Value.IsEqual<string>("a1");

            (from a in "a".AsMayBe()
             from b in ((int?)null).AsMayBe()
             select a + b).Value.IsEqual<string>(null);

            (from a in 0.AsMayBe()
             select a + 1).Value.IsEqual<int>(1);

            (from a in "a".AsMayBe()
             from b in 1.AsMayBe()
             select a + b).Value.IsEqual<string>("a1");

            (from a in "a".AsMayBe()
             from b in 0.AsMayBe()
             select a + b).Value.IsEqual<string>("a0");

            (from a in ((string)null).AsMayBe()
             from b in ((int)1).AsMayBe()
             select a + b).Value.IsEqual<string>(null);

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


            (from a in ((int?)1).AsMayBe()
             from b in ((byte?)2).AsMayBe()
             select (long)a + b).Value.IsEqual<long?>(3);

            (from a in 1.AsMayBe()
             from b in ((byte?)2).AsMayBe()
             select (long)a + b).Value.IsEqual<long?>(3);

            (from a in ((int?)null).AsMayBe()
             from b in ((byte?)2).AsMayBe()
             select (long)a + b).Value.IsEqual<long?>(null);

            (from a in ((int?)1).AsMayBe()
             from b in "test".AsMayBe()
             select a.ToString() + b).Value.IsEqual<string>("1test");

            (from a in ((int?)null).AsMayBe()
             from b in "test".AsMayBe()
             select a.ToString() + b).Value.IsEqual<string>(null);

            (from a in ((int?)1).AsMayBe()
             from b in ((string)null).AsMayBe()
             select a.ToString() + b).Value.IsEqual<string>(null);

            var x = from a in ((int?)1).AsMayBe()
                     from b in ((byte?)2).AsMayBe()
                     let c = a * 2
                     select (long)c + b;
            x.Value.IsEqual<long?>(4);

            SumIfGreater(2, 1).IsEqual<int?>(3);
            SumIfGreater(1, 2).IsEqual<int?>(null);
            SumIfGreater(2, null).IsEqual<int?>(null);
            SumIfGreater(null, 1).IsEqual<int?>(null);

            SumIfGreater2(2, 1).IsEqual<int>(3);
            SumIfGreater2(1, 2).IsEqual<int>(0);
        }
        int SumIfGreater2(int a, int b) {
            return (from x1 in a.AsMayBe()
                    from x2 in b.AsMayBe()
                    where x1 > x2
                    select x1 + x2).Value;
        }
        int? SumIfGreater(int? a, int? b) {
            return (from x1 in a.AsMayBe()
                    from x2 in b.AsMayBe()
                    where x1 > x2
                    select x1 + x2).Value;
        }
        string GetStreetName(Person person) {
            return (from p in person.AsMayBe()
                    from address in p.Address.AsMayBe()
                    from street in address.Street.AsMayBe()
                    select street.Name).Value;
        }
        string GetStreetName(Address address) {
            return (from a in address.AsMayBe()
                    from street in a.Street.AsMayBe()
                    select street.Name).Value;
        }
        string GetStreetName(Street street) {
            return (from s in street.AsMayBe()
                    from name in s.Name.AsMayBe()
                    select name).Value;
        }
        string GetStreetNameUnsafe(Street street) {
            return (from name in street.Name.AsMayBe()
                    select name).Value;
        }
        string GetStreetNameCorrect(Street street) {
            return (from s in street.AsMayBe()
                    select s.Name).Value;
        }
    }
}
