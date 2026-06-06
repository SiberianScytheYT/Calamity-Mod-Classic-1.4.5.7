using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class PlaguedFuelPack : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plagued Fuel Pack");
/*
            Tooltip.SetDefault("5% increased rogue damage\n" +
                "15% increased rogue projectile velocity\n" +
                "TOOLTIP LINE HERE" + 
                "This effect has a 3 second cooldown before it can be used again");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 36;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = 8;
            Item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */ => !player.Calamity().hasJetpack;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.Calamity().hasJetpack = true;
            player.Calamity().throwingDamage += 0.05f;
            player.Calamity().throwingVelocity += 0.15f;
            player.Calamity().plaguedFuelPack = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            string hotkey = CalRD.PlaguePackHotKey.TooltipHotkeyString();
            foreach (TooltipLine line in list)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                {
                    line.Text = "Press " + hotkey + " to consume 25% of your maximum stealth to perform a swift upwards/diagonal dash which leaves a trail of plagued clouds";
                }
            }
        }
    }
}
