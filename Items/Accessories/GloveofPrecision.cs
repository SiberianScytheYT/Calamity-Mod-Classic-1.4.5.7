using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class GloveOfPrecision : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glove Of Precision");
/*
            Tooltip.SetDefault("Decreases rogue attack speed by 20% but increases damage and crit by 12% and velocity by 25%");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 40;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.accessory = true;
            Item.rare = 7;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.gloveOfPrecision = true;
            modPlayer.throwingDamage += 0.12f;
            modPlayer.throwingCrit += 12;
            modPlayer.throwingVelocity += 0.25f;
        }
    }
}
