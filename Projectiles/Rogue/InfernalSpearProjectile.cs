using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class InfernalSpearProjectile : ModProjectile
    {
        private float speedX = -3f;
        private float speedX2 = -5f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spear");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.Calamity().rogue = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(speedX);
            writer.Write(speedX2);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            speedX = reader.ReadSingle();
            speedX2 = reader.ReadSingle();
        }

        public override void AI()
        {
			if (Projectile.timeLeft % 12 == 0)
			{
				if (Projectile.owner == Main.myPlayer)
				{
					if (Projectile.Calamity().stealthStrike)
					{
						Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<InfernalFireball>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner);
					}
				}
			}

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 24f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
			if (Projectile.Calamity().stealthStrike)
			{
				if (Projectile.owner == Main.myPlayer)
				{
					for (int x = 0; x < 3; x++)
					{
						Projectile.NewProjectile(Entity.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, speedX, -50f, ModContent.ProjectileType<InfernalFireballEruption>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
						speedX += 3f;
					}
					for (int x = 0; x < 2; x++)
					{
						Projectile.NewProjectile(Entity.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, speedX2, -75f, ModContent.ProjectileType<InfernalFireballEruption>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
						speedX2 += 10f;
					}
				}
			}
			CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 160);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int d = 0; d < 20; d++)
            {
                int fire = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 1.2f);
                Main.dust[fire].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[fire].scale = 0.5f;
                    Main.dust[fire].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int d = 0; d < 30; d++)
            {
                int fire = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 1.7f);
                Main.dust[fire].noGravity = true;
                Main.dust[fire].velocity *= 5f;
                fire = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 1f);
                Main.dust[fire].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 360);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
