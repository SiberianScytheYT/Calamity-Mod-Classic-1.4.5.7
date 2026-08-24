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
    public class ShatteredSunScorchedBlade : ModProjectile
    {
        int counter = 0;
        float multiplier = 1f;
        bool stealthOrigin = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scorched Blade");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 1;
            Projectile.Calamity().rogue = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 500;
        }

        public override void AI()
        {
            counter++;
            if (counter == 1)
            {
                stealthOrigin = Projectile.ai[0] == 1f;
                Projectile.alpha += (int) Projectile.ai[1];
                Projectile.ai[0] = 0f;
            }
            if (counter == 20 && !Projectile.Calamity().stealthStrike && !stealthOrigin)
            {
                Projectile.tileCollide = true;
            }
            if (counter % 5 == 0)
            {
                Projectile.velocity *= 1.15f;
            }
            if (counter % 10 == 0)
            {
                multiplier -= 0.005f;
                if (multiplier >= 0.5f && !stealthOrigin && Projectile.alpha < 200)
                    Projectile.alpha += Main.rand.Next(5, 7);
            }
            if (counter % 9 == 0 || (counter % 5 == 0 && Projectile.Calamity().stealthStrike))
            {
                int timesToSpawnDust = Projectile.Calamity().stealthStrike  ? 2 : 1;
                for (int i = 0; i < timesToSpawnDust; i++)
                {
                    int num624 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 127, 0f, 0f, 100, default, Projectile.Calamity().stealthStrike ? 1.8f : 1.3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 127, 0f, 0f, 100, default, Projectile.Calamity().stealthStrike ? 1.8f : 1.3f);
                    Main.dust[num624].velocity *= 2f;
                }
            }

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 2.355f;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= 1.57f;
            }

            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0f);
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 20f, 20f);
            float num633 = 700f;
            Vector2 vector46 = Projectile.position;
            bool flag25 = false;
            for (int num645 = 0; num645 < 200; num645++)
            {
                NPC nPC2 = Main.npc[num645];
                if (nPC2.CanBeChasedBy(Projectile, false))
                {
                    float num646 = Vector2.Distance(nPC2.Center, Projectile.Center);
                    if (!flag25)
                    {
                        num633 = num646;
                        vector46 = nPC2.Center;
                        flag25 = true;
                    }
                }
            }
            if (flag25 && Projectile.ai[0] == 0f)
            {
                Vector2 vector47 = vector46 - Projectile.Center;
                float num648 = vector47.Length();
                vector47.Normalize();
                if (num648 > 200f)
                {
                    float scaleFactor2 = 8f;
                    vector47 *= scaleFactor2;
                    Projectile.velocity = (Projectile.velocity * 40f + vector47) / 41f;
                }
                else
                {
                    float num649 = 4f;
                    vector47 *= -num649;
                    Projectile.velocity = (Projectile.velocity * 40f + vector47) / 41f;
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
            if (multiplier < 0.5f)
                multiplier = 0.5f;
            Projectile.damage = stealthOrigin ? Projectile.damage : (int)((float)Projectile.damage * multiplier);
            if (Projectile.Calamity().stealthStrike)
            {
                int numProj = 2;
                float rotation = MathHelper.ToRadians(10);
                if (Projectile.owner == Main.myPlayer)
                {
                    Player owner = Main.player[Projectile.owner];
                    Vector2 correctedVelocity = target.Center - owner.Center;
                    correctedVelocity.Normalize();
                    correctedVelocity *= 10f;
                    int spread = 6;
                    for (int i = 0; i < numProj; i++)
                    {
                        Vector2 perturbedspeed = new Vector2(correctedVelocity.X, correctedVelocity.Y + Main.rand.Next(-3, 4)).RotatedBy(MathHelper.ToRadians(spread));
                        
                        int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), owner.Center.X, owner.Center.Y - 10, perturbedspeed.X, perturbedspeed.Y, ModContent.ProjectileType<ShatteredSunScorchedBlade>(), (int)((double)Projectile.damage * 0.6), 1f, Projectile.owner, 1f, Projectile.alpha);
                        spread -= Main.rand.Next(2, 6);
                        Main.projectile[proj].ai[0] = 1f;
                    }
                    Projectile.Kill();
                }
            }
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (multiplier < 0.5f)
                multiplier = 0.5f;
            info.Damage = stealthOrigin ? info.Damage : (int)((double)info.Damage * multiplier);
            if (Projectile.Calamity().stealthStrike)
            {
                int numProj = 2;
                float rotation = MathHelper.ToRadians(10);
                if (Projectile.owner == Main.myPlayer)
                {
                    Player owner = Main.player[Projectile.owner];
                    Vector2 correctedVelocity = target.Center - owner.Center;
                    correctedVelocity.Normalize();
                    correctedVelocity *= 10f;
                    int spread = 6;
                    for (int i = 0; i < numProj; i++)
                    {
                        Vector2 perturbedspeed = new Vector2(correctedVelocity.X, correctedVelocity.Y + Main.rand.Next(-3, 4)).RotatedBy(MathHelper.ToRadians(spread));
                        
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center.X, owner.Center.Y - 10, perturbedspeed.X, perturbedspeed.Y, ModContent.ProjectileType<ShatteredSunScorchedBlade>(), (int)((double)Projectile.damage * 0.55), 1f, Projectile.owner, 0f, 0f);
                        spread -= Main.rand.Next(2, 6);
                        Main.projectile[proj].ai[0] = 1f;
                    }
                    Projectile.Kill();
                }
            }
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = Projectile.height = 200;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int num621 = 0; num621 < 4; num621++)
            {
                int num622 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 12; num623++)
            {
                int num624 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;

            }
			CalamityUtils.ExplosionGores(Projectile.GetSource_FromThis(), Projectile.Center, 3);
        }
    }
}
