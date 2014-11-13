using Newtonsoft.Json;
using UnityEngine;

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