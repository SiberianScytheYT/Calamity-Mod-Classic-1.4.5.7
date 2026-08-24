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
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Boss;
using CalRD.Tiles.Ores;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
/* states:
 * 0 = slow drift
 * 1 = reelback and teleport after spawn enemy
 * 2 = reelback for spin lunge + death legacy
 * 3 = spin lunge
 * 4 = semicircle spawn arc
 * 5 = raindash
 * 6 = deceleration
 */

namespace CalRD.NPCs.HiveMind
{
	[AutoloadBossHead]
    public class HiveMindP2 : ModNPC
    {
        //this block of values can be modified in SetDefaults() based on difficulty mode or something
        int minimumDriftTime = 300;
        int teleportRadius = 300;
        int decelerationTime = 30;
        int reelbackFade = 2;       //divide 255 by this for duration of reelback in ticks
        float arcTime = 45f;        //ticks needed to complete movement for spawn and rain attacks (DEATH ONLY)
        float driftSpeed = 1f;      //default speed when slowly floating at player
        float driftBoost = 1f;      //max speed added as health decreases
        int lungeDelay = 90;        //# of ticks long hive mind spends sliding to a stop before lunging
        int lungeTime = 33;
        int lungeFade = 15;         //divide 255 by this for duration of hive mind spin before slowing for lunge
        double lungeRots = 0.2;     //number of revolutions made while spinning/fading in for lunge
        bool dashStarted = false;
        int phase2timer = 360;
        int rotationDirection;
        double rotation;
        double rotationIncrement;
        int state = 0;
        int previousState = 0;
        int nextState = 0;
        int reelCount = 0;
        Vector2 deceleration;
        int counter = 0;
        bool initialised = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Hive Mind");
            Main.npcFrameCount[NPC.type] = 16;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
			NPC.GetNPCDamage();
			NPC.width = 177;
            NPC.height = 142;
            NPC.defense = 5;
            NPC.LifeMaxNERB(5800, 7560, 3000000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.buffImmune[ModContent.BuffType<GlacialState>()] = true;
            NPC.buffImmune[ModContent.BuffType<TemporalSadness>()] = true;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 6, 0, 0);
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
           Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/HiveMind");
            NPCID.Sets.TrailCacheLength[NPC.type] = 8;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            if (Main.expertMode)
            {
                minimumDriftTime = 120;
                reelbackFade = 4;
            }
            if (CalamityWorld.revenge)
            {
                lungeRots = 0.3;
                minimumDriftTime = 90;
                reelbackFade = 5;
                lungeTime = 28;
                driftSpeed = 2f;
                driftBoost = 2f;
            }
            if (CalamityWorld.death)
            {
                lungeRots = 0.4;
                minimumDriftTime = 60;
                reelbackFade = 6;
                lungeTime = 23;
                driftSpeed = 3f;
                driftBoost = 1f;
            }
            if (BossRushEvent.BossRushActive)
            {
                lungeRots = 0.4;
                minimumDriftTime = 20;
                reelbackFade = 15;
                lungeTime = 10;
                driftSpeed = 12f;
            }
            phase2timer = minimumDriftTime;
            rotationIncrement = 0.0246399424 * lungeRots * lungeFade;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(state);
            writer.Write(nextState);
            writer.Write(phase2timer);
            writer.Write(dashStarted);
            writer.Write(rotationDirection);
            writer.Write(rotation);
            writer.Write(previousState);
            writer.Write(reelCount);
            writer.Write(counter);
            writer.Write(initialised);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            state = reader.ReadInt32();
            nextState = reader.ReadInt32();
            phase2timer = reader.ReadInt32();
            dashStarted = reader.ReadBoolean();
            rotationDirection = reader.ReadInt32();
            rotation = reader.ReadDouble();
            previousState = reader.ReadInt32();
            reelCount = reader.ReadInt32();
            counter = reader.ReadInt32();
            initialised = reader.ReadBoolean();
        }

        public override void FindFrame(int frameHeight)
        {
            int width = NPC.width;
            int height = NPC.height;

            if (!initialised)
            {
                counter = 8;
                NPC.frameCounter = 6;
                initialised = true;
            }

            //ensure width and height are set.
            NPC.frame.Width = width;
            NPC.frame.Height = height;
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                NPC.frame.X = counter >= 8 ? width + 3 : 0;
                if (counter == 8)
                    NPC.frame.Y = 0;
                else
                    NPC.frame.Y += height;
                NPC.frameCounter = 0;
                counter++;
            }
            if (counter == 16)
            {
                counter = 1;
                NPC.frame.Y = 0;
                NPC.frame.X = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            Color color24 = drawColor;
            color24 = NPC.GetAlpha(color24);
            Color color25 = Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
            Texture2D texture2D3 = ModContent.Request<Texture2D>("CalRD/NPCs/HiveMind/HiveMindP2").Value;
            int num156 = TextureAssets.Npc[NPC.type].Value.Height / 8;
            Rectangle rectangle = new Rectangle(NPC.frame.X, NPC.frame.Y, NPC.frame.X, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            int num157 = 8;
            int num158 = 2;
            int num159 = 1;
            float num160 = 0f;
            int num161 = num159;
            while (state != 0 && CalamityConfig.Instance.Afterimages && ((num158 > 0 && num161 < num157) || (num158 < 0 && num161 > num157)))
            {
                Color color26 = color25;
                color26 = NPC.GetAlpha(color26);
                {
                    goto IL_6899;
                }
                IL_6881:
                num161 += num158;
                continue;
                IL_6899:
                float num164 = (float)(num157 - num161);
                if (num158 < 0)
                {
                    num164 = (float)(num159 - num161);
                }
                color26 *= num164 / ((float)NPCID.Sets.TrailCacheLength[NPC.type] * 1.5f);
                Vector2 value4 = NPC.oldPos[num161];
                float num165 = NPC.rotation;
                SpriteEffects effects = spriteEffects;
                Main.spriteBatch.Draw(texture2D3, value4 + NPC.Size / 2f - Main.screenPosition + new Vector2(0, NPC.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, num165 + NPC.rotation * num160 * (float)(num161 - 1) * -(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin2, NPC.scale, effects, 0f);
                goto IL_6881;
            }

            var something = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, color24, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, something, 0);
            return false;
        }

        private void SpawnStuff()
        {
			int maxSpawns = (CalamityWorld.death || BossRushEvent.BossRushActive) ? 5 : CalamityWorld.revenge ? 4 : Main.expertMode ? Main.rand.Next(3, 5) : Main.rand.Next(2, 4);
			for (int i = 0; i < maxSpawns; i++)
			{
				int type = NPCID.EaterofSouls;
				int choice = -1;
				do
				{
					choice++;
					switch (choice)
					{
						case 0:
						case 1:
							type = NPCID.EaterofSouls;
							break;
						case 2:
							type = NPCID.DevourerHead;
							break;
						case 3:
						case 4:
							type = ModContent.NPCType<DankCreeper>();
							break;
						default:
							break;
					}
				}
				while (NPC.AnyNPCs(type) && choice < 5);

				if (choice < 5)
					NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + Main.rand.Next(NPC.width), (int)NPC.position.Y + Main.rand.Next(NPC.height), type);
			}
        }

        private void ReelBack()
        {
            NPC.alpha = 0;
            phase2timer = 0;
            deceleration = NPC.velocity / 255f * reelbackFade;
            if (CalamityWorld.revenge || BossRushEvent.BossRushActive)
            {
                state = 2;
                SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.Center);
            }
            else
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    SpawnStuff();
                state = nextState;
                nextState = 0;
                if (state == 2)
                {
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.Center);
                }
            }
        }

        public override void AI()
        {
			// Target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			Player player = Main.player[NPC.target];

			float enrageScale = 0f;
			if ((NPC.position.Y / 16f) < Main.worldSurface)
				enrageScale += 1f;
			if (!player.ZoneCorrupt)
				enrageScale += 1f;

			if (BossRushEvent.BossRushActive)
				enrageScale = 0f;

			if (NPC.alpha != 0)
            {
                if (NPC.damage != 0)
                    NPC.damage = 0;
            }
            else
                NPC.damage = NPC.defDamage;

            switch (state)
            {
                case 0: //slowdrift
                    if (NPC.alpha > 0)
                        NPC.alpha -= 3;
                    if (nextState == 0)
                    {
						NPC.TargetClosest(true);
						if (CalamityWorld.revenge && NPC.life < NPC.lifeMax * 0.66)
                        {
							if (CalamityWorld.death || BossRushEvent.BossRushActive)
							{
								do
									nextState = Main.rand.Next(3, 6);
								while (nextState == previousState);
								previousState = nextState;
							}
							else if (NPC.life < NPC.lifeMax * 0.33)
							{
								do
									nextState = Main.rand.Next(3, 6);
								while (nextState == previousState);
								previousState = nextState;
							}
							else
							{
								do
									nextState = Main.rand.Next(3, 5);
								while (nextState == previousState);
								previousState = nextState;
							}
                        }
                        else
                        {
                            if (CalamityWorld.revenge && (Main.rand.NextBool(3) || reelCount == 2))
                            {
                                reelCount = 0;
                                nextState = 2;
                            }
                            else
                            {
                                reelCount++;
								if (Main.expertMode && reelCount == 2)
								{
									reelCount = 0;
									nextState = 2;
								}
								else
									nextState = 1;

                                NPC.ai[1] = 0f;
                                NPC.ai[2] = 0f;
                            }
                        }
                        if (nextState == 3)
                            rotation = MathHelper.ToRadians(Main.rand.Next(360));
                        NPC.netUpdate = true;
                    }

                    if (!player.active || player.dead || Vector2.Distance(NPC.Center, player.Center) > 5000f)
                    {
                        NPC.TargetClosest(false);
						player = Main.player[NPC.target];
						if (!player.active || player.dead || Vector2.Distance(NPC.Center, player.Center) > 5000f)
						{
							if (NPC.timeLeft > 60)
								NPC.timeLeft = 60;
							if (NPC.localAI[3] < 120f)
							{
								float[] aiArray = NPC.localAI;
								int number = 3;
								float num244 = aiArray[number];
								aiArray[number] = num244 + 1f;
							}
							if (NPC.localAI[3] > 60f)
							{
								NPC.velocity.Y += (NPC.localAI[3] - 60f) * 0.5f;
							}
							return;
						}
                    }
					else if (NPC.timeLeft < 1800)
						NPC.timeLeft = 1800;

					if (NPC.localAI[3] > 0f)
                    {
                        float[] aiArray = NPC.localAI;
                        int number = 3;
                        float num244 = aiArray[number];
                        aiArray[number] = num244 - 1f;
                        return;
                    }

                    NPC.velocity = player.Center - NPC.Center;
                    phase2timer--;
                    if (phase2timer <= -180) //no stalling drift mode forever
                    {
                        NPC.velocity *= 2f / 255f * (reelbackFade + 2 * (int)enrageScale);
                        ReelBack();
                        NPC.netUpdate = true;
                    }
                    else
                    {
                        NPC.velocity.Normalize();
                        if (Main.expertMode || BossRushEvent.BossRushActive) //variable velocity in expert and up
                        {
                            NPC.velocity *= driftSpeed + enrageScale + (driftBoost + enrageScale) * (NPC.lifeMax - NPC.life) / NPC.lifeMax;
                        }
                        else
                        {
                            NPC.velocity *= driftSpeed + enrageScale;
                        }
                    }
                    break;
                case 1: //reelback and teleport
                    NPC.alpha += reelbackFade + 2 * (int)enrageScale;
                    NPC.velocity -= deceleration;
                    if (NPC.alpha >= 255)
                    {
                        NPC.alpha = 255;
                        NPC.velocity = Vector2.Zero;
                        state = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[1] != 0f && NPC.ai[2] != 0f)
                        {
                            NPC.position.X = NPC.ai[1] * 16 - NPC.width / 2;
                            NPC.position.Y = NPC.ai[2] * 16 - NPC.height / 2;
                        }
                        phase2timer = minimumDriftTime + Main.rand.Next(121);
                        NPC.netUpdate = true;
                    }
                    else if (NPC.ai[1] == 0f && NPC.ai[2] == 0f)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            int posX = (int)player.Center.X / 16 + Main.rand.Next(15, 46) * (Main.rand.NextBool(2) ? -1 : 1);
                            int posY = (int)player.Center.Y / 16 + Main.rand.Next(15, 46) * (Main.rand.NextBool(2) ? -1 : 1);
                            if (!WorldGen.SolidTile(posX, posY) && Collision.CanHit(new Vector2(posX * 16, posY * 16), 1, 1, player.position, player.width, player.height))
                            {
                                NPC.ai[1] = posX;
                                NPC.ai[2] = posY;
                                NPC.netUpdate = true;
                                break;
                            }
                        }
                    }
                    break;
                case 2: //reelback for lunge + death legacy
                    NPC.alpha += reelbackFade + 2 * (int)enrageScale;
                    NPC.velocity -= deceleration;
                    if (NPC.alpha >= 255)
                    {
                        NPC.alpha = 255;
                        NPC.velocity = Vector2.Zero;
                        dashStarted = false;
                        if (CalamityWorld.revenge && NPC.life < NPC.lifeMax * 0.66)
                        {
							state = nextState;
                            nextState = 0;
                            previousState = state;
                        }
                        else
                        {
                            state = 3;
                        }
                        if (player.velocity.X > 0)
                            rotationDirection = 1;
                        else if (player.velocity.X < 0)
                            rotationDirection = -1;
                        else
                            rotationDirection = player.direction;
                    }
                    break;
                case 3: //lunge
                    NPC.netUpdate = true;
                    if (NPC.alpha > 0)
                    {
                        NPC.alpha -= lungeFade;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.Center = player.Center + new Vector2(teleportRadius, 0).RotatedBy(rotation);
                        }
                        rotation += rotationIncrement * rotationDirection;
                        phase2timer = lungeDelay;
                    }
                    else
                    {
                        phase2timer--;
                        if (!dashStarted)
                        {
                            if (phase2timer <= 0)
                            {
                                phase2timer = lungeTime - 4 * (int)enrageScale;
                                NPC.velocity = player.Center - NPC.Center;
                                NPC.velocity.Normalize();
                                NPC.velocity *= teleportRadius / (lungeTime - 4 * (int)enrageScale);
                                dashStarted = true;
                                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                            }
                            else
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    NPC.Center = player.Center + new Vector2(teleportRadius, 0).RotatedBy(rotation);
                                }
                                rotation += rotationIncrement * rotationDirection * phase2timer / lungeDelay;
                            }
                        }
                        else
                        {
                            if (phase2timer <= 0)
                            {
                                state = 6;
                                phase2timer = 0;
                                deceleration = NPC.velocity / decelerationTime;
                            }
                        }
                    }
                    break;
                case 4: //enemy spawn arc
                    if (NPC.alpha > 0)
                    {
                        NPC.alpha -= 5;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.Center = player.Center;
                            NPC.position.Y += teleportRadius;
                        }
                        NPC.netUpdate = true;
                    }
                    else
                    {
                        if (!dashStarted)
                        {
                            dashStarted = true;
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                            NPC.velocity.X = MathHelper.Pi * teleportRadius / arcTime;
                            NPC.velocity *= rotationDirection;
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            NPC.velocity = NPC.velocity.RotatedBy(MathHelper.Pi / arcTime * -rotationDirection);
                            phase2timer++;
                            if (phase2timer == (int)arcTime / 6)
                            {
                                phase2timer = 0;
                                NPC.ai[0]++;
                                if (Main.netMode != NetmodeID.MultiplayerClient && Collision.CanHit(NPC.Center, 1, 1, player.position, player.width, player.height)) //draw line of sight
                                {
                                    if (NPC.ai[0] == 2 || NPC.ai[0] == 4)
                                    {
                                        if ((Main.expertMode || BossRushEvent.BossRushActive) && !NPC.AnyNPCs(ModContent.NPCType<DarkHeart>()))
                                        {
                                            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DarkHeart>());
                                        }
									}
                                    else if (!NPC.AnyNPCs(NPCID.EaterofSouls))
                                    {
                                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.EaterofSouls);
                                    }
                                }
                                if (NPC.ai[0] == 6)
                                {
                                    NPC.velocity = NPC.velocity.RotatedBy(MathHelper.Pi / arcTime * -rotationDirection);
                                    SpawnStuff();
                                    state = 6;
                                    NPC.ai[0] = 0;
                                    deceleration = NPC.velocity / decelerationTime;
                                }
                            }
                        }
                    }
                    break;
                case 5: //raindash
                    if (NPC.alpha > 0)
                    {
                        NPC.alpha -= 5;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.Center = player.Center;
                            NPC.position.Y -= teleportRadius;
                            NPC.position.X += teleportRadius * rotationDirection;
                        }
                        NPC.netUpdate = true;
                    }
                    else
                    {
                        if (!dashStarted)
                        {
                            dashStarted = true;
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                            NPC.velocity.X = teleportRadius / arcTime * 3;
                            NPC.velocity *= -rotationDirection;
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            phase2timer++;
                            if (phase2timer == (int)arcTime / 20)
                            {
                                phase2timer = 0;
                                NPC.ai[0]++;
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
									int type = ModContent.ProjectileType<ShadeNimbusHostile>();
									int damage = NPC.GetProjectileDamage(type);
									Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position.X + Main.rand.Next(NPC.width), NPC.position.Y + Main.rand.Next(NPC.height), 0, 0, type, damage, 0, Main.myPlayer, 11, 0);
                                }
                                if (NPC.ai[0] == 10)
                                {
                                    state = 6;
                                    NPC.ai[0] = 0;
                                    deceleration = NPC.velocity / decelerationTime;
                                }
                            }
                        }
                    }
                    break;
                case 6: //deceleration
                    NPC.velocity -= deceleration;
                    phase2timer++;
                    if (phase2timer == decelerationTime)
                    {
                        phase2timer = minimumDriftTime + Main.rand.Next(121);
                        state = 0;
                        NPC.netUpdate = true;
                    }
                    break;
            }
        }

        public override bool CanHitNPC(NPC target)/* tModPorter Suggestion: Return true instead of null */
        {
            if (NPC.alpha > 0)
                return false;
            return true;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.alpha <= 0; //no damage when not fully visible
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            modifiers.ModifyHitInfo += ReelbackToggle;
        }

        public void ReelbackToggle(ref NPC.HitInfo hit)
        {
            if (phase2timer < 0 && hit.Damage > 1)
            {
                NPC.velocity *= -4f;
                ReelBack();
                NPC.netUpdate = true;
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < hit.Damage / NPC.lifeMax * 100.0; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 14, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(15) && NPC.CountNPCS(ModContent.NPCType<HiveBlob2>()) < 2)
            {
                Vector2 spawnAt = NPC.Center + new Vector2(0f, NPC.height / 2f);
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<HiveBlob2>());
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    int goreAmount = 10;
                    for (int i = 1; i <= goreAmount; i++)
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HiveMindP2Gore" + i).Type, 1f);
                }
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 200;
                NPC.height = 150;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 14, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 14, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 14, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void OnKill()
        {
            DropHelper.DropBags(ModContent.ItemType<HiveMindBag>(), NPC);

            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<HiveMindTrophy>(), 10);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeHiveMind>(), true, !CalamityWorld.downedHiveMind);
            DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedHiveMind, 2, 0, 0);

			CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Dryad }, CalamityWorld.downedHiveMind);

			// All other drops are contained in the bag, so they only drop directly on Normal
			if (!Main.expertMode)
            {
                // Materials
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<TrueShadowScale>(), 25, 30);
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ItemID.DemoniteBar, 7, 10);
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ItemID.RottenChunk, 9, 15);
                if (Main.hardMode)
                    DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ItemID.CursedFlame, 10, 20);

                // Weapons
                float w = DropHelper.DirectWeaponDropRateFloat;
                DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC,
                    DropHelper.WeightStack<PerfectDark>(w),
                    DropHelper.WeightStack<LeechingDagger>(w),
                    DropHelper.WeightStack<Shadethrower>(w),
                    DropHelper.WeightStack<ShadowdropStaff>(w),
                    DropHelper.WeightStack<ShaderainStaff>(w),
                    DropHelper.WeightStack<DankStaff>(w),
                    DropHelper.WeightStack<RotBall>(w, 30, 50)
                );

                //Equipment
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<FilthyGlove>(), 4);

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<HiveMindMask>(), 7);
            }

            // If neither The Hive Mind nor The Perforator Hive have been killed yet, notify players of Aerialite Ore
            if (!CalamityWorld.downedHiveMind && !CalamityWorld.downedPerforator)
            {
                string key = "The ground is glittering with cyan light.";
                Color messageColor = Color.Cyan;
                WorldGenerationMethods.SpawnOre(ModContent.TileType<AerialiteOre>(), 12E-05, .4f, .6f);

                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }

            // Mark The Hive Mind as dead
            CalamityWorld.downedHiveMind = true;
            CalamityNetcode.SyncWorld();
        }
    }
}
