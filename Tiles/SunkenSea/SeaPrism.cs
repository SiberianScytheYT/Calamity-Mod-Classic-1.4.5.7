using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Tiles.SunkenSea
{
    public class SeaPrism : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithDesert(Type);

            TileID.Sets.ChecksForMerge[Type] = true;
            DustType = 33;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.SeaPrism>();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Sea Prism");
            AddMapEntry(new Color(0, 150, 200), name);
            MineResist = 3f;
            MinPick = 55;
            HitSound = SoundID.Tink;
            Main.tileSpelunker[Type] = true;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return CalamityWorld.downedDesertScourge;
        }

        public override bool CanExplode(int i, int j)
        {
            return CalamityWorld.downedDesertScourge;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            TileFraming.CustomMergeFrame(i, j, Type, ModContent.TileType<Navystone>(), false, false, false, false, resetFrame);
            return false;
        }
    }
}
