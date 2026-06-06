using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class PhantasmalRuinProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/PhantasmalRuin";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Phantasmal Ruin");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            Projectile.Calamity().rogue = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.785f;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 175, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default(Color), 0.85f);
			if (Projectile.timeLeft % 18 == 0)
			{
				if (Projectile.owner == Main.myPlayer)
				{
					if (Projectile.Calamity().stealthStrike)
					{
						Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, ModContent.ProjectileType<PhantasmalRuinGhost>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
					}
					else
					{
						Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0f, Main.rand.NextFloat(-2,2), ModContent.ProjectileType<LostSoulFriendly>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
					}
				}
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			float spread = 45f * 0.0174f;
			double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
			double deltaAngle = spread / 8f;
			double offsetAngle;
			int i;
			if (Projectile.owner == Main.myPlayer)
			{
				if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<PhantasmalSoul>()] < 8)
				{
					for (i = 0; i < 8; i++)
					{
						float ai1 = Main.rand.NextFloat() + 0.5f;
						float randomSpeed = (float)Main.rand.Next(1, 7);
						float randomSpeed2 = (float)Main.rand.Next(1, 7);
						offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
						int num23 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f) + randomSpeed, ModContent.ProjectileType<PhantasmalSoul>(), (int)((double)Projectile.damage * 0.05), 0f, Projectile.owner, 1f, ai1);
						int num24 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f) + randomSpeed2, ModContent.ProjectileType<PhantasmalSoul>(), (int)((double)Projectile.damage * 0.05), 0f, Projectile.owner, 1f, ai1);
					}
				}
				else
				{
					Projectile.damage = (int)(Projectile.damage * 0.9);
				}
			}
		}

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*{
			float spread = 45f * 0.0174f;
			double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
			double deltaAngle = spread / 8f;
			double offsetAngle;
			int i;
			if (Projectile.owner == Main.myPlayer && Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<PhantasmalSoul>()] < 8)
			{
				for (i = 0; i < 8; i++)
				{
					float ai1 = Main.rand.NextFloat() + 0.5f;
					float randomSpeed = (float)Main.rand.Next(1, 7);
					float randomSpeed2 = (float)Main.rand.Next(1, 7);
					offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
					int num23 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f) + randomSpeed, ModContent.ProjectileType<PhantasmalSoul>(), (int)((double)Projectile.damage * 0.05), 0f, Projectile.owner, 1f, ai1);
					int num24 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f) + randomSpeed2, ModContent.ProjectileType<PhantasmalSoul>(), (int)((double)Projectile.damage * 0.05), 0f, Projectile.owner, 1f, ai1);
				}
			}
			else
			{
				damage = (int)(damage * 0.9);
			}
		}*/
    }
}
