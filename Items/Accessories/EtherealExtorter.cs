using CalRD.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class EtherealExtorter : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ethereal Extorter");
/*
            Tooltip.SetDefault("Infuses souls into your weapons and body generating different boosts that vary with the environment\nRogue projectiles rarely explode into homing souls\n10% increased rogue damage but reduced life regen");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = 8;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.etherealExtorter = true;
            modPlayer.throwingDamage += 0.1f;
			player.lifeRegen -= 1;
			if (Main.moonPhase == 4) // 4 = New Moon
				modPlayer.rogueStealthMax += 0.1f;
        }
    }
}
