using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class OldDie : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Old Die");
/*
            Tooltip.SetDefault("Lucky for you, the curse doesn't affect you. Mostly.\n" +
                               "Increases the randomness of attack damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.rare = 3;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.oldDie = true;
        }
    }
}
