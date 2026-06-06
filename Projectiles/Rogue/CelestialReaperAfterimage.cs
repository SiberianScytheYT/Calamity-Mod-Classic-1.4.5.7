using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class CelestialReaperAfterimage : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/CelestialReaper";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Reaper");
        }

        public override void SetDefaults()
        {
            Projectile.width = 66;
            Projectile.height = 76;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 51;
            Projectile.tileCollide = false;
            Projectile.Calamity().rogue = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(30f); // Buzzsaw scythe.
            NPC target = Projectile.position.ClosestNPCAt(640f);
            if (target != null)
            {
                Projectile.velocity = (Projectile.velocity * 20f + Projectile.DirectionTo(target.Center) * 20f) / 21f;
            }
            Projectile.alpha += 5;
        }
    }
}
