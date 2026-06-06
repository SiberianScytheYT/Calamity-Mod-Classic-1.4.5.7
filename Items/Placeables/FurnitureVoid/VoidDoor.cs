using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.Tiles.FurnitureVoid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureVoid
{
    public class VoidDoor : ModItem
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
            Item.consumable = true;
            Item.createTile = ModContent.TileType<VoidDoorClosed>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<SmoothVoidstone>(), 6);
            recipe.AddTile(ModContent.TileType<VoidCondenser>());
            recipe.Register();
        }
    }
}
