using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRD.Tiles.Abyss
{
	public class SulphurousPots : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileOreFinderPriority[Type] = (short)100;
			Main.tileSpelunker[Type] = true;
			Main.tileCut[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 4;
			TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Pot");
			AddMapEntry(new Color(226, 205, 101), name);

			DustType = (int)CalamityDusts.SulfurousSeaAcid;
			HitSound = SoundID.Shatter;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}
