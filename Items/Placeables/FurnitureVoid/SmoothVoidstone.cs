using CalRD.Items.Placeables.Walls;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureVoid
{
    public class SmoothVoidstone : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureVoid.SmoothVoidstone>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Voidstone>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SmoothVoidstoneWall>(), 4);
            recipe.AddTile(ModContent.TileType<VoidCondenser>());
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SmoothVoidstonePlatform>(), 2);
            recipe.AddTile(ModContent.TileType<VoidCondenser>());
            recipe.Register();
        }
    }
}
