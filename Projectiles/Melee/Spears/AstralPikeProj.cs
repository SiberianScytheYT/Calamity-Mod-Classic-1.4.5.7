using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using CalRD.Projectiles.BaseProjectiles;

namespace CalRD.Projectiles.Melee.Spears
{
    public class AstralPikeProj : BaseSpearProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pike");
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;  //The width of the .png file in pixels divided by 2.
            // Projectile.aiStyle = 19;
            Projectile.DamageType = DamageClass.Melee;  //Dictates whether projectile is a melee-class weapon.
            Projectile.timeLeft = 90;
            Projectile.height = 40;  //The height of the .png file in pixels divided by 2.
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            //projectile.Calamity().trueMelee = true;
        }

        public override float InitialSpeed => 3f;
        public override float ReelbackSpeed => 2.4f;
        public override float ForwardSpeed => 0.95f;
        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
            target.immune[Projectile.owner] = 6;
            if (hit.Crit)
            {
                for (int i = 0; i < 3; i++)
                {
                    float xPos = Projectile.position.X + 800f * Main.rand.NextBool(2).ToDirectionInt();
                    Vector2 spawnPosition = new Vector2(xPos, Projectile.position.Y - Main.rand.Next(-800, 801));
                    float speedX = target.position.X - spawnPosition.X;
                    float speedY = target.position.Y - spawnPosition.Y;
                    float magnitude = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                    magnitude = 10f / xPos;
                    speedX *= magnitude * 150f;
                    speedY *= magnitude * 150f;
                    speedX = MathHelper.Clamp(speedX, -15f, 15f);
                    speedY = MathHelper.Clamp(speedY, -15f, 15f);
                    if (Projectile.owner == Main.myPlayer)
                    {
                        int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), spawnPosition.X, spawnPosition.Y, speedX, speedY, ModContent.ProjectileType<AstralStar>(), (int)(Projectile.damage * 0.4), 1f, Projectile.owner, 3f, 0f);
                        Main.projectile[proj].Calamity().forceMelee = true;
                    }
                }
            }
        }
    }
}
