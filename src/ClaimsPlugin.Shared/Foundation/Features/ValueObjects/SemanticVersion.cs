//using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
//using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

//namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class SemanticVersion :BaseValueObject
//{
//    protected SemanticVersion()
//    {
//    }

//    private SemanticVersion(int major, int minor, int patch)
//    {
//        Major = major;
//        Minor = minor;
//        Patch = patch;
//    }

//    public int Major { get; private set; } = default!;
//    public int Minor { get; private set; } = default!;
//    public int Patch { get; private set; } = default!;

//    public static SemanticVersion Create(int major, int minor, int patch)
//    {
//        AbortWhen(major.IsNegative(), "Major number cannot be a negative number.");
//        AbortWhen(minor.IsNegative(), "Minor number cannot be a negative number.");
//        AbortWhen(patch.IsNegative(), "Patch number cannot be a negative number.");
        
//        return new SemanticVersion(major, minor, patch);
//    }

//    public SemanticVersion IncreasePatchNumber()
//    {
//        return new SemanticVersion(Major, Minor, Patch ++);
//    }
    
//    public SemanticVersion IncreaseMinorNumber()
//    {
//        return new SemanticVersion(Major, Minor++, Patch);
//    }
    
//    public SemanticVersion IncreaseMajorNumber()
//    {
//        return new SemanticVersion(Major++, Minor, Patch);
//    }

//    public static implicit operator string(SemanticVersion semanticVersion)
//    {
//        return semanticVersion.ToString();
//    }

//    public override string ToString()
//    {
//        return $"{Major}.{Minor}.{Patch}";
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return Major;
//        yield return Minor;
//        yield return Patch;
//    }
//}
