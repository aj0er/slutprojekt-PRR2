using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Server.Store
{
    /// <summary>
    /// Sparar och läser dynamiska objekt till/från en JSON-fil.
    /// </summary>
    /// <typeparam name="T">Objektets IDs typ.</typeparam>
    /// <typeparam name="TE">Objektets typ.</typeparam>
    public class JsonStore<T, TE> where TE: IStoreEntity<T>
    {

        private readonly Dictionary<T, TE> _cache;
        private readonly string _fileName;

        /// <summary>
        /// Skapar en ny JsonStore.
        /// </summary>
        /// <param name="fileName">Filens som vi spara JSON i.</param>
        public JsonStore(string fileName)
        {
            _cache = new Dictionary<T, TE>();
            _fileName = fileName;
        }

        /// <summary>
        /// Deserialiserar alla objekt från filen.
        /// </summary>
        public void LoadAll()
        {
            string text;
            try
            {
                if (!File.Exists(_fileName)) 
                {
                    // Om filen inte finns behöver vi inte läsa något från den. Så fort vi faktiskt lägger till något skapas filen automatiskt, annars är cachen tom tills dess.
                    return;
                }

                text = File.ReadAllText(_fileName);
            } catch(IOException exception) 
            {
                Console.WriteLine($"Unable to load file {_fileName} for JSON store: {exception.Message}");
                return;
            }
            
            List<TE> entities;
            try
            {
                entities = JsonConvert.DeserializeObject<List<TE>>(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to parse JSON file, is your {_fileName} file corrupt? {ex.Message}");
                return;
            }
            
            if (entities == null)
                return;
            
            entities.ForEach(entity => _cache.TryAdd(entity.Id, entity));
        }

        /// <summary>
        /// Försöker hitta en entitet beroende på dess ID.
        /// </summary>
        /// <param name="id">ID på entiteten som ska hittas.</param>
        /// <returns>Hittad entitet eller null om sådan inte finns.</returns>
        public TE GetById(T id)
        {
            _cache.TryGetValue(id, out TE value);
            return value;
        }

        /// <summary>
        /// Lägger till en ny entitet i databasen.
        /// </summary>
        /// <param name="entity">Entitet att lägga till.</param>
        public void Add(TE entity)
        {
            if (_cache.ContainsKey(entity.Id))
            {
                Console.Error.WriteLine($"Entity with ID {entity.Id} is already stored!");
                return;
            }

            _cache.Add(entity.Id, entity);
            Save();
        }

        /// <summary>
        /// Tar bort en entitet från databasen.
        /// </summary>
        /// <param name="entity">Entitet att ta bort.</param>
        public void Delete(TE entity)
        {
            if (_cache.Remove(entity.Id))
                Save();
        }

        /// <summary>
        /// Serialiserar cachen och sparar den i den specificerade filen.
        /// </summary>
        private void Save()
        {
            try
            {
                string serialized = JsonConvert.SerializeObject(_cache.Values);
                File.WriteAllText(_fileName, serialized);
            } catch(Exception ex)
            {
                Console.Error.WriteLine("Unable to save JsonStore: " + ex.Message);
            }
        }

        /// <summary>
        /// En kollektion med alla värden i cachen.
        /// </summary>
        public ICollection<TE> All => _cache.Values;

    }
}
