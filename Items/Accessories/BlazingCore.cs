using CalRD.CalPlayer;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class BlazingCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blazing Core");
/*
            Tooltip.SetDefault("The searing core of the profaned goddess\n" +
                               "10% damage reduction\n" +
                               "Being hit creates a miniature sun that lingers, dealing damage to nearby enemies\n" +
                               "The sun will slowly drag enemies into it\n" +
                               "Only one sun can be active at once\n" +
							   "Provides a moderate amount of light in the Abyss");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 6));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 46;
            Item.accessory = true;
            Item.expert = true;
            Item.rare = 9;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip5")
					{
						line2.Text = "Provides a moderate amount of light in the Abyss\n" +
						"Provides heat and cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.blazingCore = true;
        }
    }
}
