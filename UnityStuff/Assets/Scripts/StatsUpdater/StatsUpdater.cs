using System;
using System.Collections.Generic;
using System.IO;

using innlevering2.Model;

using Newtonsoft.Json;
using UnityEngine;
using System.Reflection;

public class StatsUpdater : MonoBehaviour
{
    private const string FileName = @"Assets\Scripts\Misc\stats.json";
    private const string EnemyParentComponent = "Enemies";

    private List<GameObject> relevantGameObjects = new List<GameObject>();
    private List<string> relevantGameObjectsNames = new List<string>();

    private StatsObjectList stats = new StatsObjectList
                                        {
                                            UnnamedEntities = new List<StatsObject>(),
                                            NamedEntities = new List<StatsObject>()
                                        };

	// Use this for initialization
	void Start ()
	{
	    this.FillGameObjectsList();

	    foreach (GameObject gameObject in relevantGameObjects)
	    {
	        StatsObject newStatsObject = new StatsObject {
	            Name = gameObject.name, 
                ScaleX = gameObject.transform.localScale.x,
                ScaleY = gameObject.transform.localScale.y,
                ScaleZ = gameObject.transform.localScale.z
	        };
	        SetHealthVariables(gameObject, newStatsObject);
	        SetSpeedVariables(gameObject, newStatsObject);

	        int firstOccurenceOfName = relevantGameObjectsNames.IndexOf(gameObject.name);
	        if (relevantGameObjectsNames.IndexOf(gameObject.name, firstOccurenceOfName+1) >= 0)
	        {
	            stats.UnnamedEntities.Add(newStatsObject);
	        }
	        else
	        {
	            stats.NamedEntities.Add(newStatsObject);
	        }
	    }

        Export();
	}

    private void FillGameObjectsList()
    {
        GameObject player = GameObject.Find("Player");
        relevantGameObjects.Add(player);
        relevantGameObjectsNames.Add(player.name);

        GameObject enemies = GameObject.Find(EnemyParentComponent);
        CheckForHealthScriptInChildren(enemies.transform);
    }

    private void CheckForHealthScriptInChildren(Transform enemies)
    {
        foreach (Transform possiblyRelevantObject in enemies.transform)
        {
            if (possiblyRelevantObject.GetComponent("Health") != null)
            {
                relevantGameObjects.Add(possiblyRelevantObject.gameObject);
                relevantGameObjectsNames.Add(possiblyRelevantObject.gameObject.name);
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
            foreach (var field in fields)
            {
                switch (field.Name)
                {
                    case "maxHealth":
                        outputObject.MaxHealth = (float)field.GetValue(health);
                        break;
                    case "health":
                        outputObject.Health = (float)field.GetValue(health);
                        break;
                    case "regenerateSpeed":
                        outputObject.RegenerateSpeed = (float)field.GetValue(health);
                        break;
                    case "invincible":
                        outputObject.Invincible = (bool)field.GetValue(health);
                        break;
                }
            }
        }
    }

    private void SetSpeedVariables(GameObject inputObject, StatsObject outputObject)
    {
        Component speed = inputObject.GetComponent("FreeMovementMotor");
        if (speed == null)
        {
            speed = inputObject.GetComponent("KamikazeMovementMotor");
            if (speed == null)
            {
                speed = inputObject.GetComponent("MechMovementMotor");
            }
        }

        if (speed != null)
        {
            Type type = speed.GetType();
            FieldInfo[] fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.Name == "flyingSpeed" || field.Name == "walkingSpeed")
                {
                    outputObject.MovementSpeed = (float)field.GetValue(speed);
                }
                else if (field.Name == "turningSpeed")
                {
                    outputObject.TurningSpeed = (float)field.GetValue(speed);
                }
                else if (field.Name == "aimingSpeed")
                {
                    outputObject.AimingSpeed = (float)field.GetValue(speed);
                }
            }
        }
    }
    private void Export()
    {

        if (stats == null)
        {
            //				SetInfoText("Nothing to export!");
            return;
        }

        using (var writer = new StreamWriter(FileName))
        {
            writer.Write((stats.Serialize()));
        }
        //			SetInfoText("All data exported");
    }

    // Update is called once per frame
	void Update () {
	
	}
}
