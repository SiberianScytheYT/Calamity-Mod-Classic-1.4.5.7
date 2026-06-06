using Terraria;
using Terraria.ModLoader;
using WallTiles = CalRD.Walls;
using Terraria.ID;

namespace CalRD.Items.Placeables.Walls
{
    public class AstralIceWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Ice Wall");
        }

        public override void SetDefaults()
        {
            Item.createWall = ModContent.WallType<WallTiles.AstralIceWallSafe>();
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
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddIngredient(ModContent.ItemType<AstralIce>());
            recipe.Register();
            base.AddRecipes();
        }
    }
}
