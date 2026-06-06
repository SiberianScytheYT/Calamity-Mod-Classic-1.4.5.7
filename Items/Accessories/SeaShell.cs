using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class SeaShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sea Shell");
/*
            Tooltip.SetDefault("Increased defense and damage reduction when submerged in liquid\n" +
                "Increased movement speed when submerged in liquid");
*/
        }

        public override void SetDefaults()
        {
            Item.defense = 2;
            Item.width = 20;
            Item.height = 24;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.rare = 2;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.seaShell = true;
        }
    }
}
