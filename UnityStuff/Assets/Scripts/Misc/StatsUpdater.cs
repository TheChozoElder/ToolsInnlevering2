using UnityEngine;
using System.Collections;

public class StatsUpdater : MonoBehaviour
{

    private GameObject enemies;

	// Use this for initialization
	void Start ()
	{
	    enemies = GameObject.Find("Enemies");
	    int count = 0;
        foreach (Transform child in enemies.transform)
        {
            count++;
            Debug.Log(count + " " + child.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
