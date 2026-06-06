using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class FlakToxicannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flak Toxicannon");
/*
            Tooltip.SetDefault("Fires angled shots in the direction of the cursor\n" +
                               "Can only be shot in a cone direction above the player\n" +
                               "High IQ required");
*/
        }
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 88;
            Item.height = 28;
            Item.useTime = Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ToxicannonShot>();
            Item.shootSpeed = 9f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float angle = new Vector2(velocity.X, velocity.Y).ToRotation() + MathHelper.PiOver2;
            if (angle <= -MathHelper.PiOver4 || angle >= MathHelper.PiOver4)
                return false;
            angle -= MathHelper.PiOver2;

            Vector2 velocity1 = angle.ToRotationVector2() * (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y) * new Vector2(1f, 2f);

            Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<ToxicannonShot>(), damage, Item.knockBack, player.whoAmI);
            return false;
        }
    }
}
