using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse_MAUI.Helpers
{
    public class General
    {
        /// <summary>
        /// Nullables the integer to int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static int NullableIntegerToInt(int? value, int replacement)
        {
            if (value == null)
            {
                return replacement;
            } else
            {
                return (int)value;
            }
        }
    }
}
