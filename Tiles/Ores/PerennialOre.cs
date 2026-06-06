
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles.Ores
{
    public class PerennialOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileOreFinderPriority[Type] = 710;

            CalamityUtils.MergeWithGeneral(Type);

            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Ores.PerennialOre>();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Perennial Ore");
            AddMapEntry(new Color(200, 250, 100), name);
            MineResist = 3f;
            MinPick = 200;
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

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.04f;
            g = 0.10f;
            b = 0.02f;
        }
    }
}
