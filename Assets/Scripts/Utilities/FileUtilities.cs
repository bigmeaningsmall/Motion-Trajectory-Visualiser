using UnityEngine;
using System.IO;

public static class FileUtilities
{
    public static string RemoveFileExtension(string filePath)
    {
        int lastDotIndex = filePath.LastIndexOf('.');
        if (lastDotIndex == -1)
        {
            Debug.LogWarning("The specified file path does not have a file extension.");
            return filePath;
        }
        else
        {
            return filePath.Substring(0, lastDotIndex);
        }
    }
    
    public static string GetFileName(string filePath, string fileType)
    {
        // Get the file name from the path
        string fileName = Path.GetFileName(filePath);

        // Get the file extension (e.g. ".csv")
        string extension = Path.GetExtension(filePath);

        // Check if the file extension is ".csv"
        if (extension == "." + fileType)
        {
            return fileName;
        }
        else
        {
            Debug.LogWarning("The specified file is not a .csv file.");
            return null;
        }
    }
    
    public static bool CheckFileExtension(string filePath, string extensionToCheck)
    {
        string fileExtension = Path.GetExtension(filePath);

        if (fileExtension == extensionToCheck)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
