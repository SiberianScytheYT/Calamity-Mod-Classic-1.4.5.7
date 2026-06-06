using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Hybrid
{
    public class AccretionDisk2 : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/AccretionDisk";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Accretion Disk");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.alpha = 120;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 60;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(10))
            {
                int num250 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, (float)(Projectile.direction * 2), 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 0.5f);
                Main.dust[num250].noGravity = true;
                Main.dust[num250].velocity *= 0f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
        }
        */

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }
    }
}
