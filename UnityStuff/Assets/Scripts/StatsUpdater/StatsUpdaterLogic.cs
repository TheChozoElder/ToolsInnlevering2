using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using innlevering2.Model;
using Newtonsoft.Json;
using UnityEngine;
using System.Reflection;

public class StatsUpdaterLogic : MonoBehaviour
{
    private const string EnemyParentComponent = "Enemies";

    #region Public Methods
    public void ExportStats(string filePath)
    {
        List<GameObject> unityObjects = new List<GameObject>();
        List<string> unityObjectNames = new List<string>();

        StatsObjectList statsObjects = new StatsObjectList
        {
            UnnamedEntities = new List<StatsObject>(),
            NamedEntities = new List<StatsObject>()
        };

        GetGameObjectsFromUnity(unityObjects, unityObjectNames, null);

        foreach (GameObject gameObject in unityObjects)
        {
            StatsObject newStatsObject = new StatsObject
            {
                InstanceID = gameObject.GetInstanceID(),
                Name = gameObject.name,
                ScaleX = gameObject.transform.localScale.x,
                ScaleY = gameObject.transform.localScale.y,
                ScaleZ = gameObject.transform.localScale.z,
                TurningSpeed = -1,
                AimingSpeed = -1
            };
            SetHealthVariables(gameObject, newStatsObject, false);
            SetSpeedVariables(gameObject, newStatsObject, false);

            int firstOccurenceOfName = unityObjectNames.IndexOf(gameObject.name);
            if (unityObjectNames.IndexOf(gameObject.name, firstOccurenceOfName + 1) >= 0)
            {
                statsObjects.UnnamedEntities.Add(newStatsObject);
            }
            else
            {
                statsObjects.NamedEntities.Add(newStatsObject);
            }
        }

        using (var writer = new StreamWriter(filePath))
        {
            writer.Write((statsObjects.Serialize()));
        }
    }

    public void ImportStats(string filePath)
    {
        var unityObjects = new List<GameObject>();
        var unityObjectIDs = new List<int>();

        var statsObjects = new StatsObjectList
        {
            UnnamedEntities = new List<StatsObject>(),
            NamedEntities = new List<StatsObject>()
        };

        GetGameObjectsFromUnity(unityObjects, null, unityObjectIDs);

        var jsonStream = new StreamReader(filePath);
        var jsonString = jsonStream.ReadToEnd();
        jsonStream.Close();

        statsObjects.Deserialize(jsonString);

        foreach (StatsObject statsObject in statsObjects)
        {
            int currentObjectID = 0;
            int currentObjectIDIndex = -1;
            while (currentObjectID != statsObject.InstanceID )
            {
                currentObjectIDIndex++;
                currentObjectID = unityObjectIDs[currentObjectIDIndex];
            }
            Debug.Log(unityObjects.Count);
            GameObject unityObject = unityObjects[currentObjectIDIndex];

            unityObject.transform.localScale = new Vector3(statsObject.ScaleX, statsObject.ScaleY, statsObject.ScaleZ);
            SetHealthVariables(unityObject, statsObject, true);
            SetSpeedVariables(unityObject, statsObject, true);
        }
    }
    #endregion

    #region Private Methods
    private void GetGameObjectsFromUnity(List<GameObject> listOfObjectsToFill, List<string> listOfNamesToFill, List<int> listOfIDsToFill)
    {
        GameObject player = GameObject.Find("Player");
        listOfObjectsToFill.Add(player);
        if(listOfNamesToFill != null)
            listOfNamesToFill.Add(player.name);
        if(listOfIDsToFill != null)
            listOfIDsToFill.Add(player.GetInstanceID());

        GameObject enemies = GameObject.Find(EnemyParentComponent);
        GetChildrenWithHealthScript(enemies.transform, listOfObjectsToFill, listOfNamesToFill, listOfIDsToFill);
    }

    private void GetChildrenWithHealthScript(Transform parent, List<GameObject> listOfObjectsToFill, List<string> listOfNamesToFill, List<int> listOfIDsToFill)
    {
        foreach (Transform possiblyRelevantObject in parent.transform)
        {
            if (possiblyRelevantObject.GetComponent("Health") != null)
            {
                listOfObjectsToFill.Add(possiblyRelevantObject.gameObject);
                if(listOfNamesToFill != null)
                    listOfNamesToFill.Add(possiblyRelevantObject.gameObject.name);
                if(listOfIDsToFill != null)
                    listOfIDsToFill.Add(possiblyRelevantObject.gameObject.GetInstanceID());
            }
            else if (possiblyRelevantObject.childCount > 0)
            {
                GetChildrenWithHealthScript(possiblyRelevantObject, listOfObjectsToFill, listOfNamesToFill, listOfIDsToFill);
            }
        }
    }

    private void SetHealthVariables(GameObject unityObject, StatsObject statsObject, bool getFromFileAndSetInUnity)
    {
        Component health = unityObject.GetComponent("Health");
        if (health != null)
        {
            Type type = health.GetType();
            FieldInfo[] fields = type.GetFields();
            foreach (var field in fields)
            {
                switch (field.Name)
                {
                    case "maxHealth":
                        if(getFromFileAndSetInUnity)
                            field.SetValue(health, statsObject.MaxHealth);
                        else
                            statsObject.MaxHealth = (float)field.GetValue(health);
                        break;
                    case "health":
                        if (getFromFileAndSetInUnity)
                            field.SetValue(health, statsObject.Health);
                        else
                            statsObject.Health = (float)field.GetValue(health);
                        break;
                    case "regenerateSpeed":
                        if (getFromFileAndSetInUnity)
                            field.SetValue(health, statsObject.RegenerateSpeed);
                        else
                            statsObject.RegenerateSpeed = (float)field.GetValue(health);
                        break;
                    case "invincible":
                        if (getFromFileAndSetInUnity)
                            field.SetValue(health, statsObject.Invincible);
                        else
                            statsObject.Invincible = (bool)field.GetValue(health);
                        break;
                }
            }
        }
    }

    private void SetSpeedVariables(GameObject unityObject, StatsObject statsObject, bool getFromFileAndSetInUnity)
    {
        Component speed = unityObject.GetComponent("FreeMovementMotor");
        if (speed == null)
        {
            speed = unityObject.GetComponent("KamikazeMovementMotor");
            if (speed == null)
            {
                speed = unityObject.GetComponent("MechMovementMotor");
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
                    if (getFromFileAndSetInUnity)
                        field.SetValue(speed, statsObject.MovementSpeed);
                    else
                        statsObject.MovementSpeed = (float)field.GetValue(speed);
                }
                else if (field.Name == "turningSpeed")
                {
                    if (getFromFileAndSetInUnity)
                        field.SetValue(speed, statsObject.TurningSpeed);
                    else
                        statsObject.TurningSpeed = (float)field.GetValue(speed);
                }
                else if (field.Name == "aimingSpeed")
                {
                    if (getFromFileAndSetInUnity)
                        field.SetValue(speed, statsObject.AimingSpeed);
                    else
                        statsObject.AimingSpeed = (float)field.GetValue(speed);
                }
            }
        }
    }
    #endregion
}