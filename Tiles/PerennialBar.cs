using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using CalRD.Items.Materials;

namespace CalRD.Tiles
{
    public class PerennialBar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 1000;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            DustType = 44;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<DraedonBar>();

            AddMapEntry(new Color(157, 255, 0)); //chart reuse
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            if (Main.rand.NextBool(2))
            {
                type = 44;
            }
            else
            {
                type = 157;
            }
            return true;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            WorldGen.Check1x1(i, j, Type);
            return true;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            Main.tile[i, j].TileFrameX = 0;
            Main.tile[i, j].TileFrameY = 0;
        }
    }
}
