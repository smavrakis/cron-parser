namespace CronParser.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException()
        {
        }

        public ParseException(string message = "Unable to parse the cron exception. Allowed values: { 0 - 9, ',', '-', '*', '/', JAN - DEC, SUN - SAT }")
            : base(message)
        {
        }

        public ParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
