using Terraria;
using Terraria.ModLoader;
using WallTiles = CalRD.Walls.DraedonStructures;
using TileItems = CalRD.Items.Placeables.DraedonStructures;
using Terraria.ID;

namespace CalRD.Items.Placeables.Walls.DraedonStructures
{
    public class RustedPlatingWall : ModItem
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
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<WallTiles.RustedPlatingWall>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<TileItems.RustedPlating>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
