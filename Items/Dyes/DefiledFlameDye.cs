using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Dyes
{
    public class DefiledFlameDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/DefiledFlameDyeShader"), "DyePass").
            UseColor(new Color(106, 190, 48)).UseSecondaryColor(new Color(204, 248, 48)).UseImage("Images/Misc/Perlin");
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Defiled Flame Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 4;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
        }
    }
}