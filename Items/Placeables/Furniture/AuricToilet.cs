using CalRD.Items.Materials;
using CalRD.Items.Placeables.FurnitureBotanic;
using CalRD.Items.Placeables.FurnitureCosmilite;
using CalRD.Items.Placeables.FurnitureSilva;
using CalRD.Tiles.Furniture;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.Furniture
{
    public class AuricToilet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Auric Toilet");
/*
            Tooltip.SetDefault("This was used by the gods");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 30;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<AuricToiletTile>();
			Item.Calamity().postMoonLordRarity = 15;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BotanicChair>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteChair>());
            recipe.AddIngredient(ModContent.ItemType<SilvaChair>());
            recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
