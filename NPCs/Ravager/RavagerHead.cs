using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
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
	public class RavagerHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ravager");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 80;
            NPC.height = 80;
            NPC.defense = 50;
			NPC.DR_NERD(0.1f);
            NPC.lifeMax = 32705;
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
			NPC.buffImmune[BuffID.BetsysCurse] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
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
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.noGravity = true;
            NPC.canGhostHeal = false;
            NPC.noTileCollide = true;
            NPC.alpha = 255;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = null;
            if (CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive)
            {
                NPC.defense *= 2;
                NPC.lifeMax *= 7;
            }
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 450000;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
        }

        public override void AI()
        {
            bool provy = CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

			if (CalamityGlobalNPC.scavenger < 0 || !Main.npc[CalamityGlobalNPC.scavenger].active)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }

            if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

            float speed = 21f;
            float centerX = Main.npc[CalamityGlobalNPC.scavenger].Center.X - NPC.Center.X;
            float centerY = Main.npc[CalamityGlobalNPC.scavenger].Center.Y - NPC.Center.Y;
            centerY -= 20f;
            centerX += 1f;
            float totalSpeed = (float)Math.Sqrt(centerX * centerX + centerY * centerY);
            if (totalSpeed < 20f)
            {
                NPC.rotation = 0f;
                NPC.velocity.X = centerX;
                NPC.velocity.Y = centerY;
            }
            else
            {
                totalSpeed = speed / totalSpeed;
                NPC.velocity.X = centerX * totalSpeed;
                NPC.velocity.Y = centerY * totalSpeed;
                NPC.rotation = NPC.velocity.X * 0.1f;
            }

            if (NPC.alpha > 0)
            {
                NPC.alpha -= 10;
                if (NPC.alpha < 0)
                    NPC.alpha = 0;
            }

            NPC.ai[1] += 1f;
            if (NPC.ai[1] >= (death ? 420f : 480f))
            {
                SoundEngine.PlaySound(SoundID.Item62, NPC.position);
                NPC.TargetClosest(true);
                NPC.ai[1] = 0f;
				int type = ModContent.ProjectileType<ScavengerNuke>();
				int damage = NPC.GetProjectileDamage(type);
				if (Main.netMode != NetmodeID.MultiplayerClient)
                {
					Vector2 shootFromVector = new Vector2(NPC.Center.X, NPC.Center.Y - 20f);
					Vector2 velocity = new Vector2(0f, -15f);
                    int nuke = Projectile.NewProjectile(NPC.GetSource_FromThis(), shootFromVector, velocity, type, damage + (provy ? 30 : 0), 0f, Main.myPlayer, NPC.target, 0f);
                    Main.projectile[nuke].velocity.Y = -15f;
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
            if (NPC.life > 0)
            {
                int num285 = 0;
                while ((double)num285 < hit.Damage / (double)NPC.lifeMax * 100.0)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)hit.HitDirection, -1f, 0, default, 1f);
                    num285++;
                }
            }
            else if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.position.Y + NPC.height, ModContent.NPCType<RavagerHead2>(), NPC.whoAmI);
            }
        }
    }
}
