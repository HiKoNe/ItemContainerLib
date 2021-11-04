using System.Diagnostics.CodeAnalysis;
using Terraria;

namespace ConduitLib.Test.APIs
{
    public interface IItemContainer
    {
        string Name { get; }
        int ContainerSize { get; }

        [NotNull]
        Item this[int slot] { get; set; }
        bool ValidItem(int slot, Item item);
        int MaxStack(int slot);
    }
}
