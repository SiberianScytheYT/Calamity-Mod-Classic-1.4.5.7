using CalRD.Items.Placeables.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader; // If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"

namespace CalRD.Items.Placeables.FurnitureProfaned
{
    public class RunicProfanedBrick : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.FurnitureProfaned.RunicProfanedBrick>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(5);
            recipe.AddIngredient(ModContent.ItemType<ProfanedRock>(), 4);
            recipe.AddIngredient(ModContent.ItemType<ProfanedCrystal>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
            recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<RunicProfanedBrickWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
