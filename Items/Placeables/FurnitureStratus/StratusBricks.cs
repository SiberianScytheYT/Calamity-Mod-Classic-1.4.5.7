using CalRD.Items.Materials;
using CalRD.Items.Placeables.Ores;
using CalRD.Items.Placeables.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.FurnitureStratus
{
    public class StratusBricks : ModItem
    {
        public override void SetStaticDefaults()
        {
			//DisplayName.SetDefault("Stratus Brick");
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
            Item.createTile = ModContent.TileType<Tiles.FurnitureStratus.StratusBricks>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(50);
            recipe.AddIngredient(ItemID.StoneBlock, 50);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 3);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 1);
            recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
            recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<StratusWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StratusPlatform>(), 2);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
