using CalRD.Items.Placeables.Ores;
using CalRD.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class VerstaltiteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryonic Bar");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<CryonicBar>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 60);
            Item.rare = 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CryonicOre>(), 5);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
        }
    }
}
