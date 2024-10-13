#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;
using UnityEditor;


public static class Setup
{
    [MenuItem("Tools/Setup/Create Default Folders")]
    public static void CreateDefaultFolders()
    {
        Folders.CreateDefault("_Project", "Animation", "Art", "Materials", "Prefabs", "ScriptableObjects", "Scripts", "Settings");
        UnityEditor.AssetDatabase.Refresh();
    }

    static class Folders
    {
        public static void CreateDefault(string root, params string[] folders)
        {
            string fullpath = Path.Combine(Application.dataPath, root);
            foreach (var folder in folders)
            {
                var path = Combine(fullpath, folder);
                if (!Directory.Exists(path))  // Fix here
                {
                    Directory.CreateDirectory(path);  // Fix here
                }
            }
        }
    }
}
#endif
