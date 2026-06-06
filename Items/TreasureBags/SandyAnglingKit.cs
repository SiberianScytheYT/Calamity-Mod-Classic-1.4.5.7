using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class SandyAnglingKit : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sandy Angling Kit");
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
            Item.rare = 1;
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
			int fishingAccChance = !Main.expertMode ? 15 : 12;
			int fishFindAccChance = !Main.expertMode ? 10 : 9;
			int anglerArmorChance = !Main.expertMode ? 5 : 4;
			int potionChance = !Main.expertMode ? 5 : 4;
			int bugNetChance = !Main.expertMode ? 20 : 18;
			// Fishing
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
			DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ItemID.GoldenBugNet, NPC.downedBoss3, bugNetChance, 1, 1);
        }
    }
}
