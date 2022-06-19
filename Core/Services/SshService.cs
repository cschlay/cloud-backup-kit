using Core.Services.Interfaces;
using Core.Services.ServiceDataModels;

namespace Core.Services;

public class SshService : ISshService
{
    /// <inheritdoc />
    public SshConfig ReadSshConfigFile()
    {
        return new SshConfig();
    }
}