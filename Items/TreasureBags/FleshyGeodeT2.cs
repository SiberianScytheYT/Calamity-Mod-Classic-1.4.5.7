using CalRD.Items.Materials;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class FleshyGeodeT2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Necromantic Geode");
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
            Item.rare = 11;
			Item.Calamity().postMoonLordRarity = 12;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            // Different drop rates on Normal and Expert, so define normal first, then expert
            // 5-10 bars on Normal, 7-12 bars on Expert
            // 1-3 cores on Normal, 2-4 cores on Expert
            // 50% chance of life alloy on Normal, 100% on Expert
            // 33% chance of core of calamity on Normal, 50% on Expert
            // 50-60 bloodstone on Normal, 60-70 bloodstone on Expert
            var normalOnly = itemLoot.DefineNormalOnlyDropSet();
            normalOnly.Add(ModContent.ItemType<VerstaltiteBar>(), 1, 5, 10);
            normalOnly.Add(ModContent.ItemType<DraedonBar>(), 1, 5, 10);
            normalOnly.Add(ModContent.ItemType<CruptixBar>(), 1, 5, 10);
            normalOnly.Add(ModContent.ItemType<CoreofEleum>(), 1, 1, 3);
            normalOnly.Add(ModContent.ItemType<CoreofCinder>(), 1, 1, 3);
            normalOnly.Add(ModContent.ItemType<CoreofChaos>(), 1, 1, 3);
            normalOnly.Add(ModContent.ItemType<BarofLife>(), 2);
            normalOnly.Add(ModContent.ItemType<CoreofCalamity>(), 3);
            normalOnly.Add(ModContent.ItemType<Bloodstone>(), 1, 50, 60);
            
            var expertPlus = itemLoot.DefineConditionalDropSet(new Conditions.IsExpert());
            expertPlus.Add(ModContent.ItemType<VerstaltiteBar>(), 1, 7, 12);
            expertPlus.Add(ModContent.ItemType<DraedonBar>(), 1, 7, 12);
            expertPlus.Add(ModContent.ItemType<CruptixBar>(), 1, 7, 12);
            expertPlus.Add(ModContent.ItemType<CoreofEleum>(), 1, 2, 4);
            expertPlus.Add(ModContent.ItemType<CoreofCinder>(), 1, 2, 4);
            expertPlus.Add(ModContent.ItemType<CoreofChaos>(), 1, 2, 4);
            expertPlus.Add(ModContent.ItemType<BarofLife>());
            expertPlus.Add(ModContent.ItemType<CoreofCalamity>(), 2);
            expertPlus.Add(ModContent.ItemType<Bloodstone>(), 1, 60, 70);
        }
    }
}
