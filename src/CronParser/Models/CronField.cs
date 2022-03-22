using CronParser.Constants;
using System.Text;

namespace CronParser.Models
{
    public class CronField
    {
        public CronField(string name, List<CronFieldRange> ranges)
        {
            Name = name ?? string.Empty;
            Ranges = ranges ?? new List<CronFieldRange>();
        }

        public string Name { get; }
        public List<CronFieldRange> Ranges { get; }

        public string ToFormattedString()
        {
            var rangesBuilder = new StringBuilder();

            foreach (var range in Ranges)
            {
                if (range == null || range.Step < 1 || range.End < range.Start)
                {
                    continue;
                }

                var rangeBuilder = new StringBuilder();

                for (var i = range.Start; i <= range.End; i += range.Step)
                {
                    rangeBuilder.Append($"{i} ");
                }

                rangesBuilder.Append($"{rangeBuilder.ToString()}");
            }

            return $"{Name.PadRight(CronFieldLimits.NameOutputLength)} {rangesBuilder}".TrimEnd();
        }
    }
}
