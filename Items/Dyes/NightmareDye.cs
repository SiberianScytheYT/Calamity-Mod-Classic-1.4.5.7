using CalRD.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Dyes
{
    public class NightmareDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/NightmareDyeShader"), "DyePass").
            UseColor(new Color(249, 81, 0)).UseSecondaryColor(new Color(255, 203, 106));
        public override void SafeSetStaticDefaults()
        {
            //DisplayName.SetDefault("Nightmare Dye");
        }

		public override void SafeSetDefaults()
		{
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemID.BottledWater, 2);
            recipe.AddIngredient(ModContent.ItemType<NightmareFuel>(), 5);
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }
    }
}