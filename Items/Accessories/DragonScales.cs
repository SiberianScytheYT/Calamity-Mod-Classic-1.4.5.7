using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class DragonScales : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragon Scales");
/*
            Tooltip.SetDefault("Only a living dragon holds true treasure\n" +
							   "Rogue projectiles create slow fireballs as they travel\n" +
                               "Stealth strikes create infernados on death\n" +
                               "After Yharon is dead, you gain 10% movement speed and acceleration");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Violet;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.dragonScales = true;
        }
    }
}
