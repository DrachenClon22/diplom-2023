namespace diplomOriginal.Modules
{
    public static class ShoppingCart
    {
        private static List<Item> Items = new List<Item>();

        public static Item[]? GetAllItems()
        {
            if (Items.Count == 0)
            {
                return null;
            }

            return Items.ToArray();
        }

        public static void PrintAllItemsInConsole()
        {
            Item[]? items = GetAllItems();
            if (items is not null)
            {
                ConsoleLogger.Log(LogStatus.WARNING, $"Shopping Cart, amount of items: {items.Length}");
                foreach (var item in items)
                {
                    ConsoleLogger.Log(LogStatus.WARNING, $"{item.Name} - {Database.GetPrice(item.PartNumber).Result} RUR");
                }
            } else
            {
                ConsoleLogger.Log(LogStatus.WARNING, $"No items in shopping cart");
            }
        }

        public static void Clear()
        {
            if (Items.Count > 0)
            {
                Items.Clear();
            }
        }

        public static Item? GetItem(string id)
        {
            Item target = ItemsRepository.GetItem(id).Result ?? new Item();
            if (Items.Contains(target))
            {
                return target;
            }

            return null;
        }

        public static bool IsItemInCart(string id)
        {
            return (GetItem(id) is not null);
        }

        public static bool AddItem(string id)
        {
            Item? target = ItemsRepository.GetItem(id).Result;

            if (target is not null)
            {
                Items.Add(target);
                return true;
            }

            return false;
        }

        public static bool RemoveItem(string id)
        {
            Item target = ItemsRepository.GetItem(id).Result ?? new Item();
            if (Items.Contains(target))
            {
                Items.Remove(target);
                return true;
            }

            return false;
        }
    }
}
