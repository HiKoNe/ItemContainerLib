using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ConduitLib.Test.APIs
{
    public abstract class ItemContainer : ModTileEntity, IItemContainer
    {
        protected Item[] container;

        public ItemContainer(int size)
        {
            container = new Item[size];
            for (int i = 0; i < size; i++)
                container[i] = new();
        }

        public Item this[int slot]
        {
            get
            {
                if (this.SlotExist(slot))
                    return container[slot].Clone();
                return new();
            }
            set
            {
                if (this.SlotExist(slot))
                {
                    container[slot] = value.Clone();
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID);
                }
            }
        }

        public int ContainerSize => container.Length;

        public abstract bool ValidItem(int slot, Item item);

        public abstract int MaxStack(int slot);

        public override void SaveData(TagCompound tag)
        {
            var tagItems = new List<TagCompound>();

            for (int i = 0; i < ContainerSize; i++)
                tagItems.Add(ItemIO.Save(this[i]));

            tag["items"] = tagItems;
        }

        public override void LoadData(TagCompound tag)
        {
            if (!tag.ContainsKey("items"))
                return;

            var list = tag.GetList<TagCompound>("items");
            for (int i = 0; i < list.Count && i < ContainerSize; i++)
                this[i] = ItemIO.Load(list[i]);
        }
    }
}
