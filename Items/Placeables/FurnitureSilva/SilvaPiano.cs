using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureSilva
{
    public class SilvaPiano : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureSilva.SilvaPiano>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SilvaCrystal>(), 15);
            recipe.AddIngredient(ItemID.Bone, 4);
            recipe.AddIngredient(ItemID.Book);
            recipe.AddTile(ModContent.TileType<SilvaBasin>());
            recipe.Register();
        }
    }
}
