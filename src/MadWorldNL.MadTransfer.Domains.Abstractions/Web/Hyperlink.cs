using MadWorldNL.MadTransfer.Exceptions;
using MadWorldNL.MadTransfer.Primitives;

namespace MadWorldNL.MadTransfer.Web;

public sealed class Hyperlink : ValueObject
{
    public string Value { get; } = null!;

    private Hyperlink(string value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Only used by entity framework
    /// </summary>
    [UsedImplicitly]
    private Hyperlink()
    {
    }
    
    public static Hyperlink Create(Guid id)
    {
        var value = GuidToShortString(id);
        
        return new Hyperlink(value!);
    }

    public static Fin<Hyperlink> Create(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return Fin<Hyperlink>.Fail(new EmptyException(nameof(id)));
        }
        
        if (!IsShortStringGuid(id))
        {
            return Fin<Hyperlink>.Fail(new InvalidGuidException(nameof(id)));
        }

        return new Hyperlink(id);
    }
    
    public static Hyperlink FromDatabase(string value)
    {
        return new Hyperlink(value);
    }
    
    private static string GuidToShortString(Guid guid)
    {
        var base64 = Convert.ToBase64String(guid.ToByteArray());
        return base64
            .Replace("+", "-")  // URL-safe
            .Replace("/", "_")
            .Substring(0, 22);  // Remove "==" padding
    }

    private static bool IsShortStringGuid(string shortString)
    {
        var guidString = ShortStringToGuidString(shortString);
        return Guid.TryParse(guidString, out _);
    }
    
    private static string ShortStringToGuidString(string shortString)
    {
        return shortString
            .Replace("-", "+")  // URL-safe
            .Replace("_", "/")
            .PadRight(22, '=');  // Add "==" padding
    }

    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}