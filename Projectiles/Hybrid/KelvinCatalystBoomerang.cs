using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Hybrid
{
    public class KelvinCatalystBoomerang : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/KelvinCatalyst";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Kelvin Catalyst");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.coldDamage = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.3f / 255f, Main.DiscoR * 0.4f / 255f, Main.DiscoR * 0.5f / 255f);

            if (Projectile.ai[1] == 1f)
            {
                Projectile.Calamity().rogue = true;
            }

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            if (Projectile.ai[0] == 0f)
            {
                Projectile.localAI[0] += 1f;
                if (Projectile.localAI[0] >= 75f)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.localAI[0] = 0f;
                    Projectile.width = Projectile.height = 60;
                    Projectile.tileCollide = false;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                float num42 = 20f;
                float num43 = 2f;
                Vector2 vector2 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num44 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector2.X;
                float num45 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector2.Y;
                float num46 = (float)Math.Sqrt((double)(num44 * num44 + num45 * num45));

                if (num46 > 3000f)
                    Projectile.Kill();

                num46 = num42 / num46;
                num44 *= num46;
                num45 *= num46;

                if (Projectile.velocity.X < num44)
                {
                    Projectile.velocity.X = Projectile.velocity.X + num43;
                    if (Projectile.velocity.X < 0f && num44 > 0f)
                        Projectile.velocity.X = Projectile.velocity.X + num43;
                }
                else if (Projectile.velocity.X > num44)
                {
                    Projectile.velocity.X = Projectile.velocity.X - num43;
                    if (Projectile.velocity.X > 0f && num44 < 0f)
                        Projectile.velocity.X = Projectile.velocity.X - num43;
                }
                if (Projectile.velocity.Y < num45)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y + num43;
                    if (Projectile.velocity.Y < 0f && num45 > 0f)
                        Projectile.velocity.Y = Projectile.velocity.Y + num43;
                }
                else if (Projectile.velocity.Y > num45)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y - num43;
                    if (Projectile.velocity.Y > 0f && num45 < 0f)
                        Projectile.velocity.Y = Projectile.velocity.Y - num43;
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new Rectangle((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
                    if (rectangle.Intersects(value2))
                        Projectile.Kill();
                }
            }

            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 67, 0f, 0f, 100, default, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;

            Projectile.rotation += 0.25f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = 1f;
            Projectile.localAI[0] = 0f;
            Projectile.width = Projectile.height = 60;
            Projectile.tileCollide = false;
            Projectile.netUpdate = true;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 240);
            if (Projectile.owner == Main.myPlayer && Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<KelvinCatalystStar>()] < 25)
            {
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                int i;
                for (i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 4f), (float)(Math.Cos(offsetAngle) * 4f),
                        ModContent.ProjectileType<KelvinCatalystStar>(), Projectile.damage / 6, Projectile.knockBack * 0.5f, Projectile.owner, 0f, 0f);

                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 4f), (float)(-Math.Cos(offsetAngle) * 4f),
                        ModContent.ProjectileType<KelvinCatalystStar>(), Projectile.damage / 6, Projectile.knockBack * 0.5f, Projectile.owner, 0f, 0f);
                }
            }
            SoundEngine.PlaySound(SoundID.Item30, Projectile.position);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            target.AddBuff(BuffID.Frostburn, 240);
            if (Projectile.owner == Main.myPlayer && Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<KelvinCatalystStar>()] < 25)
            {
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                int i;
                for (i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 4f), (float)(Math.Cos(offsetAngle) * 4f),
                        ModContent.ProjectileType<KelvinCatalystStar>(), Projectile.damage / 6, Projectile.knockBack * 0.5f, Projectile.owner, 0f, 0f);

                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 4f), (float)(-Math.Cos(offsetAngle) * 4f),
                        ModContent.ProjectileType<KelvinCatalystStar>(), Projectile.damage / 6, Projectile.knockBack * 0.5f, Projectile.owner, 0f, 0f);
                }
            }
            SoundEngine.PlaySound(SoundID.Item30, Projectile.position);
        }
        */

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }
    }
}
