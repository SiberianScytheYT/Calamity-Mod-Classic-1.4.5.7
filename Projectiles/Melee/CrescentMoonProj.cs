using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRD.Buffs.DamageOverTime;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
	public class CrescentMoonProj : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Magic/Crescent";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crescent Moon");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
			Projectile.extraUpdates = 1;
			Projectile.aiStyle = 18;
			AIType = ProjectileID.DeathSickle;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0f, 0.6f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 30 + Main.rand.Next(50);
                if (Main.rand.NextBool(10))
                {
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
            }

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 460f, 15f, 20f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), 180);
        }
    }
}
