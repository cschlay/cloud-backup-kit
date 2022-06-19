using Core.Services.Interfaces;
using Renci.SshNet;

namespace Core.Services;

public class SqlDumpService : ISqlDumpService
{
    public SqlDumpService(ISshService ssh)
    {
        
    }
    
    public void CreateDump()
    {
        
    }

    public void DumpPostgres()
    {
    }
}
