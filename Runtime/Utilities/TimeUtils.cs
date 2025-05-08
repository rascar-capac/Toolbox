using System;
using System.Collections.Generic;

namespace Rascar.Toolbox.Utilities
{
    public static class TimeUtils
    {
        public static int GetMinutesFromSeconds(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).Minutes;
        }

        public static int GetHoursFromSeconds(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).Hours;
        }

        public static string GetFormattedTimeFromSeconds(double seconds, bool showHours = true, bool showMinutes = true, bool showSeconds = true)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            List<string> tokens = new();
            List<object> parameters = new();

            int formatIndex = 0;

            if (showHours)
            {
                tokens.Add("{" + formatIndex + ":D2}");
                parameters.Add(t.Hours);
                formatIndex++;
            }
            if (showMinutes)
            {
                tokens.Add("{" + formatIndex + ":D2}");
                parameters.Add(t.Minutes);
                formatIndex++;
            }
            if (showSeconds)
            {
                tokens.Add("{" + formatIndex + ":D2}");
                parameters.Add(t.Seconds);
            }

            string formatString = string.Join(":", tokens.ToArray());

            return string.Format(formatString, parameters.ToArray());
        }
    }
}
