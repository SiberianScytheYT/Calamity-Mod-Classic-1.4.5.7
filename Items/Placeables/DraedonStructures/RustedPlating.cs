using CalRD.Items.Placeables.Walls.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.DraedonStructures
{
    public class RustedPlating : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.DraedonStructures.RustedPlating>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar);
            recipe.AddIngredient(ItemID.StoneBlock, 3);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<LaboratoryPlating>());
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RustedShelf>(), 2);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RustedPlatingWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RustedPlateBeam>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RustedPlatePillar>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
