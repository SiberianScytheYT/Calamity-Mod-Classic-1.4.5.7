using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class EvergreenGin : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Evergreen Gin");
/*
            Tooltip.SetDefault("It tastes like a Christmas tree if you can imagine that\n"
                               +"Boosts nature-based weapon damage by 15% and damage reduction by 5%\n"
                               +"Reduces life regen by 1");
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
            Item.buffType = ModContent.BuffType<EvergreenGinBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 16, 60, 0);
        }
    }
}
