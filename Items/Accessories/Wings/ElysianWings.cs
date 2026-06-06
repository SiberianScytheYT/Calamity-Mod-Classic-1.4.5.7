using CalRD.CalPlayer;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class ElysianWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elysian Wings");
/*
            Tooltip.SetDefault("Blessed by the Profaned Flame\n" +
				"Horizontal speed: 9.75\n" +
                "Acceleration multiplier: 2.7\n" +
                "Great vertical speed\n" +
                "Flight time: 200\n" +
				"Temporary immunity to lava and 40% increased movement speed");
*/
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(200, 2.7f, 9.75f);
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip5")
					{
						line2.Text = "Temporary immunity to lava and 40% increased movement speed\n" +
						"Provides heat protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.moveSpeed += 0.4f;
            player.lavaMax += 240;
            player.wingTimeMax = 200;
            player.noFallDmg = true;
            modPlayer.elysianFire = true;
            if (hideVisual)
            {
                modPlayer.elysianFire = false;
            }
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.135f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 9.75f;
            acceleration *= 2.7f;
        }
    }
}
