using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class NuclearRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nuclear Rod");
/*
            Tooltip.SetDefault("Minions release an irradiated aura on enemy hits\n" +
                               "+1 max minion");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions++;
            player.Calamity().nuclearRod = true;
        }
    }
}
