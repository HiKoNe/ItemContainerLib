using Terraria;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;

namespace ConduitLib.Test.APIs.Vanilla
{
    public class FoodPlatterContainer : IItemContainer
    {
        readonly TEFoodPlatter foodPlatter;

        public FoodPlatterContainer(TEFoodPlatter foodPlatter)
        {
            this.foodPlatter = foodPlatter;
        }

        public Item this[int slot]
        {
            get
            {
                if (this.SlotExist(slot))
                    return foodPlatter.item.Clone();
                return new();
            }
            set
            {
                if (this.SlotExist(slot))
                {
                    foodPlatter.item = value.Clone();
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, foodPlatter.ID);
                }
            }
        }

        public string Name => Main.item[ItemID.FoodPlatter].Name;

        public int ContainerSize => 1;

        public int MaxStack(int slot)
        {
            if (this.SlotExist(slot))
                return 1;
            return 0;
        }

        public bool ValidItem(int slot, Item item)
        {
            return slot == 0 && ItemID.Sets.IsFood[item.type];
        }
    }
}
