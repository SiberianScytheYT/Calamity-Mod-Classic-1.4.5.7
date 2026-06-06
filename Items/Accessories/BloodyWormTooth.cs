using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class BloodyWormTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloody Worm Tooth");
/*
            Tooltip.SetDefault("5% increased damage reduction and increased melee stats\n" +
                               "10% increased damage reduction and melee stats when below 50% life");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 15;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.expert = true;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.bloodyWormTooth = true;
        }
    }
}
