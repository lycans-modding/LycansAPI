using BepInEx.Logging;
using LycansAPI.Core.Extensions;
using System.Collections.Generic;

namespace LycansAPI.Core;

internal static class Log
{
    private static ManualLogSource? _logSource;

    internal static void Init(ManualLogSource logSource)
    {
        _logSource = logSource;
    }

    internal static void Debug(object data) => _logSource?.LogDebug(data);
    internal static void Error(object data) => _logSource?.LogError(data);
    internal static void Fatal(object data) => _logSource?.LogFatal(data);
    internal static void Info(object data) => _logSource?.LogInfo(data);
    internal static void Message(object data) => _logSource?.LogMessage(data);
    internal static void Warning(object data) => _logSource?.LogWarning(data);
    internal static void BlockError(IEnumerable<string?>? lines, int width = 70) => _logSource?.LogBlockError(lines, width);
    internal static void BlockWarning(IEnumerable<string?>? lines, int width = 70) => _logSource?.LogBlockWarning(lines, width);
    internal static void Block(LogLevel level, string? header, IEnumerable<string?>? lines, int width = 70)
        => _logSource?.LogBlock(level, header, lines, width);
}