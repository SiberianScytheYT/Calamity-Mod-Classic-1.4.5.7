using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class WhiteWine : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("White Wine");
/*
            Tooltip.SetDefault("I drank a full barrel of this stuff once in one night, I couldn't remember who I was the next day\n"
                               +"Boosts magic damage by 10%\n"
                               +"Reduces defense by 6 and life regen by 1");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.rare = 4;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.healMana = 400;
            Item.buffType = ModContent.BuffType<WhiteWineBuff>();
            Item.buffTime = 10800; //3 minutes
            Item.value = Item.buyPrice(0, 16, 60, 0);
        }

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			if (PlayerInput.Triggers.JustPressed.QuickBuff)
			{
				player.statMana += Item.healMana;
				if (player.statMana > player.statManaMax2)
				{
					player.statMana = player.statManaMax2;
				}
				player.AddBuff(BuffID.ManaSickness, Player.manaSickTime, true);
				if (Main.myPlayer == player.whoAmI)
				{
					player.ManaEffect(Item.healMana);
				}
			}
            player.AddBuff(Item.buffType, Item.buffTime);
			return true;
		}
    }
}
