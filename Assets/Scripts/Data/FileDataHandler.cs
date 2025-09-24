using UnityEngine;
using System;
using System.IO;
// Code Written By Nathaniel King <3
// With help of how from https://www.youtube.com/watch?v=aUi9aijvpgs&list=WL&index=241&t=1422s.

public class FileDataHandler
{
    private string directivePath = "";
    private string fileName;

    public FileDataHandler(string inDirectivePath, string inFileName)
    {
        directivePath = inDirectivePath;
        fileName = inFileName;
    }

    public gameData Load()
    {
        string path = Path.Combine(directivePath, fileName);
        gameData loadData = null;
        if (File.Exists(path))
        {
            try
            {
                string loadingData = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        loadingData = reader.ReadToEnd();
                    }
                        
                }

                loadData = JsonUtility.FromJson<gameData>(loadingData);
            }
            catch (Exception ex)
            {
                Debug.LogError("Nemrod, There ain't no directive at this point: " + path + ex);
            }
        }
        return loadData;
    }

    public void Save(gameData data)
    {
        string path = Path.Combine(directivePath, fileName); // Combining these can help find directives that don't use '/'.
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)); // Making a directory at this point if there isn't any.

            string newSavedData = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(newSavedData);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Nemrod, There ain't no directive at this point: " + path + ex);
        }
    }
}
