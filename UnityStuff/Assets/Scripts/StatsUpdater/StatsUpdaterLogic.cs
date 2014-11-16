using System;
using System.Collections.Generic;
using System.IO;
using innlevering2.Model;
using UnityEngine;
using System.Reflection;

public static class StatsUpdaterLogic
{
    //file path to json file
    public static string filePath = @"Assets\Scripts\StatsUpdater\stats.json";

    //name of parent component of all enemies (all objects you want to update, except for player)
    private const string EnemyParentComponent = "Enemies";

    #region Public Methods
    /// <summary>
    /// Write stats from Unity to file
    /// </summary>
    /// <param name="filePath"></param>
    public static void ExportStats()
    {
        //A list of the Unity GameObjects that will have their stats exported, and a list of their names
        List<GameObject> unityObjects = new List<GameObject>();
        List<string> unityObjectNames = new List<string>();

        GetGameObjectsFromUnity(unityObjects, unityObjectNames, null);

        //The class that will be serialized and written to file
        StatsObjectList statsObjects = new StatsObjectList
        {
            UnnamedEntities = new List<StatsObject>(),
            NamedEntities = new List<StatsObject>()
        };

        //creates new StatsObject per GameObject from Unity
        foreach (GameObject gameObject in unityObjects)
        {
            //Set stats
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

            //sort into named and unnamed entities (check if the objects name occurs more than once)
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

        //write to file
        using (var writer = new StreamWriter(filePath))
        {
            writer.Write((statsObjects.Serialize()));
        }
    }


    public static void ImportStats()
    {
        //A list of the Unity GameObjects that will have their stats imported, and a list of their unique ID's
        var unityObjects = new List<GameObject>();
        var unityObjectIDs = new List<int>();

        GetGameObjectsFromUnity(unityObjects, null, unityObjectIDs);

        //The class that will contain deserialized data from file
        var statsObjects = new StatsObjectList
        {
            UnnamedEntities = new List<StatsObject>(),
            NamedEntities = new List<StatsObject>()
        };

        //read file
        var jsonStream = new StreamReader(filePath);
        var jsonString = jsonStream.ReadToEnd();
        jsonStream.Close();

        //deserialize file into our class
        statsObjects.Deserialize(jsonString);

        //update stats in Unity
        foreach (StatsObject statsObject in statsObjects)
        {
            /* We pair objects from the file with objects in Unity by Unity's unique identifires.
             * These identifiers might change everytime the game is restarted, so exporting and importing should be done in the same session!*/
            int currentObjectIDIndex = 0;
            int currentObjectID = unityObjectIDs[currentObjectIDIndex];
            while (currentObjectID != statsObject.InstanceID && currentObjectIDIndex < unityObjectIDs.Count)
            {
                currentObjectID = unityObjectIDs[currentObjectIDIndex];
                currentObjectIDIndex++;
            }
            if (currentObjectIDIndex >= unityObjectIDs.Count)
                break;
            GameObject unityObject = unityObjects[currentObjectIDIndex];

            unityObject.transform.localScale = new Vector3(statsObject.ScaleX, statsObject.ScaleY, statsObject.ScaleZ);
            SetHealthVariables(unityObject, statsObject, true);
            SetSpeedVariables(unityObject, statsObject, true);
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Fill lists with data from Unity
    /// </summary>
    /// <param name="listOfObjectsToFill"></param>
    /// <param name="listOfNamesToFill">Can be null</param>
    /// <param name="listOfIDsToFill">Can be null</param>
    private static void GetGameObjectsFromUnity(List<GameObject> listOfObjectsToFill, List<string> listOfNamesToFill, List<int> listOfIDsToFill)
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
    /// <summary>
    /// Recursive method to add GameObjects containing a "Health"-script to our list
    /// </summary>
    /// <param name="parent">Parent to search in</param>
    /// <param name="listOfObjectsToFill"></param>
    /// <param name="listOfNamesToFill">Can be null</param>
    /// <param name="listOfIDsToFill">Can be null</param>
    private static void GetChildrenWithHealthScript(Transform parent, List<GameObject> listOfObjectsToFill, List<string> listOfNamesToFill, List<int> listOfIDsToFill)
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

    /// <summary>
    /// Updates the stats related to health, either from Unity to file, or from file to Unity
    /// </summary>
    /// <param name="unityObject"></param>
    /// <param name="statsObject"></param>
    /// <param name="getFromFileAndSetInUnity">Are you importing from file into Unity?</param>
    private static void SetHealthVariables(GameObject unityObject, StatsObject statsObject, bool getFromFileAndSetInUnity)
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

    /// <summary>
    /// Updates the stats related to speed, either from Unity to file, or from file to Unity
    /// </summary>
    /// <param name="unityObject"></param>
    /// <param name="statsObject"></param>
    /// <param name="getFromFileAndSetInUnity">Are you importing from file into Unity?</param>
    private static void SetSpeedVariables(GameObject unityObject, StatsObject statsObject, bool getFromFileAndSetInUnity)
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