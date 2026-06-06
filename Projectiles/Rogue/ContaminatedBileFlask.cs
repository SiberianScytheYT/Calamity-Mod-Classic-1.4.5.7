using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class ContaminatedBileFlask : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/ContaminatedBile";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Contaminated Bile");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.Calamity().rogue = true;
        }
        public override void AI()
        {
            if (Projectile.ai[0]++ > 45f)
            {
                if (Projectile.velocity.Y < 10f)
                {
                    Projectile.velocity.Y += 0.15f;
                }
            }
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length());
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107, Projectile.Bottom);
            Projectile explosion = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BileExplosion>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner);
            explosion.Calamity().stealthStrike = Projectile.Calamity().stealthStrike;
            explosion.timeLeft = explosion.Calamity().stealthStrike ? 60 : 20;
        }
    }
}
