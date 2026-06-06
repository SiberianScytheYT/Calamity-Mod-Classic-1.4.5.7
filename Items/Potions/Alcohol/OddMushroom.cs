using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class OddMushroom : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Odd Mushroom");
/*
            Tooltip.SetDefault("Trippy");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 48;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.rare = 3;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<Trippy>();
            Item.buffTime = 216000;
            Item.value = Item.buyPrice(1, 0, 0, 0);
        }
    }
}
