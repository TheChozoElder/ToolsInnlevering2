using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace innlevering2.Model
{
	class EnemyList
	{
		//[JsonProperty("Enemy")]
		public List<Enemy> ListOfEnemies { get; set; }

		/// <summary>
		/// Serializes current enemylist to file
		/// </summary>
		public string Serialize()
		{
			//TODO: Fix settings
			var settings = new JsonSerializerSettings();
			//settings.Xxx = ...;
			//settings.Converters.Add(new Xxx());

			//DEBUG
			PrintList();

			// Convert object to json string.
			return JsonConvert.SerializeObject(ListOfEnemies, Formatting.Indented, settings);
		}

		/// <summary>
		/// Deserializes current enemylist to file
		/// </summary>
		public void Deserialize()
		{

			//TODO: Is this supposed to be here?
			var path = @"E:\sak\file.json";

			var jsonStream = new StreamReader(path);
			var jsonString = jsonStream.ReadToEnd();
			//TODO: end

			ListOfEnemies = JsonConvert.DeserializeObject<List<Enemy>>(jsonString);

			//DEBUG
			PrintList();
		}

		/// <summary>
		/// Writes all names of current enemies in list
		/// </summary>
		private void PrintList()
		{
			foreach (var enemies in ListOfEnemies)
			{
				Console.WriteLine(enemies.Name);
			}
		}

	}

}
