using CalRD.Events;
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.DesertScourge
{
	public class DesertScourgeBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Desert Scourge");
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 32;
            NPC.height = 36;
            NPC.defense = 6;
			NPC.DR_NERD(0.05f);
            NPC.LifeMaxNERB(2300, 2650, 16500000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = 6;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.boss = true;
            Mod CalamityModMusic = ModLoader.HasMod("CalamityModMusic") ? ModLoader.GetMod("CalamityModMusic") : null;
            if (CalamityModMusic != null)
                Music = MusicLoader.GetMusicSlot("CalamityModMusic/Sounds/Music/DesertScourge");
            else
                Music = MusicID.Boss1;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.dontCountMe = true;

			if (CalamityWorld.death || BossRushEvent.BossRushActive)
				NPC.scale = 1.25f;
			else if (CalamityWorld.revenge)
				NPC.scale = 1.15f;
			else if (Main.expertMode)
				NPC.scale = 1.1f;
		}

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void AI()
        {
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}

			Player player = Main.player[NPC.target];
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
			float burrowTimeGateValue = death ? 420f : 540f;
			bool burrow = Main.npc[(int)NPC.ai[2]].Calamity().newAI[0] >= burrowTimeGateValue;

			if (!Main.npc[(int)NPC.ai[1]].active)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }

            if (Main.npc[(int)NPC.ai[1]].alpha < 128)
            {
                NPC.alpha -= 42;
                if (NPC.alpha < 0)
                    NPC.alpha = 0;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && revenge && !burrow)
            {
                NPC.localAI[0] += (float)Main.rand.Next(4);
                if (NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive))
                {
                    NPC.localAI[0] += 4f;
                }
                if (NPC.localAI[0] >= (float)Main.rand.Next(1400, 26001))
                {
                    NPC.localAI[0] = 0f;
                    if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                    {
                        Vector2 vector104 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)(NPC.height / 2));
                        float num942 = player.position.X + (float)player.width * 0.5f - vector104.X;
                        float num943 = player.position.Y + (float)player.height * 0.5f - vector104.Y;
                        float num944 = (float)Math.Sqrt((double)(num942 * num942 + num943 * num943));
                        int projectileType = ModContent.ProjectileType<SandBlast>();
                        float num941 = BossRushEvent.BossRushActive ? 12f : 6f;
                        num944 = num941 / num944;
                        num942 *= num944;
                        num943 *= num944;
                        vector104.X += num942 * 5f;
                        vector104.Y += num943 * 5f;
                        if (Main.rand.NextBool(2) || BossRushEvent.BossRushActive)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), vector104.X, vector104.Y, num942, num943, projectileType, NPC.GetProjectileDamage(projectileType), 0f, Main.myPlayer, 0f, 0f);
                        }
                        NPC.netUpdate = true;
                    }
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

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= NPC.lifeMax * 0.75f && NPC.CountNPCS(ModContent.NPCType<DriedSeekerHead>()) < 3)
            {
                if (Main.rand.NextBool(10) && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 spawnAt = NPC.Center + new Vector2(0f, (float)NPC.height / 2f);
                    int seeker = NPC.NewNPC(NPC.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<DriedSeekerHead>());
                    if (Main.netMode == NetmodeID.Server && seeker < 200)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, seeker, 0f, 0f, 0f, 0, 0, 0);
                    }
                    NPC.netUpdate = true;
                }
            }
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    float randomSpread = (float)(Main.rand.Next(-100, 100) / 100);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("ScourgeBody").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("ScourgeBody2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("ScourgeBody3").Type, 1f);
                }
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 120, true);
        }
    }
}
