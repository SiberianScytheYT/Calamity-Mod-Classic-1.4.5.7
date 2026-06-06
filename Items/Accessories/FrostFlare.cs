using CalRD.CalPlayer;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class FrostFlare : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Flare");
/*
            Tooltip.SetDefault("All melee attacks and projectiles inflict frostburn\n" +
                "Immunity to frostburn, chilled, and frozen\n" +
                "Resistant to cold attacks and +1 life regen\n" +
                "Being above 75% life grants the player 10% increased damage\n" +
                "Being below 25% life grants the player 10 defense and 15% increased max movement speed and acceleration\n" +
				"Revengeance drop");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.lifeRegen = 1;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
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
						line2.Text = "Provides heat and cold protection in Death Mode\n" +
						"Revengeance drop";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.frostFlare = true;
        }
    }
}
