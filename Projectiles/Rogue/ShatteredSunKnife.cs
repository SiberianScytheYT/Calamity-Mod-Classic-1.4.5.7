using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class ShatteredSunKnife : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/ShatteredSun";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shattered Sun");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] < 5f)
            {
                Projectile.alpha -= 50;
            }
            if (Projectile.ai[1] == 5f)
            {
                Projectile.alpha = 0;
                Projectile.tileCollide = false;
            }

            if (Projectile.ai[1] == 20f)
            {
                int numProj = 5;
                float rotation = MathHelper.ToRadians(10);
                if (Projectile.owner == Main.myPlayer)
                {
                    int spread = 6;
                    for (int i = 0; i < numProj; i++)
                    {
                        Vector2 perturbedspeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y + Main.rand.Next(-3, 4)).RotatedBy(MathHelper.ToRadians(spread));
                        int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedspeed.X * 0.2f, perturbedspeed.Y * 0.2f, ModContent.ProjectileType<ShatteredSunScorchedBlade>(), (int)((double)Projectile.damage * 0.75), 1f, Projectile.owner, 0f, 0f);
                        Main.projectile[proj].Calamity().stealthStrike = Projectile.Calamity().stealthStrike;
                        spread -= Main.rand.Next(2, 6);
                    }
                    SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
                    Projectile.active = false;
                    for (int num621 = 0; num621 < 8; num621++)
                    {
                        int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 127, 0f, 0f, 100, default, 2f);
                        Main.dust[num622].velocity *= 3f;
                        if (Main.rand.NextBool(2))
                        {
                            //Main.dust[num622].scale = 0.5f;
                            Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                        }
                    }
                    for (int num623 = 0; num623 < 16; num623++)
                    {
                        int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 127, 0f, 0f, 100, default, 3f);
                        Main.dust[num624].noGravity = true;
                        Main.dust[num624].velocity *= 5f;
                        num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 127, 0f, 0f, 100, default, 2f);
                        Main.dust[num624].velocity *= 2f;
                    }
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
            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ShatteredExplosion>(), (int) ((double) Projectile.damage * 0.15), Projectile.knockBack, Projectile.owner, 0f, 0f);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*{
            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ShatteredExplosion>(), (int)((double)damage * 0.15), Projectile.knockBack, Projectile.owner, 0f, 0f);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }*/

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ShatteredExplosion>(), (int)((double)Projectile.damage * 0.15), Projectile.knockBack, Projectile.owner, 0f, 0f);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 246, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f, 0, default, 1f);
            }
        }
    }
}
