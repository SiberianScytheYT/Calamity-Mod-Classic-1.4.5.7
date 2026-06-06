using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WallTiles = CalRD.Walls;
namespace CalRD.Items.Placeables.Walls
{
    public class SulphurousSandstoneWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphurous Sandstone Wall");
        }

        public override void SetDefaults()
        {
            Item.createWall = ModContent.WallType<WallTiles.SulphurousSandstoneWallSafe>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddTile(18);
            recipe.AddIngredient(ModContent.ItemType<SulphurousSandstone>());
            recipe.Register();
            base.AddRecipes();
        }
    }
}
