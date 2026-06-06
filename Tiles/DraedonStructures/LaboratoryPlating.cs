using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles.DraedonStructures
{
    public class LaboratoryPlating : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);

            HitSound = SoundID.Tink;
            DustType = 30;
            MinPick = 30;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.DraedonStructures.LaboratoryPlating> ();
            AddMapEntry(new Color(162, 157, 150));
        }

        public override bool CanExplode(int i, int j) => false;

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return TileFraming.BetterGemsparkFraming(i, j, resetFrame);
        }
    }
}
