using Terraria;
using Terraria.ModLoader;
using CalRD.Items.Materials;

namespace CalRD.Items.Fishing
{
    public class FishofEleum : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fish of Eleum");
/*
            Tooltip.SetDefault("Right click to extract essence");
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<EssenceofEleum>(), 5, 10);
        }
    }
}
