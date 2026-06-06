using CalRD.Items.Placeables.FurnitureEutrophic;
using CalRD.Items.Placeables.Walls;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables
{
    public class Navystone : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Navystone");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.SunkenSea.Navystone>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<NavystoneWallSafe>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EutrophicPlatform>(), 2);
            recipe.AddTile(ModContent.TileType<EutrophicCrafting>());
            recipe.Register();
        }
    }
}
