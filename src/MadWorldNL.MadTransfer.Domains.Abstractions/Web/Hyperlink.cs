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
    
    public static Hyperlink FromDatabase(string value)
    {
        return new Hyperlink(value);
    }

    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}