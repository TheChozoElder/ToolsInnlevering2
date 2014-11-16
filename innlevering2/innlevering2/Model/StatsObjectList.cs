namespace innlevering2.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class StatsObjectList
    {
        //Player / objects with unique name
        public List<StatsObject> NamedEntities { get; set; }

        //Objects who share name with at least one other object
        public List<StatsObject> UnnamedEntities { get; set; }

        /// <summary>
        /// Serializes this statsObjectList
        /// </summary>
        public string Serialize()
        {
            var settings = new JsonSerializerSettings();

            // Convert object to json string.
            return JsonConvert.SerializeObject(this, Formatting.Indented, settings);
        }

        /// <summary>
        /// Deserializes JSON data file and updates itself based on the deserialized data
        /// </summary>
        public void Deserialize(string jsonData)
        {
            var statsObjectList = JsonConvert.DeserializeObject<StatsObjectList>(jsonData);
            NamedEntities = statsObjectList.NamedEntities;
            UnnamedEntities = statsObjectList.UnnamedEntities;
        }

        /// <summary>
        /// Enumerator will iterate through both lists in the class, first NamedEntities, then UnnamedEntites
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            foreach (var namedEntity in NamedEntities)
            {
                yield return namedEntity;
            }
            foreach (var unnamedEntity in UnnamedEntities)
            {
                yield return unnamedEntity;
            }
        }
    }
}