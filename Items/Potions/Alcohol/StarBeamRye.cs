using CalRD.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions.Alcohol
{
    public class StarBeamRye : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Beam Rye");
/*
            Tooltip.SetDefault("Made from some stuff I found near the Astral Meteor crash site, don't worry it's safe, trust me\n"
                               +"Boosts max mana by 50, magic damage by 8%,\n"
                               +"and reduces mana usage by 10%\n"
                               +"Reduces defense by 6 and life regen by 1");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.rare = 3;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<StarBeamRyeBuff>();
            Item.buffTime = 18000; //5 minutes
            Item.value = Item.buyPrice(0, 13, 30, 0);
        }
    }
}
