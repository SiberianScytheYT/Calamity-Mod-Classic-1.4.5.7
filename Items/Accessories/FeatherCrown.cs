using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class FeatherCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Feather Crown");
/*
            Tooltip.SetDefault("15% increased rogue projectile velocity\n" +
                "Stealth strikes cause feathers to fall from the sky on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 38;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.throwingVelocity += 0.15f;
            modPlayer.featherCrown = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldCrown);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 6);
            recipe.AddIngredient(ItemID.Feather, 8);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumCrown);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 6);
            recipe.AddIngredient(ItemID.Feather, 8);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }
    }
}
