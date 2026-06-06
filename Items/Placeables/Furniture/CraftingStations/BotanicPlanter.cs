using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.Furniture.CraftingStations
{
    public class BotanicPlanter : ModItem
    {
        public override void SetStaticDefaults()
        {
/*
            Tooltip.SetDefault("Used for special crafting");
*/
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
            Item.value = 0;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Furniture.CraftingStations.BotanicPlanter>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<UelibloomBrick>(), 20);
            recipe.AddIngredient(ItemID.JungleSpores, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
