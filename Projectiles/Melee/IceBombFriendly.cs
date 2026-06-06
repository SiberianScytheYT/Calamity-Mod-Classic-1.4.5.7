using CalRD.Buffs.StatDebuffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Projectiles.Melee
{
    public class IceBombFriendly : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Boss/IceBomb";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
			Projectile.coldDamage = true;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.985f;
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            float spread = 90f * 0.0174f;
            double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle;
            int i;
            if (Projectile.owner == Main.myPlayer)
            {
                for (i = 0; i < 2; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    int projectile1 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ProjectileID.FrostShard, (int)(Projectile.damage * 0.5), 0f, Projectile.owner, 0f, 0f);
                    Main.projectile[projectile1].Calamity().forceMelee = true;
                    int projectile2 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ProjectileID.FrostShard, (int)(Projectile.damage * 0.5), 0f, Projectile.owner, 0f, 0f);
                    Main.projectile[projectile2].Calamity().forceMelee = true;
                }
            }
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 60);
        }
    }
}
