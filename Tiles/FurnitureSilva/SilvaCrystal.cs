
using CalRD.Dusts.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles.FurnitureSilva
{
    public class SilvaCrystal : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            HitSound = SoundID.Tink;
            MineResist = 2f;
            MinPick = 275;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.FurnitureSilva.SilvaCrystal>();
            AddMapEntry(new Color(49, 100, 99));
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, ModContent.DustType<SilvaTileGold>(), 0f, 0f, 1, new Color(255, 255, 255), 1f);
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 157, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return TileFraming.BetterGemsparkFraming(i, j, resetFrame);
        }
    }
}
