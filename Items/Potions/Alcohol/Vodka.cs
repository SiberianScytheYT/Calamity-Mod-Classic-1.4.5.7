using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class Vodka : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vodka");
/*
            Tooltip.SetDefault("The number one alcohol for creating great mixed drinks\n"
                               +"Boosts damage by 6% and critical strike chance by 2%\n"
                               +"Reduces life regen by 1 and defense by 4");
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
            Item.buffType = ModContent.BuffType<VodkaBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 3, 30, 0);
        }
    }
}
