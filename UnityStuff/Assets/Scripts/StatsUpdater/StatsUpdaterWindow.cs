using UnityEngine;
using UnityEditor;
public class StatsUpdaterWindow : EditorWindow
{
    private string filePath = @"Assets\Scripts\StatsUpdater\stats.json";

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Stats Updater")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        StatsUpdaterWindow window = (StatsUpdaterWindow)EditorWindow.GetWindow(typeof(StatsUpdaterWindow));
    }

    void OnGUI()
    { 
        if (GUILayout.Button("Import"))
        {
            GameObject statsUpater = GameObject.Find("StatsUpdater");
            StatsUpdaterLogic statsUpdaterLogic = statsUpater.GetComponent<StatsUpdaterLogic>();
            statsUpdaterLogic.ImportStats(filePath);
        }
        else if (GUILayout.Button("Export"))
        {
            GameObject statsUpater = GameObject.Find("StatsUpdater");
            StatsUpdaterLogic statsUpdaterLogic = statsUpater.GetComponent<StatsUpdaterLogic>();
            statsUpdaterLogic.ExportStats(filePath);
        }
        else if (GUILayout.Button("Change JSON file"))
        {
            filePath = EditorUtility.OpenFilePanel("Select JSON file to use", "", "json");
        }
    }
}