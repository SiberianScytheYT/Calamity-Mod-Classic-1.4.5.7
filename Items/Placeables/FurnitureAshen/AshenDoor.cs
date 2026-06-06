using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.Tiles.FurnitureAshen;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables.FurnitureAshen
{
    public class AshenDoor : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 28;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 3;
            Item.consumable = true;
            Item.value = 0;
            Item.createTile = ModContent.TileType<AshenDoorClosed>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<SmoothBrimstoneSlag>(), 6);
            recipe.AddTile(ModContent.TileType<AshenAltar>());
            recipe.Register();
        }
    }
}
