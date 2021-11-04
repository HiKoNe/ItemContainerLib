using ConduitLib.Test.APIs.Vanilla;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;

namespace ConduitLib.Test.APIs
{
    public static class ItemContainerUtil
    {
        public static bool TryGetItemContainer(int i, int j, out IItemContainer itemContainer)
        {
            itemContainer = null;
            var chest = Chest.FindChest(i, j);
            if (chest >= 0)
                itemContainer = new ChestContainer(chest);

            foreach (var TE in TileEntity.ByID.Values)
            {
                if (TE.Position == new Point16(i, j))
                {
                    if (TE is TEItemFrame itemFrame)
                        itemContainer = new ItemFrameContainer(itemFrame);
                    else if (TE is TEDisplayDoll displayDoll)
                        itemContainer = new DisplayDollContainer(displayDoll);
                    else if (TE is TEFoodPlatter foodPlatter)
                        itemContainer = new FoodPlatterContainer(foodPlatter);
                    else if (TE is TEHatRack hatRack)
                        itemContainer = new HatRackContainer(hatRack);
                    else if (TE is TEWeaponsRack weaponsRack)
                        itemContainer = new WeaponsRackContainer(weaponsRack);
                    else if (TE is IItemContainer itemContainer2)
                        itemContainer = itemContainer2;
                }
            }

            return itemContainer is not null;
        }

        public static bool SlotExist(this IItemContainer itemContainer, int slot) =>
            slot > -1 && slot < itemContainer.ContainerSize;

        public static bool IsEmpty(this IItemContainer itemContainer)
        {
            for (int i = 0; i < itemContainer.ContainerSize; i++)
                if (itemContainer[i].stack > 0)
                    return false;

            return true;
        }

        public static Item RemoveItem(this IItemContainer itemContainer, int slot)
        {
            if (itemContainer.SlotExist(slot))
            {
                var item = itemContainer[slot];
                itemContainer[slot] = new();

                return item;
            }
            return new();
        }

        public static Item DecreaseItem(this IItemContainer itemContainer, int slot, int count)
        {
            if (itemContainer.SlotExist(slot) && count > 0)
            {
                var item = itemContainer[slot];
                var newItem = item.Clone();
                int stack = Math.Min(count, item.stack);

                newItem.stack -= stack;
                item.stack = stack;

                if (newItem.stack <= 0)
                    newItem = new();

                itemContainer[slot] = newItem;

                return item;
            }
            return new();
        }

        public static Item AddItem(this IItemContainer itemContainer, Item item)
        {
            item = item.Clone();

            for (int i = 0; i < itemContainer.ContainerSize; i++)
            {
                var origItem = itemContainer[i];
                
                if (origItem.IsTheSameAs(item))
                {
                    int stack = Math.Min(item.stack, Math.Min(itemContainer.MaxStack(i), origItem.maxStack) - origItem.stack);
                    if (stack > 0)
                    {
                        origItem.stack += stack;
                        itemContainer[i] = origItem;
                        item.stack -= stack;
                    }
                }
            }

            if (item.stack > 0)
            {
                for (int i = 0; i < itemContainer.ContainerSize; i++)
                {
                    if (!itemContainer.ValidItem(i, item))
                        continue;

                    if (itemContainer[i].IsAir)
                    {
                        int stack = Math.Min(itemContainer.MaxStack(i), item.stack);
                        itemContainer[i] = new(item.type, stack);
                        item.stack -= stack;
                    }

                    if (item.stack <= 0)
                        break;
                }
            }
            return item;
        }
    }
}
