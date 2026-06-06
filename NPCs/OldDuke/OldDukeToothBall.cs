using CalRD.Buffs.StatDebuffs;
using CalRD.Projectiles.Boss;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Dusts;
using CalRD.Events;

namespace CalRD.NPCs.OldDuke
{
	public class OldDukeToothBall : ModNPC
	{
		bool spawnedProjectiles = false;
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Tooth Ball");
		}
		
		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			AIType = -1;
			NPC.GetNPCDamage();
			NPC.width = 40;
			NPC.height = 40;
			NPC.defense = 0;
			NPC.lifeMax = 5000;
			if (BossRushEvent.BossRushActive)
			{
				NPC.lifeMax = 75000;
			}
			NPC.alpha = 255;
            NPC.knockBackResist = 0f;
			for (int k = 0; k < NPC.buffImmune.Length; k++)
			{
				NPC.buffImmune[k] = true;
			}
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath11;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.dontTakeDamage);
			writer.Write(spawnedProjectiles);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.dontTakeDamage = reader.ReadBoolean();
			spawnedProjectiles = reader.ReadBoolean();
		}

		public override void AI()
		{
            NPC.rotation += NPC.velocity.X * 0.05f;
            if (NPC.alpha > 0)
            {
                NPC.alpha -= 15;
            }
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            else if (NPC.timeLeft < 600)
            {
                NPC.timeLeft = 600;
            }
            Vector2 vector = player.Center - NPC.Center;
            if (vector.Length() < 40f || NPC.ai[3] >= 900f)
            {
                NPC.dontTakeDamage = false;
                NPC.life = 0;
				NPC.HitEffect(0, 10.0);
				NPC.checkDead();
				NPC.active = false;
				return;
            }

            NPC.ai[3] += 1f;
            NPC.dontTakeDamage = NPC.ai[3] >= 600f ? false : true;
            if (NPC.ai[3] >= 480f)
            {
                NPC.velocity *= 0.985f;
                return;
            }

            float num1372 = 12f;
			if (Main.expertMode || BossRushEvent.BossRushActive)
				num1372 += Vector2.Distance(player.Center, NPC.Center) * 0.01f;

			Vector2 vector167 = new Vector2(NPC.Center.X + NPC.direction * 20, NPC.Center.Y + 6f);
            float num1373 = player.position.X + player.width * 0.5f - vector167.X;
            float num1374 = player.Center.Y - vector167.Y;
            float num1375 = (float)Math.Sqrt(num1373 * num1373 + num1374 * num1374);
            float num1376 = num1372 / num1375;
            num1373 *= num1376;
            num1374 *= num1376;
            NPC.ai[0] -= Main.rand.Next(6);
            if (num1375 < 300f || NPC.ai[0] > 0f)
            {
                if (num1375 < 300f)
                {
                    NPC.ai[0] = 100f;
                }
                if (NPC.velocity.X < 0f)
                {
                    NPC.direction = -1;
                }
                else
                {
                    NPC.direction = 1;
                }
                return;
            }

            NPC.velocity.X = (NPC.velocity.X * 50f + num1373) / 51f;
            NPC.velocity.Y = (NPC.velocity.Y * 50f + num1374) / 51f;
            if (num1375 < 350f)
            {
                NPC.velocity.X = (NPC.velocity.X * 10f + num1373) / 11f;
                NPC.velocity.Y = (NPC.velocity.Y * 10f + num1374) / 11f;
            }
            if (num1375 < 300f)
            {
                NPC.velocity.X = (NPC.velocity.X * 7f + num1373) / 8f;
                NPC.velocity.Y = (NPC.velocity.Y * 7f + num1374) / 8f;
            }

			float num1247 = 0.5f;
			for (int num1248 = 0; num1248 < Main.maxNPCs; num1248++)
			{
				if (Main.npc[num1248].active)
				{
					if (num1248 != NPC.whoAmI && Main.npc[num1248].type == NPC.type)
					{
						if (Vector2.Distance(NPC.Center, Main.npc[num1248].Center) < 48f)
						{
							if (NPC.position.X < Main.npc[num1248].position.X)
								NPC.velocity.X -= num1247;
							else
								NPC.velocity.X += num1247;

							if (NPC.position.Y < Main.npc[num1248].position.Y)
								NPC.velocity.Y -= num1247;
							else
								NPC.velocity.Y += num1247;
						}
					}
				}
			}
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			cooldownSlot = 1;
			return NPC.alpha == 0;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
            target.AddBuff(BuffID.Venom, 180, true);
			target.AddBuff(BuffID.Poisoned, 180, true);
			target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
		}

        public override bool CheckDead()
        {
            SoundEngine.PlaySound(SoundID.Item14, NPC.position);

            NPC.position.X = NPC.position.X + (NPC.width / 2);
            NPC.position.Y = NPC.position.Y + (NPC.height / 2);
            NPC.width = NPC.height = 96;
            NPC.position.X = NPC.position.X - (NPC.width / 2);
            NPC.position.Y = NPC.position.Y - (NPC.height / 2);

            for (int num621 = 0; num621 < 15; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
                Main.dust[num622].noGravity = true;
            }

            for (int num623 = 0; num623 < 30; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
                Main.dust[num624].noGravity = true;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && !spawnedProjectiles)
            {
				spawnedProjectiles = true;
				int totalProjectiles = 4;
				float radians = MathHelper.TwoPi / totalProjectiles;
				int damage = Main.expertMode ? 55 : 70;
				for (int k = 0; k < totalProjectiles; k++)
				{
					float velocity = Main.rand.Next(7, 11);
					Vector2 vector255 = new Vector2(0f, -velocity).RotatedBy(radians * k);
					int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, ModContent.ProjectileType<SandTooth>(), damage, 0f, Main.myPlayer, 0f, 0f);
					Main.projectile[proj].timeLeft = 360;
				}
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<SandPoisonCloud>(), damage, 0f, Main.myPlayer, 0f, 0f);
            }

            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
		{
			for (int k = 0; k < 3; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
			}
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 15; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
				}
			}
		}
	}
}