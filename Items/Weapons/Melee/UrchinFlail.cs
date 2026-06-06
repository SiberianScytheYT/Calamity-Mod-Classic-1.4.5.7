using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class UrchinFlail : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Urchin Flail");
/*
            Tooltip.SetDefault("Launch an urchin ball, which shoots a spike on contact with an enemy");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 44;
            Item.height = 36;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<UrchinBall>();
            Item.shootSpeed = 12f;
        }
    }
}
