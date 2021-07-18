using System;
using System.Collections.Generic;
using System.Linq;

namespace Projector.Core.Logic.Utilities.MyLinq
{
    public static class MyLinq
    {
        

        public static void MyForEach<T>(this IEnumerable<T> list, Action<T> action)
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
        

        public static IEnumerable<TResult> MySelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {

            var result = new List<TResult>();
            foreach (var item in source)
            {
                result.Add(selector(item));
            }

            return result;
            


        }


        public static int MyCount<T>(this IEnumerable<T> source)
        {
            int count = 0;
            foreach (var item in source)
            {
                count++;
            }
            return count;
            
        }

        public static bool MyExists<TSource>(this IEnumerable<TSource> source, Func<TSource,bool> match)
        {
            foreach (var item in source)
            {
                if (match(item))
                    return true;
                
            }
            return false;
        }
        public static TSource MyFirst<TSource>(this IEnumerable<TSource> source, Func<TSource,bool> match)
        {

            TSource result = default;
            foreach (var item in source)
            {
                if (match(item))
                    result = item;           
            }
            return result;
            
            
        }
        public static bool MyContains<TSource>(this IEnumerable<TSource> source, TSource item)
        {
            foreach (var items in source)
            {
                if(items.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static IEnumerable<TResult> MyConvertAll<TSource,TResult>(this IEnumerable<TSource> source, Converter<TSource, TResult> converter)
        {
            var list = new List<TResult>();
            foreach (var item in source)
            {
                
                list.Add(converter(item));
            }
            return list;
        }
        public static TSource MySingle<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            TSource result = default;
            int count = 0;
            foreach (var item in source)
            {
                if(predicate(item))
                {
                    result = item;
                    count++;
                }
            }
            if (count == 1)
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        public static int MyRemoveAll<TSource>(this IEnumerable<TSource> source, Func<TSource,bool> match)
        {
            int count = 0;
            var newList = new List<TSource>();
            foreach (var item in newList)
            {
                if (match(item))
                {
                    newList.Add(item);
                    count++;
                }
                    
            }
            foreach (var item in newList)
            {
                source.ToList().Remove(item);
            }
            return count;
        }
        public static bool MyRemove<TSource>(this IEnumerable<TSource> source, TSource item)
        {
            try
            {
                source.ToList().Remove(item);
                return true;
            }
            catch
            {
                return false;
            }

        }
        // Count
        //bool Exists -- Func<Item, bool>
        // Contains -- Item
        // First -- 
        //item Find -- Func<Item,bool>
        // RemoveAll Func<Item,bool>
        // Remove -- item
        // ConvertAll -- Func<Item, TResult>
        // GroupBy -- Func<
        //Item Single -- Func<Item, bool>

        //public static IEnumerable<IGrouping<TKey, TSource>> MyGroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        //{
        //    var group = new List<TSource>();
        //    var list = new List<List<TSource>>();
        //    list.RemoveAll
        //    foreach (var item in source)
        //    {
        //        while(!(keySelector(item).Equals(key)))
        //        {
        //            group.Add(item);
        //        }
        //    }
        //}   
    }
}