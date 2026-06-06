using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalRD.Tiles.Abyss
{
    public class SulphurousCactus : ModCactus
    {
        public override void SetStaticDefaults()
        {
            // Grows on sulphurous sand
            GrowsOnTileId = new int[] { ModContent.TileType<SulphurousSand>() };
        }
        
        public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("CalRD/Tiles/Abyss/SulphurousCactus");
        
        public override Asset<Texture2D> GetFruitTexture() => null;
    }
}
