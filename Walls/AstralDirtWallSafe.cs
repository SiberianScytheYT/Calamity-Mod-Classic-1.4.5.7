using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Walls
{
    public class AstralDirtWallSafe : ModWall
    {

        public override void SetStaticDefaults()
        {
            // TODO -- Change this dust to be one more befitting Astral Dirt.
            DustType = DustID.Shadowflame;
            Main.wallHouse[Type] = true;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Walls.AstralDirtWall>();
            AddMapEntry(new Color(26, 22, 32));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
