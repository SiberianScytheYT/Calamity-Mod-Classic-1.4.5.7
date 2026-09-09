using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.SummonItems;
using CalRD.Items.Weapons.Typeless;
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Events;
using Terraria.GameContent.Bestiary;

namespace CalRD.NPCs.ProfanedGuardians
{
    [AutoloadBossHead]
    public class ProfanedGuardianBoss : ModNPC
    {
        private int spearType = 0;
        private int dustTimer = 3;
        private int immuneTimer = 300;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Profaned Guardian");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionXOverride = 0,
                PortraitScale = 0.75f,
                Scale = 0.75f
            };
            value.Position.X += 25;
            value.Position.Y += 15;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
		
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("mr. donut one, he stabs real good")
            });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 20f;
            NPC.aiStyle = -1;
			NPC.GetNPCDamage();
			NPC.width = 100;
            NPC.height = 80;
            NPC.defense = 50;
			NPC.DR_NERD(0.4f);
            NPC.LifeMaxNERB(102500, 112500, 1650000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = -1;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Guardians");
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
            NPC.value = Item.buyPrice(0, 25, 0, 0);
            NPC.HitSound = SoundID.NPCHit52;
            NPC.DeathSound = SoundID.NPCDeath55;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(spearType);
            writer.Write(dustTimer);
            writer.Write(immuneTimer);
            writer.Write(NPC.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            spearType = reader.ReadInt32();
            dustTimer = reader.ReadInt32();
            immuneTimer = reader.ReadInt32();
            NPC.dontTakeDamage = reader.ReadBoolean();
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
            CalamityGlobalNPC.doughnutBoss = NPC.whoAmI;

            // Percent life remaining
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

			Vector2 vectorCenter = NPC.Center;
			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[1] == 0f)
            {
                NPC.localAI[1] = 1f;
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)vectorCenter.X, (int)vectorCenter.Y, ModContent.NPCType<ProfanedGuardianBoss2>());
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)vectorCenter.X, (int)vectorCenter.Y, ModContent.NPCType<ProfanedGuardianBoss3>());
            }
            
            NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (!Main.dayTime || !player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!Main.dayTime || !player.active || player.dead)
                {
					if (NPC.velocity.Y > 3f)
						NPC.velocity.Y = 3f;
					NPC.velocity.Y -= 0.1f;
					if (NPC.velocity.Y < -12f)
						NPC.velocity.Y = -12f;

					if (NPC.timeLeft > 60)
                        NPC.timeLeft = 60;

					if (NPC.ai[0] != 0f)
					{
						NPC.ai[0] = 0f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.ai[3] = 0f;
						NPC.netUpdate = true;
					}
					return;
                }
            }
            else if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

			bool isHoly = player.ZoneHallow;
			bool isHell = player.ZoneUnderworldHeight;
			bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

            // Become immune over time if target isn't in hell or hallow
            if (!isHoly && !isHell && !BossRushEvent.BossRushActive)
            {
                if (immuneTimer > 0)
                    immuneTimer--;
            }
            else
                immuneTimer = 300;

            // Take damage or not
            NPC.dontTakeDamage = immuneTimer <= 0;

			bool flag100 = false;
            for (int num569 = 0; num569 < Main.maxNPCs; num569++)
            {
                if ((Main.npc[num569].active && Main.npc[num569].type == ModContent.NPCType<ProfanedGuardianBoss2>()) || (Main.npc[num569].active && Main.npc[num569].type == ModContent.NPCType<ProfanedGuardianBoss3>()))
                {
                    flag100 = true;
                }
            }
            if (flag100)
            {
                NPC.dontTakeDamage = true;
            }
            else
            {
                NPC.dontTakeDamage = false;
            }
            if (Math.Sign(NPC.velocity.X) != 0)
            {
                NPC.spriteDirection = -Math.Sign(NPC.velocity.X);
            }
            NPC.spriteDirection = Math.Sign(NPC.velocity.X);
            float num1000 = lifeRatio < 0.75f ? 14f : 16f;
            if (revenge)
            {
                num1000 *= 0.9f;
            }
            float num1006 = 0.111111117f * num1000;
            int num1009 = (NPC.ai[0] == 2f) ? 2 : 1;
            int num1010 = (NPC.ai[0] == 2f) ? 80 : 60;
            for (int num1011 = 0; num1011 < 2; num1011++)
            {
                if (Main.rand.Next(3) < num1009)
                {
                    int num1012 = Dust.NewDust(vectorCenter - new Vector2(num1010), num1010 * 2, num1010 * 2, (int)CalamityDusts.ProfanedFire, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f, 90, default, 1.5f);
                    Main.dust[num1012].noGravity = true;
                    Main.dust[num1012].velocity *= 0.2f;
                    Main.dust[num1012].fadeIn = 1f;
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
				float shootBoost = death ? 3f * (1f - lifeRatio) : 2f * (1f - lifeRatio);
                NPC.localAI[0] += 1f + shootBoost;
                if (NPC.localAI[0] >= (BossRushEvent.BossRushActive ? 210f : 240f) && Vector2.Distance(vectorCenter, player.Center) > 160f)
                {
                    NPC.localAI[0] = 0f;

                    SoundEngine.PlaySound(SoundID.Item20, NPC.position);

                    int totalProjectiles = 10;
					float velocity = 5f;
					int type = ModContent.ProjectileType<ProfanedSpear>();
					int damage = NPC.GetProjectileDamage(type);

					switch (spearType)
                    {
                        case 0:
							break;
                        case 1:
                            totalProjectiles = 12;
                            velocity = 5.5f;
                            break;
                        case 2:
                            totalProjectiles = 8;
                            velocity = 6f;
                            break;
                        default:
                            break;
                    }

					float radians = MathHelper.TwoPi / totalProjectiles;
					for (int i = 0; i < totalProjectiles; i++)
					{
						Vector2 vector255 = new Vector2(0f, -velocity).RotatedBy(radians * i);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, type, damage, 0f, Main.myPlayer, 0f, 0f);
					}

					spearType++;
                    if (spearType > 2)
                        spearType = 0;
                }
            }
            if (NPC.ai[0] == 0f)
            {
                float scaleFactor6 = death ? 16f : 14f;
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
                flag64 = flag64 && vector126.ToRotation() > MathHelper.Pi / num1014 && vector126.ToRotation() < MathHelper.Pi - MathHelper.Pi / num1014;
                if (num1013 > 1200f || !flag64)
                {
                    NPC.velocity.X = (NPC.velocity.X * (num1000 - 1f) + vector127.X) / num1000;
                    NPC.velocity.Y = (NPC.velocity.Y * (num1000 - 1f) + vector127.Y) / num1000;
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
                    if (lifeRatio < 0.5f && dustTimer <= 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item20, NPC.position);
						int type = ModContent.ProjectileType<FlareDust>();
						int damage = NPC.GetProjectileDamage(type);
						Vector2 vector173 = Vector2.Normalize(player.Center - vectorCenter) * (NPC.width + 20) / 2f + vectorCenter;
						int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector173, Vector2.Zero, type, damage, 0f, Main.myPlayer, 2f, 0f);
                        Main.projectile[projectile].timeLeft = 120;
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
                        vec2 = new Vector2(NPC.direction, 0f);
                    }
                    NPC.velocity = (NPC.velocity * (num1000 - 1f) + vec2 * (NPC.velocity.Length() + num1006)) / num1000;
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
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
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
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
					vector41 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - screenPos;
			vector43 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
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
					color41 *= (num153 - num163) / 15f;
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
					vector44 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "A Profaned Guardian";
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Profaned Guardians have no actual drops and no treasure bag
            npcLoot.Add(ModContent.ItemType<ProfanedGuardianMask>(), 7);
            npcLoot.Add(ModContent.ItemType<ProfanedGuardianTrophy>(), 10);
            npcLoot.Add(ModContent.ItemType<RelicOfDeliverance>(), 4);
            npcLoot.Add(ModContent.ItemType<ProfanedCore>());
            npcLoot.AddConditionalPerPlayer(() => !CalamityWorld.downedGuardians, ModContent.ItemType<KnowledgeProfanedGuardians>(), 1);
            npcLoot.AddResidentEvilAmmo(info => !CalamityWorld.downedGuardians, 5, 2, 1);
        }
        
        public override void OnKill()
        {
           CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Wizard }, CalamityWorld.downedGuardians);

            // Mark the Profaned Guardians as dead
            CalamityWorld.downedGuardians = true;
            CalamityNetcode.SyncWorld();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 300, true);
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
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
                for (int k = 0; k < 50; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.ProfanedFire, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
