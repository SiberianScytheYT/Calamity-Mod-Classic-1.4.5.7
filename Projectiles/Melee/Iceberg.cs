using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Melee
{
    public class Iceberg : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Boss/IceBomb";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Iceberg");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
			Projectile.coldDamage = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.5f;
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67, 0f, 0f, 100, default, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            float newDamageMult = 1.0f - (Projectile.timeLeft / 300.0f);
            modifiers.SourceDamage *= newDamageMult;
            Projectile.knockBack = 0f;
            modifiers.CritDamage += 1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int debuffDuration = 300 - Projectile.timeLeft;
            if (Projectile.timeLeft < 270)
                target.AddBuff(ModContent.BuffType<GlacialState>(), debuffDuration);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }
    }
}
