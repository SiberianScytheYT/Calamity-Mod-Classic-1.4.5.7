using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class CaribbeanRum : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Caribbean Rum");
/*
            Tooltip.SetDefault("Why is the rum gone?\n"
                               +"Boosts life regen by 2 and movement speed and wing flight time by 20%\n"
                               +"Makes you floaty and reduces defense by 12");
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
            Item.buffType = ModContent.BuffType<CaribbeanRumBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 20, 0, 0);
        }
    }
}
