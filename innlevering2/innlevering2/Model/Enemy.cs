using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace innlevering2.Model
{

	class EnemyList
	{
//		[JsonProperty("Enemy")]
		public List<Enemy> ListOfEnemies { get; set; }

		public string Serialize()
		{
			var settings = new JsonSerializerSettings();
			//settings.Xxx = ...;
			//settings.Converters.Add(new Xxx());

			// Convert object to json string.
			return JsonConvert.SerializeObject(ListOfEnemies, Formatting.Indented, settings);;
		}

		public EnemyList Deserialize(string jsonData)
		{
			return JsonConvert.DeserializeObject<EnemyList>(jsonData);
		}

	}

	class Enemy
	{

		public string Name { get; set; }
		public int MaxHealth { get; set; }
		public int Scale { get; set; }
		public int MovementSpeed { get; set; }
		public int RegenerateSpeed { get; set; }
		public bool Invisible { get; set; }
		public int AimingSpeed { get; set; }

	}
}
