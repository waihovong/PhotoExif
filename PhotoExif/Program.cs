using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using ConsoleTables;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
public class Program
{
    public class ImageExif
    {
        public string? File { get; set; }
        public string? FStop { get; set; }
        public string? ISO { get; set; }
        public string? ShutterSpeed { get; set; }
        public string? FocalLength { get; set; }
        public string? ExposureBias { get; set; }
        public string? DateTime { get; set; }
        public string? WhiteBalance { get; set; }
        public string? CameraMakeModel { get; set; }
    }

    public enum ImageFileExtention
    {
        RAF,
        JPG,
        PNG,
        //add more as needed
    }

    public static List<ImageExif> GetImagesFromPath(string searchFolder)
    {
        List<ImageExif> exifData = new List<ImageExif>();
        if (string.IsNullOrEmpty(searchFolder))
        {
            Console.WriteLine("File not found");
            return exifData;
        }

        string[] imageFiles = System.IO.Directory.GetFiles(searchFolder);

        if (imageFiles == null)
        {
            Console.WriteLine("File not found");
            return exifData;
        }

        var filterImageFiles = imageFiles.Where(
            r => r.Contains(ImageFileExtention.JPG.ToString(), StringComparison.OrdinalIgnoreCase)); // Update this to your image extention

        if (filterImageFiles == null)
        {
            Console.WriteLine("Images not found");
            return exifData;
        }

        try
        {
            foreach (var image in filterImageFiles)
            {
                var directories = ImageMetadataReader.ReadMetadata(image);

                //Image related exif
                var exifSubIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                if (exifSubIfdDirectory != null)
                {
                    // Exposure Bias simplified
                    var descriptor = new ExifSubIfdDescriptor(exifSubIfdDirectory);
                    String evBias = descriptor.GetExposureBiasDescription();
                    //for camera make and model
                    var exifIfd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
                    if (exifIfd0Directory != null)
                    {
                        exifData.Add(new ImageExif
                        {
                            File = image.Split("\\").Last(),
                            FStop = exifSubIfdDirectory.GetDescription(ExifSubIfdDirectory.TagFNumber),
                            ISO = exifSubIfdDirectory.GetDescription(ExifSubIfdDirectory.TagIsoEquivalent),
                            ShutterSpeed = exifSubIfdDirectory.GetDescription(ExifSubIfdDirectory.TagExposureTime),
                            FocalLength = exifSubIfdDirectory.GetDescription(ExifSubIfdDirectory.TagFocalLength),
                            ExposureBias = evBias,
                            DateTime = exifSubIfdDirectory.GetDescription(ExifSubIfdDirectory.TagDateTimeOriginal),
                            WhiteBalance = exifSubIfdDirectory.GetDescription(ExifSubIfdDirectory.TagWhiteBalanceMode),
                            CameraMakeModel = exifIfd0Directory.GetDescription(ExifIfd0Directory.TagMake) + " " + exifIfd0Directory.GetDescription(ExifIfd0Directory.TagModel)
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading EXIF data {ex.Message}");
        }

        return exifData;
    }

    public static void PrintImageExifTable(List<ImageExif> images)
    {
        var table = new ConsoleTable("File", "F-Stop", "ISO", "Shutter Speed", "Focal Length", "Exposure Bias", "Date Time YYYY/MM/DD HH:MM", "White Balance", "Camera");

        // Print data
        foreach (var image in images)
        {
            table.AddRow(
                image.File, 
                image.FStop, 
                $"ISO-{image.ISO}", 
                image.ShutterSpeed, 
                image.FocalLength, 
                image.ExposureBias, 
                image.DateTime, 
                image.WhiteBalance, 
                image.CameraMakeModel
            );
        }

        table.Write();
        Console.WriteLine();
    }

    public static List<(string FStop, int Count)> GetFStopTable(List<ImageExif> _images)
    {
        return _images.GroupBy(x => x.FStop)
                   .OrderByDescending(g => g.Count())
                   .Select(g => (FStop: g.Key, Count: g.Count()))
                   .ToList();
    }

    public static List<(string ISO, int Count)> GetISOTable(List<ImageExif> _images)
    {
        return _images.GroupBy(x => x.ISO)
                   .OrderByDescending(g => g.Count())
                   .Select(g => (ISO: g.Key, Count: g.Count()))
                   .ToList();
    }

    public static List<(string FStop, int Count)> GetFocalLengthTable(List<ImageExif> _images)
    {
        return _images.GroupBy(x => x.FocalLength)
                   .OrderByDescending(g => g.Count())
                   .Select(g => (FStop: g.Key, Count: g.Count()))
                   .ToList();
    }

    public static List<(string ShutterSpeed, int Count)> GetShutterSpeedTable(List<ImageExif> _images)
    {
        return _images.GroupBy(x => x.ShutterSpeed)
                   .OrderByDescending(g => g.Count())
                   .Select(g => (ShutterSpeed: g.Key, Count: g.Count()))
                   .ToList();
    }

    public static void PrintCommonExifTable(List<(string fStop, int Count)> fstop, List<(string iso, int Count)> iso, List<(string focalLength, int Count)> focalLength, List<(string ShutterSpeed, int Count)> shutterSpeed)
    {
        var table = new ConsoleTable("F-Stop", "Count", "ISO", "Count", "Focal Length", "Count", "Shutter Speed", "Count");

        // Print data
        int rows = Math.Max(fstop.Count, Math.Max(iso.Count, Math.Max(shutterSpeed.Count, focalLength.Count)));

        for(int i = 0; i < rows; i++)
        {
            string fstopValue = i < fstop.Count ? fstop[i].fStop : "";
            int fstopCount = i < fstop.Count ? fstop[i].Count : 0;

            string isoValue = i < iso.Count ? iso[i].iso : "";
            int isoCount = i < iso.Count ? iso[i].Count : 0;

            string focalLengthValue = i < focalLength.Count ? focalLength[i].focalLength : "";
            int focalLengthCount = i < focalLength.Count ? focalLength[i].Count : 0;

            string shutterSpeedValue = i < shutterSpeed.Count ? shutterSpeed[i].ShutterSpeed : "";
            int shutterSpeedCount = i < shutterSpeed.Count ? shutterSpeed[i].Count : 0;


            table.AddRow(fstopValue, fstopCount, isoValue, isoCount, focalLengthValue, focalLengthCount, shutterSpeedValue, shutterSpeedCount);
        }

        table.Write();
        Console.WriteLine();
    }

    public static void DisplayLoadingBar(int totalSteps)
    {
        int width = 50; // The width of the loading bar
        for (int i = 0; i <= totalSteps; i++)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("[");
            int position = i * width / totalSteps;

            for (int j = 0; j < width; j++)
            {
                if (j < position)
                {
                    Console.Write("=");
                }
                else if (j == position)
                {
                    Console.Write(">");
                }
                else
                {
                    Console.Write(" ");
                }
            }

            Console.Write($"] {i}%");
        }
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("\n################### PhotoExif ###################");

        Stopwatch sw = new Stopwatch();
        sw.Start();
        List<ImageExif> images = Program.GetImagesFromPath(""); // Update This to your image path
        var fstopTable = Program.GetFStopTable(images);
        var isoTable = Program.GetISOTable(images);
        var focalLengthTable = Program.GetFocalLengthTable(images);
        var shutterSpeedTable = Program.GetShutterSpeedTable(images);

        int totalSteps = 100;

        // Display the loading bar
        Console.WriteLine("Processing...");
        DisplayLoadingBar(totalSteps);

        Console.WriteLine("\n################### Exif Stats ###################");
        PrintCommonExifTable(fstopTable, isoTable, focalLengthTable, shutterSpeedTable);

        Console.WriteLine("\n################### Individual Image Info ###################");
        PrintImageExifTable(images);

        Console.WriteLine("\nComputation completed.");
        sw.Stop();
        Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
    }
}
