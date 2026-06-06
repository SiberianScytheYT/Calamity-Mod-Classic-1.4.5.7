using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles.DraedonStructures
{
    public class RustedPipes : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<LaboratoryPipePlating>()] = true;

            HitSound = SoundID.Item52;
            DustType = 32;
            MinPick = 30;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.DraedonStructures.RustedPipes>();
            AddMapEntry(new Color(128, 90, 77));
        }

        public override bool CanExplode(int i, int j) => false;

        public override void PlaceInWorld(int i, int j, Item item)
        {
            SoundEngine.PlaySound(SoundID.Item52.WithVolumeScale(0.75f).WithPitchOffset(-0.5f), new Vector2(i * 16, j * 16));
        }
    }
}
