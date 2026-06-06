using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class PermafrostsConcoction : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Permafrost's Concoction");
/*
            Tooltip.SetDefault("Increases maximum mana by 50 and reduces mana cost by 15%\nIncreases life regen as life decreases\nIncreases life regen when afflicted with Poison, On Fire, or Brimstone Flames\nYou will survive fatal damage and revive with 30% life on a 3 minute cooldown\nYou are encased in an ice barrier for 3 seconds when revived");
*/
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 36;
            Item.height = 34;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip4")
					{
						line2.Text = "You are encased in an ice barrier for 3 seconds when revived\n" +
						"Provides heat and cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Calamity().permafrostsConcoction = true;
        }
    }
}
