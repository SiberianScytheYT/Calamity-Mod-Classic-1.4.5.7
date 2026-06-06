
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Tiles.AstralDesert
{
    public class AstralFossil : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithDesert(Type);
            CalamityUtils.MergeAstralTiles(Type);

            DustType = ModContent.DustType<AstralBasic>();
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.AstralFossil>();

            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Celestial Remains");
            AddMapEntry(new Color(59, 50, 77));

            TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            TileFraming.CustomMergeFrame(i, j, Type, ModContent.TileType<AstralSandstone>(), false, false, false, false, resetFrame);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
