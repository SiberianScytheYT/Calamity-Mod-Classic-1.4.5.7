using CalRD.Items.Placeables.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables
{
    public class HardenedAstralSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hardened Astral Sand");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.AstralDesert.HardenedAstralSand>();
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
            Recipe recipe = CreateRecipe(1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddIngredient(ModContent.ItemType<HardenedAstralSandWall>(), 4);
            recipe.Register();
            base.AddRecipes();
        }
    }
}
