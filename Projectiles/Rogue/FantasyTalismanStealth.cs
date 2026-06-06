using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class FantasyTalismanStealth : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Rogue/FantasyTalismanProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Talisman");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.Calamity().rogue = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.BulletHighVelocity;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 175, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
			if (Projectile.ai[0] != 1f)
			{
				if (Projectile.timeLeft % 10 == 0)
				{
					if (Projectile.owner == Main.myPlayer)
					{
						Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LostSoulFriendly>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
					}
				}
			}
            Projectile.StickyProjAI(4);
            if (Projectile.ai[0] == 1f)
            {
				if (Projectile.timeLeft % 4 == 0)
				{
					if (Main.rand.NextBool(2))
					{
						int spiritDamage = Projectile.damage / 2;
						Projectile ghost = CalamityGlobalProjectile.SpawnOrb(Projectile, spiritDamage, ProjectileID.SpectreWrath, 800f, 4f);
						ghost.Calamity().forceRogue = true;
						ghost.penetrate = 1;
					}
				}
            }
		}

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Projectile.ModifyHitNPCSticky(1, true);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return null;
        }
    }
}
