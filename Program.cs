using System;
using System.Collections.Immutable;
using System.IO;
using MimeDetective;
using MimeDetective.Engine;
using MimeMapping;

class Program
{
    static void Main(string[] args)
    {
        string rootDirectory = ".";
        CheckDirectory(rootDirectory);
    }

    static void CheckDirectory(string directoryPath)
    {
        try
        {
            string[] files = Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                CheckFile(file);
            }

            string[] subDirectories = Directory.GetDirectories(directoryPath);
            foreach (string subDirectory in subDirectories)
            {
                CheckDirectory(subDirectory);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing {directoryPath}: {ex.Message}");
        }
    }

    static void CheckFile(string filePath)
    {
        try
        {
            string extension = Path.GetExtension(filePath).ToLower();
            if (string.IsNullOrEmpty(extension))
            {
                return; // Skip files without extensions
            }

            byte[] fileData = File.ReadAllBytes(filePath);
            var inspector = new ContentInspectorBuilder().Build();
            ImmutableArray<DefinitionMatch> matches = inspector.Inspect(fileData);
            var contentType = GetMostLikelyMimeType(matches);

            string expectedMimeType = MimeUtility.GetMimeMapping(filePath);

            if (!expectedMimeType.Equals(contentType, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Mismatch: {filePath} (Extension: {extension} | Content Type: {contentType})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing {filePath}: {ex.Message}");
        }
    }

    static string GetMostLikelyMimeType(ImmutableArray<DefinitionMatch> matches)
    {
        if (matches.Any())
        {
            // Assuming the first match is the best one
            var bestMatch = matches.First();
            return bestMatch.Definition.
        }
        return "unknown/unknown";
    }
}
