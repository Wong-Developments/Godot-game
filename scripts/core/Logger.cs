using Game.Scripts.Utils;
using Godot;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Game.Scripts.Core;

public static class Logger
{
    private static void LogMessage(LogLevel level, params object[] message)
    {
        var stackTrace = new StackTrace(true);
        var frame = stackTrace.GetFrames()?.FirstOrDefault(f => f.GetMethod()?.DeclaringType?.Name != nameof(Logger));

        if (frame != null)
        {
            //string file = Path.GetFileName(frame.GetFileName() ?? "N/A");
            int line = frame.GetFileLineNumber();
            var method = frame.GetMethod();

            var timeStampHeader = $"{DateTime.Now:HH:mm:ss.fff}".Color("GREEN").Header();
            var levelHeader = $"{level}".Color(level.GetLevelColor()).Header();
            var namespaceHeader = $"{method.DeclaringType.Namespace}".Color("ORANGE").Header();
            var callHeader = $"{method.DeclaringType.Name}.{method.Name}():{line}".Color("YELLOW").Header();

            string logHeader = $"{timeStampHeader} {namespaceHeader} {callHeader} {levelHeader}";

            GD.PrintRich($"{logHeader} {string.Join(" ", message.Select(m => m.ToString().Color(level.GetLevelColor())))}");
        }
        else        
            GD.PrintRich([$"[color=RED]Frame Error: unable to capture logging context[/color] ", .. message]);
    }

    public static string GetLevelColor(this LogLevel level) => level switch
    {
        LogLevel.DEBUG => "WHITE",
        LogLevel.INFO => "CYAN",
        LogLevel.WARNING => "YELLOW",
        LogLevel.ERROR => "RED",
        _ => "GRAY",
    };
    public static void Debug(params object[] message) => LogMessage(LogLevel.DEBUG, message);
    public static void Info(params object[] message) => LogMessage(LogLevel.INFO, message);
    public static void Warning(params object[] message) => LogMessage(LogLevel.WARNING, message);
    public static void Error(params object[] message) => LogMessage(LogLevel.ERROR, message);
    public static void Log(LogLevel level, params object[] message) => LogMessage(level, message);
}
