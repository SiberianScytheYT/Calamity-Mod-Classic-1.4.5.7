using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables.FurnitureEutrophic
{
    public class EutrophicPlatform : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 10;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureEutrophic.EutrophicPlatform>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ModContent.ItemType<Navystone>());
            recipe.AddTile(ModContent.TileType<EutrophicCrafting>());
            recipe.Register();
        }
    }
}
