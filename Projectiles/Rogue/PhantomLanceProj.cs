using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class PhantomLanceProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/PhantomLance";

        private int projCount = 22;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Phantom Lance");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.Calamity().rogue = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void AI()
        {
			if (Projectile.Calamity().stealthStrike != true)
			{
				if (Projectile.timeLeft <= 255)
					Projectile.alpha += 1;
				if (Projectile.timeLeft >= 75)
				{
					Projectile.velocity.X *= 0.995f;
					Projectile.velocity.Y *= 0.995f;
				}
			}
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.785f;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 175, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default, 0.85f);
			projCount--;
			if (projCount <= 0)
			{
				if (Projectile.owner == Main.myPlayer)
				{
					if (Projectile.Calamity().stealthStrike)
					{
                        int stealthSoulDamage = (int)(Projectile.damage * 0.7f);
                        float stealthSoulKB = Projectile.knockBack;
                        int stealthSoul = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Phantom>(), stealthSoulDamage, stealthSoulKB, Projectile.owner, 0f, 0f);
						Main.projectile[stealthSoul].Calamity().forceRogue = true;
						Main.projectile[stealthSoul].usesLocalNPCImmunity = true;
						Main.projectile[stealthSoul].localNPCHitCooldown = -2;
					}
					else
					{
                        float damageMult = Projectile.timeLeft * 0.7f / 300f;
                        int soulDamage = (int)(Projectile.damage * damageMult);
                        float soulKB = Projectile.knockBack;
						int soul = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Phantom>(), soulDamage, soulKB, Projectile.owner, 0f, 0f);
						Main.projectile[soul].Calamity().forceRogue = true;
						Main.projectile[soul].usesLocalNPCImmunity = true;
						Main.projectile[soul].localNPCHitCooldown = -2;
					}
				}
				projCount = 18;
			}
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 175, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
