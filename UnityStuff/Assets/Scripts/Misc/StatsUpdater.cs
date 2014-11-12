using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class StatsObject
{
	public string Name { get; set; }

    public Vector3 Scale { get; set; }

    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int RegenerateSpeed { get; set; }
    public bool Invincible { get; set; }

	public static string Serialize(StatsObject enemy)
	{
		var settings = new JsonSerializerSettings();

		return JsonConvert.SerializeObject(enemy, Formatting.Indented, settings);
	}
}

public class StatsUpdater : MonoBehaviour
{
    const string FileName = @"Assets\Scripts\Misc\stats.json";

    private const string enemyParentComponent = "Enemies";

    private List<GameObject> relevantGameObjects = new List<GameObject>();

    private List<StatsObject> namedEntities = new List<StatsObject>();
    private List<StatsObject> unnamedEntities = new List<StatsObject>();
	// Use this for initialization
	void Start ()
	{
        GameObject enemies = GameObject.Find(enemyParentComponent);
	    CheckForHealthScriptInChildren(enemies.transform);
	    foreach (GameObject gameObject in relevantGameObjects)
	    {
	        
	    }
	}

    private void CheckForHealthScriptInChildren(Transform enemies)
    {
        foreach (Transform possiblyRelevantObject in enemies.transform)
        {
            if (possiblyRelevantObject.GetComponent("Health") != null)
            {
                relevantGameObjects.Add(possiblyRelevantObject.gameObject);
            }
            else if(possiblyRelevantObject.childCount > 0)
            {
                this.CheckForHealthScriptInChildren(possiblyRelevantObject);
            }
        }
    }

    // Update is called once per frame
	void Update () {
	
	}
}
