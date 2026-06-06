using CalRD.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Dyes
{
    public class AstralBlueDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/AstralBlueDyeShader"), "DyePass").
            UseColor(new Color(109, 242, 197)).UseSecondaryColor(new Color(42, 147, 154));
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Blue Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 9;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<AstralOre>());
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }
    }
}