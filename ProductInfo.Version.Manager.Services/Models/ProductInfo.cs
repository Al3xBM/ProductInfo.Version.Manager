namespace ProductInfo.Version.Manager.Services.Models;

public class ProductInfo
{
    private int _majorVersion;
    
    public int MajorVersion
    {
        get => _majorVersion;
        set
        {
            _majorVersion = value;
            MinorVersion = 0;
        }
    }

    public int MinorVersion { get; set; }

    public string OuterVersion { get; private set; } = null!;

    public IEnumerable<string> AdditionalInfo { get; set; } = null!;

    public string FullVersion
    {
        get => $"{OuterVersion}.{MajorVersion}.{MinorVersion}";
        init
        {
            var versionParts = value.Split('.');
            if (versionParts.Length != 4)
            {
                throw new ArgumentException($"Invalid version format: {value}");
            }
            
            OuterVersion = $"{versionParts[0]}.{versionParts[1]}";
            MajorVersion = int.Parse(versionParts[2]);
            MinorVersion = int.Parse(versionParts[3]);
        }
    }
}