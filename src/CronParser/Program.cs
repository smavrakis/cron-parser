using CronParser.Models;

if (args.Length == 0)
{
    Console.WriteLine("No arguments provided, exiting...");
    return;
}

try
{
    var cronExpression = new CronExpression(args[0]);
    cronExpression.PrintToConsole();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}