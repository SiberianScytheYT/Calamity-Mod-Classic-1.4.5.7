using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Walls
{
    public class ChaoticBrickWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Walls.ChaoticBrickWall>();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Chaotic Brick Wall");
            AddMapEntry(new Color(255, 0, 0), name);
            DustType = 105;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 2;
        }
    }
}
