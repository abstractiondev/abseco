using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AbstractionBuilder
{
    public static class EcoSupport
    {
        public static void AddToDirectory(string abstractionFolder, string directoryFolder)
        {
            string abstractionID = getAbstractionID(abstractionFolder);
            string[] urlLocations = getUrlLocations(abstractionFolder);
            string content = createDirectoryContent(abstractionID, urlLocations);
            addContentToDirectory(abstractionID, directoryFolder, content);
        }

        private static void addContentToDirectory(string abstractionID, string directoryFolder, string content)
        {
            string fullFileName = getFullPathName(directoryFolder, abstractionID);
            File.WriteAllText(fullFileName, content);
        }

        private static string getFullPathName(string directoryFolder, string abstractionId)
        {
            throw new NotImplementedException();
        }

        private static string createDirectoryContent(string abstractionId, string[] urlLocations)
        {
            throw new NotImplementedException();
        }

        private static string[] getUrlLocations(string abstractionFolder)
        {
            string[] remotes = CmdSupport.ExecuteWithOutput("git", "remote -v", abstractionFolder);
            remotes = remotes.Where(remote => remote.Contains("(fetch)")).Select(remote =>
                                                                                     {
                                                                                         string[] split =
                                                                                             remote.Split(
                                                                                                 new char[] {'\t', ' '},
                                                                                                 StringSplitOptions.
                                                                                                     RemoveEmptyEntries);
                                                                                         return split[1];
                                                                                     }).ToArray();
            return remotes;
        }

        private static string getAbstractionID(string abstractionFolder)
        {
            string abstractionID =
                CmdSupport.ExecuteWithOutput("git", "log --format=%H -n 1", abstractionFolder).First();
            return abstractionID;
        }
    }

    internal static class CmdSupport
    {
        public static string[] ExecuteWithOutput(string command, string arguments, string workingDirectory)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.FileName = command;
            startInfo.Arguments = arguments;
            startInfo.RedirectStandardOutput = true;
            Process proc = Process.Start(startInfo);
            proc.WaitForExit();
            string stdOutput = proc.StandardOutput.ReadToEnd();
            return stdOutput.Split('\n');
        }
    }
}