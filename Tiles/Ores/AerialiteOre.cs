
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles.Ores
{
    public class AerialiteOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileOreFinderPriority[Type] = 450;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.Ores.AerialiteOre>();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Aerialite Ore");
            AddMapEntry(new Color(0, 255, 255), name);
            MineResist = 2f;
            MinPick = 65;
            HitSound = SoundID.Tink;
            Main.tileSpelunker[Type] = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
