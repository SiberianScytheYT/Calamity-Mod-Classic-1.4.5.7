using CalRD.Items.Materials;
using CalRD.Items.Placeables.Walls;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureCosmilite
{
    public class CosmiliteBrick : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.FurnitureCosmilite.CosmiliteBrick>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(20);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 1);
            recipe.AddIngredient(ItemID.StoneBlock, 20);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBrickWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmilitePlatform>(), 2);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
