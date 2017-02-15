using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Extensions
{
    public static class BasicExtensions
    {
        public static IEnumerable<T1> ToAnotherType<T2, T1>(this IEnumerable<T2> data) where T2 : class
        {
            return data.Select(x => (T1)Activator.CreateInstance(typeof(T1), x));
        }
    }
}
