using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class BlindingLight : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        private const float Radius = 1400f;
        private const int Lifetime = 45;
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blinding Light");
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = Lifetime;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float dist1 = Projectile.Distance(targetHitbox.TopLeft());
            float dist2 = Projectile.Distance(targetHitbox.TopRight());
            float dist3 = Projectile.Distance(targetHitbox.BottomLeft());
            float dist4 = Projectile.Distance(targetHitbox.BottomRight());

            float minDist = dist1;
            if (dist2 < minDist)
                minDist = dist2;
            if (dist3 < minDist)
                minDist = dist3;
            if (dist4 < minDist)
                minDist = dist4;

            return minDist <= Radius;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => modifiers.SetCrit();

        public override void AI()
        {
            if (Projectile.timeLeft == Lifetime)
            {
                ConsumeNearbyBlades();
                DivideDamageAmongstTargets();
            }

            Projectile.ai[0]++;
            float progress = (float)Math.Sin(Projectile.ai[0] / Lifetime * MathHelper.Pi);
            if (Projectile.ai[0] > 55f)
                progress = MathHelper.Lerp(progress, 0f, (Projectile.ai[0] - 55f) / 5f);
            if (Projectile.ai[0] > 15f) // Otherwise a white flash appears, but it quickly disappears.
            {
                if (Main.netMode != NetmodeID.Server && !Filters.Scene["CalRD:LightBurst"].IsActive())
                {
                    Filters.Scene.Activate("CalRD:LightBurst", Projectile.Center).GetShader().UseTargetPosition(Projectile.Center).UseProgress(0f);
                }
                Filters.Scene["CalRD:LightBurst"].GetShader().UseProgress(progress);
            }
        }

        public override void OnKill(int timeLeft) => Filters.Scene.Deactivate("CalRD:LightBurst");

        private void ConsumeNearbyBlades()
        {
            int lightBlade = ModContent.ProjectileType<LightBlade>();
            int extraDamage = 0;
            for (int i = 0; i < Main.maxProjectiles; ++i)
            {
                Projectile otherProj = Main.projectile[i];
                if (otherProj is null || !otherProj.active || otherProj.owner != Projectile.owner || otherProj.type != lightBlade)
                    continue;

                // Can only consume blades within the flash radius (which should be most if not all of them anyway)
                if (Projectile.Distance(otherProj.Center) > Radius)
                    continue;
                extraDamage += otherProj.damage / 2;
                otherProj.Kill();
            }
            Projectile.damage += extraDamage;
        }

        private void DivideDamageAmongstTargets()
        {
            int numTargets = 0;
            for(int i = 0; i < Main.maxNPCs; ++i)
            {
                NPC npc = Main.npc[i];
                if (npc is null || !npc.active || npc.friendly || npc.dontTakeDamage || npc.immortal)
                    continue;
                if (Projectile.Colliding(default, npc.Hitbox))
                    ++numTargets;
            }

            // The number of targets is minimum one to prevent dividing by zero.
            if (numTargets <= 0)
                numTargets = 1;

            // Divide damage by the square root of nearby targets. 25 targets = 1/5th damage, for example.
            Projectile.damage = (int)(Projectile.damage / Math.Sqrt(numTargets));
        }
    }
}
