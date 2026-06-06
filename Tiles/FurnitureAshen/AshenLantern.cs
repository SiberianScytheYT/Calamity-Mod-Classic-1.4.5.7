using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Tiles.FurnitureAshen
{
    public class AshenLantern : ModTile
    {
        public override void SetStaticDefaults()
        {
            this.SetUpLantern(true);
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Ashen Lantern");
            AddMapEntry(new Color(191, 142, 111), name);

            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.Torches };
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 60, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 1, 0f, 0f, 1, new Color(100, 100, 100), 1f);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<Items.Placeables.FurnitureAshen.AshenLantern>());
        }*/

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = 1f;
                g = 0.5f;
                b = 0.5f;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }
        }

        public override void HitWire(int i, int j)
        {
            CalamityUtils.LightHitWire(Type, i, j, 1, 2);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            CalamityUtils.DrawFlameEffect(ModContent.Request<Texture2D>("CalRD/Tiles/FurnitureAshen/AshenLanternFlame").Value, i, j);
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 18)
            {
                CalamityUtils.DrawFlameSparks(60, 5, i, j);
            }
        }
    }
}
