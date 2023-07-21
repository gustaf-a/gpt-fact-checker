using JsonClient;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson
{
    public class RepositoryBaseJson<T> : IRepository<T> where T : class, IIdentifiable
    {
        private readonly string JsonFilePath;

        public RepositoryBaseJson(IOptions<RepositoryJsonOptions> options, string fileName)
        {
            JsonFilePath = options.Value.DataFolder + fileName;
        }

        public async Task<bool> Create(List<T> items)
        {
            var existingItems = await GetExistingItems();

            var result = true;

            foreach (var item in items)
            {
                if (existingItems.Any(i => i.Id.Equals(item.Id)))
                    return false;

                existingItems.Add(item);
            }

            await JsonHelper.SaveToJson(existingItems, JsonFilePath);

            return true;
        }

        public async Task<bool> Create(T item)
        {
            var existingItems = await GetExistingItems();

            if (existingItems.Any(i => i.Id.Equals(item.Id)))
                return false;

            existingItems.Add(item);

            await JsonHelper.SaveToJson(existingItems, JsonFilePath);

            return true;
        }

        public async Task<bool> Delete(List<string> ids)
        {
            if (ids.IsNullOrEmpty())
                return true;

            var existingItems = await GetExistingItems();

            var result = true;

            foreach (var id in ids)
            {
                var item = existingItems.FirstOrDefault(i => i.Id.Equals(id));
                if (item is null)
                {
                    result = false;
                    continue;
                }

                existingItems.Remove(item);
            }

            await JsonHelper.SaveToJson(existingItems, JsonFilePath);

            return result;
        }

        public async Task<bool> Delete(string id)
        {
            var existingItems = await GetExistingItems();

            var item = existingItems.FirstOrDefault(i => i.Id.Equals(id));

            if (item is null)
                return false;

            existingItems.Remove(item);

            await JsonHelper.SaveToJson(existingItems, JsonFilePath);

            return true;
        }

        public async Task<List<T>> Get(List<string> ids)
        {
            var existingItems = await GetAll();

            var matchingItems = existingItems.Where(i => ids.Contains(i.Id));

            return matchingItems.ToList();
        }

        public async Task<T> Get(string id)
        {
            var existingItems = await GetAll();

            var matchingItem = existingItems.Find(i => i.Id.Equals(id));

            return matchingItem;
        }

        public async Task<List<T>> GetAll()
        {
            List<T> existingItems = await GetExistingItems();

            return existingItems;
        }

        private async Task<List<T>> GetExistingItems()
        {
            var existingItems = await JsonHelper.GetObjectFromJson<List<T>>(JsonFilePath);

            if (existingItems is null)
            {
                existingItems = new List<T>();

                await JsonHelper.SaveToJson(existingItems, JsonFilePath);
            }

            return existingItems;
        }
    }
}
