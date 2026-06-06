using CalRD.Items.Materials;
using CalRD.Tiles.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.DraedonStructures
{
    public class AgedLaboratoryElectricPanelItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aged Laboratory Electric Panel");
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
            Item.createTile = ModContent.TileType<AgedLaboratoryElectricPanel>();
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
