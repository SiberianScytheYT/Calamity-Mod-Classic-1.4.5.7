using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class GiantShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Giant Shell");
/*
            Tooltip.SetDefault("15% reduced movement speed\n" +
                "Taking a hit will make you move very fast for a short time");
*/
        }

        public override void SetDefaults()
        {
            Item.defense = 6;
            Item.width = 20;
            Item.height = 24;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = 1;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed -= 0.15f;
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.gShell = true;
        }
    }
}
