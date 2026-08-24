using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CalRD.Projectiles.Rogue
{
	public class HypothermiaChunk : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Ice Chunk");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 80;
            Projectile.extraUpdates = 3;
            Projectile.ignoreWater = true;
            Projectile.Calamity().rogue = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);

            Lighting.AddLight(Projectile.Center, 0.1f, 0f, 0.5f);
            if (Projectile.ai[0] < 0.2f)
            {
                Projectile.ai[0] += 0.1f;
            }
            else
            {
                Projectile.tileCollide = true;
            }

            if (Main.rand.NextBool(10))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 191, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f, 0, default, 0.8f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 3; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 191, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f, 0, default, 0.8f);
            }
            if (Projectile.owner == Main.myPlayer)
            {
				for (int i = 0; i < Main.rand.Next(1,4); i++)
				{
					Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
					int shard = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<HypothermiaShard>(), (int)(Projectile.damage * 0.33f), Projectile.knockBack * 0.75f, Main.myPlayer, Main.rand.Next(4), 1f);
				}
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300);
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frostburn, 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
