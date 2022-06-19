namespace Core.Services.ServiceDataModels;

public class SshConfig
{
    private readonly Dictionary<string, SshItem> _content;

    public SshConfig()
    {
        string homeDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var configFilePath = $"{homeDirectoryPath}/.ssh/config";
        _content = ParseConfigFile(configFilePath);
    }
    
    public SshItem GetItem(string name)
    {
        return _content[name];
    }

    private Dictionary<string, SshItem> ParseConfigFile(string filePath)
    {
        var result = new Dictionary<string, SshItem>();
        
        var item = new SshItem();
        foreach (string line in File.ReadLines(filePath))
        {
            string trimmedLine = line.Trim();
            if (trimmedLine.StartsWith("Host "))
            {
                string alias = line.Split(" ")[1];
                item = new SshItem();
                result.Add(alias, item);
            }
            else if (trimmedLine.Length > 0)
            {
                string[] content = line.Trim().Split(" ");
                item.AddValue(content[0], content[1]);
            }
        }

        return result;
    }
}

public class SshItem
{
    private readonly Dictionary<string, string> _fileContent;

    public string? HostName => ReadValue("HostName");
    public string? User => ReadValue("User");
    public string? IdentityFile => ReadValue("IdentityFile");
    
    public SshItem()
    {
        _fileContent = new Dictionary<string, string>();
    }

    public void AddValue(string key, string value)
    {
        _fileContent.Add(key, value.Replace("\"", ""));
    }

    private string? ReadValue(string key)
    {
        _fileContent.TryGetValue(key, out string? value);
        return value;
    }
}
