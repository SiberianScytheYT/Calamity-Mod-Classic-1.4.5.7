using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Walls
{
    public class AstralSandstoneWallSafe : ModWall
    {

        public override void SetStaticDefaults()
        {
            // TODO -- Change this dust to be one more befitting Astral Sandstone.
            DustType = DustID.Shadowflame;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Walls.AstralSandstoneWall>();
            Main.wallHouse[Type] = true;

            WallID.Sets.Conversion.Sandstone[Type] = true;

            AddMapEntry(new Color(29, 38, 49));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
