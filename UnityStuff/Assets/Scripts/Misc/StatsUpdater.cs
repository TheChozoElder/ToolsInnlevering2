using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public class Enemy : MonoBehaviour
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

	private GameObject enemies;

	// Use this for initialization
	void Start ()
	{

		var fileName = @"E:\sak\file.json";

	    enemies = GameObject.Find("Enemies");
        foreach (Transform child in enemies.transform)
        {
			var enemy = new Enemy();
			enemy.Name = child.name;

	        var jsonString = Enemy.Serialize(enemy);

			using (var writer = new StreamWriter(fileName))
	        {
		        writer.Write(jsonString);
	        }
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
