using System.Reflection;
using Terraria;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;

namespace ConduitLib.Test.APIs.Vanilla
{
    public class DisplayDollContainer : IItemContainer
    {
        readonly TEDisplayDoll displayDoll;

        public DisplayDollContainer(TEDisplayDoll displayDoll)
        {
            this.displayDoll = displayDoll;
        }

        public Item[] Items
        {
            get => (Item[])typeof(TEDisplayDoll).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(displayDoll);
            set => typeof(TEDisplayDoll).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(displayDoll, value);
        }
        public Item[] Dyes
        {
            get => (Item[])typeof(TEDisplayDoll).GetField("_dyes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(displayDoll);
            set => typeof(TEDisplayDoll).GetField("_dyes", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(displayDoll, value);
        }
        public Item this[int slot]
        {
            get
            {
                if (this.SlotExist(slot))
                {
                    if (slot < 8)
                        return Items[slot].Clone();
                    else
                        return Dyes[slot - 8].Clone();
                }
                return new();
            }
            set
            {
                if (this.SlotExist(slot))
                {
                    if (slot < 8)
                        Items[slot] = value.Clone();
                    else
                        Dyes[slot - 8] = value.Clone();
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, displayDoll.ID);
                }
            }
        }

        public string Name => Main.item[ItemID.Mannequin].Name;

        public int ContainerSize => 16;

        public int MaxStack(int slot)
        {
            if (this.SlotExist(slot))
                return 1;
            return 0;
        }

        public bool ValidItem(int slot, Item item)
        {
            if (this.SlotExist(slot))
            {
                if (slot == 0)
                    return item.headSlot > 0;
                else if (slot == 1)
                    return item.bodySlot > 0;
                else if (slot == 2)
                    return item.legSlot > 0;
                else if (slot < 8)
                    return item.accessory;
                else
                    return item.dye > 0;
            }
            return false;
        }
    }
}
