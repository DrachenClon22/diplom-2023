using Newtonsoft.Json;

namespace diplomOriginal.Modules
{
    public class Item
    {
        public string PartNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public static Item? JSONToItem(string json)
        {
            return JsonConvert.DeserializeObject<Item>(json);
        }
    }

    public static class ItemsRepository
    {
        private const string PATH = @"wwwroot\items\";

        private static string[]? _files;

        static ItemsRepository()
        {
            if (Directory.Exists(PATH))
            {
                ConsoleLogger.Log($"Path \"{PATH}\" found, with total files of: {Directory.GetFiles(PATH).Length}");
            } else
            {
                ConsoleLogger.Log(LogStatus.ERROR, $"No path \"{PATH}\" found!");
            }
        }

        private static bool TryGetFiles()
        {
            if (_files is not null)
            {
                return true;
            }

            string[] temp = Directory.GetFiles(PATH);
            if (temp is null)
            {
                return false;
            } else
            {
                _files = temp;
                return true;
            }
        }

        public async static Task<Item?> GetItem(string id)
        {
            if (TryGetFiles())
            {
                string? target = _files!.Where(n => n.Contains(id)).FirstOrDefault();

                if (!string.IsNullOrEmpty(target))
                {
                    return Item.JSONToItem(await File.ReadAllTextAsync(target));
                }
            }

            return null;
        }

        public async static Task<Item[]?> GetAllItems()
        {
            if (TryGetFiles())
            {
                var result = new List<Item>();

                foreach(var item in _files!)
                {
                    Item? curr = Item.JSONToItem(await File.ReadAllTextAsync(item));

                    if (curr is not null)
                    {
                        result.Add(curr);
                    }
                }

                return result.ToArray();
            }

            return null;
        }
    }
}
