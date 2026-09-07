using CalRD.Items.Potions;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class SulphuricTreasure : ModItem
    {
	    private readonly int[] SulphuricTreasurePotions = new int[]
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
            //DisplayName.SetDefault("Sulphuric Treasure");
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
            Item.rare = 2; //Green for thematics
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
	        // 1/15 chance for potions
	        var oneInFifteenPotions = itemLoot.Add(new OneFromOptionsNotScaledWithLuckDropRule(15, 1, SulphuricTreasurePotions));
	        
	        // IF YOU DONT GET POTIONS
	        // 10% chance for 2-6 Glowsticks in normal mode
	        // 10% chance for 3-13 Glowsticks in expert mode
	        // 10% chance for 10-20 Jester Arrows
	        // 10% chance for 1 Healing Potion
	        // 10% chance for 5-8 BOMBS?!
	        // 0% chance for Lamp Oil (no item in game)
	        // 60% chance for 40-60 Silver Coins
	        
	        // glowstick amounts remain accurate to calamity 1.4.5
	        // old coin code was convoluted, 40-60 silver seems fine enough (nobody will even notice)
	        
	        // 2-6 Spelunker Glowsticks
	        CommonDrop normalGlowsticks = new ItemDropWithConditionRule(ItemID.Glowstick, 1, 2, 6, new Conditions.NotExpert());
	        // 3-13 Spelunker Glowsticks
	        CommonDrop expertGlowsticks = new ItemDropWithConditionRule(ItemID.Glowstick, 1, 3, 13, new Conditions.IsExpert());
	        // 10-20 Jester Arrows
	        CommonDrop jesterArrows = new CommonDrop(ItemID.JestersArrow, 1, 10, 20);
	        // 1 Healing Potion
	        CommonDrop healingPotion = new CommonDrop(ItemID.HealingPotion, 1);
	        // 5-8 Bombs
	        CommonDrop bombs = new CommonDrop(ItemID.Bomb, 1, 5, 8);
	        // 40-60 Silver Coin
	        CommonDrop silver = new CommonDrop(ItemID.SilverCoin, 1, 40, 60);
	        
	        OneFromRulesRule otherDrops = new OneFromRulesRule(1, new IItemDropRule[] { normalGlowsticks, expertGlowsticks, jesterArrows, healingPotion, bombs, silver, silver, silver, silver, silver, silver });

	        oneInFifteenPotions.OnFailedRoll(otherDrops);
        }
    }
}
