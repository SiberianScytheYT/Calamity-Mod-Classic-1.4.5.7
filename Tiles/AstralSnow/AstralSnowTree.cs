using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;

namespace CalRD.Tiles
{
    public class AstralSnowTree : ModTree
    {
        public override void SetStaticDefaults()
        {
            // Grows on astral snow
            GrowsOnTileId = new int[1] { ModContent.TileType<AstralSnow.AstralSnow>() };
        }
        
        //Copypasted from vanilla, just as ExampleMod did, due to the lack of proper explanation
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };
        
        public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("CalRD/Tiles/AstralSnow/AstralSnowTree_Tops");

        public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("CalRD/Tiles/AstralSnow/AstralSnowTree_Branches");

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            //What does this code do?
            //treeFrame = (i + j * j) % 6;
        }
        
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalRD/Tiles/AstralSnow/AstralSnowTree");

        public override int DropWood()
        {
            return ModContent.ItemType<Items.Placeables.AstralMonolith>();
        }

        public override int CreateDust()
        {
            return ModContent.DustType<Dusts.AstralBasic>();
        }

        public override int TreeLeaf()
        {
            return -1;
        }
    }
}
