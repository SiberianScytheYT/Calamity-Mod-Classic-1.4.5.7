using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace CalRD.Projectiles.Rogue
{
    public class SphereSpiked : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/DefectiveSphere";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spiked Sphere");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0f, 0f);
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 229, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100);
            }
            if (Projectile.ai[0] == 1f)
            {
                Projectile.extraUpdates = 1;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Projectile.damage = (int)(Projectile.damage * 1.2);
            if(Main.rand.NextBool(10))
                modifiers.SetCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.NPCHit34, Projectile.position);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
