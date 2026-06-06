
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Tiles.Ores
{
    public class UelibloomOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileOreFinderPriority[Type] = 950;

            CalamityUtils.MergeWithGeneral(Type);

            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Ores.UelibloomOre>();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Uelibloom Ore");
            AddMapEntry(new Color(0, 255, 0), name);
            MineResist = 5f;
            MinPick = 225;
            HitSound = SoundID.Tink;
            Main.tileSpelunker[Type] = true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            TileFraming.CustomMergeFrame(i, j, Type, TileID.Mud, false, false, false, false, resetFrame);
            return false;
        }
    }
}
