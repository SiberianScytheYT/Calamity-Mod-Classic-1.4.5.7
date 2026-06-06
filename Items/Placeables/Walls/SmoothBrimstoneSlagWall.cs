using CalRD.Items.Placeables.FurnitureAshen;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using WallTiles = CalRD.Walls;
namespace CalRD.Items.Placeables.Walls
{
    public class SmoothBrimstoneSlagWall : ModItem
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
            Item.rare = 3;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<WallTiles.SmoothBrimstoneSlagWall>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<SmoothBrimstoneSlag>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
