using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.Furniture.CraftingStations
{
    public class DraedonsForge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Draedon's Forge");
/*
            Tooltip.SetDefault("Used to craft uber-tier items");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 5000000;
            Item.rare = 10;
            Item.createTile = ModContent.TileType<Tiles.Furniture.CraftingStations.DraedonsForge>();
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("HardmodeForge");
            recipe.AddRecipeGroup("HardmodeAnvil");
            recipe.AddIngredient(ItemID.LunarCraftingStation);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<NightmareFuel>(), 20);
            recipe.AddIngredient(ModContent.ItemType<EndothermicEnergy>(), 20);
            recipe.Register();
        }
    }
}
