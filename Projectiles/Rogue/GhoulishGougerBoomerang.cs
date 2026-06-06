using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class GhoulishGougerBoomerang : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/GhoulishGouger";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scythe");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 74;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.7f, 0f, 0.15f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= 45f)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.tileCollide = false;
                float num42 = 28f;
                float num43 = 5f;
                Vector2 vector2 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num44 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector2.X;
                float num45 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector2.Y;
                float num46 = (float)Math.Sqrt((double)(num44 * num44 + num45 * num45));
                if (num46 > 3000f)
                {
                    Projectile.Kill();
                }
                num46 = num42 / num46;
                num44 *= num46;
                num45 *= num46;
                if (Projectile.velocity.X < num44)
                {
                    Projectile.velocity.X = Projectile.velocity.X + num43;
                    if (Projectile.velocity.X < 0f && num44 > 0f)
                    {
                        Projectile.velocity.X = Projectile.velocity.X + num43;
                    }
                }
                else if (Projectile.velocity.X > num44)
                {
                    Projectile.velocity.X = Projectile.velocity.X - num43;
                    if (Projectile.velocity.X > 0f && num44 < 0f)
                    {
                        Projectile.velocity.X = Projectile.velocity.X - num43;
                    }
                }
                if (Projectile.velocity.Y < num45)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y + num43;
                    if (Projectile.velocity.Y < 0f && num45 > 0f)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y + num43;
                    }
                }
                else if (Projectile.velocity.Y > num45)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y - num43;
                    if (Projectile.velocity.Y > 0f && num45 < 0f)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y - num43;
                    }
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new Rectangle((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
                    if (rectangle.Intersects(value2))
                    {
                        Projectile.Kill();
                    }
                }
            }
            Projectile.rotation += 0.5f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250, 50);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

		public override void PostDraw(Color lightColor)
		{
			Vector2 origin = new Vector2(37f, 34f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/Items/Weapons/Rogue/GhoulishGougerGlow").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			float spread = 45f * 0.0174f;
			double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
			double deltaAngle = spread / 8f;
			double offsetAngle;
			int i;
			if (Projectile.owner == Main.myPlayer && Projectile.Calamity().stealthStrike && Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<PhantasmalSoul>()] < 8)
			{
				for (i = 0; i < 8; i++)
				{
					float ai1 = Main.rand.NextFloat() + 0.5f;
					float randomSpeed = (float)Main.rand.Next(1, 7);
					float randomSpeed2 = (float)Main.rand.Next(1, 7);
					offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
					int num23 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f) + randomSpeed, ModContent.ProjectileType<PhantasmalSoul>(), (int)((double)Projectile.damage * 0.2), 0f, Projectile.owner, 1f, ai1);
					int num24 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f) + randomSpeed2, ModContent.ProjectileType<PhantasmalSoul>(), (int)((double)Projectile.damage * 0.2), 0f, Projectile.owner, 1f, ai1);
				}
			}
		}
	}
}
