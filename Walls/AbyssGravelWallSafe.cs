using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Walls
{
    public class AbyssGravelWallSafe : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Walls.AbyssGravelWallItem>();
            AddMapEntry(new Color(41, 56, 80));
            DustType = 33;
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
