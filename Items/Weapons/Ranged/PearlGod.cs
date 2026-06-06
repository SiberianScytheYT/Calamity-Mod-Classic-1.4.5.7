using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class PearlGod : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pearl God");
/*
            Tooltip.SetDefault("Your life is mine...");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 54;
            Item.height = 38;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<ShockblastRound>();
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 2; ++index)
            {
                float speedMult = (float)(index + 1) * 0.15f;
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X * speedMult, velocity.Y * speedMult, ModContent.ProjectileType<FrostsparkBulletProj>(), (int)((double)damage * 0.75), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ShockblastRound>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
