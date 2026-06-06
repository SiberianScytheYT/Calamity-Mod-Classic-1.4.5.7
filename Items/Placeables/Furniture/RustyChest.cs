using CalRD.Items.Materials;
using CalRD.Tiles.Abyss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.Furniture
{
    public class RustyChest : ModItem
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
            Item.consumable = true;
            Item.value = 500;
            Item.createTile = ModContent.TileType<RustyChestTile>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Chest);
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
