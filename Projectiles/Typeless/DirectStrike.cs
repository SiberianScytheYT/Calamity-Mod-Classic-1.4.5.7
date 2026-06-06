using Terraria;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Typeless
{
    public class DirectStrike : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nondescript Damaging Entity");
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 2;
        }

        // If the AI parameter isn't a valid NPC slot, it can hit anything. Otherwise it can only hit one NPC.
		// TODO -- this might actually not work in multiplayer if the DirectStrike gets synced...
        public override bool? CanHitNPC(NPC target)
        {
			if (Projectile.ai[0] < 0f || Projectile.ai[0] > 199f || Projectile.ai[0] == target.whoAmI)
                return null;
            return (bool?)false;
        }
    }
}
