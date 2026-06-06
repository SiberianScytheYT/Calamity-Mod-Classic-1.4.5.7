using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace CalRD.Items.Dyes
{
    public class ProfanedMoonlightDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/ProfanedMoonlightDye"), "DyePass");
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Profaned Moonlight Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }
    }
}