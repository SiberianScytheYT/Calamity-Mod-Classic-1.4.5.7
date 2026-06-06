using CalRD.Buffs.Alcohol;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class GrapeBeer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Grape Beer");
/*
            Tooltip.SetDefault("This crap is abhorrent but you might like it\n"
                               +"Reduces defense by 2 and movement speed by 5%");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.rare = 1;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.healLife = 100;
            Item.healMana = 100;
            Item.consumable = true;
            Item.potion = true;
            Item.value = Item.buyPrice(0, 0, 65, 0);
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<GrapeBeerBuff>(), 900);
        }
    }
}
