using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Providence
{
	public class ProvSpawnOffense : ModNPC
    {
        public int dustTimer = 3;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("A Profaned Guardian");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

        public override void SetDefaults()
        {
            NPC.npcSlots = 1f;
            NPC.aiStyle = -1;
			NPC.GetNPCDamage();
			NPC.width = 100;
            NPC.height = 80;
            NPC.defense = 40;
			NPC.DR_NERD(0.3f);
            NPC.lifeMax = 42500;
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
            CalamityGlobalNPC.holyBossAttacker = NPC.whoAmI;
            bool fireDust = (double)NPC.life <= (double)NPC.lifeMax * 0.5;
            bool expertMode = Main.expertMode;
            Vector2 vectorCenter = NPC.Center;
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
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
            float num1005 = 20f;
            float num1006 = 0.111111117f * num1005;
            int num1009 = (NPC.ai[0] == 2f) ? 2 : 1;
            int num1010 = (NPC.ai[0] == 2f) ? 80 : 60;
            for (int num1011 = 0; num1011 < 2; num1011++)
            {
                if (Main.rand.Next(3) < num1009)
                {
                    int num1012 = Dust.NewDust(vectorCenter - new Vector2((float)num1010), num1010 * 2, num1010 * 2, (int)CalamityDusts.ProfanedFire, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f, 90, default, 0.5f);
                    Main.dust[num1012].noGravity = true;
                    Main.dust[num1012].velocity *= 0.2f;
                    Main.dust[num1012].fadeIn = 1f;
                }
            }
            if (NPC.ai[0] == 0f)
            {
                float scaleFactor6 = 10f;
                Vector2 center5 = player.Center;
                Vector2 vector126 = center5 - vectorCenter;
                Vector2 vector127 = vector126 - Vector2.UnitY * 300f;
                float num1013 = vector126.Length();
                vector126 = Vector2.Normalize(vector126) * scaleFactor6;
                vector127 = Vector2.Normalize(vector127) * scaleFactor6;
                bool flag64 = Collision.CanHit(vectorCenter, 1, 1, player.Center, 1, 1);
                if (NPC.ai[3] >= 120f)
                {
                    flag64 = true;
                }
                float num1014 = 8f;
                flag64 = flag64 && vector126.ToRotation() > 3.14159274f / num1014 && vector126.ToRotation() < 3.14159274f - 3.14159274f / num1014;
                if (num1013 > 800f || !flag64)
                {
                    NPC.velocity.X = (NPC.velocity.X * (num1005 - 1f) + vector127.X) / num1005;
                    NPC.velocity.Y = (NPC.velocity.Y * (num1005 - 1f) + vector127.Y) / num1005;
                    if (!flag64)
                    {
                        NPC.ai[3] += 1f;
                        if (NPC.ai[3] == 120f)
                        {
                            NPC.netUpdate = true;
                        }
                    }
                    else
                    {
                        NPC.ai[3] = 0f;
                    }
                }
                else
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[2] = vector126.X;
                    NPC.ai[3] = vector126.Y;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                NPC.velocity *= 0.8f;
                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= 5f)
                {
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                    Vector2 velocity = new Vector2(NPC.ai[2], NPC.ai[3]);
                    velocity.Normalize();
                    velocity *= 10f;
                    NPC.velocity = velocity;
                }
            }
            else if (NPC.ai[0] == 2f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    dustTimer--;
                    if (fireDust && dustTimer <= 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item20, NPC.position);
						int type = ModContent.ProjectileType<FlareDust>();
						int damage = NPC.GetProjectileDamage(type);
                        Vector2 vector173 = Vector2.Normalize(player.Center - vectorCenter) * (float)(NPC.width + 20) / 2f + vectorCenter;
                        int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), (int)vector173.X, (int)vector173.Y, (float)(NPC.direction * 2), 4f, type, damage, 0f, Main.myPlayer, 2f, 0f);
                        Main.projectile[projectile].timeLeft = 120;
                        Main.projectile[projectile].velocity.X = 0f;
                        Main.projectile[projectile].velocity.Y = 0f;
                        dustTimer = 3;
                    }
                }
                NPC.ai[1] += 1f;
                bool flag65 = vectorCenter.Y + 50f > player.Center.Y;
                if ((NPC.ai[1] >= 90f && flag65) || NPC.velocity.Length() < 8f)
                {
                    NPC.ai[0] = 3f;
                    NPC.ai[1] = 45f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    NPC.velocity /= 2f;
                    NPC.netUpdate = true;
                }
                else
                {
                    Vector2 center7 = player.Center;
                    Vector2 vec2 = center7 - vectorCenter;
                    vec2.Normalize();
                    if (vec2.HasNaNs())
                    {
                        vec2 = new Vector2((float)NPC.direction, 0f);
                    }
                    NPC.velocity = (NPC.velocity * (num1005 - 1f) + vec2 * (NPC.velocity.Length() + num1006)) / num1005;
                }
            }
            else if (NPC.ai[0] == 3f)
            {
                NPC.ai[1] -= 1f;
                if (NPC.ai[1] <= 0f)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                }
                NPC.velocity *= 0.98f;
            }
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 5;
			if (NPC.ai[0] == 2f)
				num153 = 10;

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

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/ProfanedGuardians/ProfanedGuardianBossGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Yellow, 0.5f);

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
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

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
			target.AddBuff(ModContent.BuffType<HolyFlames>(), 300, true);
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
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ProfanedGuardianBossA").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ProfanedGuardianBossA2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ProfanedGuardianBossA3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ProfanedGuardianBossA4").Type, 1f);
                }
                for (int k = 0; k < 30; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.ProfanedFire, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
