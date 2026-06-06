using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class RustyMedallion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rusty Medallion");
/*
            Tooltip.SetDefault("Causes most ranged weapons to sometimes release acid droplets from the sky");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.rare = 1;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.rustyMedal = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 20);
            recipe.AddRecipeGroup("IronBar", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
