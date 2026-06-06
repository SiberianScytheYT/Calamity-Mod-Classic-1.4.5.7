using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class MoonstoneCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moonstone Crown");
/*
            Tooltip.SetDefault("15% increased rogue projectile velocity\n" +
                "Stealth strikes summon lunar flares on enemy hits\n" +
                "Rogue projectiles very occasionally summon moon sigils behind them");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 40;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.throwingVelocity += 0.15f;
            modPlayer.moonCrown = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FeatherCrown>());
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
