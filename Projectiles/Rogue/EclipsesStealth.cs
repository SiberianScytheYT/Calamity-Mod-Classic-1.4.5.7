using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class EclipsesStealth : ModProjectile
	{
        public override string Texture => "CalRD/Items/Weapons/Rogue/EclipsesFall";

		private bool changedTimeLeft = false;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Eclipse's Stealth");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 25;
			Projectile.height = 25;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.Calamity().rogue = true;
		}

		public override void AI()
		{
			if (Projectile.timeLeft % 5 == 0) //congrats Pinkie... every 5 ticks
			{
				if (Main.rand.NextBool(2) && Main.myPlayer == Projectile.owner)
				{
					float dmgKBMult = Main.rand.NextFloat(0.4f, 0.6f);
					int spearAmt = Main.rand.Next(1, 3); //1 to 2 spears
					for (int n = 0; n < spearAmt; n++)
					{
						CalamityUtils.ProjectileRain(Projectile.GetSource_FromThis(), Projectile.Center, 400f, 100f, 500f, 800f, 29f, ModContent.ProjectileType<EclipsesSmol>(), (int)(Projectile.damage * dmgKBMult), Projectile.knockBack * dmgKBMult, Projectile.owner);
					}
				}
			}

			//Behavior when not sticking to anything
			if (Projectile.ai[0] == 0f)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				if (Main.rand.NextBool(8)) //dust
				{
					Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 138, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				}
			}

			//Ensures that a spear will last 10 seconds after it hits something
			if (Projectile.ai[0] == 1f && !changedTimeLeft)
			{
				Projectile.timeLeft = 600;
				changedTimeLeft = true;
			}

			//Sticky Behaviour
			Projectile.StickyProjAI(10);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => Projectile.ModifyHitNPCSticky(1);

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return null;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.ai[0] == 1f)
			{
				return false;
			}
			return null;
		}

		public override bool CanHitPvp(Player target) => Projectile.ai[0] != 1f;
	}
}
