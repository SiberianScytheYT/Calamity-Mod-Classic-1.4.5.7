using CalRD.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace CalRD.Items.Dyes
{
	public class DragonSoulDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/DragonSoulDyeShader"), "DyePass");
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragon Soul Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Violet;
            Item.value = Item.sellPrice(0, 8, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemID.BottledWater, 2);
            recipe.AddIngredient(ModContent.ItemType<HellcasterFragment>());
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }
    }
}