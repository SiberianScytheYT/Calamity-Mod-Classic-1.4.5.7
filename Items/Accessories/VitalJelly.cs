using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class VitalJelly : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vital Jelly");
/*
            Tooltip.SetDefault("20% increased movement speed\n" +
                "24% increased jump speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            bool autoJump = Main.player[Main.myPlayer].autoJump;
			string jumpAmt = autoJump ? "6" : "24";
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                {
                    line2.Text = jumpAmt + "% increased jump speed";
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.2f;
            player.jumpSpeedBoost += player.autoJump ? 0.3f : 1.2f;
        }
    }
}
