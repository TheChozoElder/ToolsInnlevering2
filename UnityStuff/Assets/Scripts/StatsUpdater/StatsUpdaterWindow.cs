using UnityEngine;
using UnityEditor;
public class StatsUpdaterWindow : EditorWindow
{
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Stats updater")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        StatsUpdaterWindow window = (StatsUpdaterWindow)EditorWindow.GetWindow(typeof(StatsUpdaterWindow));
    }

    void OnGUI()
    { 
        if (GUILayout.Button("Import"))
        {
            
        }
        else if (GUILayout.Button("Export"))
        {
            GameObject statsUpater = GameObject.Find("StatsUpdater");
            StatsUpdaterLogic statsUpdaterLogic = statsUpater.GetComponent<StatsUpdaterLogic>();
            statsUpdaterLogic.ExportStats();
        }
    }
}