using CalRD.Dusts;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Tiles.AstralDesert
{
    public class AstralPalmTree : ModPalmTree
    {
        public override void SetStaticDefaults()
        {
            // Grows on astral sand
            GrowsOnTileId = new int[1] { ModContent.TileType<AstralSand>() };
        }
        
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };
        
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalRD/Tiles/AstralDesert/AstralPalmTree");

        public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("CalRD/Tiles/AstralDesert/AstralPalmTree_Tops");

        public override Asset<Texture2D> GetOasisTopTextures() => ModContent.Request<Texture2D>("CalRD/Tiles/AstralDesert/AstralPalmTree_OasisTops");

        
        public override int DropWood()
        {
            return ModContent.ItemType<Items.Placeables.AstralMonolith>();
        }

        public override int CreateDust()
        {
            return ModContent.DustType<AstralBasic>();
        }

        public override int TreeLeaf()
        {
            return -1;
        }
    }
}
