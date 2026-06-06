using Terraria;
using Terraria.ModLoader;
using WallTiles = CalRD.Walls;
using Terraria.ID;

namespace CalRD.Items.Placeables.Walls
{
    public class AstralDirtWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Dirt Wall");
        }

        public override void SetDefaults()
        {
            Item.createWall = ModContent.WallType<WallTiles.AstralDirtWallSafe>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddIngredient(ModContent.ItemType<AstralDirt>());
            recipe.Register();
            base.AddRecipes();
        }
    }
}
