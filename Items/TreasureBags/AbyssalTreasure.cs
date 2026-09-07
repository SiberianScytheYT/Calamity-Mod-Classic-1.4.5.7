using CalRD.Items.Potions;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class AbyssalTreasure : ModItem
    {
	    internal static readonly int[] AbyssalTreasurePotions = new int[]
	    {
		    ItemID.SpelunkerPotion,
		    ItemID.MagicPowerPotion,
		    ItemID.ShinePotion,
		    ItemID.WaterWalkingPotion,
		    ItemID.ObsidianSkinPotion,
		    ItemID.WaterWalkingPotion,
		    ItemID.GravitationPotion,
		    ItemID.RegenerationPotion,
		    ModContent.ItemType<TriumphPotion>(),
		    ModContent.ItemType<AnechoicCoating>(),
		    ItemID.GillsPotion,
		    ItemID.EndurancePotion,
		    ItemID.HeartreachPotion,
		    ItemID.FlipperPotion,
		    ItemID.LifeforcePotion,
		    ItemID.InfernoPotion
	    };
	    
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyssal Treasure");
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
            Item.rare = 1; //Blue for thematics
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
	        // 1/10 chance for potions
	        var tenPercentPotions = itemLoot.Add(new OneFromOptionsNotScaledWithLuckDropRule(10, 1, AbyssalTreasurePotions));
			
	        // IF YOU DON'T GET POTIONS
	        // 10% chance for 2-6 spelunker glowsticks in normal mode
	        // 10% chance for 3-13 spelunker glowsticks in expert mode
	        // 10% chance 10-20 hellfire arrows
	        // 10% chance for 1 hadal stew
	        // 10% chance for 1 sticky dynamite
	        // 60% chance for 40-60 silver
	        
	        // glowstick amounts remain accurate to calamity 1.4.5
	        // old coin code was convoluted, 40-60 silver seems fine enough (nobody will even notice)
	        
	        // 2-6 Spelunker Glowsticks
	        CommonDrop normalSpelunkerGlowsticks = new ItemDropWithConditionRule(ItemID.SpelunkerGlowstick, 1, 2, 6, new Conditions.NotExpert());
	        // 3-13 Spelunker Glowsticks
	        CommonDrop expertSpelunkerGlowsticks = new ItemDropWithConditionRule(ItemID.SpelunkerGlowstick, 1, 3, 13, new Conditions.IsExpert());
	        // 10-20 Hellfire Arrows
	        CommonDrop hellfireArrows = new CommonDrop(ItemID.HellfireArrow, 1, 10, 20);
	        // 1 Hadal Stew
	        CommonDrop hadalStew = new CommonDrop(ModContent.ItemType<SunkenStew>(), 1);
	        // 1 Sticky Dynamite
	        CommonDrop stickyDynamite = new CommonDrop(ItemID.StickyDynamite, 1);
	        // 40-60 Silver Coin
	        CommonDrop silver = new CommonDrop(ItemID.SilverCoin, 1, 40, 60);
	        
	        OneFromRulesRule otherDrops = new OneFromRulesRule(1, new IItemDropRule[] { normalSpelunkerGlowsticks, expertSpelunkerGlowsticks, hellfireArrows, hadalStew, stickyDynamite, silver, silver, silver, silver, silver, silver });
	        tenPercentPotions.OnFailedRoll(otherDrops);
        }
    }
}
