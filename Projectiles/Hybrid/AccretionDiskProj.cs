using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Hybrid
{
	public class AccretionDiskProj : ModProjectile
	{
        public override string Texture => "CalRD/Items/Weapons/Rogue/AccretionDisk";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Elemental Disk");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 56;
			Projectile.height = 56;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 3;
			Projectile.timeLeft = 400;
			AIType = ProjectileID.WoodenBoomerang;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				int rainbow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, (float)(Projectile.direction * 2), 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
				Main.dust[rainbow].noGravity = true;
				Main.dust[rainbow].velocity *= 0f;
			}

			Lighting.AddLight(Projectile.Center, 0.15f, 1f, 0.25f);

			float maxDistance = 300f;
			bool homeIn = false;

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.CanBeChasedBy(Projectile, false))
				{
					float extraDistance = (npc.width / 2) + (npc.height / 2);

					bool canHit = true;
					if (extraDistance < maxDistance)
						canHit = Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1);

					if (Vector2.Distance(npc.Center, Projectile.Center) < (maxDistance + extraDistance) && canHit)
					{
						homeIn = true;
						break;
					}
				}
			}

			if (!Projectile.friendly)
			{
				homeIn = false;
			}

			if (homeIn)
			{
				if (Main.player[Projectile.owner].miscCounter % 50 == 0)
				{
					int splitProj = ModContent.ProjectileType<AccretionDisk2>();
					if (Projectile.owner == Main.myPlayer && Main.player[Projectile.owner].ownedProjectileCounts[splitProj] < 25)
					{
						float spread = 45f * 0.0174f;
						double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
						double deltaAngle = spread / 8f;
						double offsetAngle;
						for (int i = 0; i < 4; i++)
						{
							offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
							int disk = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), splitProj, Projectile.damage, Projectile.knockBack, Projectile.owner);
							Main.projectile[disk].Calamity().forceMelee = Projectile.CountsAsClass(DamageClass.Melee);
							Main.projectile[disk].Calamity().forceRogue = Projectile.Calamity().rogue;
							int disk2 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), splitProj, Projectile.damage, Projectile.knockBack, Projectile.owner);
							Main.projectile[disk2].Calamity().forceMelee = Projectile.CountsAsClass(DamageClass.Melee);
							Main.projectile[disk2].Calamity().forceRogue = Projectile.Calamity().rogue;
						}
					}
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
			target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
			target.AddBuff(ModContent.BuffType<Plague>(), 120);
			target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
		}

		//public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
		/*
		{
			target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
			target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
			target.AddBuff(ModContent.BuffType<Plague>(), 120);
			target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
		}
		*/

		public override bool PreDraw(ref Color lightColor)
		{
			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
			return false;
		}
	}
}
