using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace MadWorldNL.MadTransfer.Common;

public class XUnitLogger : ILogger
{
    private readonly ITestOutputHelper _testOutputHelper;

    public XUnitLogger(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _testOutputHelper.WriteLine("[{0}] {1}", logLevel.ToString(), formatter(state, exception));
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel > LogLevel.Debug;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
}