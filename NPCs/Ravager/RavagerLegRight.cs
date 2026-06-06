using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Ravager
{
	public class RavagerLegRight : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ravager");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 40;
			NPC.DR_NERD(0.15f);
            NPC.lifeMax = 22010;
            NPC.knockBackResist = 0f;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
			NPC.buffImmune[BuffID.Frostburn] = false;
			NPC.buffImmune[BuffID.Daybreak] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
            NPC.buffImmune[BuffID.BetsysCurse] = false;
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.noGravity = true;
            NPC.canGhostHeal = false;
            NPC.noTileCollide = true;
            NPC.alpha = 255;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath14;
            if (CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive)
            {
                NPC.defense *= 2;
                NPC.lifeMax *= 7;
            }
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 400000;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
        }

        public override void AI()
        {
            bool provy = CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive;
            Vector2 center = NPC.Center;
            if (CalamityGlobalNPC.scavenger < 0 || !Main.npc[CalamityGlobalNPC.scavenger].active)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }
            if (NPC.timeLeft < 1800)
            {
                NPC.timeLeft = 1800;
            }
            if (NPC.alpha > 0)
            {
                NPC.alpha -= 10;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                NPC.ai[1] = 0f;
            }
            if (NPC.ai[0] == 0f)
            {
                float num659 = 21f;
                Vector2 vector79 = new Vector2(center.X, center.Y);
                float num660 = Main.npc[CalamityGlobalNPC.scavenger].Center.X - vector79.X;
                float num661 = Main.npc[CalamityGlobalNPC.scavenger].Center.Y - vector79.Y;
                num661 += 88f;
                num660 += 70f;
                float num662 = (float)Math.Sqrt(num660 * num660 + num661 * num661);
                if (num662 < 12f + num659)
                {
                    NPC.rotation = 0f;
                    NPC.velocity.X = num660;
                    NPC.velocity.Y = num661;
                }
                else
                {
                    num662 = num659 / num662;
                    NPC.velocity.X = num660 * num662;
                    NPC.velocity.Y = num661 * num662;
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
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerLegRight").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerLegRight2").Type, 1f);
                }
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
