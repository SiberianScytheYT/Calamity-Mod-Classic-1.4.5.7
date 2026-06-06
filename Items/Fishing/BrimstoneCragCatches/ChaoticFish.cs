using Terraria;
using Terraria.ModLoader;
using CalRD.Items.Materials;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
    public class ChaoticFish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Chaotic Fish");
/*
            Tooltip.SetDefault("The horns lay a curse on those who touch it\n" +
			"Right click to extract essence");
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<EssenceofChaos>(), 5, 10);
        }
    }
}
