using CalRD.Items.Materials;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class BleachedAnglingKit : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bleached Angling Kit");
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
            Item.rare = 5;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
			// Fishing
			var normalOnly = itemLoot.DefineNormalOnlyDropSet();
			normalOnly.Add(ItemID.AnglerTackleBag, 18);
			normalOnly.Add(ItemID.HighTestFishingLine, 12);
			normalOnly.Add(ItemID.TackleBox, 12);
			normalOnly.Add(ItemID.AnglerEarring, 12);
			normalOnly.Add(ItemID.FishermansGuide, 9);
			normalOnly.Add(ItemID.WeatherRadio, 9);
			normalOnly.Add(ItemID.Sextant, 9);
			normalOnly.Add(ItemID.AnglerHat, 4);
			normalOnly.Add(ItemID.AnglerVest, 4);
			normalOnly.Add(ItemID.AnglerPants, 4);
			normalOnly.Add(ItemID.FishingPotion, 4, 2, 3);
			normalOnly.Add(ItemID.SonarPotion, 4, 2, 3);
			normalOnly.Add(ItemID.CratePotion, 4, 2, 3);
			normalOnly.Add(ItemID.GoldenBugNet, 15);
			
			var expertPlus = itemLoot.DefineConditionalDropSet(new Conditions.IsExpert());
			expertPlus.Add(ItemID.AnglerTackleBag, 15);
			expertPlus.Add(ItemID.HighTestFishingLine, 10);
			expertPlus.Add(ItemID.TackleBox, 10);
			expertPlus.Add(ItemID.AnglerEarring, 10);
			expertPlus.Add(ItemID.FishermansGuide, 8);
			expertPlus.Add(ItemID.WeatherRadio, 8);
			expertPlus.Add(ItemID.Sextant, 8);
			expertPlus.Add(ItemID.AnglerHat, 2);
			expertPlus.Add(ItemID.AnglerVest, 2);
			expertPlus.Add(ItemID.AnglerPants, 2);
			expertPlus.Add(ItemID.FishingPotion, 2, 2, 3);
			expertPlus.Add(ItemID.SonarPotion, 2, 2, 3);
			expertPlus.Add(ItemID.CratePotion, 2, 2, 3);
			expertPlus.Add(ItemID.GoldenBugNet, 12);
        }
    }
}
