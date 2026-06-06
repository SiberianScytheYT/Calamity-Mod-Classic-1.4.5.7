using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing
{
    public class StuffedFish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stuffed Fish");
/*
            Tooltip.SetDefault("Right click to extract herbs and seeds");
*/
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 34;
            Item.height = 30;
            Item.rare = 2;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            int herbMin = 1;
            int herbMax = 3;
            int seedMin = 2;
            int seedMax = 5;
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Daybloom, 0.25f, herbMin, herbMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Moonglow, 0.25f, herbMin, herbMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Waterleaf, 0.25f, herbMin, herbMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Deathweed, 0.25f, herbMin, herbMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Shiverthorn, 0.25f, herbMin, herbMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Fireblossom, 0.25f, herbMin, herbMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Blinkroot, 0.25f, herbMin, herbMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.DaybloomSeeds, 0.2f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.MoonglowSeeds, 0.2f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.WaterleafSeeds, 0.2f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.DeathweedSeeds, 0.2f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.ShiverthornSeeds, 0.2f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.FireblossomSeeds, 0.2f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.BlinkrootSeeds, 0.2f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.GrassSeeds, 0.1f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.JungleGrassSeeds, 0.1f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.MushroomGrassSeeds, 0.1f, seedMin, seedMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.PumpkinSeed, 0.05f, seedMin, seedMax);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ItemID.CorruptSeeds, !WorldGen.crimson, 0.05f, seedMin, seedMax);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ItemID.CrimsonSeeds, WorldGen.crimson, 0.05f, seedMin, seedMax);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ItemID.HallowedSeeds, Main.hardMode, 0.05f, seedMin, seedMax);
            ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
            if (thorium != null)
			{
				DropHelper.DropItemChance(player.GetSource_FromThis(), player, thorium.Find<ModItem>("MarineKelp").Type, 0.25f, herbMin, herbMax);
				DropHelper.DropItemChance(player.GetSource_FromThis(), player, thorium.Find<ModItem>("MarineKelpSeeds").Type, 0.1f, seedMin, seedMax);
			}
	        ModLoader.TryGetMod("SacredTools", out Mod shadowsOfAbaddon);
            if (shadowsOfAbaddon != null)
			{
				DropHelper.DropItemChance(player.GetSource_FromThis(), player, shadowsOfAbaddon.Find<ModItem>("Welkinbell").Type, 0.25f, herbMin, herbMax);
				DropHelper.DropItemChance(player.GetSource_FromThis(), player, shadowsOfAbaddon.Find<ModItem>("WelkinbellSeeds").Type, 0.1f, seedMin, seedMax);
				DropHelper.DropItemCondition(player.GetSource_FromThis(), player, shadowsOfAbaddon.Find<ModItem>("Illumifern").Type, Main.hardMode, 0.25f, herbMin, herbMax);
				DropHelper.DropItemCondition(player.GetSource_FromThis(), player, shadowsOfAbaddon.Find<ModItem>("IllumifernSeeds").Type, Main.hardMode, 0.1f, seedMin, seedMax);
				//There's no mod call for Abaddon being dead
				//DropHelper.DropItemCondition(player.GetSource_FromThis(), player, shadowsOfAbaddon.ItemType("Enduflora"), SacredTools.ModdedWorld.downedAbaddon, 0.25f, herbMin, herbMax);
				//DropHelper.DropItemCondition(player.GetSource_FromThis(), player, shadowsOfAbaddon.ItemType("EndufloraSeeds"), SacredTools.ModdedWorld.downedAbaddon, 0.1f, seedMin, seedMax);
			}
        }
    }
}
