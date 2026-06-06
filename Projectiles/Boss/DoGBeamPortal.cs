using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class DoGBeamPortal : ModProjectile
    {
        private int beamTimer = 180;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Beam Portal");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            CooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(beamTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            beamTimer = reader.ReadInt32();
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.95f, 1.15f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 5)
            {
                Projectile.frame = 0;
            }
            beamTimer--;
            if (beamTimer <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item33, Projectile.position);
				if (Projectile.owner == Main.myPlayer)
				{
					float speed = 3f;
					if (CalamityWorld.death)
					{
						speed = 5f;
					}
					else if (CalamityWorld.revenge)
					{
						speed = 4.5f;
					}
					else if (Main.expertMode)
					{
						speed = 4f;
					}
					speed *= Projectile.ai[0];
					int totalProjectiles = 8;
					float radians = MathHelper.TwoPi / totalProjectiles;
					for (int i = 0; i < totalProjectiles; i++)
					{
						Vector2 vector255 = new Vector2(0f, -speed).RotatedBy(radians * i);
						Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, vector255, ModContent.ProjectileType<DoGBeam>(), 0, 0f, Projectile.owner, Projectile.damage, 0f);
					}
				}
                beamTimer = 180;
            }
            int num103 = Player.FindClosest(Projectile.Center, 1, 1);
            float scaleFactor2 = Projectile.velocity.Length();
            Vector2 vector11 = Main.player[num103].Center - Projectile.Center;
            if (Vector2.Distance(Main.player[num103].Center, Projectile.Center) > 2000f)
            {
                Projectile.position.X = Main.player[num103].Center.X / 16 * 16f - (Projectile.width / 2);
                Projectile.position.Y = Main.player[num103].Center.Y / 16 * 16f - (Projectile.height / 2) - 250f;
                Projectile.ai[1] = 0f;
                beamTimer = 90;
            }
            vector11.Normalize();
            vector11 *= scaleFactor2;
            Projectile.velocity = (Projectile.velocity * 24f + vector11) / 25f;
            Projectile.velocity.Normalize();
            Projectile.velocity *= scaleFactor2;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
                for (int num621 = 0; num621 < 30; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1.2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 60; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1.7f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
			if (Projectile.timeLeft < 85)
				Projectile.damage = 0;
        }

        public override bool CanHitPlayer(Player target)
		{
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new Color(255, 255, 255, 100);
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)	
        {
			target.Calamity().lastProjectileHit = Projectile;
		}
    }
}
