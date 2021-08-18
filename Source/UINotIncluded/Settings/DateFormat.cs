using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;

namespace UINotIncluded.Settings
{
    public enum DateFormat : byte
    {
        MMDDYYYY,
        DDMMYYYY
    }

    public static class DateFormatExtension
    {
        public static string ToStringHuman(this DateFormat format)
        {
            switch (format)
            {
                case DateFormat.MMDDYYYY:
                    return (string)"MM-DD-YYYY";
                case DateFormat.DDMMYYYY:
                    return (string)"DD-MM-YYYY";

                default:
                    throw new NotImplementedException();
            }

        }
    }
}
