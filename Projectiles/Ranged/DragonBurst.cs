using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
    public class DragonBurst : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Burst");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.extraUpdates = 10;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi) +MathHelper.ToRadians(90) * Projectile.direction;
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {

                float num93 = Projectile.velocity.X / 3f;
                float num94 = Projectile.velocity.Y / 3f;
                int num95 = 4;
                int num96 = Dust.NewDust(new Vector2(Projectile.position.X + (float)num95, Projectile.position.Y + (float)num95), Projectile.width - num95 * 2, Projectile.height - num95 * 2, 127, 0f, 0f, 100, default, 2f);
                Main.dust[num96].noGravity = true;
                Main.dust[num96].velocity *= 0.1f;
                Main.dust[num96].velocity += Projectile.velocity * 0.1f;
                Dust expr_47FA_cp_0 = Main.dust[num96];
                expr_47FA_cp_0.position.X -= num93;
                Dust expr_4815_cp_0 = Main.dust[num96];
                expr_4815_cp_0.position.Y -= num94;
                
                if (Main.rand.NextBool(20))
                {
                    int num97 = 4;
                    int num98 = Dust.NewDust(new Vector2(Projectile.position.X + (float)num97, Projectile.position.Y + (float)num97), Projectile.width - num97 * 2, Projectile.height - num97 * 2, 127, 0f, 0f, 100, default, 0.6f);
                    Main.dust[num98].velocity *= 0.25f;
                    Main.dust[num98].velocity += Projectile.velocity * 0.5f;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                Main.projectile[proj].Calamity().forceRanged = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 600);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
