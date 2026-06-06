using CalRD.Items.Placeables.Walls.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.DraedonStructures
{
    public class HazardChevronPanels : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.DraedonStructures.HazardChevronPanels>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ModContent.ItemType<LaboratoryPanels>(), 10);
            recipe.AddIngredient(ItemID.YellowPaint);
            recipe.AddIngredient(ItemID.BlackPaint);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
            recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<HazardChevronWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
