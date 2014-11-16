using System.IO;
using UnityEditor;
using UnityEngine;
public class StatsUpdaterWindow : EditorWindow
{
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Stats Updater")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        StatsUpdaterWindow window = (StatsUpdaterWindow)EditorWindow.GetWindow(typeof(StatsUpdaterWindow));
    }

    void OnGUI()
    { 
        
        if (GUILayout.Button("Export stats"))
        {
            StatsUpdaterLogic.ExportStats();
        }
        else if (GUILayout.Button("Import stats"))
        {
            StatsUpdaterLogic.ImportStats();
        }
        else if (GUILayout.Button("Change JSON file"))
        {
            StatsUpdaterLogic.filePath = EditorUtility.OpenFilePanel("Select JSON file to use", string.Empty, "json");
        }
    }
}