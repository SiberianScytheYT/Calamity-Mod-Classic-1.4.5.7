using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class BarofLife : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Life Alloy");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = 8;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>());
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>());
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
