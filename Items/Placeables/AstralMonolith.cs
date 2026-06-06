using CalRD.Items.Placeables.FurnitureAstral;
using CalRD.Items.Placeables.Walls;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables
{
    public class AstralMonolith : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.Astral.AstralMonolith>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<AstralMonolithWall>(), 4);
            recipe.AddTile(ModContent.TileType<MonolithCrafting>());
            recipe.Register();
            recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<MonolithPlatform>(), 2);
            recipe.Register();
        }
    }
}
