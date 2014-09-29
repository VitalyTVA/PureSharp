using System;
using System.Collections.Generic;

namespace PureSharp.Extensions {
    public static class ListExtensions {
        public static IEnumerable<T> AsEnumerable<T>(this IList<T> list) {
            while(!list.IsEmpty) {
                yield return list.Head;
                list = list.Tail;
            }
        }
        public static IList<T> AsList<T>(this IEnumerable<T> enumerable) {
            var reverseList = List.Empty<T>();
            foreach(T item in enumerable) {
                reverseList = reverseList.Cons(item);
            }
            return reverseList.Reverse();
        }
    }
}
