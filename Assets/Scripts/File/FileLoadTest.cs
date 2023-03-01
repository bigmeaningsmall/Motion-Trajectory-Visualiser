using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileLoadTest : MonoBehaviour
{
    // The name of the file we want to load
    public string fileName = "";

    // Start is called before the first frame update
    void Start()
    {
        // Get the persistent data path
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Load the file as a TextAsset
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            string text = System.Text.Encoding.UTF8.GetString(bytes);
            TextAsset textAsset = new TextAsset(text);
            

            // Print the contents of the file to the console
            Debug.Log(textAsset.text);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}
