using CalRD.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Dyes
{
    public class AstralSwirlDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/AstralSwirlDyeShader"), "DyePass").
            UseColor(new Color(42, 147, 154)).UseSecondaryColor(new Color(238, 93, 82)).UseImage("Images/Misc/Perlin");
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Swirl Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 9;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ModContent.ItemType<AstralBlueDye>());
            recipe.AddIngredient(ModContent.ItemType<AstralOrangeDye>());
            recipe.AddIngredient(ModContent.ItemType<AstralOre>(), 5);
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }
    }
}