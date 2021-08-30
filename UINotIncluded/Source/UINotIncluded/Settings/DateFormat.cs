using System;
using RimWorld;
using Verse;

namespace UINotIncluded
{
    public enum DateFormat : byte
    {
        MMDDYYYY,
        DDMMYYYY,
        DDmmmYYYY,
        ddmmmYYYY,

    }

    public static class DateFormatExtension
    {
        public static string ToStringHuman(this DateFormat format)
        {
            switch (format)
            {
                case DateFormat.MMDDYYYY:
                    return (string)"MM/DD/YYYY";
                case DateFormat.DDMMYYYY:
                    return (string)"DD/MM/YYYY";
                case DateFormat.DDmmmYYYY:
                    return (string)"DD-QUA-YYYY";
                case DateFormat.ddmmmYYYY:
                    return (string)"DD QUA, YYYY";

                default:
                    throw new NotImplementedException();
            }

        }

        public static string GetFormated(this DateFormat format, long absTicks, float longitude)
        {
            int day = GenDate.DayOfQuadrum(absTicks, longitude)+1;
            Quadrum quadrum = GenDate.Quadrum(absTicks, longitude);
            int year = GenDate.Year(absTicks, longitude);

            switch (format)
            {
                case DateFormat.MMDDYYYY:
                    return String.Format("{0:D2}/{1:D2}/{2}", day, (int)quadrum+1, year);
                case DateFormat.DDMMYYYY:
                    return String.Format("{1:D2}/{0:D2}/{2}", day, (int)quadrum +1, year);
                case DateFormat.DDmmmYYYY:
                    return String.Format("{0:D2}-{1}-{2}", day, quadrum.LabelShort(), year);
                case DateFormat.ddmmmYYYY:
                    return String.Format("{0} {1}, {2}", Find.ActiveLanguageWorker.OrdinalNumber(day), quadrum.LabelShort(), year);

                default:
                    throw new NotImplementedException();
            }
        }

        public static int GetFormatedLength(this DateFormat format)
        {
            switch (format)
            {
                case DateFormat.MMDDYYYY:
                    return 100;
                case DateFormat.DDMMYYYY:
                    return 100;
                case DateFormat.DDmmmYYYY:
                    return 110;
                case DateFormat.ddmmmYYYY:
                    return 120;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
