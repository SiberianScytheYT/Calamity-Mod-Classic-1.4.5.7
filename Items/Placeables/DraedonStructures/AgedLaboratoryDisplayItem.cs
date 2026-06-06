using CalRD.Items.Materials;
using CalRD.Tiles.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.DraedonStructures
{
    public class AgedLaboratoryDisplayItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aged Laboratory Display");
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
            Item.createTile = ModContent.TileType<AgedLaboratoryDisplay>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 3);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
