namespace diplomOriginal.Modules
{
    public enum LogStatus
    {
        INFO = 0,
        DEBUG,
        WARNING,
        ERROR,
        FATAL
    }

    public static class ConsoleLogger
    {
        public static void Log(string message)
        {
            Log(LogStatus.INFO, message);
        }

        public static void Log(LogStatus status, string message)
        {
            switch (status)
            {
                case LogStatus.INFO:
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    }
                case LogStatus.DEBUG:
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                case LogStatus.WARNING:
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    }
                case LogStatus.ERROR:
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    }
                case LogStatus.FATAL:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    }
            }

            Console.WriteLine($"[{status}] {DateTime.Now.ToString("[yyyy.MM.dd] [HH:mm:ss]")} {message}");

            Console.ResetColor();
        }
    }
}
