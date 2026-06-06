using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Walls.DraedonStructures
{
    public class RustedPlatingWall : ModWall
    {

        public override void SetStaticDefaults()
        {
            DustType = 32;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Walls.DraedonStructures.RustedPlatingWall>();
            Main.wallHouse[Type] = true;

            AddMapEntry(new Color(83, 59, 50));
        }

        public override bool CanExplode(int i, int j) => false;

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
