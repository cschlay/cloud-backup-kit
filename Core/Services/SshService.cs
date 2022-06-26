using Core.Services.Interfaces;
using Core.Services.ServiceDataModels;
using Renci.SshNet;

namespace Core.Services;

public class SshService : ISshService
{
    private ConnectionInfo? _connectionInfo;

    public void ExecuteSftp(Action<SftpClient> sftpCallback)
    {
        using var client = new SftpClient(_connectionInfo);
        client.Connect();
        sftpCallback.Invoke(client);
        client.Disconnect();
    }
    
    public void ExecuteSsh(Action<SshClient> sshCallback)
    {
        using var client = new SshClient(_connectionInfo);
        client.Connect();
        sshCallback.Invoke(client);
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
