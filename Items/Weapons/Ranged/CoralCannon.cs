using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class CoralCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Coral Cannon");
/*
            Tooltip.SetDefault("Has a chance to shoot a big coral that stuns enemies");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 52;
            Item.height = 40;
            Item.crit += 10;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SmallCoral>();
            Item.shootSpeed = 10f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(5))
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<BigCoral>(), (int)((double)damage * 2.0), Item.knockBack * 2f, player.whoAmI, 0.0f, 0.0f);
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }
            return false;
        }
    }
}
