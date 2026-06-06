using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class SulphuricAcidCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphuric Acid Cannon");
/*
            Tooltip.SetDefault("Fires an acidic bubble that sticks to enemies and emits sulphuric gas");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 220;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 90;
            Item.height = 30;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item95;
            Item.shoot = ModContent.ProjectileType<SulphuricAcidBubble2>();
            Item.shootSpeed = 16f;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }
    }
}
