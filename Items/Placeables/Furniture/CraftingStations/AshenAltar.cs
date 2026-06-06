using CalRD.Items.Materials;
using CalRD.Items.Placeables.FurnitureAshen;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.Furniture.CraftingStations
{
    public class AshenAltar : ModItem
    {
        public override void SetStaticDefaults()
        {
/*
            Tooltip.SetDefault("Used for special crafting");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 3;
            Item.value = 0;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Furniture.CraftingStations.AshenAltar>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<SmoothBrimstoneSlag>(), 10);
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
