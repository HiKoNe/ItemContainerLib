using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ConduitLib.Test.APIs.Vanilla
{
    public class ChestContainer : IItemContainer
    {
        readonly int id;

        public ChestContainer(int id)
        {
            this.id = id;
        }

        public int ContainerSize => Chest.maxItems;

        public string Name
        {
            get
            {
                if (id > -1)
                {
                    var chest = Main.chest[id];
                    if (chest.name.Length > 0)
                        return chest.name;
                    else
                    {
                        var tile = Main.tile[chest.x, chest.y];
                        if (tile.type == 21)
                            return Language.GetTextValue("LegacyChestType." + tile.frameX / 36);
                        else if (tile.type == 467 && tile.frameX / 36 == 4)
                            return Lang.GetItemNameValue(3988);
                        else if (tile.type == 467)
                            return Language.GetTextValue("LegacyChestType2." + tile.frameX / 36);
                        else if (tile.type == 88)
                            return Language.GetTextValue("LegacyDresserType." + tile.frameX / 54);
                        else if (TileID.Sets.BasicChest[tile.type] || TileID.Sets.BasicDresser[tile.type])
                            return TileLoader.ContainerName(tile.type);
                    }
                }
                else if (id == -2)
                    return Language.GetTextValue("LegacyInterface.32");
                else if (id == -3)
                    return Language.GetTextValue("LegacyInterface.33");
                else if (id == -4)
                    return Lang.GetItemNameValue(3813);
                else if (id == -5)
                    return Lang.GetItemNameValue(4076);

                return "Chest";
            }
        }

        public bool ValidItem(int slot, Item item) => true;

        public int MaxStack(int slot)
        {
            if (this.SlotExist(slot))
                return int.MaxValue;
            return 0;
        }

        public Item this[int slot]
        {
            get
            {
                if (this.SlotExist(slot) && Main.chest[id] != null)
                    return Main.chest[id].item[slot].Clone();

                return new();
            }
            set
            {
                if (this.SlotExist(slot) && Main.chest[id] != null)
                {
                    Main.chest[id].item[slot] = value.Clone();
                    NetMessage.SendData(MessageID.SyncChestItem, -1, -1, null, id, slot);
                }
            }
        }
    }
}
