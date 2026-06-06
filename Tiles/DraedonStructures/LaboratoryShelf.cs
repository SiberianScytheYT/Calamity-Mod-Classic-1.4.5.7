using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Tiles.DraedonStructures
{
    public class LaboratoryShelf : ModTile
    {
        public override void SetStaticDefaults()
        {
            this.SetUpPlatform(true);
            HitSound = SoundID.Tink;
            DustType = 30;
            AddMapEntry(new Color(97, 87, 86));
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.DraedonStructures.LaboratoryShelf>();
            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.Platforms };
        }

        public override bool CanExplode(int i, int j) => false;

        public override void PostSetDefaults()
        {
            Main.tileNoSunLight[Type] = false;
        }
    }
}
