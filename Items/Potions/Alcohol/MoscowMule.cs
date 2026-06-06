using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class MoscowMule : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moscow Mule");
/*
            Tooltip.SetDefault("I once heard the copper mug can be toxic and I told 'em 'listen dummy, I'm already poisoning myself'\n"
                               +"Boosts damage and knockback by 9% and critical strike chance by 3%\n"
                               +"Reduces life regen by 2");
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
            Item.buffType = ModContent.BuffType<MoscowMuleBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 16, 60, 0);
        }
    }
}
