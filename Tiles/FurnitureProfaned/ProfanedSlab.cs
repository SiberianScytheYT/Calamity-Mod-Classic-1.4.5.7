
using CalRD.Dusts.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Tiles.FurnitureProfaned
{
    public class ProfanedSlab : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeDecorativeTiles(Type);
            CalamityUtils.MergeSmoothTiles(Type);
            CalamityUtils.SetMerge(Type, ModContent.TileType<ProfanedRock>());

            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 225;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeables.FurnitureProfaned.ProfanedSlab>();
            AddMapEntry(new Color(122, 66, 59));
            AnimationFrameHeight = 90;
        }
        int animationFrameWidth = 234;

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, ModContent.DustType<ProfanedTileRock>(), 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            frameXOffset = i % 4 * animationFrameWidth;
            frameYOffset = j % 4 * AnimationFrameHeight;
        }
    }
}
