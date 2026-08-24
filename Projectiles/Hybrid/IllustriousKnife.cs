using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Healing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Hybrid
{
    public class IllustriousKnife : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Knife");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 240f)
            {
                Projectile.alpha += 4;
                Projectile.damage = (int)((double)Projectile.damage * 0.95);
                Projectile.knockBack = (float)(int)((double)Projectile.knockBack * 0.95);
            }
            if (Projectile.ai[0] < 240f)
            {
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            }
            else
            {
                Projectile.rotation += 0.5f;
            }
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 800f, 35f, 20f);
            if (Main.rand.NextBool(6))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 20, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int num303 = 0; num303 < 3; num303++)
            {
                int num304 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 20, 0f, 0f, 100, default, 0.8f);
                Main.dust[num304].noGravity = true;
                Main.dust[num304].velocity *= 1.2f;
                Main.dust[num304].velocity -= Projectile.oldVelocity * 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 600);
            if (target.type == NPCID.TargetDummy)
            {
                return;
            }
            float healAmt = (float)Projectile.damage * 0.015f;
            if ((int)healAmt == 0)
            {
                return;
            }
            if (Main.player[Main.myPlayer].lifeSteal <= 0f)
            {
                return;
            }
			CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], healAmt, ModContent.ProjectileType<RoyalHeal>(), 1200f, 1.5f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 600);
            float healAmt = (float)info.Damage * 0.015f;
            if ((int)healAmt == 0)
            {
                return;
            }
            if (Main.player[Main.myPlayer].lifeSteal <= 0f)
            {
                return;
            }
			CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], healAmt, ModContent.ProjectileType<RoyalHeal>(), 1200f, 1.5f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
