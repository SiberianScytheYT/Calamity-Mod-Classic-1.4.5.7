using CalRD.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Dyes
{
    public class ElementalDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/ElementalDyeShader"), "DyePass").UseImage("Images/Misc/Perlin");
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Elemental Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.value = Item.sellPrice(0, 2, 50, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.SolarDye);
            recipe.AddIngredient(ItemID.VortexDye);
            recipe.AddIngredient(ItemID.StardustDye);
            recipe.AddIngredient(ItemID.NebulaDye);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>());
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }
    }
}