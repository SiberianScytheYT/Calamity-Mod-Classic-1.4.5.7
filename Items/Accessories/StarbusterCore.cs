using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class StarbusterCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Starbuster Core");
/*
            Tooltip.SetDefault("Summons release an astral explosion on enemy hits\n" +
                               "+1 max minion");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions++;
            player.Calamity().starbusterCore = true;
        }
    }
}
