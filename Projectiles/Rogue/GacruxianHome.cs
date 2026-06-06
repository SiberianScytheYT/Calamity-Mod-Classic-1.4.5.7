using CalRD.Dusts;
using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class GacruxianHome : ModProjectile
    {
        public override string Texture => "CalRD/Items/Fishing/AstralCatches/GacruxianMollusk";

        private int stealthTrailTimer = 10;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mollusk");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 18;
            Projectile.friendly = true;
            Projectile.Calamity().rogue = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            AIType = ProjectileID.DeathSickle;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            stealthTrailTimer--;
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            if (stealthTrailTimer == 0 && Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0f, Projectile.velocity.Y * 0f, ModContent.ProjectileType<UltimusCleaverDust>(), (int)((double)Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceRogue = true;
                Main.projectile[proj].localNPCHitCooldown = 10;
                Main.projectile[proj].penetrate = 3;
                stealthTrailTimer = 10;
            }
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 500f, 16f, 20f);
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
