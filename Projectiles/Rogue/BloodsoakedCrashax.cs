using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
	public class BloodsoakedCrashax : ModProjectile
	{
        public override string Texture => "CalRD/Items/Weapons/Rogue/BloodsoakedCrasher";

		private int bounce = 3; //number of times it bounces
		private int grind = 0; //used to know when to slow down

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bloodsoaked Crasher");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.penetrate = 10;
			Projectile.timeLeft = 600; //10 seconds and counting
			Projectile.aiStyle = 2;
			AIType = ProjectileID.ThrowingKnife; //Throwing Knife AI
			Projectile.Calamity().rogue = true;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 5;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			if (grind > 0)
			{
				grind--;
			}
			if (grind >= 1)
			{
				Projectile.extraUpdates = 0; //stop, you're touching an enemy
				Projectile.velocity.X *= 0.75f;
				Projectile.velocity.Y *= 0.75f;
			}
			else
			{
				Projectile.velocity.X *= 1.005f; //you broke up, time to yeet yourself out
				Projectile.velocity.Y *= 1.005f;
				if (Projectile.velocity.X > 16f)
				{
					Projectile.velocity.X = 16f;
				}
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
				Projectile.extraUpdates = 1;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			bounce--;
			if (bounce <= 0)
			{
				Projectile.Kill(); //you can only bounce so much 'til death
			}
			else
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
			OnHitEffects(!target.canGhostHeal || Main.player[Projectile.owner].moonLeech);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
			OnHitEffects(Main.player[Projectile.owner].moonLeech);
		}
		
		private void OnHitEffects(bool cannotLifesteal)
		{
			if (grind < 10)
			{
				grind += 5; //THE GRIND NEVER STOPS
			}

			if (Projectile.Calamity().stealthStrike && Projectile.owner == Main.myPlayer) //stealth strike attack
			{
				int stealth = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Blood>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
				Main.projectile[stealth].Calamity().forceRogue = true;
			}

			Player player = Main.player[Projectile.owner];
			if (cannotLifesteal) //canGhostHeal be like lol
			{
				return;
			}
			if (Main.rand.NextBool(2))
			{
				player.statLife += 1; //Trello said 2 hp per hit. Sounds like a fat balancing problem.
				player.HealEffect(1);
			}
		}

		public override bool PreDraw(ref Color lightColor) //afterimages
		{
			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
			return false;
		}
	}
}
