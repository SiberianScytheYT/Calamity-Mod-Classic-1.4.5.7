using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class FleshTotem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flesh Totem");
/*
            Tooltip.SetDefault("Halves enemy contact damage\n" +
                "When you take contact damage this effect has a 20 second cooldown");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = 8;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.fleshTotem = true;
        }
    }
}
