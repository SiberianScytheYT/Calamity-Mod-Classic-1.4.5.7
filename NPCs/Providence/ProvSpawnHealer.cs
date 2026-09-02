using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Providence
{
	public class ProvSpawnHealer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("A Profaned Guardian");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

        public override void SetDefaults()
        {
            NPC.npcSlots = 1f;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 100;
            NPC.height = 80;
            NPC.defense = 35;
			NPC.DR_NERD(0.15f);
            NPC.lifeMax = 40000;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 400000;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
            NPC.buffImmune[BuffID.BetsysCurse] = false;
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.HitSound = SoundID.NPCHit52;
            NPC.DeathSound = SoundID.NPCDeath55;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            CalamityGlobalNPC.holyBossHealer = NPC.whoAmI;
            bool expertMode = Main.expertMode;
            Vector2 vectorCenter = NPC.Center;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(false);
            if (CalamityGlobalNPC.holyBoss < 0 || !Main.npc[CalamityGlobalNPC.holyBoss].active)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }
            NPC.dontTakeDamage = Main.npc[CalamityGlobalNPC.holyBoss].dontTakeDamage;

            if (Math.Sign(NPC.velocity.X) != 0)
            {
                NPC.spriteDirection = -Math.Sign(NPC.velocity.X);
            }
            NPC.spriteDirection = Math.Sign(NPC.velocity.X);
            int num1009 = (NPC.ai[0] == 0f) ? 1 : 2;
            int num1010 = (NPC.ai[0] == 0f) ? 60 : 80;
            for (int num1011 = 0; num1011 < 2; num1011++)
            {
                if (Main.rand.Next(3) < num1009)
                {
                    int dustType = Main.rand.Next(2);
                    if (dustType == 0)
                    {
                        dustType = (int)CalamityDusts.ProfanedFire;
                    }
                    else
                    {
                        dustType = 107;
                    }
                    int num1012 = Dust.NewDust(NPC.Center - new Vector2((float)num1010), num1010 * 2, num1010 * 2, dustType, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f, 90, default, 0.5f);
                    Main.dust[num1012].noGravity = true;
                    Main.dust[num1012].velocity *= 0.2f;
                    Main.dust[num1012].fadeIn = 1f;
                }
            }
            if (NPC.ai[0] == 0f)
            {
                Vector2 vector96 = new Vector2(NPC.Center.X, NPC.Center.Y);
                float num784 = Main.npc[CalamityGlobalNPC.holyBoss].Center.X - vector96.X;
                float num785 = Main.npc[CalamityGlobalNPC.holyBoss].Center.Y - vector96.Y;
                float num786 = (float)Math.Sqrt((double)(num784 * num784 + num785 * num785));
                if (num786 > 360f)
                {
                    num786 = 8f / num786; //8f
                    num784 *= num786;
                    num785 *= num786;
                    NPC.velocity.X = (NPC.velocity.X * 15f + num784) / 16f;
                    NPC.velocity.Y = (NPC.velocity.Y * 15f + num785) / 16f;
                    return;
                }
                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < 8f) //8f
                {
                    NPC.velocity *= 1.05f; //1.05f
                }
                if (Main.netMode != NetmodeID.MultiplayerClient && ((expertMode && Main.rand.NextBool(2000)) || Main.rand.NextBool(1000)))
                {
                    NPC.TargetClosest(true);
                    vector96 = new Vector2(NPC.Center.X, NPC.Center.Y);
                    num784 = player.Center.X - vector96.X;
                    num785 = player.Center.Y - vector96.Y;
                    num786 = (float)Math.Sqrt((double)(num784 * num784 + num785 * num785));
                    num786 = 24f / num786; //8f
                    NPC.velocity.X = num784 * num786;
                    NPC.velocity.Y = num785 * num786;
                    NPC.ai[0] = 1f;
                    NPC.netUpdate = true;
                }
            }
            else
            {
                if (expertMode)
                {
                    Vector2 value4 = player.Center - NPC.Center;
                    value4.Normalize();
                    value4 *= 9f; //9f
                    NPC.velocity = (NPC.velocity * 99f + value4) / 99f; //100
                }
                Vector2 vector97 = new Vector2(NPC.Center.X, NPC.Center.Y);
                float num787 = Main.npc[CalamityGlobalNPC.holyBoss].Center.X - vector97.X;
                float num788 = Main.npc[CalamityGlobalNPC.holyBoss].Center.Y - vector97.Y;
                float num789 = (float)Math.Sqrt((double)(num787 * num787 + num788 * num788));
                if (num789 > 700f || NPC.justHit)
                {
                    NPC.ai[0] = 0f;
                }
            }
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Texture2D texture2D16 = ModContent.Request<Texture2D>("CalRD/NPCs/ProfanedGuardians/ProfanedGuardianBoss3Glow2").Value;
			Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 5;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (float)(num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/ProfanedGuardians/ProfanedGuardianBoss3Glow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Yellow, 0.5f);
			Color color42 = Color.Lerp(Color.White, Color.Violet, 0.5f);

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num163 = 1; num163 < num153; num163++)
				{
					Color color41 = color37;
					color41 = Color.Lerp(color41, color36, amount9);
					color41 = NPC.GetAlpha(color41);
					color41 *= (float)(num153 - num163) / 15f;
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - Main.screenPosition;
					vector44 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

					Color color43 = color42;
					color43 = Color.Lerp(color43, color36, amount9);
					color43 = NPC.GetAlpha(color43);
					color43 *= (float)(num153 - num163) / 15f;
					spriteBatch.Draw(texture2D16, vector44, NPC.frame, color43, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			spriteBatch.Draw(texture2D16, vector43, NPC.frame, color42, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool CheckActive()
        {
            return false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire, 600, true);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.ProfanedFire, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
	            if (Main.netMode != NetmodeID.Server)
	            {
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ProfanedGuardianBossH").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ProfanedGuardianBossH2").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ProfanedGuardianBossH3").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ProfanedGuardianBossH4").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ProfanedGuardianBossH5").Type, 1f);
                }
				for (int k = 0; k < 50; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.ProfanedFire, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
