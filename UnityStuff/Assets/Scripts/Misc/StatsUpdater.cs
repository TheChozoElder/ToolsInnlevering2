using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Enemy
{
	public string Name { get; set; }

    public int Health { get; set; }

	public static string Serialize(Enemy enemy)
	{
		var settings = new JsonSerializerSettings();

		return JsonConvert.SerializeObject(enemy, Formatting.Indented, settings);
	}
}

public class StatsUpdater : MonoBehaviour
{
    const string FileName = @"Assets\Scripts\Misc\stats.json";

	// Use this for initialization
	void Start ()
	{
	    string serialized = "";
        GameObject enemies = GameObject.Find("Enemies");
        
        foreach (Transform child in enemies.transform)
        {
            var enemy = new Enemy { Name = child.name };

            serialized += Enemy.Serialize(enemy);
        }
        using (var writer = new StreamWriter(FileName))
        {
            writer.Write(serialized);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
