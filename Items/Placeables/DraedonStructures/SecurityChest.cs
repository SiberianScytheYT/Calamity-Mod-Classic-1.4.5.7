using CalRD.Tiles.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.DraedonStructures
{
    public class SecurityChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Security Chest");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 26;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 14;
            Item.rare = 3;
            Item.Calamity().customRarity = CalamityRarity.DraedonRust;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 500;
            Item.createTile = ModContent.TileType<SecurityChestTile>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
            recipe.AddIngredient(ModContent.ItemType<AgedSecurityChest>());
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
