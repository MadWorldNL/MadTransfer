using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace MadWorldNL.MadTransfer.Common;

public class XUnitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _testOutputHelper;

    public XUnitLoggerProvider(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new XUnitLogger(_testOutputHelper);
    }
}