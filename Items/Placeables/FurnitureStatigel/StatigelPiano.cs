using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureStatigel
{
    public class StatigelPiano : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureStatigel.StatigelPiano>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.Bone, 4);
            recipe.AddIngredient(ModContent.ItemType<StatigelBlock>(), 15);
            recipe.AddIngredient(ItemID.Book);
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();
        }
    }
}
