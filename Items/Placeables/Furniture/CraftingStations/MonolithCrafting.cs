using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.Furniture.CraftingStations
{
    public class MonolithCrafting : ModItem
    {
        public override void SetStaticDefaults()
        {
/*
            Tooltip.SetDefault("Used for special crafting");
*/
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("Monolith Amalgam");
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Furniture.CraftingStations.MonolithCrafting>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<AstralMonolith>(), 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
