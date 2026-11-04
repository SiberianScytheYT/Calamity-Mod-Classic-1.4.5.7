using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
namespace CalRD.Items.Fishing
{
	public class EnchantedStarfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Enchanted Starfish");
/*
            Tooltip.SetDefault("Permanently increases maximum mana by 20");
*/
        }

        public override void SetDefaults()
        {
            Item.rare = 2;
            Item.width = 30;
            Item.height = 26;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item29;
            Item.consumable = true;
			Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
			if (player.itemAnimation > 0 && (player.ConsumedManaCrystals < Player.ManaCrystalMax && player.itemTime == 0))
			{
				player.itemTime = Item.useTime;
				player.UseManaMaxIncreasingItem(20);
				player.ConsumedManaCrystals++;
				AchievementsHelper.HandleSpecialEvent(player, 1);
			}
			return false;
        }
    }
}
