using System;
using NUnit.Framework;
using PureSharp.Tests.Utils;
using PureSharp.MayBeTransformer;
//using PureSharp.MayBeMonad2;
//using PureSharp.LazyMonad;

using LazyMayBeInt = System.Lazy<PureSharp.MayBeMonad2.MayBe<int?>>;

namespace PureSharp.Tests {
    partial class MayBeTransfromerTests {
        LazyMayBeInt SumWhere(LazyMayBeInt a, LazyMayBeInt b) {
            return from x in a
                   from y in b
                   where x > y
                   select x + y;
        }
    }
}