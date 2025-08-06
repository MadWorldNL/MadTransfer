using MadWorldNL.MadTransfer.Primitives;

namespace MadWorldNL.MadTransfer.Web;

public class Hyperlink : ValueObject
{
    public string Value { get; init; } = null!;

    private Hyperlink(string value)
    {
        Value = value;
    }
    
    private Hyperlink()
    {
    }

    public static Hyperlink Create(string? value)
    {
        return new Hyperlink(value!);
    }
    
    public static Hyperlink Create(Guid id)
    {
        var value = GuidToShortString(id);
        
        return new Hyperlink(value!);
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

    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}