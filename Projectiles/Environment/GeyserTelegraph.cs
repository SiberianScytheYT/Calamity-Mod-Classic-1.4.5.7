using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Environment
{
	public class GeyserTelegraph : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

		private bool initialized = false;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Geyser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
			Projectile.trap = true;
        }

        public override void AI()
        {
			if (!initialized)
			{
				int projectileType = ModContent.ProjectileType<SmokeTelegraph>();
				float randomVelocity = Main.rand.NextFloat() + 0.5f;
				int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, -8f * randomVelocity, projectileType, 0, 0f, Projectile.owner, 0f, 0f);
				Main.projectile[proj].netUpdate = true;
				initialized = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			int projectileType = ProjectileID.GeyserTrap;
			if (Projectile.ai[0] == 1f)
			{
				projectileType = ModContent.ProjectileType<BrimstoneGeyser>();
			}
			float randomVelocity = Main.rand.NextFloat() + 0.5f;
			int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, -8f * randomVelocity, projectileType, 20, 2f, Projectile.owner, 0f, 0f);
			Main.projectile[proj].friendly = false;
			Main.projectile[proj].netUpdate = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;
	}
}
