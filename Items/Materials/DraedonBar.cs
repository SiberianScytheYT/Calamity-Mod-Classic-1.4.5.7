using CalRD.Items.Placeables.Ores;
using CalRD.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class DraedonBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Perennial Bar");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<PerennialBar>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = 7;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PerennialOre>(), 5);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
        }
    }
}
