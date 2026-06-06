using CalRD.Dusts;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Tiles.Abyss
{
    public class AcidWoodTree : ModPalmTree
    {
        public override void SetStaticDefaults()
        {
            // Grows on sulphurous sand
            GrowsOnTileId = new int[] { ModContent.TileType<SulphurousSand>() };
        }

        //Copypasted from vanilla, just as ExampleMod did, due to the lack of proper explanation
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 0.153f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.8802f,
            SpecialGroupMaximumSaturationValue = 1f
        };
        public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("CalRD/Tiles/Abyss/AcidWoodTreeTops");
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalRD/Tiles/Abyss/AcidWoodTree");
        public override Asset<Texture2D> GetOasisTopTextures() => ModContent.Request<Texture2D>("CalRD/Tiles/Abyss/AcidWoodTreeOasisTops");
        public override int DropWood() => ModContent.ItemType<Acidwood>();
        public override int CreateDust() => (int)CalamityDusts.SulfurousSeaAcid;
    }
}
