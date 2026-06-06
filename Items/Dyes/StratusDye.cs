using CalRD.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using CalRD.Items.Placeables.Ores;

namespace CalRD.Items.Dyes
{
    public class StratusDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/StratusDyeShader"), "DyePass").
            UseColor(new Color(36, 86, 163)).UseSecondaryColor(new Color(124, 204, 223)).UseImage("Images/Misc/Perlin");
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Stratus Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.value = Item.sellPrice(0, 4, 50, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemID.BottledWater, 2);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>());
            recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>());
            recipe.AddIngredient(ModContent.ItemType<Lumenite>());
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }
    }
}