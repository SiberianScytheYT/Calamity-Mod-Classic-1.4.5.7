using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class ExecutionersBladeStealthProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/ExecutionersBlade";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blade");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 100;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft >= 75)
                Projectile.velocity.Y *= 1.3f;
            Projectile.rotation += 0.5f * (float)Projectile.direction;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 173, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }

        public override void PostDraw(Color lightColor)
        {
            Vector2 origin = new Vector2(32f, 32f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/Items/Weapons/Rogue/ExecutionersBladeGlow").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 450);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 450);
        }
        */

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 128;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
            for (int num621 = 0; num621 < 3; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 5; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
