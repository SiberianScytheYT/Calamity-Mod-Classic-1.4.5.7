using CalRD.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Dyes
{
    public class ShadowspecDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/ShadowspecDyeShader"), "DyePass").
            UseColor(new Color(46, 27, 60)).UseSecondaryColor(new Color(132, 142, 191)).UseImage("Images/Misc/Perlin");
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Shadowspec Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Developer;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemID.BottledWater, 2);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>());
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }
    }
}