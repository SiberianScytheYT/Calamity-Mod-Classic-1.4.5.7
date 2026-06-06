using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class Rum : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rum");
/*
            Tooltip.SetDefault("Sweet and potent, just how I like it\n"
                               +"Boosts life regen by 2 and movement speed by 10%\n"
                               +"Reduces defense by 8");
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
            Item.buffType = ModContent.BuffType<RumBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }
    }
}
