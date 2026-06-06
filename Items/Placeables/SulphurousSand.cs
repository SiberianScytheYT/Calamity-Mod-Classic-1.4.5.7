using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables
{
    public class SulphurousSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphurous Sand");
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.Abyss.SulphurousSandNoWater>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddIngredient(ModContent.ItemType<Walls.SulphurousSandWall>(), 4);
            recipe.Register();
            base.AddRecipes();
        }
    }
}
