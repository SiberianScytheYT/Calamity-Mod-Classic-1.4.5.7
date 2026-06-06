using CalRD.CalPlayer;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class FrostBarrier : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Barrier");
/*
            Tooltip.SetDefault("You will freeze enemies near you when you are struck\n" +
                               "You are immune to the chilled debuff");
*/
        }

        public override void SetDefaults()
        {
            Item.defense = 4;
            Item.width = 20;
            Item.height = 24;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.rare = 4;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
					{
						line2.Text = "You are immune to the chilled debuff\n" +
						"Provides heat and cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.fBarrier = true;
            player.buffImmune[BuffID.Chilled] = true;
        }
    }
}
