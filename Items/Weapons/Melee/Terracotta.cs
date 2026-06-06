using CalRD.Projectiles.Healing;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Terracotta : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra-cotta");
/*
            Tooltip.SetDefault("Causes enemies to erupt into healing projectiles on death\n" +
                "Enemies explode on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 50;
			Item.height = 62;
			Item.scale = 1.5f;
			Item.damage = 125;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shootSpeed = 5f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0)
            {
                float randomSpeedX = (float)Main.rand.Next(3);
                float randomSpeedY = (float)Main.rand.Next(3, 5);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<TerracottaProj>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<TerracottaProj>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, -randomSpeedY, ModContent.ProjectileType<TerracottaProj>(), 0, 0f, player.whoAmI);
            }
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<TerracottaExplosion>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, player.whoAmI);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (target.statLife <= 0)
            {
                float randomSpeedX = (float)Main.rand.Next(3);
                float randomSpeedY = (float)Main.rand.Next(3, 5);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<TerracottaProj>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<TerracottaProj>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, -randomSpeedY, ModContent.ProjectileType<TerracottaProj>(), 0, 0f, player.whoAmI);
            }
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<TerracottaExplosion>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, player.whoAmI);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 246);
            }
        }
    }
}
