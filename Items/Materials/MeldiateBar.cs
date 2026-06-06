using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class MeldiateBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Meld Construct");
        }

        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 12;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 1, silver: 20);
            Item.rare = 9;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ModContent.ItemType<MeldBlob>(), 6);
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
