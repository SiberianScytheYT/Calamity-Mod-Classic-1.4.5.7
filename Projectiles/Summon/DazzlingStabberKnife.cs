using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Summon
{
    public class DazzlingStabberKnife : ModProjectile
    {
        public const float HomingSpeed = 17f;
        public const float HomingInertia = 20f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Knife");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0f;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            NPC potentialTarget = Projectile.Center.MinionHoming(750f, Main.player[Projectile.owner]);
            if (potentialTarget != null)
            {
                if (Projectile.Distance(potentialTarget.Center) > 70f)
                {
                    Projectile.velocity = (Projectile.velocity * (HomingInertia - 1f) + Projectile.DirectionTo(potentialTarget.Center) * HomingSpeed) / HomingInertia;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 60);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 60);
        }
    }
}
