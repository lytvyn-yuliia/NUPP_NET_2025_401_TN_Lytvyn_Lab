using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journalism.Common
{
    public static class Extensions
    {
        // Метод розширення для класу string
        public static string Quote(this string text)
        {
            return $"\"{text}\"";
        }
    }
}
