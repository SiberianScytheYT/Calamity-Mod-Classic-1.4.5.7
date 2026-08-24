using Terraria;
using Terraria.ModLoader;
using CalRD.Buffs.DamageOverTime;
namespace CalRD.Projectiles.Typeless
{
    public class SabatonBoom : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 450;
            Projectile.height = 450;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
        }
    }
}
