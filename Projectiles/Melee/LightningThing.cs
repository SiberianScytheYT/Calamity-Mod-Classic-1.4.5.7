using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Weapons.Melee;
namespace CalRD.Projectiles.Melee
{
	public class LightningThing : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lightning");
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.timeLeft = 90;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void OnKill(int timeLeft)
        {
            int damage = GaelsGreatsword.BaseDamage;
            for (int i = 0; i < 3; i++)
            {
                int idx = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center + new Vector2(Main.rand.NextFloat(-35f, 35f), -1600f), Vector2.UnitY * 12f,
                    ProjectileID.CultistBossLightningOrbArc, GaelsGreatsword.BaseDamage, 0f, Projectile.owner,
                    MathHelper.PiOver2, Main.rand.Next(100));
                Main.projectile[idx].Calamity().forceMelee = true;
                Main.projectile[idx].usesLocalNPCImmunity = true;
                Main.projectile[idx].localNPCHitCooldown = GaelsGreatsword.ImmunityFrames;
            }
        }
    }
}
