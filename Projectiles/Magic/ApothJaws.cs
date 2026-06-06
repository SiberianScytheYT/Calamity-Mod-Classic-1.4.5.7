using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using System;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
    public class ApothJaws : ModProjectile
    {
        private const float degrees = (float)(Math.PI / 180) * 2;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Jaws of Annihilation");
        }

        public override void SetDefaults()
        {
            Projectile.width = 144;
            Projectile.height = 72;
            Projectile.alpha = 70;
            Projectile.timeLeft = 240;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.light = 1.5f;
        }

        public override void AI()
        {
            if (Projectile.timeLeft % 8 == 0)
            {
                double angle = (double)Main.rand.Next(360) * Math.PI / 180;
                float offsetX = Projectile.position.X + (float)Main.rand.Next((int)Projectile.width);
                float offsetY = Projectile.position.Y + (float)Main.rand.Next((int)Projectile.height);
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), offsetX, offsetY, 14 * (float)Math.Cos(angle), 14 * (float)Math.Sin(angle), ModContent.ProjectileType<ApothChloro>(), Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
                }
            }
            if (Projectile.timeLeft < 30)
                Projectile.alpha = Projectile.alpha + 6;
            else if (Projectile.timeLeft < 210)
            {
                Projectile.velocity.X *= 0.9f;
                Projectile.velocity.Y *= 0.9f;
            }
            else if (Projectile.timeLeft < 240)
            {
                if (Projectile.ai[1] == 0)
                    Projectile.rotation += 1.3f * degrees;
                else
                    Projectile.rotation -= 1.3f * degrees;
            }
            else if (Projectile.timeLeft == 240)
            {
                if (Projectile.ai[1] == 0)
                    Projectile.rotation = Projectile.ai[0] - 30 * degrees;
                else
                {
                    Projectile.rotation = Projectile.ai[0] + 30 * degrees + (float)Math.PI;
                    Projectile.spriteDirection = -1;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 600, true);
            target.AddBuff(ModContent.BuffType<DemonFlames>(), 600, true);
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 600, true);
            if (Main.rand.NextBool(30))
            {
                target.AddBuff(ModContent.BuffType<ExoFreeze>(), 120, true);
            }
        }
    }
}
