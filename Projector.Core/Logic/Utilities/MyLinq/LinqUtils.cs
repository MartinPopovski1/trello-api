using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector.Core.Logic.Utilities.MyLinq
{
    public class LinqUtils<TSource> 
    {
        public static void ForEach(IEnumerable<TSource> list, Action<TSource> action)
        {
            foreach (var item in list)
            {
                action(item);
            }

            //while (list.GetEnumerator().MoveNext())   
            //{
            //    action(list.GetEnumerator().Current);
            //}
        }

        public static IEnumerable<TResult> MySelect<TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return null;
        }
    }
}