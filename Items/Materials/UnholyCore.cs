using CalRD.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class UnholyCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Unholy Core");
        }

        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 12;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 80);
            Item.rare = 6;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CharredOre>(), 4);
            recipe.AddIngredient(ItemID.Hellstone, 4);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
        }
    }
}
