using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.AquaticScourge
{
	public class AquaticScourgeBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquatic Scourge");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.width = 32;
            NPC.height = 32;
            NPC.defense = 20;
			NPC.DR_NERD(0.05f);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.alpha = 255;
            NPC.LifeMaxNERB(73000, 85000, 10000000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.dontCountMe = true;
            NPC.chaseable = false;
            NPC.canGhostHeal = false;

			if (CalamityWorld.death || BossRushEvent.BossRushActive)
				NPC.scale = 1.2f;
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
			CalamityAI.AquaticScourgeAI(NPC, Mod, false);
		}

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.minion)
            {
                return NPC.Calamity().newAI[0] == 1f;
            }
            return null;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                    }
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ASBody").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ASBody2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ASBody3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ASBody4").Type, 1f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 180, true);
            target.AddBuff(BuffID.Venom, 180, true);
        }
    }
}
