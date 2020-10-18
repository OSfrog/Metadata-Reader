using System;
using System.IO;
using System.Threading;

namespace MetadataReader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LoadData(PathChecker(args));
        }

        private static string PathChecker(string[] args)
        {
            string path;

            if (args.Length != 0)
            {
                path = args[0];
            }
            else
            {
                Console.WriteLine("Input filepath to be read: ");
                path = Console.ReadLine();
            }

            while (!File.Exists(path))
            {
                Console.WriteLine("File not found!");
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("Please insert a proper path from your computer: ");
                path = Console.ReadLine();
            }

            return path;
        }

        private static void LoadData(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open);

            var data = DataChecker.ReadFileHeader(fileStream);

            var fileType = DataChecker.FileChecker(data);
            var fileResultString = fileType != Filetypes.Invalid
                ? $"This file is a {fileType}.\n"
                : "This file is invalid!";

            Console.WriteLine($"{fileResultString}");

            var resolution = DataChecker.GetResolution(fileStream, fileType);
            Console.WriteLine($"The resolution is {resolution}.\n");

            if (fileType == Filetypes.PNG)
            {
                foreach (var chunk in DataChecker.GetImageInfo(fileStream))
                {
                    Console.WriteLine($"Chunk number {chunk.ChunkNumber}: type/name: {chunk.Type}, chunksize: {chunk.Size} bytes.");
                }
            }

            Console.ReadKey();
        }
    }
}