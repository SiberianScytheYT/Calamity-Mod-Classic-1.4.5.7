using CalRD.Buffs.Potions;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
    public class Bloodfin : ModItem
    {
		public static int BuffType = ModContent.BuffType<BloodfinBoost>();
		public static int BuffDuration = 600;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodfin");
/*
            Tooltip.SetDefault("The wonders of angiogenesis\n"
                               +"Grants a buff that boosts life regen for 10 seconds\n"
                               +"The life regen boost is stronger if below 75% health\n"
                               +"10 second duration");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.healLife = 240;
            Item.potion = true;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType, BuffDuration);
        }
    }
}
