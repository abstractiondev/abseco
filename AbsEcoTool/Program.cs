using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CommandLine.Utility;

namespace AbsEcoTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Arguments arguments = new Arguments(args);
            if(args == null || args.Length < 2)
            {
                printUsage();
                return;
            }
            //Debugger.Launch();
            string command = args[0];
            switch (command)
            {
                case "add":
                    {
                        string absPath = arguments.RequireParameter("abspath");
                        string dirPath = arguments.RequireParameter("dirpath");
                        EcoSupport.AddToDirectory(absPath, dirPath);
                        break;
                    }
                case "sign":
                    {
                        string absPath = arguments.RequireParameter("abspath");
                        string certDir = arguments.RequireParameter("certpath");
                        string keyId = arguments.RequireParameter("key");
                        EcoSupport.SignAbstractionWithKey(absPath, certDir, keyId);
                        break;
                    }
                default:
                    printUsage();
                    break;
            }
        }

        private static void printUsage()
        {
            Console.WriteLine("Usage: AbsEcoTool add -abspath=<absdirectory> -dirpath=<directorydirectory>");
            Console.WriteLine("Usage: AbsEcoTool sign -abspath=<absdirectory> -certpath=<certdirectory> -key=<keyid>");
        }
    }
}

namespace CommandLine.Utility
{
}

