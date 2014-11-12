using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using System.Reflection;

public class StatsObject
{
	public string Name { get; set; }

    public Vector3 Scale { get; set; }

    public float MaxHealth { get; set; }
    public float Health { get; set; }
    public float RegenerateSpeed { get; set; }
    public bool Invincible { get; set; }

    public float MovementSpeed { get; set; }
    public float TurningSpeed { get; set; }
    public float AimingSpeed { get; set; }


	public static string Serialize(StatsObject enemy)
	{
		var settings = new JsonSerializerSettings();

		return JsonConvert.SerializeObject(enemy, Formatting.Indented, settings);
	}
}

public class StatsUpdater : MonoBehaviour
{
    private const string FileName = @"Assets\Scripts\Misc\stats.json";
    private const string EnemyParentComponent = "Enemies";

    private List<GameObject> relevantGameObjects = new List<GameObject>();

    private List<StatsObject> namedEntities = new List<StatsObject>();
    private List<StatsObject> unnamedEntities = new List<StatsObject>();
	// Use this for initialization
	void Start ()
	{
        GameObject enemies = GameObject.Find(EnemyParentComponent);
	    CheckForHealthScriptInChildren(enemies.transform);
	    foreach (GameObject gameObject in relevantGameObjects)
	    {
	        StatsObject newStatsObject = new StatsObject { Name = gameObject.name, Scale = gameObject.transform.localScale};
	        SetHealthVariables(gameObject, newStatsObject);
            Debug.Log(newStatsObject.Name + " has a health of " + newStatsObject.Health);
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
                CheckForHealthScriptInChildren(possiblyRelevantObject);
            }
        }
    }

    private void SetHealthVariables(GameObject inputObject, StatsObject outputObject)
    {
        Component health = inputObject.GetComponent("Health");
        if (health != null)
        {
            Type type = health.GetType();
            FieldInfo[] fields = type.GetFields();
            int numberOfFoundFields = 0;
            foreach (var field in fields)
            {
                switch (field.Name)
                {
                    case "maxHealth":
                        outputObject.MaxHealth = (float)field.GetValue(health);
                        numberOfFoundFields++;
                        break;
                    case "health":
                        outputObject.Health = (float)field.GetValue(health);
                        numberOfFoundFields++;
                        break;
                    case "regenerateSpeed":
                        outputObject.RegenerateSpeed = (float)field.GetValue(health);
                        numberOfFoundFields++;
                        break;
                }
                if(numberOfFoundFields >= 3)
                    break;
            }
        }
    }

    // Update is called once per frame
	void Update () {
	
	}
}
