using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Perforator
{
	public class PerforatorBodyLarge : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Perforator");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
	            Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 8;
			NPC.LifeMaxNERB(2500, 2700, 800000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = 6;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.alpha = 255;
            NPC.buffImmune[ModContent.BuffType<TimeSlow>()] = false;
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
			// Target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			Player player = Main.player[NPC.target];
			if (player.dead)
			{
				NPC.TargetClosest(false);
			}
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
                {
                    NPC.alpha = 0;
                }
            }
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = NPC.localAI[3] == 1f ? ModContent.Request<Texture2D>("CalRD/NPCs/Perforator/PerforatorBodyLargeAlt").Value : TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / 2));

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height)) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = NPC.localAI[3] == 1f ? ModContent.Request<Texture2D>("CalRD/NPCs/Perforator/PerforatorBodyLargeAltGlow").Value : ModContent.Request<Texture2D>("CalRD/NPCs/Perforator/PerforatorBodyLargeGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Yellow, 0.5f);

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
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
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
	            if (Main.netMode != NetmodeID.Server)
	            {
		            for (int k = 0; k < 20; k++)
		            {
			            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
		            }
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LargePerf2").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LargePerf3").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LargePerf4").Type, 1f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 180, true);
            target.AddBuff(BuffID.Bleeding, 180, true);
        }
    }
}
