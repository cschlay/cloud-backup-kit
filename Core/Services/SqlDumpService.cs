using Core.Services.Interfaces;
using Core.Services.ServiceDataModels;

namespace Core.Services;

public class SqlDumpService : ISqlDumpService
{
    private readonly ISshService _sshService;

    public SqlDumpService(ISshService ssh)
    {
        _sshService = ssh;
    }
    
    public void CreateDump(string databaseName)
    {
        SshConfig config = _sshService.ReadSshConfigFile();
        string dumpFilePath = DumpPostgres(databaseName);
        // TODO: SFTP and download it.
    }

    private string DumpPostgres(string databaseName)
    {
        const string homeDirectory = "cloud-backup-kit";
        var timestamp = DateTime.UtcNow.ToString("s");
        var dumpFilePath = $"{homeDirectory}/database-dumps/main-{timestamp}.gz";
        var commands = new []
        {
            $"mkdir {homeDirectory}",
            $"mkdir {homeDirectory}/database-dumps",
            $"pg_dump {databaseName} | gzip > {dumpFilePath}"
        };
        _sshService.ExecuteCommands(commands);
        
        return dumpFilePath;
    }
}
