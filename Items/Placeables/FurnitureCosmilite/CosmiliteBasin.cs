using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Tiles.FurnitureCosmilite;
using CalRD.Tiles.Furniture.CraftingStations;

namespace CalRD.Items.Placeables.FurnitureCosmilite
{
	public class CosmiliteBasin : ModItem
	{
		public override void SetStaticDefaults()
		{
            //DisplayName.SetDefault("Cosmilite Basin");
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
			Item.createTile = ModContent.TileType<CosmiliteBasinTile>();
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBrick>(), 10);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
	}
}