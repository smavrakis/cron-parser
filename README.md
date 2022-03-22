# Cron Expression Parser

## Description
A basic cron expression parser. Not all cron strings are supported due to lack of time.

- Supported fields: minute, hour, day of month, month, and day of week
- Supported values: numbers, JAN-DEC and SUN-SAT
- Supported special characters: , - * /

The app takes in a string as an argument and prints each field to console to show the times at which it will run.

## How to run
Depending on your environment, please choose the appropriate asset from the releases page (see below). Then unzip the file, navigate to the root folder and open the command line. 
Finally, run the **CronParser** file with your desired argument. For example, for Windows you can run the command `.\CronParser.exe "*/15 0 1,15 * 1-5 /usr/bin/find"`.

### Releases

#### .NET 6.0 Framework Installed

- win-x64
- linux-x64
- osx-x64

#### .NET 6.0 Framework Not Installed

- win-x64-self-contained
- linux-x64-self-contained
- osx-x64-self-contained

## Future work
Some additional features that I would like to add if I had more time:

- Expression validation
- Support for additional fields: second, year
- Support for additional special characters: ? L W #
- Additional unit tests