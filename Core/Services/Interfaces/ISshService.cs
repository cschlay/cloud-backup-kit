using Core.Services.ServiceDataModels;
using Renci.SshNet;

namespace Core.Services.Interfaces;

public interface ISshService
{
    public void ExecuteSftp(Action<SftpClient> sftpCallback);
    public void ExecuteSsh(Action<SshClient> sshCallback);
    public void InitializeConnectionDetails(SshItem item);
    
    /// <summary>
    /// Reads the SSH config file ~/.ssh/config.
    /// </summary>
    /// <seealso href="https://www.ssh.com/academy/ssh/config"/>
    /// <returns>the parsed configuration</returns>
    public SshConfig ReadSshConfigFile();
}
