using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.StatDebuffs;

namespace CalRD.Projectiles.Magic
{
	public class SlitheringEelProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eel");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 360;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            if (Projectile.frameCounter++ % 8f == 7f)
            {
                Projectile.frame++;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
            Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = (Projectile.spriteDirection * Projectile.velocity).ToRotation();
            if (Projectile.ai[0] >= 3f)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
            }
            if (Projectile.timeLeft % 80f < 35f && Projectile.Distance(Main.MouseWorld) > 70f)
            {
                float angleToTarget = Projectile.AngleTo(Main.MouseWorld);
                float angleDifference = MathHelper.WrapAngle(angleToTarget - Projectile.velocity.ToRotation());
                Projectile.velocity = Projectile.velocity.RotatedBy(angleDifference / 9f);
            }
            if (Projectile.timeLeft % 65f == 64f)
            {
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.UnitY * 7f, ModContent.ProjectileType<EelDrop>(), Projectile.damage / 2, 2f, Projectile.owner);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, new Color(255, 255, 255, 127), ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0]++;
            target.AddBuff(BuffID.Venom, 180);
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.ai[0]++;
            target.AddBuff(BuffID.Venom, 180);
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
        }
        
        public override void OnKill(int timeLeft)
        {
            for (int dust = 0; dust <= 22; dust++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, (int)CalamityDusts.SulfurousSeaAcid, 0f, 0f);
            }
        }
    }
}
