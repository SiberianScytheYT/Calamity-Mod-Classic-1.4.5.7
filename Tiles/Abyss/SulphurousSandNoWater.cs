using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles.Abyss
{
    public class SulphurousSandNoWater : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithAbyss(Type);

            DustType = 32;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.SulphurousSand>();
            AddMapEntry(new Color(150, 100, 50));
            MineResist = 1f;
            MinPick = 55;
            HitSound = SoundID.Dig;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            TileFraming.CustomMergeFrame(i, j, Type, ModContent.TileType<SulphurousSandstone>(), false, false, false, false, resetFrame);
            return false;
        }
    }
}
