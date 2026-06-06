using CalRD.Items.Placeables.FurniturePlaguedPlate;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WallTiles = CalRD.Walls;
namespace CalRD.Items.Placeables.Walls
{
    public class PlaguedPlateWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plagued Containment Wall");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<WallTiles.PlaguedPlateWall>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<PlaguedPlate>());
            recipe.AddTile(ModContent.TileType<PlagueInfuser>());
            recipe.Register();
        }
    }
}
