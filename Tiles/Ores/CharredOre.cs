using CalRD.World;
using CalRD.Tiles.Crags;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles.Ores
{
    public class CharredOre : ModTile
    {
        private int sheetWidth = 288;
        private int sheetHeight = 270;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileOreFinderPriority[Type] = 720;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithHell(Type);

            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Ores.CharredOre>();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Charred Ore");
            AddMapEntry(new Color(17, 16, 26), name);
            MineResist = 6f;
            MinPick = 180;
            HitSound = SoundID.Tink;
            DustType = 235;
            Main.tileSpelunker[Type] = true;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return CalamityWorld.downedBrimstoneElemental;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            frameXOffset = i % 2 * sheetWidth;
            frameYOffset = j % 2 * sheetHeight;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.50f;
            g = 0.00f;
            b = 0.00f;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            TileFraming.CustomMergeFrame(i, j, Type, ModContent.TileType<BrimstoneSlag>(), false, false, false, false, resetFrame);
            return false;
        }
    }
}
