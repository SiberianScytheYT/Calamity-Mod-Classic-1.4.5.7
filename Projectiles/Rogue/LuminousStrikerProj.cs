using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.CalPlayer;

namespace CalRD.Projectiles.Rogue
{
	public class LuminousStrikerProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/LuminousStriker";

    	public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Luminous Striker");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.Calamity().rogue = true;
		}

        public override void AI()
        {
            CalamityPlayer modPlayer = Main.player[Projectile.owner].Calamity();
			if (Projectile.ai[0] == 0f && modPlayer.StealthStrikeAvailable())
			{
                Projectile.Calamity().stealthStrike = true;
				Projectile.timeLeft = 600;
				Projectile.ai[0] = 1f;
			}

        	if (Main.rand.NextBool(4))
            	Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 176, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.785f;

        	if (Projectile.timeLeft % 4 == 0)
			{
        		if (Projectile.owner == Main.myPlayer)
        		{
					if (Projectile.Calamity().stealthStrike)
					{
						Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X + Main.rand.NextFloat(-15f, 15f), Projectile.Center.Y + Main.rand.NextFloat(-15f, 15f), Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<LuminousShard>(), (int)(Projectile.damage * 0.25), Projectile.knockBack * 0.25f, Projectile.owner, 0f, 0f);
					}
					else
						Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0f, -2f, ModContent.ProjectileType<LuminousShard>(), (int)(Projectile.damage * 0.25), Projectile.knockBack * 0.25f, Projectile.owner, 0f, 0f);
                }
			}
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Projectile.owner == Main.myPlayer)
			{
				for (int i = 0; i < 7; i++)
				{
					Vector2 speed = new Vector2((float)Main.rand.Next(-50, 51), (float)Main.rand.Next(-50, 51));
					while (speed.X == 0f && speed.Y == 0f)
					{
						speed = new Vector2((float)Main.rand.Next(-50, 51), (float)Main.rand.Next(-50, 51));
					}
					speed.Normalize();
					speed *= ((float)Main.rand.Next(30, 61) * 0.1f) * 2f;
					Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<LuminousShard>(), (int)(Projectile.damage * 0.25), Projectile.knockBack * 0.25f, Projectile.owner, 0f, 0f);
				}
			}
		}

		//public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
			if (Projectile.owner == Main.myPlayer)
			{
				for (int i = 0; i < 7; i++)
				{
					Vector2 speed = new Vector2((float)Main.rand.Next(-50, 51), (float)Main.rand.Next(-50, 51));
					while (speed.X == 0f && speed.Y == 0f)
					{
						speed = new Vector2((float)Main.rand.Next(-50, 51), (float)Main.rand.Next(-50, 51));
					}
					speed.Normalize();
					speed *= ((float)Main.rand.Next(30, 61) * 0.1f) * 2f;
					Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<LuminousShard>(), (int)(Projectile.damage * 0.25), Projectile.knockBack * 0.25f, Projectile.owner, 0f, 0f);
				}
			}
		}
		*/

        public override void OnKill(int timeLeft)
        {
        	for (int i = 0; i <= 10; i++)
        	{
        		Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 176, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
        	}
        }
    }
}
