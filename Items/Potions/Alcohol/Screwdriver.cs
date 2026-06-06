using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class Screwdriver : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Screwdriver");
/*
            Tooltip.SetDefault("Do you have a screw loose?\n"
                               +"Boosts piercing projectile damage by 10%\n"
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
            Item.buffType = ModContent.BuffType<ScrewdriverBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 16, 60, 0);
        }
    }
}
