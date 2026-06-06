using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class CrownJewel : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crown Jewel");
/*
            Tooltip.SetDefault("Boosts life regen even while under the effects of a damaging debuff\n" +
                "While under the effects of a damaging debuff you will gain 10 defense\n" +
                "Revengeance drop");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = 1;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.crownJewel = true;
        }
    }
}
