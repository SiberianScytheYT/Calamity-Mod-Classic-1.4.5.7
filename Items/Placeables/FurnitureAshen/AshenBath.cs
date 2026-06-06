using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables.FurnitureAshen
{
    public class AshenBath : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("Ashen Bathtub");
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 3;
            Item.value = 0;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureAshen.AshenBath>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<SmoothBrimstoneSlag>(), 14);
            recipe.AddTile(ModContent.TileType<AshenAltar>());
            recipe.Register();
        }
    }
}
