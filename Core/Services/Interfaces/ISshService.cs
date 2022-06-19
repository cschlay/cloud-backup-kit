using Core.Services.ServiceDataModels;

namespace Core.Services.Interfaces;

public interface ISshService
{
    public void ExecuteCommands(string[] commands);
    public void InitializeConnectionDetails(SshItem item);
    
    /// <summary>
    /// Reads the SSH config file ~/.ssh/config.
    /// </summary>
    /// <seealso href="https://www.ssh.com/academy/ssh/config"/>
    /// <returns>the parsed configuration</returns>
    public SshConfig ReadSshConfigFile();
}
