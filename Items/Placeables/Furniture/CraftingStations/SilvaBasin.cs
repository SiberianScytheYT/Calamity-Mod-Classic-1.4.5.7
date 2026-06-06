using CalRD.Items.Placeables.FurnitureSilva;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.Furniture.CraftingStations
{
    public class SilvaBasin : ModItem
    {
        public override void SetStaticDefaults()
        {
/*
            Tooltip.SetDefault("Used for special crafting");
*/
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("Effulgent Manipulator");
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Furniture.CraftingStations.SilvaBasin>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SilvaCrystal>(), 10);
            recipe.AddRecipeGroup("AnyGoldBar", 5);
            recipe.AddTile(ModContent.TileType<Tiles.Furniture.CraftingStations.DraedonsForge>());
            recipe.Register();
        }
    }
}
