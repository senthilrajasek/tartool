using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;

namespace Tools.CommandLine
{

    public class TarTool
    {
        private static void Usage()
        {
            Console.Out.WriteLine(@"Usage : >TarTool sourceFile destinationDirectory");
            Console.Out.WriteLine(@">TarTool D:\sample.tar.gz ./");
            Console.Out.WriteLine(@">TarTool sample.tgz temp");
            Console.Out.WriteLine(@">TarTool -x sample.tar temp");
        }

        private static void JustUntar(string sourceFile, string dstDir)
        {
            TarArchive archive = null;

            try
            {
                FileStream inStream = File.OpenRead(sourceFile);

                TarInputStream tarIn = new TarInputStream(inStream);

                archive = TarArchive.CreateInputTarArchive(tarIn);

                if (!string.IsNullOrEmpty(dstDir))
                    archive.ExtractContents(dstDir);
                else
                    archive.ExtractContents("./");

                archive.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error extracting {0} ", sourceFile));
                Console.WriteLine(string.Format("Exception message {0}", ex.Message));
                Usage();
            }
            finally
            {
                if (archive != null)
                    archive.Close();
            }
        }

        public static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Usage();
                return;
            }

            string sourceFile = args[0];

            string dstDir = null;

            if (args.Length == 2)
                dstDir = args[1];

            if (!string.IsNullOrEmpty(args[0]))
            {
                if (args[0].StartsWith("-", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (string.Compare("-x", args[0], StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        sourceFile = args[1];

                        if (args.Length == 3)
                            dstDir = args[2];

                        JustUntar(sourceFile, dstDir);
                        return;
                    }
                }
            }

            TarArchive archive = null;

            try
            {
                GZipInputStream inStream = new GZipInputStream(File.OpenRead(sourceFile));

                TarInputStream tarIn = new TarInputStream(inStream);

                archive = TarArchive.CreateInputTarArchive(tarIn);

                if (!string.IsNullOrEmpty(dstDir))
                    archive.ExtractContents(dstDir);
                else
                    archive.ExtractContents("./");

                archive.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error extracting {0} ", sourceFile));
                Console.WriteLine(string.Format("Exception message {0}", ex.Message));
                Usage();
            }
            finally
            {
                if (archive != null)
                    archive.Close();
            }
        }
    }
}
