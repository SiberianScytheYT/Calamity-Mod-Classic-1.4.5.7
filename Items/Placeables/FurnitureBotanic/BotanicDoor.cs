using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.Tiles.FurnitureBotanic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables.FurnitureBotanic
{
    public class BotanicDoor : ModItem
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
            Item.createTile = ModContent.TileType<BotanicDoorClosed>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<UelibloomBrick>(), 6);
            recipe.AddTile(ModContent.TileType<BotanicPlanter>());
            recipe.Register();
        }
    }
}
