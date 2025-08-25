using ArchUnitNET.Fluent;
using ArchUnitNET.xUnitV3;

namespace MadWorldNL.MadTransfer;

using static ArchRuleDefinition;

public class ProjectStructureTests
{
    private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(
        typeof(IAbstractionsMarker).Assembly,
        typeof(IFunctionsMarker).Assembly,
        typeof(IDatabasesMarker).Assembly,
        typeof(IKeyCloakMarker).Assembly,
        typeof(IOvhCloudStorageMarker).Assembly
    ).Build();

    private readonly IObjectProvider<IType> _abstractionsLayer =
        Types().That().ResideInAssembly(typeof(IAbstractionsMarker).Assembly).As("Abstractions Layer");
    
    private readonly IObjectProvider<IType> _functionsLayer =
        Types().That().ResideInAssembly(typeof(IFunctionsMarker).Assembly).As("Functions Layer");
    
    private readonly IObjectProvider<IType> _databasesLayer =
        Types().That().ResideInAssembly(typeof(IDatabasesMarker).Assembly).As("Databases Layer");
    
    private readonly IObjectProvider<IType> _keyCloakLayer =
        Types().That().ResideInAssembly(typeof(IDatabasesMarker).Assembly).As("Key Cloak Layer");
    
    private readonly IObjectProvider<IType> _ovhCloudStorage =
        Types().That().ResideInAssembly(typeof(IOvhCloudStorageMarker).Assembly).As("Ovh Cloud Storage Layer");

    private const string DefaultFaultyReasonDomainLayers = "Domains layers should not access infrastructure layers";
    
    [Fact]
    public void FunctionsLayerShouldNotAccessInfrastructureLayers()
    {
        IArchRule functionsLayerShouldNotAccessDatabasesLayer = Types().That().Are(_functionsLayer).Should()
            .NotDependOnAny(_databasesLayer)
            .Because(DefaultFaultyReasonDomainLayers);
        
        IArchRule functionsLayerShouldNotAccessKeyCloakLayer = Types().That().Are(_functionsLayer).Should()
            .NotDependOnAny(_keyCloakLayer)
            .Because(DefaultFaultyReasonDomainLayers);
        
        IArchRule functionsLayerShouldNotAccessOvhCloudStorageLayer = Types().That().Are(_functionsLayer).Should()
            .NotDependOnAny(_ovhCloudStorage)
            .Because(DefaultFaultyReasonDomainLayers);
        
        functionsLayerShouldNotAccessDatabasesLayer.Check(Architecture);
        functionsLayerShouldNotAccessKeyCloakLayer.Check(Architecture);
        functionsLayerShouldNotAccessOvhCloudStorageLayer.Check(Architecture);
    }
    
    [Fact]
    public void AbstractionsLayerShouldNotAccessInfrastructureLayers()
    {
        IArchRule abstractionsLayerShouldNotAccessDatabasesLayer = Types().That().Are(_abstractionsLayer).Should()
            .NotDependOnAny(_databasesLayer)
            .Because(DefaultFaultyReasonDomainLayers);
        
        IArchRule abstractionsLayerShouldNotAccessKeyCloakLayer = Types().That().Are(_abstractionsLayer).Should()
            .NotDependOnAny(_keyCloakLayer)
            .Because(DefaultFaultyReasonDomainLayers);
        
        IArchRule abstractionsLayerShouldNotAccessOvhCloudStorageLayer = Types().That().Are(_abstractionsLayer).Should()
            .NotDependOnAny(_ovhCloudStorage)
            .Because(DefaultFaultyReasonDomainLayers);
        
        abstractionsLayerShouldNotAccessDatabasesLayer.Check(Architecture);
        abstractionsLayerShouldNotAccessKeyCloakLayer.Check(Architecture);
        abstractionsLayerShouldNotAccessOvhCloudStorageLayer.Check(Architecture);
    }
}