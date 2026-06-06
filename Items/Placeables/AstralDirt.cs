using CalRD.Items.Placeables.Walls;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables
{
    public class AstralDirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Dirt");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.Astral.AstralDirt>();
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
            recipe.AddIngredient(ModContent.ItemType<AstralDirtWall>(), 4);
            recipe.Register();
            base.AddRecipes();
        }
    }
}
