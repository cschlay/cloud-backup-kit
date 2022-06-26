using ConsoleTools;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

switch (args[0])
{
    case SqlDumpCommand.Command: 
        SqlDumpCommand.Run(configuration["SqlDump:Directory"], args);
        break;
}
