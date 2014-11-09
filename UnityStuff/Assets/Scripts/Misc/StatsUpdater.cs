using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public class Enemy
{
	public string Name { get; set; }

	public static string Serialize(Enemy enemy)
	{
		var settings = new JsonSerializerSettings();

		return JsonConvert.SerializeObject(enemy, Formatting.Indented, settings);
	}

}

public class StatsUpdater : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{

        var fileName = @"Assets\Scripts\Misc\stats.json";
	    string serialized = "";
        GameObject enemies = GameObject.Find("Enemies");

        foreach (Transform child in enemies.transform)
        {
			var enemy = new Enemy();
			enemy.Name = child.name;

            serialized += Enemy.Serialize(enemy);
        }
        using (var writer = new StreamWriter(fileName))
        {
            writer.Write(serialized);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
