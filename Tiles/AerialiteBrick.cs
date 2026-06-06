using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles
{
    public class AerialiteBrick : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            HitSound = SoundID.Tink;
            MinPick = 50;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.AerialiteBrick>();
            AddMapEntry(new Color(68, 58, 145));
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return TileFraming.BetterGemsparkFraming(i, j, resetFrame);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 1, 0f, 0f, 1, new Color(119, 102, 255), 1f);
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 10, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }
    }
}
