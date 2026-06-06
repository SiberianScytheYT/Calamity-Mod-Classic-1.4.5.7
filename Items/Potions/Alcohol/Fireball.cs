using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class Fireball : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fireball");
/*
            Tooltip.SetDefault("A great-tasting cinnamon whiskey\n"
                               +"Boosts all fire-based weapon damage by 10%\n"
                               +"Cursed flame, shadowflame, god slayer inferno, brimstone flame, and frostburn weapons will not receive this benefit\n"
                               +"The weapon must be more fire-related than anything else\nReduces life regen by 1");
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
            Item.buffType = ModContent.BuffType<FireballBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 6, 60, 0);
        }
    }
}
