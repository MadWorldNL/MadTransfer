using MadWorldNL.MadTransfer.Primitives;

namespace MadWorldNL.MadTransfer.Files;

public class FileMetaData : ValueObject
{
    public string Name { get; init; } = null!;
    public string InternalName { get; init; } = null!;
    public string Extension { get; init; } = null!;
    public long ByteSize { get; init; }

    private FileMetaData()
    {
    }
    
    private FileMetaData(string name, string internalName, string extension, long byteSize)
    {
        Name = name;
        InternalName = internalName;
        Extension = extension;
        ByteSize = byteSize;
    }
    
    public static FileMetaData Create(string name, string internalName, string extension, long byteSize)
    {
        return new FileMetaData(name, internalName,extension, byteSize);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return InternalName;
        yield return Extension;
        yield return ByteSize;
    }
}