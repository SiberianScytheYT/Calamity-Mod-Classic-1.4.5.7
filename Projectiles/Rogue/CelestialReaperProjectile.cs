using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
	public class CelestialReaperProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/CelestialReaper";

        public int HomingCooldown = 0;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Reaper");
        }

        public override void SetDefaults()
        {
            Projectile.width = 66;
            Projectile.height = 76;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            Projectile.tileCollide = false;
            Projectile.Calamity().rogue = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(30f) / (float)Math.Log(6f - Projectile.penetrate + 2f) / 1.4f; // Slow down the more hits the scythe has accumulated.
            if (HomingCooldown > 0)
            {
                HomingCooldown--;
            }
            else
            {
                NPC target = Projectile.position.ClosestNPCAt(640f);
                if (target != null)
                {
                    Projectile.velocity = (Projectile.velocity * 20f + Projectile.DirectionTo(target.Center) * 20f) / 21f;
                }
            }
            if (Projectile.ai[0] == 1f)
            {
                // Damaging afterimage projectiles.
                float framesNeeded = 60f;
                framesNeeded /= 6f - Projectile.penetrate + 1f; // The addition of 1 is to prevent division by zero. The more hits, the more afterimages.
                if (Projectile.timeLeft % (int)framesNeeded == 0f)
                {
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<CelestialReaperAfterimage>(), Projectile.damage / 2, Projectile.knockBack / 2f, Projectile.owner);
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
		{
			if (HomingCooldown > 0)
				return false;
			return null;
		}

        public override bool CanHitPvp(Player target) => HomingCooldown <= 0;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            HomingCooldown = 25;
            Projectile.velocity *= -0.75f; // Bounce off of the enemy.
        }
        public override void OnKill(int timeLeft)
        {
            for (float i = 0f; i < 7f; i += 1f)
            {
                float angle = MathHelper.TwoPi * i / 7f;
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, angle.ToRotationVector2() * 12f, ModContent.ProjectileType<CelestialReaperAfterimage>(), Projectile.damage, 2f, Projectile.owner);
            }
        }
    }
}
