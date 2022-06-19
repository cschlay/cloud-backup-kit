using Core.Services.ServiceDataModels;

namespace Core.Services.Interfaces;

public interface ISshService
{
    /// <summary>
    /// Reads the SSH config file ~/.ssh/config.
    /// </summary>
    /// <seealso href="https://www.ssh.com/academy/ssh/config"/>
    /// <returns>the parsed configuration</returns>
    public SshConfig ReadSshConfigFile();
}
