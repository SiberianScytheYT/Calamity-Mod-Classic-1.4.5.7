using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing
{
    public class FishofNight : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fish of Night");
/*
            Tooltip.SetDefault("Right click to extract souls");
*/
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 34;
            Item.height = 34;
            Item.rare = 3;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.SoulofNight, 2, 5);
        }
    }
}
