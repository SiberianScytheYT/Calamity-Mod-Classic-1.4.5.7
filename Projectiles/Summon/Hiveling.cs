using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Summon
{
    public class Hiveling : ModProjectile
    {
        public override string Texture => "CalRD/NPCs/Astral/Hiveling";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hiveling");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
			Player player = Main.player[Projectile.owner];
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 3)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4;
            Projectile.spriteDirection = -Projectile.direction;
            Projectile.rotation = Projectile.velocity.X * 0.05f;

            NPC potentialTarget = Projectile.Center.MinionHoming(1200f, Main.player[Projectile.owner]);
            if (potentialTarget != null)
                Projectile.velocity = (Projectile.velocity * 30f + Projectile.DirectionTo(potentialTarget.Center) * 10f) / 31f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 60);
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralBlue>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
