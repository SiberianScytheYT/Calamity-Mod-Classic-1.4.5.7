using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Walls
{
    public class SulphurousSandstoneWallSafe : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = 32;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Walls.SulphurousSandstoneWall>();
            AddMapEntry(new Color(57, 45, 38));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
