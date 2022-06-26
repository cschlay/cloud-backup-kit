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
    
    public void CreateDump(string sshConfigName, string databaseName, string localDumpDir)
    {
        SshConfig config = _sshService.ReadSshConfigFile();
        _sshService.InitializeConnectionDetails(config.GetItem(sshConfigName));
        (string remoteDumpDir, string filename) = DumpPostgres(databaseName);
        DownloadDumpFile($"{localDumpDir}/{filename}", $"{remoteDumpDir}/{filename}");
    }

    private void DownloadDumpFile(string localPath, string remotePath)
    {
        _sshService.ExecuteSftp(sftp =>
        {
            new FileInfo(localPath).Directory!.Create();
            using FileStream localFileStream = File.Create(localPath);
            sftp.DownloadFile(remotePath, localFileStream);
        });
    }

    private (string, string) DumpPostgres(string databaseName)
    {
        const string homeDirectory = "cloud-backup-kit/database-dumps";
        string timestamp = DateTime.UtcNow.ToString("s").Replace(":", "");
        var filename = $"main-{timestamp}.sql.gz";
        _sshService.ExecuteSsh(ssh =>
        {
            ssh.RunCommand($"mkdir -p {homeDirectory}");
            ssh.RunCommand($"pg_dump {databaseName} | gzip > {homeDirectory}/{filename}");
        });
        return (homeDirectory, filename);
    }
}
