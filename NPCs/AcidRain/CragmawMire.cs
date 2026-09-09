using CalRD.Dusts;
using CalRD.Items.Accessories;
using CalRD.Projectiles.Enemy;
using CalRD.Projectiles.Environment;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using CalRD.BiomeManagers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Weapons.Rogue;
using Terraria.GameContent.Bestiary;

namespace CalRD.NPCs.AcidRain
{
    public class CragmawMire : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cragmaw Mire");
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.width = 68;
            NPC.height = 54;
            NPC.aiStyle = AIType = -1;

            NPC.damage = 66;
            NPC.lifeMax = 4000;
            NPC.defense = 25;

            if (CalamityWorld.downedPolterghast)
            {
                NPC.damage = 160;
                NPC.lifeMax = 146600;
                NPC.defense = 80;
            }

            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 3, 60, 0);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("A slime that against all odds developed a calcified shell.")
            });
        }
        public bool Phase2
        {
            get
            {
                float ratio = CalamityWorld.revenge ? 0.85f : 0.7f;
                return NPC.life / (float)NPC.lifeMax < ratio && CalamityWorld.downedPolterghast;
            }
        }
        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];

            NPC.ai[0]++;

            // Summons a spinning spiral of dust and a dust telegraph
            if (NPC.ai[0] % 420f > 240f && NPC.ai[0] % 420f < 300f)
            {
                float vectorRotationAngle = (NPC.ai[0] % 420f - 240f) / 60f * MathHelper.Pi * 4f;
                for (int i = 0; i < 25; i++)
                {
                    float angle = MathHelper.TwoPi * i / 25f;
                    float y = (float)Math.Sin(angle) * (float)Math.Log(Math.Abs(Math.Cos(angle)));
                    if (!float.IsNaN(y))
                    {
                        Vector2 velocity = new Vector2((float)Math.Cos(angle), y) * MathHelper.Lerp(2.4f, 4.2f, (NPC.ai[0] - 240f) / 60f);
                        Dust dust = Dust.NewDustPerfect(NPC.Center, (int)CalamityDusts.SulfurousSeaAcid);
                        dust.velocity = velocity.RotatedBy(vectorRotationAngle);
                        dust.noGravity = true;
                        dust.scale = MathHelper.Lerp(1.2f, 2.5f, (NPC.ai[0] - 240f) / 60f);
                    }
                }

                float length = MathHelper.Lerp(20f, 1200f, (NPC.ai[0] % 420f - 240f) / 60f);
                float outwardness = 1f - (NPC.ai[0] % 420f - 240f) / 60f;
                for (float i = NPC.Top.Y + 4f; i >= NPC.Top.Y + 4f - length; i -= 6f)
                {
                    float angle = i / 32f;
                    vectorRotationAngle = -(i / 20f) % 0.4f - 0.2f;
                    Vector2 spawnPosition = new Vector2(NPC.Center.X, i);
                    Dust dust = Dust.NewDustPerfect(spawnPosition, (int)CalamityDusts.SulfurousSeaAcid);
                    dust.scale = 1.5f;
                    dust.velocity = (Vector2.UnitX * (float)Math.Cos(angle) * 4f * outwardness).RotatedBy(vectorRotationAngle);
                    dust.noGravity = true;
                }
            }
            // Release a laser beam and create an explosion
            if (NPC.ai[0] % 420f == 300f)
            {
                NPC.ai[1] = Main.rand.NextFloat(3f);
                NPC.ai[2] = Main.rand.NextFloat(4f);
                NPC.netUpdate = true;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(SoundID.Zombie104, NPC.position); // Moon lord beam sound
                    int damage = CalamityWorld.downedPolterghast ? 120 : 40;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.UnitY, ModContent.ProjectileType<CragmawBeam>(), damage, 4f, Main.myPlayer, 0f, NPC.whoAmI);
                }
                Vector2 circlePointVector = -Vector2.UnitY;
                float lerpStart = Main.rand.Next(12, 16);
                float lerpEnd = Main.rand.Next(3, 6);
                for (int h = 0; h < 4; h++)
                {
                    for (float i = 0; i < 9f; ++i)
                    {
                        for (int j = 0; j < 2; ++j)
                        {
                            Vector2 randomCirclePointRotated = circlePointVector.RotatedBy((j == 0 ? 1 : -1) * MathHelper.TwoPi / 18f).RotatedBy(h / 4f * MathHelper.Pi);
                            for (float k = 0f; k < 20f; ++k)
                            {
                                Vector2 randomCirclePointLerped = Vector2.Lerp(circlePointVector, randomCirclePointRotated, k / 20f);
                                float lerpMultiplier = MathHelper.Lerp(lerpStart, lerpEnd, k / 20f) * 0.9f;
                                int dustIndex = Dust.NewDust(NPC.Top + 6f * Vector2.UnitY, 0, 0,
                                    (int)CalamityDusts.SulfurousSeaAcid,
                                    0f, 0f, 100, default, 1.3f);
                                Main.dust[dustIndex].noGravity = true;
                                Main.dust[dustIndex].scale = 2f;
                                Main.dust[dustIndex].velocity = randomCirclePointLerped * lerpMultiplier;
                            }
                        }

                        circlePointVector = circlePointVector.RotatedBy(MathHelper.TwoPi / 9f);
                    }
                }
            }
            else if (NPC.ai[0] % 420f > 300f && NPC.ai[0] % 7f == 6f && CalamityWorld.downedPolterghast)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.UnitY.RotatedByRandom(0.6f) * Main.rand.NextFloat(10f, 15f), ModContent.ProjectileType<AcidDrop>(), 69, 4f, Main.myPlayer, 0f, NPC.whoAmI);
                }
            }

            if (!Phase2)
            {
                NPC.Calamity().DR = 0.4f;
                if (!player.wet)
                    NPC.Calamity().DR = 0.6f;
                NPC.HitSound = SoundID.NPCHit42;
                if (NPC.ai[0] % 420f < 240f)
                {
                    float bubbleShootTimer = CalamityWorld.downedPolterghast ? 24f : 50f;
                    if (NPC.ai[0] % bubbleShootTimer == bubbleShootTimer - 1f && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int damage = CalamityWorld.downedPolterghast ? 47 : 30;
                        int idx = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Top + new Vector2(0f, 5f), -Vector2.UnitY.RotatedByRandom(0.25f) * 4f, ModContent.ProjectileType<CragmawBubble>(), damage, 1f);
                        Main.projectile[idx].timeLeft = Main.rand.Next(120, 180);
                        Main.projectile[idx].netUpdate = true;
                    }
                    if (NPC.ai[0] % 50f == 49f && Main.rand.NextBool(4))
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            List<int> possibleEnemies = new List<int>();
                            if (CalamityWorld.downedPolterghast)
                            {
                                possibleEnemies.Add(ModContent.NPCType<GammaSlime>());
                                possibleEnemies.Add(ModContent.NPCType<NuclearToad>());
                                possibleEnemies.Add(ModContent.NPCType<Orthocera>());
                            }
                            else
                            {
                                possibleEnemies.Add(ModContent.NPCType<WaterLeech>());
                                possibleEnemies.Add(ModContent.NPCType<Trilobite>());
                            }

                            Vector2 spawnPosition = NPC.Center + Utils.RandomVector2(Main.rand, -360f, 360f);
                            int attempts = 0;
                            while (!WorldGen.InWorld((int)spawnPosition.X / 16, (int)spawnPosition.Y / 16))
                            {
                                spawnPosition = NPC.Center + Utils.RandomVector2(Main.rand, -460f, 460f);
                                attempts++;
                                if (attempts > 200)
                                    return;
                            }
                            attempts = 0;

                            while (CalamityUtils.TileSelectionSolidSquare((int)spawnPosition.X / 16, (int)spawnPosition.Y / 16, 8, 8) ||
                                CalamityUtils.ParanoidTileRetrieval((int)spawnPosition.X / 16, (int)spawnPosition.Y / 16).LiquidAmount != 255)
                            {
                                spawnPosition = NPC.Center + Utils.RandomVector2(Main.rand, -620f, 620f);
                                attempts++;
                                if (attempts > 300)
                                    return;
                            }

                            NPC.NewNPC(NPC.GetSource_FromThis(), (int)spawnPosition.X, (int)spawnPosition.Y, Utils.SelectRandom(Main.rand, possibleEnemies.ToArray()));
                        }
                    }
                }
            }
            else
            {
                if (NPC.localAI[0] == 0f)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, -Vector2.UnitY.RotatedByRandom(0.4f) * 4f, Mod.Find<ModGore>("CragmawMireP1Gore").Type, NPC.scale);
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, -Vector2.UnitY.RotatedByRandom(0.4f) * 4f, Mod.Find<ModGore>("CragmawMireP1Gore2").Type, NPC.scale);
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, -Vector2.UnitY.RotatedByRandom(0.4f) * 4f, Mod.Find<ModGore>("CragmawMireP1Gore3").Type, NPC.scale);
                    }
                    NPC.localAI[0] = 1f;
                }
                NPC.Calamity().DR = CalamityWorld.revenge ? 0.125f : 0f;
                NPC.HitSound = SoundID.NPCHit1;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (NPC.ai[0] % 600f == 20f)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY, ModContent.ProjectileType<CragmawVibeCheckChain>(), 0, 0f, Main.myPlayer, NPC.whoAmI, NPC.target);
                    }
                    if (NPC.ai[0] % 600f == 240f)
                    {
                        int damage = CalamityWorld.downedPolterghast ? 52 : 33;
                        for (int i = 0; i < 16; i++)
                        {
                            float angle = MathHelper.TwoPi / 16f * i;
                            float angleDelta = Main.rand.NextFloat(MathHelper.TwoPi / 16f) * 0.6f;
                            angle += angleDelta - angleDelta / 2f;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, angle.ToRotationVector2() * 6f, ModContent.ProjectileType<CragmawSpike>(), damage, 4f);
                        }
                    }
                }
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * 0.85f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Phase2 ? ModContent.Request<Texture2D>("CalRD/NPCs/AcidRain/CragmawMire2").Value : ModContent.Request<Texture2D>("CalRD/NPCs/AcidRain/CragmawMire").Value;
            if (NPC.IsABestiaryIconDummy)
            {
                Main.EntitySpriteDraw(texture, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, 0, 0);
                return false;
            }
            CalRD.DrawTexture(spriteBatch, texture, 0, NPC, drawColor, true);
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter % 6 == 5)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 2)
            {
                NPC.frame.Y = 0;
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, -Vector2.UnitY.RotatedByRandom(0.4f) * 4f, Mod.Find<ModGore>("CragmawMireP2Gore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, -Vector2.UnitY.RotatedByRandom(0.4f) * 4f, Mod.Find<ModGore>("CragmawMireP2Gore2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, -Vector2.UnitY.RotatedByRandom(0.4f) * 4f, Mod.Find<ModGore>("CragmawMireP2Gore3").Type, NPC.scale);
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.AddIf(() => CalamityWorld.downedPolterghast, ModContent.ItemType<NuclearRod>(), 10);
            npcLoot.AddIf(() => !CalamityWorld.downedPolterghast, ModContent.ItemType<NuclearRod>());
            npcLoot.AddIf(() => CalamityWorld.downedPolterghast, ModContent.ItemType<SpentFuelContainer>(), 10);
            npcLoot.AddIf(() => !CalamityWorld.downedPolterghast, ModContent.ItemType<SpentFuelContainer>());
        }
    }
}
