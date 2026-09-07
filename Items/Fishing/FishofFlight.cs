using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing
{
    public class FishofFlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fish of Flight");
/*
            Tooltip.SetDefault("Right click to extract souls");
*/
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 30;
            Item.height = 30;
            Item.rare = 3;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot) => itemLoot.Add(ItemID.SoulofFlight, 1, 2, 5);
    }
}
