using CalRD.Tiles.Furniture;
using CalRD.Items.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.Furniture
{
	public class SeaMinnowJar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sea Minnow Jar");
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<SeaMinnowJarTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeaMinnowItem>());
            recipe.AddIngredient(ItemID.Bottle);
            recipe.Register();
        }
    }
}
