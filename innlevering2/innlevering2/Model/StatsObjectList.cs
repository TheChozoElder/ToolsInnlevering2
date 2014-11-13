﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace innlevering2.Model
{
	public class StatsObjectList
	{
		//[JsonProperty("Enemy")]
		public List<StatsObject> ListOfEnemies { get; set; }

		/// <summary>
		/// Serializes current enemylist to file
		/// </summary>
		public string Serialize()
		{
			//TODO: Fix settings
			var settings = new JsonSerializerSettings();
			//settings.Xxx = ...;
			//settings.Converters.Add(new Xxx());

			//TODO: Remove this when you're done DEBUGGING!!
			PrintList();

			// Convert object to json string.
			return JsonConvert.SerializeObject(ListOfEnemies, Formatting.Indented, settings);
		}

		/// <summary>
		/// Deserializes current enemylist to file
		/// </summary>
		public void Deserialize(string jsonData)
		{
			ListOfEnemies = JsonConvert.DeserializeObject<List<StatsObject>>(jsonData);

			//TODO: Remove this when you're done DEBUGGING!!
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
