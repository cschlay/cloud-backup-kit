using Core.Services;

namespace ConsoleTools;

public static class SqlDumpCommand
{
    public const string Command = "sqldump";

    public static void Run(string localDirectory, string[] args)
    {
        if (args.Length != 3)
        {
            throw new ArgumentException("Invalid number of arguments, expected 'sqldump <sshConfigName> <databaseName>'.");
        }
        
        var sqlDumpService = new SqlDumpService(new SshService());
        sqlDumpService.CreateDump(args[1], args[2], localDirectory);
    }
}
