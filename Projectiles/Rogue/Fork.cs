using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class Fork : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fork");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 113;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.BoneJavelin;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            if (Main.rand.Next(5) == 0)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 5, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 300);
            player.AddBuff(BuffID.WellFed, 180);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(BuffID.Bleeding, 300);
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 300);
            player.AddBuff(BuffID.WellFed, 180);
        }
        */

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
