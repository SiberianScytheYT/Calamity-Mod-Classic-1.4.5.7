using Terraria;
using Terraria.ModLoader;
using CalRD.Items.Materials;

namespace CalRD.Items.Fishing
{
	public class Xerocodile : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Xerocodile");
/*
            Tooltip.SetDefault("Right click to extract blood orbs");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = 2;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<BloodOrb>(), 5, 15);
        }

        /*public override void RightClick(Player player)
        {
			if (Main.rand.NextBool(3))
				DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<TheCamper>(), 1, 1);
			else if (Main.rand.NextBool(2))
				DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<CheatTestThing>(), 1, 1);
			else
			{
				DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<CheatTestThing>(), 1, 1);
				DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<TheCamper>(), 1, 1);
			}
        }*/
    }
}
