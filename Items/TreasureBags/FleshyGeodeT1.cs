using CalRD.Items.Materials;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class FleshyGeodeT1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fleshy Geode");
/*
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
*/
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = 8;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            // Different drop rates on Normal and Expert, so define normal first, then expert
            // 1-3 bars on Normal, 2-3 bars on Expert
            // 1-2 cores on Normal, 1-3 cores on Expert
            var normalOnly = itemLoot.DefineNormalOnlyDropSet();
            normalOnly.Add(ModContent.ItemType<VerstaltiteBar>(), 1, 1, 3);
            normalOnly.Add(ModContent.ItemType<DraedonBar>(), 1, 1, 3);
            normalOnly.Add(ModContent.ItemType<CruptixBar>(), 1, 1, 3);
            normalOnly.Add(ModContent.ItemType<CoreofCinder>(), 1, 1, 2);
            normalOnly.Add(ModContent.ItemType<CoreofEleum>(), 1, 1, 2);
            normalOnly.Add(ModContent.ItemType<CoreofChaos>(), 1, 1, 2);
            
            var expertPlus = itemLoot.DefineConditionalDropSet(new Conditions.IsExpert());
            expertPlus.Add(ModContent.ItemType<VerstaltiteBar>(), 2, 3);
            expertPlus.Add(ModContent.ItemType<DraedonBar>(), 2, 3);
            expertPlus.Add(ModContent.ItemType<CruptixBar>(), 2, 3);
            expertPlus.Add(ModContent.ItemType<CoreofCinder>(), 1, 3);
            expertPlus.Add(ModContent.ItemType<CoreofEleum>(), 1, 3);
            expertPlus.Add(ModContent.ItemType<CoreofChaos>(), 1, 3);
        }
    }
}
