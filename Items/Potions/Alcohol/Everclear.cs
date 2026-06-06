using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class Everclear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Everclear");
/*
            Tooltip.SetDefault("This is the most potent booze I have, be careful with it\n"
                               +"Boosts damage by 25%\n"
                               +"Reduces life regen by 10 and defense by 40");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.rare = 2;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<EverclearBuff>();
            Item.buffTime = 900; //15 seconds
            Item.value = Item.buyPrice(0, 6, 60, 0);
        }
    }
}
