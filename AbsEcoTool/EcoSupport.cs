using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AbsEcoTool
{
    public static class EcoSupport
    {
        public static void SignAbstractionWithKey(string abstractionFolder, string certDir, string keyId)
        {
            string abstractionID = getAbstractionID(abstractionFolder);
            executeGetSignContent(certDir, abstractionFolder);
            signContentWithKey(certDir, abstractionID, keyId);
        }

        private static void signContentWithKey(string certDir, string abstractionId, string keyId)
        {
            CmdSupport.ExecuteScriptWithOutput("signXwithKeyY.cmd " + abstractionId + " " + keyId, certDir);
        }

        private static void executeGetSignContent(string certDir, string abstractionFolder)
        {
            CmdSupport.ExecuteScriptWithOutput("gitgetcommitdata.cmd ..\\" + abstractionFolder, certDir);
        }

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
            return Path.Combine(directoryFolder, abstractionId);
        }

        private static string createDirectoryContent(string abstractionId, string[] urlLocations)
        {
            return String.Join("\n", urlLocations);
        }

        private static string[] getUrlLocations(string abstractionFolder)
        {
            string[] remotes = CmdSupport.ExecuteGitWithOutput("remote -v", abstractionFolder);
            if(remotes.Length == 0)
                throw new NotSupportedException("Git remotes are empty: " + abstractionFolder);
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
                CmdSupport.ExecuteGitWithOutput("log --format=%H -n 1", abstractionFolder).First();
            return abstractionID;
        }
    }

    internal static class CmdSupport
    {

        public static string[] ExecuteScriptWithOutput(string arguments, string workingDirectory)
        {
            return ExecuteWithOutput("cmd", "/c " + arguments, workingDirectory);
        }

        public static string[] ExecuteGitWithOutput(string arguments, string workingDirectory)
        {
            return ExecuteWithOutput("cmd", "/c git " + arguments, workingDirectory);
        }

        public static string[] ExecuteWithOutput(string command, string arguments, string workingDirectory)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.FileName = command;
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            Process proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();
            proc.WaitForExit();
            string stdOutput = proc.StandardOutput.ReadToEnd();
            return stdOutput.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}