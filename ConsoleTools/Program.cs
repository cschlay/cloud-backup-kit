// See https://aka.ms/new-console-template for more information

using Core.Services;

var s = new SshService();
s.ReadSshConfigFile();