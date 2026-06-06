using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WallTiles = CalRD.Walls;

namespace CalRD.Items.Placeables.Walls
{
    public class AbyssGravelWallItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyss Gravel Wall");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<WallTiles.AbyssGravelWallSafe>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<AbyssGravel>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

            recipe = Recipe.Create(ModContent.ItemType<AbyssGravel>());
            recipe.AddIngredient(this, 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
