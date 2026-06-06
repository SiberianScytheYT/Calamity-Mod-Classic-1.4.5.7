using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class MoltenAmputatorProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/MoltenAmputator";

        private int stealthyBlobTimer = 6;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Amputator");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 74;
            Projectile.height = 74;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 180;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.Calamity().rogue = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }

        private void fireInTheBlob(int blobCount)
        {
            for (int i = 0; i < blobCount; i++)
            {
                Vector2 iAmSpeed = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                while (iAmSpeed.X == 0f && iAmSpeed.Y == 0f)
                {
                    iAmSpeed = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                }
                iAmSpeed.Normalize();
                iAmSpeed *= (float)Main.rand.Next(70, 101) * 0.1f;
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, iAmSpeed.X, iAmSpeed.Y, ModContent.ProjectileType<MoltenBlobThrown>(), (int)(Projectile.damage * 0.25), 0f, Projectile.owner, 0f, 0f);
            }
        }

        public override void AI()
        {
            if (Projectile.Calamity().stealthStrike)
            {
                stealthyBlobTimer--;
                if (stealthyBlobTimer <= 0 && Projectile.timeLeft % 2 == 0)
                {
                    fireInTheBlob(1);
                    stealthyBlobTimer = Main.rand.Next(4, 10);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 4;
            if (Projectile.owner == Main.myPlayer)
            {
                fireInTheBlob(Projectile.Calamity().stealthStrike ? Main.rand.Next(3, 5) : Main.rand.Next(1, 3));
            }
            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 244, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            if (Projectile.owner == Main.myPlayer)
            {
                fireInTheBlob(Projectile.Calamity().stealthStrike ? Main.rand.Next(3, 5) : Main.rand.Next(1, 3));
            }
            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 244, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
        */
    }
}
