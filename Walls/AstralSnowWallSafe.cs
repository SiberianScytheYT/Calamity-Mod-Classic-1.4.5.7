using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
namespace CalRD.Walls
{
	public class AstralSnowWallSafe : ModWall
    {

        public override void SetStaticDefaults()
        {
            DustType = ModContent.DustType<Dusts.AstralBasic>();
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Walls.AstralSnowWall>();
            Main.wallHouse[Type] = true;

            AddMapEntry(new Color(135, 145, 149));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
