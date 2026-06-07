using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.NPCs.TownNPCs;
using CalRD.Projectiles.Boss;
using CalRD.Tiles.Ores;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalRD.NPCs.Cryogen
{
    [AutoloadBossHead]
    public class Cryogen : ModNPC
    {
        private int time = 0;
        private int iceShard = 0;
        private int currentPhase = 1;
        private int teleportLocationX = 0;

        public override string Texture => "CalRD/NPCs/Cryogen/Cryogen_Phase1";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryogen");
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 24f;
			NPC.GetNPCDamage();
			NPC.width = 86;
            NPC.height = 88;
            NPC.defense = 12;
			NPC.DR_NERD(0.1f);
            NPC.LifeMaxNERB(17900, 26300, 3000000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 12, 0, 0);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[ModContent.BuffType<MarkedforDeath>()] = false;
            NPC.buffImmune[BuffID.OnFire] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.buffImmune[BuffID.Daybreak] = false;
			NPC.buffImmune[BuffID.BetsysCurse] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
			NPC.buffImmune[BuffID.BoneJavelin] = false;
			NPC.buffImmune[BuffID.ShadowFlame] = false;
			NPC.buffImmune[BuffID.SoulDrain] = false;
			NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.buffImmune[ModContent.BuffType<SulphuricPoisoning>()] = false;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath15;
            Mod CalamityModMusic = ModLoader.HasMod("CalamityModMusic") ? ModLoader.GetMod("CalamityModMusic") : null;
            if (CalamityModMusic != null)
                Music = MusicLoader.GetMusicSlot("CalamityModMusic/Sounds/Music/Cryogen");
            else
                Music = MusicID.FrostMoon; 
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(time);
            writer.Write(iceShard);
            writer.Write(teleportLocationX);
            writer.Write(NPC.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            time = reader.ReadInt32();
            iceShard = reader.ReadInt32();
            teleportLocationX = reader.ReadInt32();
            NPC.dontTakeDamage = reader.ReadBoolean();
        }

        public override void AI()
        {
            Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0f, 1f, 1f);

			// Get a target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			Player player = Main.player[NPC.target];

			bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

			float enrageScale = 0f;
			if (!player.ZoneSnow)
				enrageScale += 2f;

			if (BossRushEvent.BossRushActive)
				enrageScale = 0f;

			// Percent life remaining
			float lifeRatio = NPC.life / (float)NPC.lifeMax;

			// Phases
			bool phase2 = lifeRatio < 0.85f;
			bool phase3 = lifeRatio < 0.7f;
			bool phase4 = lifeRatio < 0.55f;
			bool phase5 = lifeRatio < 0.4f;
			bool phase6 = lifeRatio < (death ? 0.3f : revenge ? 0.25f : expertMode ? 0.2f : 0.15f);
			bool phase7 = lifeRatio < (death ? 0.15f : 0.1f) && revenge;
			bool phase8 = lifeRatio < (revenge ? 0.075f : 0.05f);
			bool phase9 = lifeRatio < (revenge ? 0.05f : 0.025f) || death;

			if ((int)NPC.ai[0] + 1 > currentPhase && currentPhase < 6)
            {
                HandlePhaseTransition((int)NPC.ai[0] + 1);
            }

			if (NPC.ai[2] == 0f && NPC.localAI[1] == 0f && Main.netMode != NetmodeID.MultiplayerClient && (NPC.ai[0] < 4f || BossRushEvent.BossRushActive)) //spawn shield for phase 0 1 2 3, not 4 5 6
            {
                int num6 = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CryogenIce>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                NPC.ai[2] = num6 + 1;
                NPC.localAI[1] = -1f;
                NPC.netUpdate = true;
                Main.npc[num6].ai[0] = NPC.whoAmI;
                Main.npc[num6].netUpdate = true;
            }

            int num7 = (int)NPC.ai[2] - 1;
            if (num7 != -1 && Main.npc[num7].active && Main.npc[num7].type == ModContent.NPCType<CryogenIce>())
            {
                NPC.dontTakeDamage = true;
            }
            else
            {
                NPC.dontTakeDamage = false;
                NPC.ai[2] = 0f;
                if (NPC.localAI[1] == -1f)
                {
                    NPC.localAI[1] = expertMode ? 720f : 1080f;
                }
                if (NPC.localAI[1] > 0f)
                {
                    NPC.localAI[1] -= 1f;
                }
            }

            if (NPC.ai[0] != 5f)
            {
                NPC.rotation = NPC.velocity.X * 0.1f;
            }

			CalRD.StopRain();

			if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
					if (NPC.velocity.Y > 3f)
						NPC.velocity.Y = 3f;
					NPC.velocity.Y -= 0.1f;
					if (NPC.velocity.Y < -12f)
						NPC.velocity.Y = -12f;

					if (NPC.timeLeft > 60)
                        NPC.timeLeft = 60;

					if (NPC.ai[1] != 0f)
					{
						NPC.ai[1] = 0f;
						teleportLocationX = 0;
						iceShard = 0;
						NPC.netUpdate = true;
					}
					return;
                }
            }
            else if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

            if (Main.netMode != NetmodeID.MultiplayerClient && expertMode && (NPC.ai[0] < 5f || !phase7))
            {
                time++;
                if (time >= 600)
                {
					int totalProjectiles = 4;
					float radians = MathHelper.TwoPi / totalProjectiles;
					int type = ModContent.ProjectileType<IceBomb>();
					int damage = NPC.GetProjectileDamage(type);
					float velocity = BossRushEvent.BossRushActive ? 12f : 4f;
					Vector2 spinningPoint = Main.rand.NextBool(2) ? new Vector2(0f, -velocity) : Vector2.Normalize(new Vector2(-velocity, -velocity)) * velocity;
					for (int k = 0; k < totalProjectiles; k++)
					{
						Vector2 vector255 = spinningPoint.RotatedBy(radians * k);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, type, damage, 0f, Main.myPlayer, 0f, 0f);
					}
                    time = 0;
                }
            }

            if (NPC.ai[0] == 0f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] >= 120f)
                    {
                        NPC.localAI[0] = 0f;
                        NPC.netUpdate = true;
                        if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                        {
							int totalProjectiles = 16;
							float radians = MathHelper.TwoPi / totalProjectiles;
							int type = ModContent.ProjectileType<IceBlast>();
							int damage = NPC.GetProjectileDamage(type);
							float velocity = BossRushEvent.BossRushActive ? 12f : 8f;
							for (int k = 0; k < totalProjectiles; k++)
							{
								Vector2 vector255 = new Vector2(0f, -velocity).RotatedBy(radians * k);
								Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, type, damage, 0f, Main.myPlayer, 0f, 0f);
							}
                        }
                    }
                }

                Vector2 vector142 = new Vector2(NPC.Center.X, NPC.Center.Y);
                float num1243 = player.Center.X - vector142.X;
                float num1244 = player.Center.Y - vector142.Y;
                float num1245 = (float)Math.Sqrt(num1243 * num1243 + num1244 * num1244);
                float num1246 = death ? 5f : 4f;
				num1246 += 2f * enrageScale;
                if (BossRushEvent.BossRushActive)
                {
                    num1246 = 14f;
                }
                num1245 = num1246 / num1245;
                num1243 *= num1245;
                num1244 *= num1245;
                NPC.velocity.X = (NPC.velocity.X * 50f + num1243) / 51f;
                NPC.velocity.Y = (NPC.velocity.Y * 50f + num1244) / 51f;

                if (phase2)
                {
					NPC.TargetClosest(true);
					NPC.ai[0] = 1f;
                    NPC.localAI[0] = 0f;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] >= 120f)
                    {
                        NPC.localAI[0] = 0f;
                        if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                        {
							int totalProjectiles = 12;
							float radians = MathHelper.TwoPi / totalProjectiles;
							int type = ModContent.ProjectileType<IceBlast>();
							int damage = NPC.GetProjectileDamage(type);
							float velocity2 = BossRushEvent.BossRushActive ? 12f : 8f;
							for (int k = 0; k < totalProjectiles; k++)
							{
								Vector2 vector255 = new Vector2(0f, -velocity2).RotatedBy(radians * k);
								Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, type, damage, 0f, Main.myPlayer, 0f, 0f);
							}
                        }
                    }
                }

                float velocity = death ? 3.5f : 4f;
                float acceleration = 0.15f;
				velocity -= enrageScale;
				acceleration += 0.07f * enrageScale;
				if (BossRushEvent.BossRushActive)
				{
					velocity = 3f;
					acceleration *= 1.5f;
				}

				if (NPC.position.Y > player.position.Y - 375f)
				{
					if (NPC.velocity.Y > 0f)
						NPC.velocity.Y *= 0.98f;

					NPC.velocity.Y -= acceleration;

					if (NPC.velocity.Y > velocity)
						NPC.velocity.Y = velocity;
				}
				else if (NPC.position.Y < player.position.Y - 425f)
				{
					if (NPC.velocity.Y < 0f)
						NPC.velocity.Y *= 0.98f;

					NPC.velocity.Y += acceleration;

					if (NPC.velocity.Y < -velocity)
						NPC.velocity.Y = -velocity;
				}

				if (NPC.position.X + (NPC.width / 2) > player.position.X + (player.width / 2) + 300f)
				{
					if (NPC.velocity.X > 0f)
						NPC.velocity.X *= 0.98f;

					NPC.velocity.X -= acceleration;

					if (NPC.velocity.X > velocity)
						NPC.velocity.X = velocity;
				}
				if (NPC.position.X + (NPC.width / 2) < player.position.X + (player.width / 2) - 300f)
				{
					if (NPC.velocity.X < 0f)
						NPC.velocity.X *= 0.98f;

					NPC.velocity.X += acceleration;

					if (NPC.velocity.X < -velocity)
						NPC.velocity.X = -velocity;
				}

				if (phase3)
                {
					NPC.TargetClosest(true);
					NPC.ai[0] = 2f;
                    NPC.localAI[0] = 0f;
                    iceShard = 0;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 2f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] >= 120f)
                    {
                        NPC.localAI[0] = 0f;
                        NPC.netUpdate = true;
                        if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                        {
                            if (Main.rand.NextBool(2))
                            {
								int totalProjectiles = 12;
								float radians = MathHelper.TwoPi / totalProjectiles;
								int type = ModContent.ProjectileType<IceBlast>();
								int damage = NPC.GetProjectileDamage(type);
								float velocity = BossRushEvent.BossRushActive ? 14f : 9f;
								for (int k = 0; k < totalProjectiles; k++)
								{
									Vector2 vector255 = new Vector2(0f, -velocity).RotatedBy(radians * k);
									Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, type, damage, 0f, Main.myPlayer, 0f, 0f);
								}
                            }
                            else
                            {
                                float num179 = revenge ? 9f : 7f;
                                if (BossRushEvent.BossRushActive)
                                    num179 = 14f;

                                Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                                float num180 = player.position.X + player.width * 0.5f - value9.X;
                                float num181 = Math.Abs(num180) * 0.1f;
                                float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
                                float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
                                num183 = num179 / num183;
                                num180 *= num183;
                                num182 *= num183;
								int type = ModContent.ProjectileType<IceRain>();
								int damage = NPC.GetProjectileDamage(type);
								value9.X += num180;
                                value9.Y += num182;
                                for (int num186 = 0; num186 < 6; num186++)
                                {
                                    num180 = player.position.X + player.width * 0.5f - value9.X;
                                    num182 = player.position.Y + player.height * 0.5f - value9.Y;
                                    num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
                                    num183 = num179 / num183;
                                    num180 += Main.rand.Next(-360, 361);
                                    num182 += Main.rand.Next(-360, 361);
                                    num180 *= num183;
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), value9.X, value9.Y, num180, -8f, type, damage, 0f, Main.myPlayer, 0f, 0f);
                                }
                            }
                        }
                    }
                }

                Vector2 vector142 = new Vector2(NPC.Center.X, NPC.Center.Y);
                float num1243 = player.Center.X - vector142.X;
                float num1244 = player.Center.Y - vector142.Y;
                float num1245 = (float)Math.Sqrt(num1243 * num1243 + num1244 * num1244);
                float num1246 = death ? 7f : 6f;
				num1246 += 2f * enrageScale;
				if (BossRushEvent.BossRushActive)
                {
                    num1246 = 20f;
                }
                num1245 = num1246 / num1245;
                num1243 *= num1245;
                num1244 *= num1245;
                NPC.velocity.X = (NPC.velocity.X * 50f + num1243) / 51f;
                NPC.velocity.Y = (NPC.velocity.Y * 50f + num1244) / 51f;

				if (phase4)
                {
					NPC.TargetClosest(true);
					NPC.ai[0] = 3f;
                    NPC.localAI[0] = 0f;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 3f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] >= 120f)
                    {
                        NPC.localAI[0] = 0f;
                        if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                        {
                            float num179 = revenge ? 9f : 7f;
                            if (BossRushEvent.BossRushActive)
                                num179 = 14f;

                            Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                            float num180 = player.position.X + player.width * 0.5f - value9.X;
                            float num181 = Math.Abs(num180) * 0.1f;
                            float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
                            float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
                            NPC.netUpdate = true;
                            num183 = num179 / num183;
                            num180 *= num183;
                            num182 *= num183;
							int type = ModContent.ProjectileType<IceRain>();
							int damage = NPC.GetProjectileDamage(type);
							value9.X += num180;
                            value9.Y += num182;
                            for (int num186 = 0; num186 < 12; num186++)
                            {
                                num180 = player.position.X + player.width * 0.5f - value9.X;
                                num182 = player.position.Y + player.height * 0.5f - value9.Y;
                                num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
                                num183 = num179 / num183;
                                num180 += Main.rand.Next(-360, 361);
                                num182 += Main.rand.Next(-360, 361);
                                num180 *= num183;
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), value9.X, value9.Y, num180, -2.5f, type, damage, 0f, Main.myPlayer, 1f, 0f);
                            }
                        }
                    }
                }

				float velocity = death ? 3.5f : 4f;
				float acceleration = 0.15f;
				velocity -= enrageScale;
				acceleration += 0.07f * enrageScale;
				if (BossRushEvent.BossRushActive)
				{
					velocity = 3f;
					acceleration *= 1.5f;
				}

				if (NPC.position.Y > player.position.Y - 375f)
				{
					if (NPC.velocity.Y > 0f)
						NPC.velocity.Y *= 0.98f;

					NPC.velocity.Y -= acceleration;

					if (NPC.velocity.Y > velocity)
						NPC.velocity.Y = velocity;
				}
				else if (NPC.position.Y < player.position.Y - 425f)
				{
					if (NPC.velocity.Y < 0f)
						NPC.velocity.Y *= 0.98f;

					NPC.velocity.Y += acceleration;

					if (NPC.velocity.Y < -velocity)
						NPC.velocity.Y = -velocity;
				}

				if (NPC.position.X + (NPC.width / 2) > player.position.X + (player.width / 2) + 300f)
				{
					if (NPC.velocity.X > 0f)
						NPC.velocity.X *= 0.98f;

					NPC.velocity.X -= acceleration;

					if (NPC.velocity.X > velocity)
						NPC.velocity.X = velocity;
				}
				if (NPC.position.X + (NPC.width / 2) < player.position.X + (player.width / 2) - 300f)
				{
					if (NPC.velocity.X < 0f)
						NPC.velocity.X *= 0.98f;

					NPC.velocity.X += acceleration;

					if (NPC.velocity.X < -velocity)
						NPC.velocity.X = -velocity;
				}

				if (phase5)
                {
					NPC.TargetClosest(true);
					NPC.ai[0] = 4f;
                    NPC.localAI[0] = 0f;
                    iceShard = 0;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 4f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] >= 60f && NPC.Opacity == 1f)
                    {
                        NPC.localAI[0] = 0f;
                        if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                        {
							int totalProjectiles = 12;
							float radians = MathHelper.TwoPi / totalProjectiles;
							int type = ModContent.ProjectileType<IceBlast>();
							int damage = NPC.GetProjectileDamage(type);
							float velocity = BossRushEvent.BossRushActive ? 14f : 9f;
							for (int k = 0; k < totalProjectiles; k++)
							{
								Vector2 vector255 = new Vector2(0f, -velocity).RotatedBy(radians * k);
								Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, type, damage, 0f, Main.myPlayer, 0f, 0f);
							}
                        }
                    }
                }

                Vector2 vector142 = new Vector2(NPC.Center.X, NPC.Center.Y);
                float num1243 = player.Center.X - vector142.X;
                float num1244 = player.Center.Y - vector142.Y;
                float num1245 = (float)Math.Sqrt(num1243 * num1243 + num1244 * num1244);
                float speed = revenge ? 5.5f : 5f;
                if (BossRushEvent.BossRushActive)
                {
                    speed = 10f;
                }
                num1245 = speed / num1245;
                num1243 *= num1245;
                num1244 *= num1245;
                NPC.velocity.X = (NPC.velocity.X * 50f + num1243) / 51f;
                NPC.velocity.Y = (NPC.velocity.Y * 50f + num1244) / 51f;

                if (NPC.ai[1] == 0f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.localAI[2] += 1f;
                        if (NPC.localAI[2] >= (BossRushEvent.BossRushActive ? 90f : 180f))
                        {
							NPC.TargetClosest(true);
							NPC.localAI[2] = 0f;
                            int num1249 = 0;
                            int num1250;
                            int num1251;
                            while (true)
                            {
                                num1249++;
                                num1250 = (int)player.Center.X / 16;
                                num1251 = (int)player.Center.Y / 16;

                                int min = 16;
                                int max = 20;

                                if (Main.rand.NextBool(2))
                                    num1250 += Main.rand.Next(min, max);
                                else
                                    num1250 -= Main.rand.Next(min, max);

                                if (Main.rand.NextBool(2))
                                    num1251 += Main.rand.Next(min, max);
                                else
                                    num1251 -= Main.rand.Next(min, max);

                                if (!WorldGen.SolidTile(num1250, num1251) && Collision.CanHit(new Vector2(num1250 * 16, num1251 * 16), 1, 1, player.position, player.width, player.height))
                                    break;

                                if (num1249 > 100)
                                    goto Block;
                            }
                            NPC.ai[1] = 1f;
                            teleportLocationX = num1250;
                            iceShard = num1251;
                            NPC.netUpdate = true;
                            Block:
                            ;
                        }
                    }
                }
                else if (NPC.ai[1] == 1f)
                {
                    Vector2 position = new Vector2(teleportLocationX * 16f - (NPC.width / 2), iceShard * 16f - (NPC.height / 2));
                    for (int m = 0; m < 5; m++)
                    {
                        int dust = Dust.NewDust(position, NPC.width, NPC.height, 67, 0f, 0f, 100, default, 2f);
                        Main.dust[dust].noGravity = true;
                    }

                    NPC.Opacity -= 0.008f;
                    if (NPC.Opacity <= 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                        NPC.Opacity = 0f;
                        NPC.position = position;

                        for (int n = 0; n < 15; n++)
                        {
                            int num39 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 67, 0f, 0f, 100, default, 3f);
                            Main.dust[num39].noGravity = true;
                        }

                        NPC.ai[1] = 2f;
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[1] == 2f)
                {
                    NPC.Opacity += 0.2f;
                    if (NPC.Opacity >= 1f)
                    {
                        NPC.Opacity = 1f;
                        NPC.ai[1] = 0f;
                        NPC.netUpdate = true;
                    }
                }

				if (phase6)
                {
                    for (int num621 = 0; num621 < 40; num621++)
                    {
                        int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 67, 0f, 0f, 100, default, 2f);
                        Main.dust[num622].velocity *= 3f;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num622].scale = 0.5f;
                            Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                        }
                    }
                    for (int num623 = 0; num623 < 70; num623++)
                    {
                        int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 67, 0f, 0f, 100, default, 3f);
                        Main.dust[num624].noGravity = true;
                        Main.dust[num624].velocity *= 5f;
                        num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 67, 0f, 0f, 100, default, 2f);
                        Main.dust[num624].velocity *= 2f;
                    }
                    // these gores don't exist anymore
                    /*
                    if (Main.netMode != NetmodeID.Server)
                    {
	                    float randomSpread = Main.rand.Next(-200, 200) / 100;
	                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CryoGore1").Type, 1f);
	                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CryoGore2").Type, 1f);
	                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CryoGore3").Type, 1f);
                    }
                    */
					NPC.TargetClosest(true);
					NPC.ai[0] = 5f;
                    NPC.ai[1] = 0f;
                    NPC.localAI[0] = 0f;
                    NPC.localAI[2] = 0f;
					NPC.Opacity = 1f;
                    teleportLocationX = 0;
                    iceShard = 0;
                    NPC.netUpdate = true;

					int chance = 100;
					if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1)
					{
						chance = 20;
					}
					if (Main.rand.NextBool(chance))
					{
						string key = "Cryogen is derping out!";
						Color messageColor = Color.Cyan;
                        CalamityUtils.DisplayLocalizedText(key, messageColor);
                    }
                }
            }
			else if (NPC.ai[0] == 5f)
			{
                NPC.damage = NPC.defDamage;

				if (phase7)
				{
					if (NPC.ai[1] == 60f)
						NPC.velocity = Vector2.Normalize(player.Center - NPC.Center) * (BossRushEvent.BossRushActive ? 30f : 18f + enrageScale * 2f);

					NPC.ai[1] -= 1f;
					if (NPC.ai[1] <= 0f)
					{
						NPC.ai[3] += 1f;
						if (NPC.ai[3] > 2f)
						{
							NPC.TargetClosest(true);
							NPC.damage = 0;
							NPC.defense = NPC.defDefense + 8;
							NPC.ai[0] = 6f;
							NPC.ai[1] = 0f;
							NPC.ai[3] = 0f;
							time = 0;
						}
						else
							NPC.ai[1] = 60f;

						NPC.rotation = NPC.velocity.X * 0.1f;
					}
					else if (NPC.ai[1] <= 15f)
					{
						NPC.velocity *= 0.95f;
						NPC.rotation = NPC.velocity.X * 0.15f;
					}
					else
						NPC.rotation += NPC.direction * 0.5f;

					return;
				}

				float num1372 = 16f + enrageScale * 2f;
				if (BossRushEvent.BossRushActive)
                {
                    num1372 = 32f;
                }
                Vector2 vector167 = new Vector2(NPC.Center.X + (NPC.direction * 20), NPC.Center.Y + 6f);
                float num1373 = player.position.X + player.width * 0.5f - vector167.X;
                float num1374 = player.Center.Y - vector167.Y;
                float num1375 = (float)Math.Sqrt(num1373 * num1373 + num1374 * num1374);
                float num1376 = num1372 / num1375;
                num1373 *= num1376;
                num1374 *= num1376;
                iceShard--;

				if (phase9)
                {
                    if (num1375 < 170f || iceShard > 0)
                    {
                        if (num1375 < 170f)
                        {
                            iceShard = 17;
                        }
                        NPC.rotation += NPC.direction * 0.5f;
                        return;
                    }
                }
                else if (phase8)
                {
                    if (num1375 < 190f || iceShard > 0)
                    {
                        if (num1375 < 190f)
                        {
                            iceShard = 19;
                        }
                        NPC.rotation += NPC.direction * 0.35f;
                        return;
                    }
                }
                else
                {
                    if (num1375 < 200f || iceShard > 0)
                    {
                        if (num1375 < 200f)
                        {
                            iceShard = 20;
                        }
                        NPC.rotation += NPC.direction * 0.3f;
                        return;
                    }
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
                NPC.rotation = NPC.velocity.X * 0.15f;
            }
			else
			{
				time++;
				if (time >= 75)
				{
					int totalProjectiles = 4;
					float radians = MathHelper.TwoPi / totalProjectiles;
					int type = ModContent.ProjectileType<IceBomb>();
					int damage = NPC.GetProjectileDamage(type);
					float velocity2 = BossRushEvent.BossRushActive ? 16f : 6f;
					Vector2 spinningPoint = Main.rand.NextBool(2) ? new Vector2(0f, -velocity2) : Vector2.Normalize(new Vector2(-velocity2, -velocity2)) * velocity2;
					for (int k = 0; k < totalProjectiles; k++)
					{
						Vector2 vector255 = spinningPoint.RotatedBy(radians * k);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, type, damage, 0f, Main.myPlayer, 0f, 0f);
					}
					time = 0;
				}

				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 180f)
				{
					NPC.TargetClosest(true);
					NPC.ai[0] = 5f;
					NPC.ai[1] = 60f;
					time = 0;
					iceShard = 0;
					NPC.netUpdate = true;
				}

				float velocity = death ? 5f : 6f;
				float acceleration = 0.2f;
				velocity -= enrageScale;
				acceleration += 0.07f * enrageScale;
				if (BossRushEvent.BossRushActive)
				{
					velocity = 2f;
					acceleration *= 1.5f;
				}

				if (NPC.position.Y > player.position.Y - 375f)
				{
					if (NPC.velocity.Y > 0f)
						NPC.velocity.Y *= 0.98f;

					NPC.velocity.Y -= acceleration;

					if (NPC.velocity.Y > velocity)
						NPC.velocity.Y = velocity;
				}
				else if (NPC.position.Y < player.position.Y - 400f)
				{
					if (NPC.velocity.Y < 0f)
						NPC.velocity.Y *= 0.98f;

					NPC.velocity.Y += acceleration;

					if (NPC.velocity.Y < -velocity)
						NPC.velocity.Y = -velocity;
				}

				if (NPC.position.X + (NPC.width / 2) > player.position.X + (player.width / 2) + 350f)
				{
					if (NPC.velocity.X > 0f)
						NPC.velocity.X *= 0.98f;

					NPC.velocity.X -= acceleration;

					if (NPC.velocity.X > velocity)
						NPC.velocity.X = velocity;
				}
				if (NPC.position.X + (NPC.width / 2) < player.position.X + (player.width / 2) - 350f)
				{
					if (NPC.velocity.X < 0f)
						NPC.velocity.X *= 0.98f;

					NPC.velocity.X += acceleration;

					if (NPC.velocity.X < -velocity)
						NPC.velocity.X = -velocity;
				}
			}

			if (!phase7)
			{
				if (NPC.ai[3] == 0f && NPC.life > 0)
				{
					NPC.ai[3] = NPC.lifeMax;
				}
				if (NPC.life > 0)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num660 = (int)(NPC.lifeMax * 0.075);
						if ((NPC.life + num660) < NPC.ai[3])
						{
							NPC.ai[3] = NPC.life;
							for (int num662 = 0; num662 < 2; num662++)
							{
								int x = (int)(NPC.position.X + Main.rand.Next(NPC.width - 32));
								int y = (int)(NPC.position.Y + Main.rand.Next(NPC.height - 32));
								int random = 1;
								switch ((int)NPC.ai[0])
								{
									case 0:
									case 1:
										break;
									case 2:
									case 3:
										random = 2;
										break;
									case 4:
									case 5:
										random = 3;
										break;
									default:
										break;
								}
								int randomSpawn = Main.rand.Next(random);
								if (randomSpawn == 0)
								{
									randomSpawn = ModContent.NPCType<Cryocore>();
								}
								else if (randomSpawn == 1)
								{
									randomSpawn = ModContent.NPCType<IceMass>();
								}
								else
								{
									randomSpawn = ModContent.NPCType<Cryocore2>();
								}
								int num664 = NPC.NewNPC(NPC.GetSource_FromThis(), x, y, randomSpawn, 0, 0f, 0f, 0f, 0f, 255);
								if (Main.netMode == NetmodeID.Server && num664 < 200)
								{
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
								}
							}
						}
					}
				}
			}
        }

        private void HandlePhaseTransition(int newPhase)
        {
            int chipGoreAmount = newPhase >= 5 ? 3 : newPhase >= 3 ? 2 : 1;
            if (Main.netMode != NetmodeID.Server)
            {
	            for (int i = 1; i < chipGoreAmount; i++)
	            {
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("CryoChipGore" + i).Type, 1f);
	            }
            }
            currentPhase = newPhase;
            SoundEngine.PlaySound(SoundID.NPCDeath14, NPC.position);
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) //for alt textures
        {
            if (currentPhase > 1)
            {
                string phase = "CalRD/NPCs/Cryogen/Cryogen_Phase" + currentPhase;
                Texture2D texture = ModContent.Request<Texture2D>(phase).Value;

				SpriteEffects spriteEffects = SpriteEffects.None;
				if (NPC.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;

				Vector2 origin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
				Vector2 drawPos = NPC.Center - Main.screenPosition;
				drawPos -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
				drawPos += origin * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
				spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
				return false;
            }
            return true;
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
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 67, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 67, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 67, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 67, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }

                if (Main.netMode != NetmodeID.Server)
                {
	                float randomSpread = Main.rand.Next(-200, 200) / 100;
	                for (int i = 1; i < 4; i++)
	                {
		                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CryoDeathGore" + i).Type, 1f);
		                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CryoChipGore" + i).Type, 1f);
	                }
				}
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void OnKill()
        {
            DropHelper.DropBags(ModContent.ItemType<CryogenBag>(), NPC);

            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CryogenTrophy>(), 10);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeCryogen>(), true, !CalamityWorld.downedCryogen);
            DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedCryogen, 4, 2, 1);

            if (!Main.expertMode)
            {
                // Materials
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CryoBar>(), 15, 25);
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofEleum>(), 4, 8);
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.FrostCore);

                // Weapons
                float w = DropHelper.DirectWeaponDropRateFloat;
                DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC,
                    DropHelper.WeightStack<Avalanche>(w),
                    DropHelper.WeightStack<GlacialCrusher>(w),
                    DropHelper.WeightStack<EffluviumBow>(w),
                    DropHelper.WeightStack<BittercoldStaff>(w),
                    DropHelper.WeightStack<SnowstormStaff>(w),
                    DropHelper.WeightStack<Icebreaker>(w)
                );

                // Equipment
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CryoStone>(), 10);
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Regenator>(), DropHelper.RareVariantDropRateInt);

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CryogenMask>(), 7);

                // Other
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ItemID.FrozenKey, 5);
            }

            // Spawn Permafrost if he isn't in the world
            int permafrostNPC = NPC.FindFirstNPC(ModContent.NPCType<DILF>());
            if (permafrostNPC == -1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DILF>(), 0, 0f, 0f, 0f, 0f, 255);
            }

            // If Cryogen has not been killed, notify players about Cryonic Ore
            if (!CalamityWorld.downedCryogen)
            {
                string key = "The ice caves are crackling with frigid energy.";
                Color messageColor = Color.LightSkyBlue;
                WorldGenerationMethods.SpawnOre(ModContent.TileType<CryonicOre>(), 15E-05, .45f, .65f);

                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }

            // Mark Cryogen as dead
            CalamityWorld.downedCryogen = true;
            CalamityNetcode.SyncWorld();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Frostburn, 120, true);
            target.AddBuff(BuffID.Chilled, 90, true);
        }
    }
}
