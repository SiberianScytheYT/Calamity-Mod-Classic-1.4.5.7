using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.DamageOverTime;

namespace CalRD.Projectiles.Rogue
{
    public class TheSyringeProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/TheSyringe";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Syringe");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;
            Projectile.friendly = true;
            Projectile.Calamity().rogue = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.BulletHighVelocity;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, Main.rand.Next(2) == 1 ? 107 : 89, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Projectile.damage += Projectile.Calamity().defDamage / 200;
		}

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 600);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*{
            target.AddBuff(ModContent.BuffType<Plague>(), 600);
        }*/

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 100;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item107, Projectile.position);
            for (int k = 0; k < 7; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 89, Projectile.oldVelocity.X, Projectile.oldVelocity.Y);
            }
            int fireAmt = Main.rand.Next(1, 3);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int f = 0; f < fireAmt; f++)
                {
					Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<TheSyringeCinder>(), (int)(Projectile.damage * 0.5), 0f, Main.myPlayer, 0f, 0f);
                }
                for (int s = 0; s < 2; ++s)
                {
                    float SpeedX = -Projectile.velocity.X * Main.rand.Next(40, 70) * 0.01f + Main.rand.Next(-20, 21) * 0.4f;
                    float SpeedY = -Projectile.velocity.Y * Main.rand.Next(40, 70) * 0.01f + Main.rand.Next(-20, 21) * 0.4f;
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X + SpeedX, Projectile.Center.Y + SpeedY, SpeedX, SpeedY, ModContent.ProjectileType<TheSyringeS1>(), (int)(Projectile.damage * 0.25), 0f, Main.myPlayer, Main.rand.Next(3), 0f);
                }
            }
            if (Projectile.owner == Main.myPlayer && Projectile.ai[1] == 1)
            {
                for (int b = 0; b < 5; b++)
                {
                    float speedX = (float)Main.rand.Next(-35, 36) * 0.02f;
                    float speedY = (float)Main.rand.Next(-35, 36) * 0.02f;
                    int bee = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speedX, speedY, ModContent.ProjectileType<PlaguenadeBee>(), (int)(Projectile.damage * 0.5), 0f, Main.myPlayer, 0f, 0f);
					Main.projectile[bee].penetrate = 1;
                }
            }
		}
    }
}
