using Core.Services.Interfaces;
using Core.Services.ServiceDataModels;
using Renci.SshNet;

namespace Core.Services;

public class SshService : ISshService
{
    private ConnectionInfo? _connectionInfo;
    
    public void ExecuteCommands(string[] commands)
    {
        using var client = new SshClient(_connectionInfo);
        client.Connect();
        foreach (string command in commands)
        {
            using SshCommand executableCommand = client.CreateCommand(command);
            string result = executableCommand.Execute();
            Console.WriteLine(result);
        }
        client.Disconnect();
    }
    
    public void InitializeConnectionDetails(SshItem item)
    {
        var privateKey = new PrivateKeyFile(item.IdentityFile, "");
        var authentication = new PrivateKeyAuthenticationMethod(
            keyFiles: new []{ privateKey },
            username: item.User);
        _connectionInfo = new ConnectionInfo(
            authenticationMethods: new AuthenticationMethod[]{ authentication },
            host: item.HostName,
            port: item.Port,
            username: item.User);
    }

    /// <inheritdoc />
    public SshConfig ReadSshConfigFile() => new SshConfig();
}
