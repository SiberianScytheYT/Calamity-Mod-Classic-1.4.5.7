using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class BloodPact : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Pact");
/*
            Tooltip.SetDefault("Doubles your max HP\n" +
                "Allows you to be critically hit 25% of the time\n" +
				"After a critical hit, you gain various buffs for ten seconds\n" +
				"Any healing potions consumed during this time period heal 50% more health");
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
            modPlayer.bloodPact = true;
        }
    }
}
