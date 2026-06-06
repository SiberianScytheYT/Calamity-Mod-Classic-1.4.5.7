using CalRD.Dusts;
using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class GacruxianProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Fishing/AstralCatches/GacruxianMollusk";

        private int sparkTrailTimer = 10;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mollusk");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 113;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.BoneJavelin;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            sparkTrailTimer--;
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            if (sparkTrailTimer == 0)
            {
                if (Projectile.Calamity().stealthStrike == true)
                {
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0f, Projectile.velocity.Y * 0f, ModContent.ProjectileType<GacruxianHome>(), (int)((double)Projectile.damage * 0.3), Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
                else
                {
                    int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0f, Projectile.velocity.Y * 0f, ModContent.ProjectileType<UltimusCleaverDust>(), (int)((double)Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, 0f, 0f);
                    Main.projectile[proj].Calamity().forceRogue = true;
                    Main.projectile[proj].localNPCHitCooldown = 10;
                    Main.projectile[proj].penetrate = 3;
                }
                sparkTrailTimer = 10;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
