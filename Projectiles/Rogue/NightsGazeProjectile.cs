using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class NightsGazeProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/NightsGaze";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Night's Gaze");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(45);
            }

            if (Projectile.Calamity().stealthStrike)
            {
                if (Main.rand.NextBool(8))
                {
                    int projectileDamage = (int)(Projectile.damage * 0.75f);
                    int projectileType = ModContent.ProjectileType<NightsGazeStar>();
                    Vector2 velocity = Projectile.velocity;

                    int p = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, projectileType, projectileDamage, 5f, Projectile.owner, 1f, 0f);
                    Main.projectile[p].penetrate = 1;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), Projectile.timeLeft);
			OnHitEffects();
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), Projectile.timeLeft);
			OnHitEffects();
		}
		*/

		private void OnHitEffects()
		{
            int starCount = 6;
            int starFrequency = 3;
            float spread = 20f;
            for (int i = 0; i < starCount; i++)
            {
                int projectileDamage = (int)(Projectile.damage * 0.75f);
                int projectileType = ModContent.ProjectileType<NightsGazeSpark>();
                if (Main.rand.Next(starCount) < starFrequency)
                {
                    projectileType = ModContent.ProjectileType<NightsGazeStar>();
                }
                Vector2 velocity = Projectile.oldVelocity.RotateRandom(MathHelper.ToRadians(spread));
                float speed = Main.rand.NextFloat(1.5f, 2f);
                float moveDuration = Main.rand.Next(5, 15);

                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity * speed, projectileType, projectileDamage, 5f, Projectile.owner, 0f, moveDuration);
            }

            SoundEngine.PlaySound(SoundID.Item62.WithVolumeScale(0.6f), Projectile.position);
            SoundEngine.PlaySound(SoundID.Item68.WithVolumeScale(0.2f), Projectile.position);
            SoundEngine.PlaySound(SoundID.Item122.WithVolumeScale(0.4f), Projectile.position);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Projectile.Kill();
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Rogue/NightsGazeGlow").Value;
			Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
				int dustType = Utils.SelectRandom(Main.rand, new int[]
				{
					109,
					111,
					132
				});

                int dust = Dust.NewDust(Projectile.Center, 1, 1, dustType, Projectile.velocity.X / 3f, Projectile.velocity.Y / 3f, 0, default, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
