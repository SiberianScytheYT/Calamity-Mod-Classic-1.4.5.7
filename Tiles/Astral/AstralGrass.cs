using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Tiles.Astral
{
	public class AstralGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBrick[Type] = true;

            CalamityUtils.SetMerge(Type, ModContent.TileType<AstralDirt>());
            CalamityUtils.SetMerge(Type, TileID.Grass);
            CalamityUtils.SetMerge(Type, TileID.CorruptGrass);
            CalamityUtils.SetMerge(Type, TileID.HallowedGrass);
            CalamityUtils.SetMerge(Type, TileID.CrimsonGrass);

            DustType = ModContent.DustType<AstralBasic>();
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.AstralDirt>();

            AddMapEntry(new Color(133, 109, 140));

            TileID.Sets.Grass[Type] = true;
            TileID.Sets.Conversion.Grass[Type] = true;

            //Grass framing (<3 terraria devs)
            TileID.Sets.NeedsGrassFraming[Type] = true;
            TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<AstralDirt>();

        }

        public override void NumDust(int i, int j, bool fail, ref int Type)
        {
            Type = fail ? 1 : 3;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<AstralDirt>();
            }
        }
    }
}
