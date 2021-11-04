using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;

namespace ConduitLib.Test.APIs.Vanilla
{
    public class ItemFrameContainer : IItemContainer
    {
        readonly TEItemFrame itemFrame;

        public ItemFrameContainer(TEItemFrame itemFrame)
        {
            this.itemFrame = itemFrame;
        }

        public int ContainerSize => 1;

        public string Name => Main.item[ItemID.ItemFrame].Name;

        public bool ValidItem(int slot, Item item) => true;

        public int MaxStack(int slot)
        {
            if (this.SlotExist(slot))
                return 1;
            return 0;
        }

        public Item this[int slot]
        {
            get
            {
                if (this.SlotExist(slot))
                    return itemFrame.item.Clone();
                return new();
            }
            set
            {
                if (this.SlotExist(slot))
                {
                    itemFrame.item = value.Clone();
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, itemFrame.ID);
                }
            }
        }
    }
}
