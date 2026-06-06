using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class ScuttlersJewel : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scuttler's Jewel");
/*
            Tooltip.SetDefault("Rogue javelin projectiles have a chance to spawn a jewel spike when destroyed");
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
            modPlayer.scuttlersJewel = true;
        }
    }
}
