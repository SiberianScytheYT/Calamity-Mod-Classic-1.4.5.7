using CalRD.Items.Placeables.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables
{
    public class BrimstoneSlag : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Slag");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.Crags.BrimstoneSlag>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
            Item.value = 1000;
            Item.rare = 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BrimstoneSlagWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
