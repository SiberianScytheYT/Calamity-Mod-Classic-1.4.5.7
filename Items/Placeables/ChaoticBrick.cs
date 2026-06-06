using CalRD.Items.Placeables.Ores;
using CalRD.Items.Placeables.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables
{
    public class ChaoticBrick : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 25;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = 1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.ChaoticBrick>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.StoneBlock, 1);
            recipe.AddIngredient(ModContent.ItemType<ChaoticOre>(), 1);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();

            recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<ChaoticBrickWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
