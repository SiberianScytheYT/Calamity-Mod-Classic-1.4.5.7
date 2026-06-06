using CalRD.Items.Materials;
using Terraria;
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

        public override void RightClick(Player player)
        {
			int anglerTackleBagChance = !Main.expertMode ? 18 : 15;
			int fishingAccChance = !Main.expertMode ? 12 : 10;
			int fishFindAccChance = !Main.expertMode ? 9 : 8;
			int anglerArmorChance = !Main.expertMode ? 4 : 2;
			int potionChance = !Main.expertMode ? 4 : 2;
			int bugNetChance = !Main.expertMode ? 15 : 12;
			// Fishing
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.AnglerTackleBag, anglerTackleBagChance, 1, 1);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.HighTestFishingLine, fishingAccChance);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.TackleBox, fishingAccChance);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.AnglerEarring, fishingAccChance);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.FishermansGuide, fishFindAccChance);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.WeatherRadio, fishFindAccChance);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Sextant, fishFindAccChance);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.AnglerHat, anglerArmorChance);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.AnglerVest, anglerArmorChance);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.AnglerPants, anglerArmorChance);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.FishingPotion, potionChance, 2, 3);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.SonarPotion, potionChance, 2, 3);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.CratePotion, potionChance, 2, 3);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.GoldenBugNet, bugNetChance, 1, 1);
        }
    }
}
