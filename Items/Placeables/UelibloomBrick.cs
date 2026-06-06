using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.Items.Placeables.Ores;
using CalRD.Items.Placeables.Walls;
using CalRD.Items.Placeables.FurnitureBotanic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables
{
    public class UelibloomBrick : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.UelibloomBrick>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UelibloomOre>());
            recipe.AddIngredient(ItemID.StoneBlock);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UelibloomBrickWall>(), 4);
            recipe.AddTile(ModContent.TileType<BotanicPlanter>());
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BotanicPlatform>(), 2);
            recipe.AddTile(ModContent.TileType<BotanicPlanter>());
            recipe.Register();
        }
    }
}
