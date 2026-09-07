using Terraria;
using Terraria.GameContent.ItemDropRules;
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

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int herbMin = 1;
            int herbMax = 3;
            int seedMin = 2;
            int seedMax = 5;
            itemLoot.Add(ItemID.Daybloom, 4, herbMin, herbMax);
            itemLoot.Add(ItemID.Moonglow, 4, herbMin, herbMax);
            itemLoot.Add(ItemID.Waterleaf, 4, herbMin, herbMax);
            itemLoot.Add(ItemID.Deathweed, 4, herbMin, herbMax);
            itemLoot.Add(ItemID.Shiverthorn, 4, herbMin, herbMax);
            itemLoot.Add(ItemID.Fireblossom, 4, herbMin, herbMax);
            itemLoot.Add(ItemID.Blinkroot, 4, herbMin, herbMax);
            itemLoot.Add(ItemID.DaybloomSeeds, 5, seedMin, seedMax);
            itemLoot.Add(ItemID.MoonglowSeeds, 5, seedMin, seedMax);
            itemLoot.Add(ItemID.WaterleafSeeds, 5, seedMin, seedMax);
            itemLoot.Add(ItemID.DeathweedSeeds, 5, seedMin, seedMax);
            itemLoot.Add(ItemID.ShiverthornSeeds, 5, seedMin, seedMax);
            itemLoot.Add(ItemID.FireblossomSeeds, 5, seedMin, seedMax);
            itemLoot.Add(ItemID.BlinkrootSeeds, 5, seedMin, seedMax);
            itemLoot.Add(ItemID.GrassSeeds, 10, seedMin, seedMax);
            itemLoot.Add(ItemID.JungleGrassSeeds, 10, seedMin, seedMax);
            itemLoot.Add(ItemID.MushroomGrassSeeds, 10, seedMin, seedMax);
            itemLoot.Add(ItemID.PumpkinSeed, 20, seedMin, seedMax);
            itemLoot.AddIf(() => !WorldGen.crimson, ItemID.CorruptSeeds, 20, seedMin, seedMax);
            itemLoot.AddIf(() => WorldGen.crimson, ItemID.CrimsonSeeds, 20, seedMin, seedMax);
            itemLoot.AddIf(() => Main.hardMode, ItemID.HallowedSeeds, 20, seedMin, seedMax);
            ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
            if (thorium is not null)
            {
	            try
	            {
		            itemLoot.Add(thorium.Find<ModItem>("MarineKelp").Type, 4, herbMin, herbMax);
		            itemLoot.Add(thorium.Find<ModItem>("MarineKelpSeeds").Type, 10, seedMin, seedMax); 
	            }
	            catch
	            {
		            CalRD.Instance.Logger.Debug("One of the items in this file got renamed internally. Please report this in the Calamity Mod Classic 1.4.5.7 bug reports thread in the #bug-reports forum found in the YuHther mods discord server.");
	            }
            }
	        ModLoader.TryGetMod("SacredTools", out Mod shadowsOfAbaddon);
            if (shadowsOfAbaddon is not null)
			{
				try
				{
					itemLoot.Add(shadowsOfAbaddon.Find<ModItem>("Welkinbell").Type, 4, herbMin, herbMax);
					itemLoot.Add(shadowsOfAbaddon.Find<ModItem>("WelkinbellSeeds").Type, 10, seedMin, seedMax);
					itemLoot.AddIf(() => Main.hardMode, shadowsOfAbaddon.Find<ModItem>("Illumifern").Type, 4, herbMin, herbMax);
					itemLoot.AddIf(() => Main.hardMode, shadowsOfAbaddon.Find<ModItem>("IllumifernSeeds").Type, 10, seedMin, seedMax);
					//There's no mod call for Abaddon being dead
					//itemLoot.AddIf(() => SacredTools.ModdedWorld.downedAbaddon, shadowsOfAbaddon.ItemType("Enduflora"), 4, herbMin, herbMax);
					//itemLoot.AddIf(() => SacredTools.ModdedWorld.downedAbaddon, shadowsOfAbaddon.ItemType("EndufloraSeeds"), 10, seedMin, seedMax);
				}
				catch
				{
					CalRD.Instance.Logger.Debug("One of the items in this file got renamed internally. Please report this in the Calamity Mod Classic 1.4.5.7 bug reports thread in the #bug-reports forum found in the YuHther mods discord server.");
				}
			}
        }
    }
}
