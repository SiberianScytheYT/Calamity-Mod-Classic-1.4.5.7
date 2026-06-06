using CalRD.Buffs.DamageOverTime;
using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Items.Accessories
{
    public class AstralBulwark : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Bulwark");
/*
            Tooltip.SetDefault("Taking damage drops astral stars from the sky\n" +
                               "Provides immunity to the astral infection debuff");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity9BuyPrice;
            Item.expert = true;
            Item.rare = 9;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.aBulwark = true;
            player.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
        }
    }
}
