using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class RuinMedallion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ruin Medallion");
/*
            Tooltip.SetDefault("Stealth strikes only expend 50% of your max stealth\n" +
                "6% increased rogue damage, and 6% increased rogue crit chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.stealthStrikeHalfCost = true;
            modPlayer.throwingCrit += 6;
            modPlayer.throwingDamage += 0.06f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CoinofDeceit>());
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 4);
            recipe.AddIngredient(ModContent.ItemType<EssenceofChaos>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
