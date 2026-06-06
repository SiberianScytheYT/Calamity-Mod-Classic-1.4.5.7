using CalRD.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Dyes
{
	public class ReaverDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/ReaverDyeShader"), "DyePass").
            UseColor(new Color(54, 164, 66)).UseSecondaryColor(new Color(224, 115, 65));
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Reaver Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 7;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemID.BottledWater, 2);
            recipe.AddIngredient(ModContent.ItemType<PerennialOre>(), 4);
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }
    }
}