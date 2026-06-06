using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class UnstablePrism : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Unstable Prism");
/*
            Tooltip.SetDefault("Three sparks are released on critical hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.unstablePrism = true;
        }
    }
}
