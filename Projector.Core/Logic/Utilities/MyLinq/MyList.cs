using System;
using System.Collections.Generic;
using System.Web;

namespace Projector.Core.Logic.Utilities.MyLinq
{
    public class MyList<T> : List<T>
    {
        public void ForEach(Action<T> action)
        {
            foreach(var item in this)
            {
                action(item);
            }

            var enumerator = this.GetEnumerator();
            
            while (this.GetEnumerator().MoveNext())
            {
                var currentItem = this.GetEnumerator().Current;
                action(this.GetEnumerator().Current);
            }
        }

        public IEnumerable<TResult> Select<TResult>(Func<T,TResult> select)
        {
            return null;

        }
        //public TSelect Select(this IEnumerable<out T> list, Func<T,TSelect> selectFunc)
        //{

        //}
    }
}