using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class TheBee : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Bee");
/*
            Tooltip.SetDefault("Causes stars to fall and releases bees when injured\n" +
							   "When at full HP, your damage is increased based on your damage reduction\n" +
                               "Damage taken at full HP is halved\n" +
							   "This has a 10 second cooldown");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.rare = 4;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.theBee = true;
        }
    }
}
