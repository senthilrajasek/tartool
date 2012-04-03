using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;

namespace Tools.CommandLine
{
    enum CompressionTechnique
    {
        Zip = 0,
        Gzip = 1,
        BZip2 = 2
    }

    public class TarTool
    {
        private static void Usage()
        {
            Console.Out.WriteLine(@"Usage : >TarTool -options sourceFile destinationDirectory");
            Console.Out.WriteLine(@">TarTool D:\sample.tar.gz ./");
            Console.Out.WriteLine(@">TarTool sample.tgz temp");
            Console.Out.WriteLine(@">TarTool -xj sample.tar.bz2 temp");
            Console.Out.WriteLine(@">TarTool -x sample.tar temp");
            Console.Out.WriteLine(@">TarTool -j sample.bz2 ");
        }

        private static void DeCompressArchive(string sourceFile, string dstDir, CompressionTechnique compressionTechnique)
        {
            TarArchive archive = null;
            Stream inStream = null;

            try
            {
                switch (compressionTechnique)
                {
                    case CompressionTechnique.Gzip:
                        inStream = new GZipInputStream(File.OpenRead(sourceFile));
                        break;
                    case CompressionTechnique.BZip2:
                        inStream = new BZip2InputStream(File.OpenRead(sourceFile));
                        break;
                    default:
                        Console.WriteLine("Unknown Compression Technique.");
                        break;
                }

                if (inStream == null)
                {
                    Console.WriteLine(string.Format("Error reading {0} ", sourceFile));
                    return;
                }

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

        private static void JustDecompress(string sourceFile, CompressionTechnique compressionTechnique)
        {
            Stream inStream = null;
            try
            {
                switch (compressionTechnique)
                {
                    case CompressionTechnique.Gzip:
                        inStream = new GZipInputStream(File.OpenRead(sourceFile));
                        break;
                    case CompressionTechnique.BZip2:
                        inStream = new BZip2InputStream(File.OpenRead(sourceFile));
                        break;
                    default:
                        Console.WriteLine("Unknown Compression Technique.");
                        break;
                }

                if (inStream == null)
                {

                    Console.WriteLine(string.Format("Error reading {0} ", sourceFile));
                    return;
                }

                using (inStream)
                using (FileStream outStream = File.Create(Path.GetFileNameWithoutExtension(sourceFile)))
                {
                    byte[] buffer = new byte[4096];
                    StreamUtils.Copy(inStream, outStream, buffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error extracting {0} ", sourceFile));
                Console.WriteLine(string.Format("Exception message {0}", ex.Message));
                Usage();
            }
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
                    else if(string.Compare("-j", args[0], StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        sourceFile = args[1];

                        JustDecompress(sourceFile, CompressionTechnique.BZip2);
                        return;
                    }
                    else if (string.Compare("-jx", args[0], StringComparison.CurrentCultureIgnoreCase) == 0
                        || string.Compare("-xj", args[0], StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        sourceFile = args[1];

                        if (args.Length == 3)
                            dstDir = args[2];

                        DeCompressArchive(sourceFile, dstDir, CompressionTechnique.BZip2);
                        return;
                    }
                }
            }

            DeCompressArchive(sourceFile, dstDir, CompressionTechnique.Gzip);
            
        }
    }
}
