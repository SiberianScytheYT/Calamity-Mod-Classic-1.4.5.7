using CalRD.Dusts;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRD.Tiles
{
    public class ShadowspecBarTile : ModTile
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

            DustType = ModContent.DustType<ShadowspecBarDust>();
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<ShadowspecBar>();

            AddMapEntry(new Color(128, 41, 149));
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            type = ModContent.DustType<ShadowspecBarDust>();
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
