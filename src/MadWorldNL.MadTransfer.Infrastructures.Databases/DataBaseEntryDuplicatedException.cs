namespace MadWorldNL.MadTransfer;

public sealed class DataBaseEntryDuplicatedException(string subject, string property, string value) : Exception($"The {subject} with {property} '{value}' already exists.")
{
    public string Subject { get; } = subject;
    public string Property { get; } = property;
    public string Value { get; } = value;
}