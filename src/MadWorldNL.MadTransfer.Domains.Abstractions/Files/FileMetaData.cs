using MadWorldNL.MadTransfer.Exceptions;
using MadWorldNL.MadTransfer.Primitives;

namespace MadWorldNL.MadTransfer.Files;

public sealed class FileMetaData : ValueObject
{
    public string Name { get; init; } = null!;
    public string InternalName { get; init; } = null!;
    public string Extension { get; init; } = null!;
    public long ByteSize { get; init; }

    /// <summary>
    /// Only used by entity framework
    /// </summary>
    [UsedImplicitly]
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
    
    public static Fin<FileMetaData> Create(string name, string internalName, string extension, long byteSize)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Fin<FileMetaData>.Fail(new EmptyException(nameof(name)));
        }
        
        if (string.IsNullOrEmpty(internalName))
        {
            return Fin<FileMetaData>.Fail(new EmptyException(nameof(internalName)));
        }
        
        if (string.IsNullOrEmpty(extension))
        {
            return Fin<FileMetaData>.Fail(new EmptyException(nameof(extension)));
        }
        
        if (byteSize < 0)
        {
            return Fin<FileMetaData>.Fail(new NegativeException(nameof(byteSize)));
        }
        
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