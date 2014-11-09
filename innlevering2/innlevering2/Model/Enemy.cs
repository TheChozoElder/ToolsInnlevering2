using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace innlevering2.Model
{
	class Enemy
	{

		public string Name;
		public int MaxHealth;
		public int Scale;
		public int MovementSpeed;
		public int RegenerateSpeed;
		public bool Invisible;
		public int AimingSpeed;

		public static string Serialize(Enemy enemy)
		{
			var settings = new JsonSerializerSettings();
			//settings.Xxx = ...;
			//settings.Converters.Add(new Xxx());

			// Convert object to json string.
			return JsonConvert.SerializeObject(enemy, Formatting.Indented, settings);
		}

		public static Enemy Deserialize(string jsonData)
		{
			return JsonConvert.DeserializeObject<Enemy>(jsonData);
		}

	}
}
