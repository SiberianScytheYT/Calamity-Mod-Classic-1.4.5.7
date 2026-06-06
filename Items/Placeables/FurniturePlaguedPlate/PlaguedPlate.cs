using CalRD.Items.Materials;
using CalRD.Items.Placeables.Walls;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurniturePlaguedPlate
{
    public class PlaguedPlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plagued Containment Brick");
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
            Item.createTile = ModContent.TileType<Tiles.FurniturePlaguedPlate.PlaguedPlate>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.StoneBlock, 10);
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>());
            recipe.AddTile(ModContent.TileType<PlagueInfuser>());
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PlaguedPlateWall>(), 4);
            recipe.AddTile(ModContent.TileType<PlagueInfuser>());
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PlaguedPlatePlatform>(), 2);
            recipe.AddTile(ModContent.TileType<PlagueInfuser>());
            recipe.Register();
        }
    }
}
