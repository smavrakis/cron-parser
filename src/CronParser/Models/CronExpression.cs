using CronParser.Constants;
using CronParser.Exceptions;

namespace CronParser.Models
{
    public class CronExpression
    {
        private static readonly IReadOnlyDictionary<string, int> Months = new Dictionary<string, int>
        {
            { "JAN", 1}, { "FEB", 2}, { "MAR", 3}, { "APR", 4}, { "MAI", 5}, { "JUN", 6}, { "JUL", 7}, { "AUG", 8}, { "SEP", 9}, { "OCT", 10}, { "NOV", 11}, { "DEC", 12}
        };

        private static readonly IReadOnlyDictionary<string, int> DaysOfWeek = new Dictionary<string, int>
        {
            { "SUN", 1}, { "MON", 2}, { "TUE", 3}, { "WED", 4}, { "THU", 5}, { "FRI", 6}, { "SAT", 7}
        };

        private static readonly Dictionary<char, SpecialCharacter> SpecialCharacters = new Dictionary<char, SpecialCharacter>
        {
            { ',', SpecialCharacter.Comma}, { '-', SpecialCharacter.Dash}, { '*', SpecialCharacter.Asterisk}, { '?', SpecialCharacter.QuestionMark}, { '/', SpecialCharacter.Slash}
        };

        private readonly List<CronField> _cronFields;

        public CronExpression(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var expressionParts = expression.Split(' ');

            if (expressionParts.Length != 6)
            {
                throw new FormatException("Invalid cron epxression format, please provide five time fields (minute, hour, day of month, month, and day of week) plus a command");
            }

            Minute = new CronField(CronFieldNames.Minute, ParseRanges(CronFieldNames.Minute, CronFieldLimits.MinuteMin, CronFieldLimits.MinuteMax, expressionParts[0]));
            Hour = new CronField(CronFieldNames.Hour, ParseRanges(CronFieldNames.Hour, CronFieldLimits.HourMin, CronFieldLimits.HourMax, expressionParts[1]));
            DayOfMonth = new CronField(CronFieldNames.DayOfMonth, ParseRanges(CronFieldNames.DayOfMonth, CronFieldLimits.DayOfMonthMin, CronFieldLimits.DayOfMonthMax, expressionParts[2]));
            Month = new CronField(CronFieldNames.Month, ParseRanges(CronFieldNames.Month, CronFieldLimits.MonthMin, CronFieldLimits.MonthMax, expressionParts[3]));
            DayOfWeek = new CronField(CronFieldNames.DayOfWeek, ParseRanges(CronFieldNames.DayOfWeek, CronFieldLimits.DayOfWeekMin, CronFieldLimits.DayOfWeekMax, expressionParts[4]));
            Command = expressionParts[5];

            _cronFields = new List<CronField> { Minute, Hour, DayOfMonth, Month, DayOfWeek };
        }

        public CronField Minute { get; }
        public CronField Hour { get; }
        public CronField DayOfMonth { get; }
        public CronField Month { get; }
        public CronField DayOfWeek { get; }
        public string Command { get; }

        private List<CronFieldRange> ParseRanges(string fieldName, int fieldMin, int fieldMax, string ranges)
        {
            var cronFieldRanges = new List<CronFieldRange>();

            CronFieldRange? currentRange = null;
            SpecialCharacter? currentSpecialCharacter = null;

            for (var i = 0; i < ranges.Length; i++)
            {
                var currentChar = ranges[i];

                if (char.IsDigit(currentChar))
                {
                    var j = i + 1;

                    while (j < ranges.Length && char.IsDigit(ranges[j]))
                    {
                        j++;
                    }

                    var number = int.Parse(ranges.Substring(i, j - i));
                    currentRange = ParseRange(fieldName, fieldMin, fieldMax, currentRange, number, currentSpecialCharacter, cronFieldRanges);

                    i = j - 1;
                    currentSpecialCharacter = null;
                }
                else if (SpecialCharacters.TryGetValue(currentChar, out var specialCharacter))
                {
                    if (specialCharacter == SpecialCharacter.Asterisk || specialCharacter == SpecialCharacter.QuestionMark)
                    {
                        currentRange = new CronFieldRange { Start = fieldMin, End = fieldMax };
                    }
                    else
                    {
                        currentSpecialCharacter = specialCharacter;
                    }
                }
                else
                {
                    if (i + 2 < ranges.Length)
                    {
                        var subString = ranges.Substring(i, 3);

                        if (Months.TryGetValue(subString, out var month))
                        {
                            currentRange = ParseRange(fieldName, fieldMin, fieldMax, currentRange, month, currentSpecialCharacter, cronFieldRanges);
                        }
                        else if (DaysOfWeek.TryGetValue(subString, out var day))
                        {
                            currentRange = ParseRange(fieldName, fieldMin, fieldMax, currentRange, day, currentSpecialCharacter, cronFieldRanges);
                        }
                        else
                        {
                            throw new ParseException();
                        }

                        i += 2;
                        currentSpecialCharacter = null;
                    }
                    else
                    {
                        throw new ParseException();
                    }
                }
            }

            if (currentRange != null)
            {
                cronFieldRanges.Add(currentRange);
            }

            return cronFieldRanges;
        }

        private CronFieldRange ParseRange(string fieldName, int fieldMin, int fieldMax, CronFieldRange? currentRange, int number, SpecialCharacter? currentSpecialCharacter, List<CronFieldRange> ranges)
        {
            if (number < fieldMin || number > fieldMax)
            {
                throw new ParseException($"Found value for field {fieldName} that is outside of the range {fieldMin}-{fieldMax}");
            }

            if (currentRange == null)
            {
                currentRange = new CronFieldRange { Start = number, End = number };
            }

            if (currentSpecialCharacter == SpecialCharacter.Comma)
            {
                ranges.Add(currentRange);
                currentRange = new CronFieldRange { Start = number, End = number };
            }
            else if (currentSpecialCharacter == SpecialCharacter.Dash)
            {
                currentRange.End = number;
            }
            else if (currentSpecialCharacter == SpecialCharacter.Slash)
            {
                currentRange.Step = number;
            }

            return currentRange;
        }

        public void PrintToConsole()
        {
            foreach (var field in _cronFields)
            {
                Console.WriteLine(field.ToFormattedString());
            }
            Console.WriteLine($"{CronFieldNames.Command.PadRight(CronFieldLimits.NameOutputLength)} {Command}");
        }
    }
}
