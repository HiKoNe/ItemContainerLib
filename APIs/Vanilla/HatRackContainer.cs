using System.Reflection;
using Terraria;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;

namespace ConduitLib.Test.APIs.Vanilla
{
    public class HatRackContainer : IItemContainer
    {
        readonly TEHatRack hatRack;

        public HatRackContainer(TEHatRack hatRack)
        {
            this.hatRack = hatRack;
        }

        public Item[] Items
        {
            get => (Item[])typeof(TEHatRack).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(hatRack);
            set => typeof(TEHatRack).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(hatRack, value);
        }
        public Item[] Dyes
        {
            get => (Item[])typeof(TEHatRack).GetField("_dyes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(hatRack);
            set => typeof(TEHatRack).GetField("_dyes", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(hatRack, value);
        }

        public Item this[int slot]
        {
            get
            {
                if (this.SlotExist(slot))
                {
                    if (slot < 2)
                        return Items[slot].Clone();
                    else
                        return Dyes[slot - 2].Clone();
                }
                return new();
            }
            set
            {
                if (this.SlotExist(slot))
                {
                    if (slot < 2)
                        Items[slot] = value.Clone();
                    else
                        Dyes[slot - 2] = value.Clone();
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, hatRack.ID);
                }
            }
        }

        public string Name => Main.item[ItemID.HatRack].Name;

        public int ContainerSize => 4;

        public bool ValidItem(int slot, Item item)
        {
            if (this.SlotExist(slot))
            {
                if (slot < 2)
                    return item.headSlot > 0;
                else
                    return item.dye > 0;
            }
            return false;
        }

        public int MaxStack(int slot)
        {
            if (this.SlotExist(slot))
                return 1;
            return 0;
        }
    }
}
