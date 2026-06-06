using CalRD.Items.Materials;
using CalRD.Tiles.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.DraedonStructures
{
    public class AgedLaboratoryDoorItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aged Laboratory Door");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 28;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Red;
            Item.Calamity().customRarity = CalamityRarity.DraedonRust;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<AgedLaboratoryDoorClosed>();
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
