using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.DraedonStructures
{
    public class LaboratoryPipePlating : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.DraedonStructures.LaboratoryPipePlating>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ModContent.ItemType<LaboratoryPlating>());
            recipe.AddIngredient(ModContent.ItemType<RustedPipes>());
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
