using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables
{
    public class SeaPrism : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sea Prism");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.SunkenSea.SeaPrism>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PrismShard>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
