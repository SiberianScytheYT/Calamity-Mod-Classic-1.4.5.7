using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class Moonshine : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moonshine");
/*
            Tooltip.SetDefault("This stuff is pretty strong but I'm sure you can handle it\n"
                               +"Increases defense by 10 and damage reduction by 5%\n"
                               +"Reduces life regen by 1");
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
            Item.buffType = ModContent.BuffType<MoonshineBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 3, 30, 0);
        }
    }
}
