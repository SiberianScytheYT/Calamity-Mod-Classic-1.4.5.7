using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureBotanic
{
    public class BotanicClock : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 0;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureBotanic.BotanicClock>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<UelibloomBrick>(), 10);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 3);
            recipe.AddTile(ModContent.TileType<BotanicPlanter>());
            recipe.Register();
        }
    }
}
