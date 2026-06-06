using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurniturePlaguedPlate
{
    public class PlaguedPlateLamp : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plagued Lamp");
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 10;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurniturePlaguedPlate.PlaguedPlateLamp>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<PlaguedPlate>(), 3);
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 2);
            recipe.AddIngredient(ItemID.Wire);
            recipe.AddTile(ModContent.TileType<PlagueInfuser>());
            recipe.Register();
        }
    }
}
