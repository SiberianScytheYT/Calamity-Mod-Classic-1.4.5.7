using CalRD.Events;
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Ravager
{
    public class RavagerHead2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ravager");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 80;
            NPC.height = 80;
            NPC.lifeMax = 100;
            NPC.knockBackResist = 0f;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath14;
        }

        public override void AI()
        {
            if (CalamityGlobalNPC.scavenger < 0 || !Main.npc[CalamityGlobalNPC.scavenger].active)
            {
                NPC.dontTakeDamage = false;
                NPC.life = 0;
                NPC.HitEffect(NPC.direction, 9999);
                NPC.netUpdate = true;
                return;
            }

			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
			bool provy = CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive;

            if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

			// Get a target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			// Target variable
			Player player = Main.player[NPC.target];

			// Rotation
			float num801 = NPC.position.X + (NPC.width / 2) - player.position.X - (player.width / 2);
			float num802 = NPC.position.Y + NPC.height - 59f - player.position.Y - (player.height / 2);
			float num803 = (float)Math.Atan2(num802, num801) + MathHelper.PiOver2;
			if (num803 < 0f)
				num803 += MathHelper.TwoPi;
			else if (num803 > MathHelper.TwoPi)
				num803 -= MathHelper.TwoPi;

			float num804 = 0.1f;
			if (NPC.rotation < num803)
			{
				if ((num803 - NPC.rotation) > MathHelper.Pi)
					NPC.rotation -= num804;
				else
					NPC.rotation += num804;
			}
			else if (NPC.rotation > num803)
			{
				if ((NPC.rotation - num803) > MathHelper.Pi)
					NPC.rotation += num804;
				else
					NPC.rotation -= num804;
			}

			if (NPC.rotation > num803 - num804 && NPC.rotation < num803 + num804)
				NPC.rotation = num803;
			if (NPC.rotation < 0f)
				NPC.rotation += MathHelper.TwoPi;
			else if (NPC.rotation > MathHelper.TwoPi)
				NPC.rotation -= MathHelper.TwoPi;
			if (NPC.rotation > num803 - num804 && NPC.rotation < num803 + num804)
				NPC.rotation = num803;

			NPC.ai[1] += 1f;
			bool fireProjectiles = NPC.ai[1] >= 480f;
			if (fireProjectiles && Vector2.Distance(NPC.Center, player.Center) > 80f)
            {
				int type = ModContent.ProjectileType<ScavengerLaser>();
				int damage = NPC.GetProjectileDamage(type);
				float projectileVelocity = death ? 8f : 6f;

				if (NPC.ai[1] >= 600f)
				{
					NPC.ai[0] += 1f;
					NPC.ai[1] = 0f;

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						SoundEngine.PlaySound(SoundID.Item33, NPC.position);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(player.Center - NPC.Center) * projectileVelocity, type, damage + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, -1f);
					}
				}
				else
				{
					if (NPC.ai[1] % 40f == 0f)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							SoundEngine.PlaySound(SoundID.Item33, NPC.position);
							Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Normalize(player.Center - NPC.Center) * projectileVelocity, type, damage + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, -1f);
						}
					}
				}
            }

			float num823 = 18f;
			float num824 = 0.2f;
			if (death)
			{
				num823 += 4f;
				num824 += 0.05f;
			}
			if (provy)
			{
				num823 *= 1.25f;
				num824 *= 1.25f;
			}
			if (BossRushEvent.BossRushActive)
			{
				num823 *= 1.5f;
				num824 *= 1.5f;
			}

			Vector2 vector82 = NPC.Center;
			float distanceX = NPC.ai[0] % 2f == 0f ? 480f : -480f;
			float distanceY = fireProjectiles ? -320f : 320f;
			float num825 = player.Center.X + (fireProjectiles ? distanceX : 0f) - vector82.X;
			float num826 = player.Center.Y + distanceY - vector82.Y;
			float num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
			num827 = num823 / num827;
			num825 *= num827;
			num826 *= num827;

			if (NPC.velocity.X < num825)
			{
				NPC.velocity.X += num824;
				if (NPC.velocity.X < 0f && num825 > 0f)
					NPC.velocity.X += num824;
			}
			else if (NPC.velocity.X > num825)
			{
				NPC.velocity.X -= num824;
				if (NPC.velocity.X > 0f && num825 < 0f)
					NPC.velocity.X -= num824;
			}
			if (NPC.velocity.Y < num826)
			{
				NPC.velocity.Y += num824;
				if (NPC.velocity.Y < 0f && num826 > 0f)
					NPC.velocity.Y += num824;
			}
			else if (NPC.velocity.Y > num826)
			{
				NPC.velocity.Y -= num824;
				if (NPC.velocity.Y > 0f && num826 < 0f)
					NPC.velocity.Y -= num824;
			}
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
	            if (Main.netMode != NetmodeID.Server)
	            {
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerHead").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerHead2").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerHead3").Type, 1f);
                }
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

		public override bool CheckActive()
		{
			return false;
		}

		public override bool PreKill()
        {
            return false;
        }
    }
}
