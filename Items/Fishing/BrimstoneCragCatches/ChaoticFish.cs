using Terraria;
using Terraria.ModLoader;
using CalRD.Items.Materials;
using Terraria.GameContent.ItemDropRules;

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

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot) => itemLoot.Add(ModContent.ItemType<EssenceofChaos>(), 1, 5, 10);
    }
}
