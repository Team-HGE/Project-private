using System;
using System.IO;
using System.Collections.Generic;

interface ILogger
{
    void WriteLog(string message);
    void WriteError(string error)
    {
        WriteLog($"Error: {error}");
    }
}

class ConsoleLogger : ILogger
{
    public void WriteLog(string message)
    {
        Console.WriteLine($"[{DateTime.Now.ToLocalTime()}] {message}");
    }
}

class FileLogger : ILogger
{
    private StreamWriter writer;

    public FileLogger(string path)
    {
        writer = File.CreateText(path);
        writer.AutoFlush = true;
    }

    public void WriteLog(string message)
    {
        writer.WriteLine($"[{DateTime.Now.ToLocalTime()}] {message}");
    }
}

class ClimateMonitor
{
    public List<ILogger> loggers = new List<ILogger>();

    public ClimateMonitor(ILogger logger)
    {
        loggers.Add(logger);
    }
    public void AddLogger(ILogger logger)
    {
        loggers.Add(logger);
    }

    public void Start()
    {
        while (true)
        {
            Console.Write("온도를 입력해주세요. : ");
            string temperature = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(temperature))
            {
                WriteAllError("공백을 입력받았습니다.");
                break;
            }

            WriteAllLog("현재 온도 : " + temperature);
        }
    }

    private void WriteAllLog(string message)
    {
        foreach (var logger in loggers)
            logger.WriteLog(message);
    }

    private void WriteAllError(string error)
    {
        foreach (var logger in loggers)
            logger.WriteError(error);
    }
}

class Program
{
    static void Main(string[] args)
    {
        ClimateMonitor monitor = new ClimateMonitor(new ConsoleLogger());
        monitor.Start();

        Console.WriteLine("이제부터 파일에도 기록됩니다.");

        monitor.AddLogger(new FileLogger("MyLog.txt"));
        monitor.Start();
    }
}
