using CalRD.Items.Materials;
using CalRD.Items.Placeables.Walls;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.FurnitureStatigel
{
    public class StatigelBlock : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.FurnitureStatigel.StatigelBlock>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>());
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StatigelPlatform>(), 2);
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StatigelWall>(), 4);
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();
        }
    }
}
