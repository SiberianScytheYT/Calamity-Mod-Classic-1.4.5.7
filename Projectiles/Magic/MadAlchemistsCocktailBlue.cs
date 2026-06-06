using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Magic
{
    public class MadAlchemistsCocktailBlue : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mad Alchemist's Cocktail Blue");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            Projectile.rotation += Math.Abs(Projectile.velocity.X) * 0.04f * (float)Projectile.direction;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 90f)
            {
                Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
                Projectile.velocity.X = Projectile.velocity.X * 0.97f;
            }
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107, Projectile.position);
            Gore.NewGore(Entity.GetSource_FromThis(), Projectile.Center, -Projectile.oldVelocity * 0.2f, 704, 1f);
            Gore.NewGore(Entity.GetSource_FromThis(), Projectile.Center, -Projectile.oldVelocity * 0.2f, 705, 1f);
            int num220 = Main.rand.Next(20, 31);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int num221 = 0; num221 < num220; num221++)
                {
                    Vector2 value17 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    value17.Normalize();
                    value17 *= (float)Main.rand.Next(10, 201) * 0.01f;
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, value17.X, value17.Y, ModContent.ProjectileType<MadAlchemistsCocktailGasCloud>(), Projectile.damage / 4, 0f, Projectile.owner, 0f, (float)Main.rand.Next(-45, 1));
                }
            }
        }
    }
}
