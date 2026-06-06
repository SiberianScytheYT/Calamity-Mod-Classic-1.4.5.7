using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class AuralisBullet : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bullet");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 5;
            Projectile.alpha = 255;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 6f)
            {
                for (int d = 0; d < 5; d++)
                {
                    Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 229, Projectile.velocity.X, Projectile.velocity.Y, 100, CalamityUtils.ColorSwap(Auralis.blueColor, Auralis.greenColor, 1f), 1f)];
                    dust.velocity = Vector2.Zero;
                    dust.position -= Projectile.velocity / 5f * d;
                    dust.noGravity = true;
                    dust.scale = 0.65f;
                    dust.noLight = true;
					dust.color = CalamityUtils.ColorSwap(Auralis.blueColor, Auralis.greenColor, 1f);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			int duration = 600;
			target.AddBuff(ModContent.BuffType<MarkedforDeath>(), duration);
			target.AddBuff(BuffID.Ichor, duration);
			target.AddBuff(BuffID.CursedInferno, duration);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
			int duration = 600;
			target.AddBuff(ModContent.BuffType<MarkedforDeath>(), duration);
			target.AddBuff(BuffID.Ichor, duration);
			target.AddBuff(BuffID.CursedInferno, duration);
        }
        */
    }
}
