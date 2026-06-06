using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables.FurnitureAshen
{
    public class AshenMonolith : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 3;
            Item.consumable = true;
            Item.value = 0;
            Item.createTile = ModContent.TileType<Tiles.FurnitureAshen.AshenMonolith>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<SmoothBrimstoneSlag>(), 10);
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 9);
            recipe.AddTile(ModContent.TileType<AshenAltar>());
            recipe.Register();
        }
    }
}
