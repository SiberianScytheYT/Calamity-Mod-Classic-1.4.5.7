using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.CalPlayer;

namespace CalRD.Projectiles.Rogue
{
	public class DukesDecapitatorProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/DukesDecapitator";

		bool stealthBubbles = false;
		float rotationAmount = 1.5f;

    	public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Decapitator");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.tileCollide = false;
            Projectile.Calamity().rogue = true;
		}

		public override void AI()
        {
            CalamityPlayer modPlayer = Main.player[Projectile.owner].Calamity();
			if(Projectile.velocity.X != 0 || Projectile.velocity.Y != 0)
			{
				Projectile.velocity.X *= 0.99f;
				Projectile.velocity.Y *= 0.99f;
			}
			Projectile.ai[0] += 1f;
			if(Projectile.ai[0] == 1f && modPlayer.StealthStrikeAvailable())
			{
				stealthBubbles = true;
                Projectile.Calamity().stealthStrike = true;
			}
			if (Projectile.ai[0] == 5f)
				Projectile.tileCollide = true;

        	if ((Projectile.ai[0] % 15f) == 0f && rotationAmount > 0)
        	{
				rotationAmount -= 0.05f;
				if(stealthBubbles == true && Projectile.owner == Main.myPlayer)
        		{
					float velocityX = Main.rand.NextFloat(-0.8f, 0.8f);
					float velocityY = Main.rand.NextFloat(-0.8f, -0.8f);
					Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, velocityX, velocityY, ModContent.ProjectileType<DukesDecapitatorBubble>(), (int)((double)Projectile.damage * 0.8), Projectile.knockBack, Projectile.owner, 0f, 0f);
				}
			}
			if(rotationAmount <= 0f)
				Projectile.Kill();

			Projectile.rotation += rotationAmount;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.velocity.X = 0f;
			Projectile.velocity.Y = 0f;
			rotationAmount -= 0.05f;
		}

		public override void OnKill(int timeLeft)
        {
			for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 49, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, new Color(255, 255, 255), 0.75f);
			}
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
