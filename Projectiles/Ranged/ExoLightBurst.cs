using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class ExoLightBurst : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public const float MinDistanceFromTarget = 45f;
        public const float MaxDistanceFromTarget = 1350f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Exo Flare");
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 190;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            // localAI[0] is used by the sticky AI method.
            if (Projectile.localAI[1] == 0f)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), Projectile.Center,
                                                       Vector2.Zero,
                                                       ModContent.ProjectileType<ExoLight>(),
                                                       Projectile.damage,
                                                       Projectile.knockBack,
                                                       Projectile.owner).localAI[1] = Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI);
                    }
                }
                Projectile.localAI[1] = 1f;
            }
            if (Projectile.ai[0] == 0f)
            {
                NPC potentialTarget = Projectile.Center.ClosestNPCAt(MaxDistanceFromTarget, true, true);
                if (potentialTarget != null)
                {
                    if (Projectile.Distance(potentialTarget.Center) > MinDistanceFromTarget)
                    {
                        float angleOffset = Projectile.AngleTo(potentialTarget.Center) - Projectile.velocity.ToRotation();
                        angleOffset = MathHelper.WrapAngle(angleOffset);
                        Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Clamp(angleOffset, -0.1f, 0.1f));
                    }
                }
            }
            Projectile.StickyProjAI(5);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Projectile.ModifyHitNPCSticky(4, false);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            int width = (int)MathHelper.Min(targetHitbox.Width, 150);
            int height = (int)MathHelper.Min(targetHitbox.Height, 150);
            CalamityGlobalProjectile.ExpandHitboxBy(Projectile, width, height);
            return null;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.ExoDebuffs(2f);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
			target.ExoDebuffs(2f);
        }
        */
    }
}
