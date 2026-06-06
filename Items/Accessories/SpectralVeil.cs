using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class SpectralVeil : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectral Veil");
/*
            Tooltip.SetDefault("The inside of the cloak is full of teeth...\n" +
                "TOOLTIP LINE HERE\n" +
				"Teleportation is disabled while Chaos State is active\n" +
                "If you dodge something while invulnerable, you instantly gain full stealth\n" +
				"Revengeance drop");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 38;
            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            string hotkey = CalRD.SpectralVeilHotKey.TooltipHotkeyString();
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                {
                    line2.Text = "Press " + hotkey + " to consume 25% of your maximum stealth to perform a short range teleport and render you momentarily invulnerable";
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Calamity().spectralVeil = true;
        }
    }
}
