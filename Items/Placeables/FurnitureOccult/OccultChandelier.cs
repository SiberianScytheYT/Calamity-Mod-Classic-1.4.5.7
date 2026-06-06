using CalRD.Items.Placeables.FurnitureCosmilite;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace CalRD.Items.Placeables.FurnitureOccult
{
    public class OccultChandelier : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("Otherworldly Chandelier");
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureOccult.OccultChandelier>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<OccultStone>(), 12);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBrick>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
