using CalRD.Items.Materials;
using CalRD.Items.Placeables.Walls;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureSilva
{
    public class SilvaCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureSilva.SilvaCrystal>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(400);
            recipe.AddIngredient(ItemID.CrystalBlock, 200);
            recipe.AddRecipeGroup("AnyGoldBar", 25);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>());
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SilvaWall>(), 4);
            recipe.AddTile(ModContent.TileType<SilvaBasin>());
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SilvaPlatform>(), 2);
            recipe.AddTile(ModContent.TileType<SilvaBasin>());
            recipe.Register();
        }
    }
}
