using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Weapons.Magic;
namespace CalRD.Projectiles.Magic
{
    public class SparkInfernal : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spark");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
        }

        public override void OnKill(int timeLeft)
        {
			Player player = Main.player[Projectile.owner];
			Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<InfernadoMarkFriendly>(), (int)(TheWand.BaseDamage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Magic).Additive - 1f)), Projectile.knockBack, Projectile.owner, 0f, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 900);
            target.AddBuff(BuffID.OnFire, 900);
        }
    }
}
