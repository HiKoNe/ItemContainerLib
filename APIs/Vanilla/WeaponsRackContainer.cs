using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;

namespace ConduitLib.Test.APIs.Vanilla
{
    public class WeaponsRackContainer : IItemContainer
    {
        readonly TEWeaponsRack weaponsRack;

        public WeaponsRackContainer(TEWeaponsRack weaponsRack)
        {
            this.weaponsRack = weaponsRack;
        }

        public Item this[int slot]
        {
            get
            {
                if (this.SlotExist(slot))
                    return weaponsRack.item.Clone();
                return new();
            }
            set
            {
                if (this.SlotExist(slot))
                {
                    weaponsRack.item = value.Clone();
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, weaponsRack.ID);
                }
            }
        }

        public string Name => Main.item[ItemID.WeaponRack].Name;

        public int ContainerSize => 1;

        public int MaxStack(int slot)
        {
            if (this.SlotExist(slot))
                return 1;
            return 0;
        }

        public bool ValidItem(int slot, Item item)
        {
            return slot == 0 && TEWeaponsRack.FitsWeaponFrame(item);
        }
    }
}
