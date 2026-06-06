using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing
{
	public class Spadefish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spadefish");
/*
            Tooltip.SetDefault("How can a fish be used to dig through the ground?\n" +
				"Some questions are best left unanswered.");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 46;
            Item.height = 44;
            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.useTurn = true;
            Item.pick = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
    }
}
