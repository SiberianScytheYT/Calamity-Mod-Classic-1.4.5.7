using CalRD.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace CalRD.Items.Dyes
{
    public class CeaselessDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/CeaselessDyeShader"), "DyePass");
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Ceaseless Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemID.VoidDye);
            recipe.AddIngredient(ItemID.ShadowDye);
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>());
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }
    }
}