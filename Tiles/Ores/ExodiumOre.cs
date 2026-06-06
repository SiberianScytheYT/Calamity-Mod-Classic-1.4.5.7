
using CalRD.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles.Ores
{
    public class ExodiumOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);

            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Exodium Ore");
            AddMapEntry(new Color(51, 48, 68), name);
            MineResist = 5f;
            MinPick = 225;
            HitSound = SoundID.Tink;
            Main.tileOreFinderPriority[Type] = 760;
            Main.tileSpelunker[Type] = true;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<ExodiumClusterOre>();
            base.SetStaticDefaults();
        }

		public override bool CanExplode(int i, int j)
		{
			return false;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 2 : 4;
        }
    }
}
