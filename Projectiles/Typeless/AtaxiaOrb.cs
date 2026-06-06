using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Typeless
{
    public class AtaxiaOrb : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ataxia Orb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
        }

        public override void AI()
        {
            Dust fire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 127, 0f, 0f, 100, default, 2f);
            fire.noGravity = true;
            fire.velocity = Vector2.Zero;

            CalamityGlobalProjectile.HomeInOnNPC(Projectile, true, 400f, 9f, 20f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*{
            target.AddBuff(BuffID.OnFire, 180);
        }*/
    }
}
