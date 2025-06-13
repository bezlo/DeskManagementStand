using Windows.UI;
using System.Diagnostics;

namespace DeskManagementStand_App.Services
{
    internal class Logger
    {
        // This class is used to log messages to the console or a file.
        // It can be extended to log messages to different outputs as needed.
        public enum LogLevel
        {
            INFO,
            WARNING,
            ERROR,
            DEBUG
        };
        
        public void Log(LogLevel level, string message)
        {
            // Log the message with the specified level
            Color color_ = LevelToColor(level);
            string levelString = LevelToString(level);
            string logTimeStamp = DateTime.Now.ToString("HH:mm:ss");
            //string logMessage = $"{DateTime.Now:HH:mm:ss} [{levelString}] {message}";


            Console.WriteLine(logTimeStamp);
            Debug.WriteLine(logTimeStamp);
            switch (color_)
            {
                case Color c when c == Color.FromArgb(255, 0, 255, 0): // green
                    Console.ForegroundColor = System.ConsoleColor.Green;
                    break;
                case Color c when c == Color.FromArgb(255, 255, 255, 0): // yellow
                    Console.ForegroundColor = System.ConsoleColor.Yellow;
                    break;
                case Color c when c == Color.FromArgb(255, 255, 0, 0): // red
                    Console.ForegroundColor = System.ConsoleColor.Red;
                    break;
                case Color c when c == Color.FromArgb(255, 255, 255, 255): // white
                    Console.ForegroundColor = System.ConsoleColor.White;
                    break;
                default: // blue as default for unknown levels
                    Console.ForegroundColor = System.ConsoleColor.Blue;
                    break;
            }

            Console.WriteLine(levelString);
            Debug.WriteLine(levelString);
            Console.ResetColor();
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }
        Color LevelToColor(LogLevel level)
        {
            switch (level)
            {
                //case level == LogLevel.INFO:return "Green"; its enum
                case LogLevel.INFO: return Color.FromArgb(255, 0, 255, 0); // green
                case LogLevel.WARNING: return Color.FromArgb(255, 255, 255, 0); //yellow
                case LogLevel.ERROR: return Color.FromArgb(255, 255, 0, 0); // red
                case LogLevel.DEBUG: return Color.FromArgb(255, 255, 255, 255); // white
            }
            return Color.FromArgb(255, 0, 0, 255); // blue as default for unknown levels
        }
        string LevelToString(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.INFO: return "INFO";
                case LogLevel.WARNING: return "WARNING";
                case LogLevel.ERROR: return "ERROR";
                case LogLevel.DEBUG: return "DEBUG";
            }
            return "UNKNOWN"; // Default if no match found
        }

        //shortcuts
        public void Info(string message) => Log(LogLevel.INFO, message);
        public void Warning(string message) => Log(LogLevel.WARNING, message);
        public void Error(string message) => Log(LogLevel.ERROR, message);
        public void DebugLog(string message) => Log(LogLevel.DEBUG, message);
    }
}
