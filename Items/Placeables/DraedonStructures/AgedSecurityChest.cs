using CalRD.Items.Materials;
using CalRD.Tiles.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.DraedonStructures
{
    public class AgedSecurityChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aged Security Chest");
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
            Item.rare = 2;
            Item.Calamity().customRarity = CalamityRarity.DraedonRust;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 500;
            Item.createTile = ModContent.TileType<AgedSecurityChestTile>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 4);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 4);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
