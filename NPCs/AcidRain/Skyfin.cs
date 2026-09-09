using CalRD.Dusts;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Rogue;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using CalRD.BiomeManagers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.StatDebuffs;
using Terraria.GameContent.Bestiary;

namespace CalRD.NPCs.AcidRain
{
    public class Skyfin : ModNPC
    {
        public const float DiveDelay = 120f;
        public const float DiveTime = 90f;
        public const float TotalTime = DiveDelay + DiveTime;
        public bool Flying = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Skyfin");
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Rotation = MathHelper.Pi
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Likely flying fish that adapted to their harsh conditions.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 46;
            NPC.height = 22;
            NPC.aiStyle = AIType = -1;

            NPC.damage = 12;
            NPC.lifeMax = 70;
            NPC.defense = 6;
            NPC.knockBackResist = 1f;

            if (CalamityWorld.downedPolterghast)
            {
				NPC.knockBackResist = 0.8f;
                NPC.damage = 88;
                NPC.lifeMax = 5500;
                NPC.defense = 18;
				NPC.DR_NERD(0.05f);
            }
            else if (CalamityWorld.downedAquaticScourge)
            {
                NPC.damage = 38;
                NPC.lifeMax = 220;
				NPC.DR_NERD(0.05f);
            }

            NPC.value = Item.buyPrice(0, 0, 3, 65);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SkyfinBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Flying);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Flying = reader.ReadBoolean();
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (!Flying)
            {
                NPC.ai[0] += 1f;
                if (NPC.ai[1] > 0f)
                    NPC.ai[1] -= 1f;
                if (NPC.wet)
                {
                    // Swim around, moving towards the player
                    bool canSwimToPlayer = Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
                    if (canSwimToPlayer)
                    {
                        if (NPC.ai[0] % 55f == 54f)
                        {
                            float horizontalSchoolingSpeed = 9f;
                            if (CalamityWorld.downedAquaticScourge)
                            {
                                horizontalSchoolingSpeed = 15f;
                            }
                            if (CalamityWorld.downedPolterghast)
                            {
                                horizontalSchoolingSpeed = 24f;
                            }
                            NPC.velocity = Vector2.UnitX * (player.Center.X - NPC.Center.X > 0).ToDirectionInt() * horizontalSchoolingSpeed;
                        }
                        if ((Math.Abs(player.Center.Y - NPC.Center.Y) > 50f && player.wet) || (!player.wet && NPC.ai[1] <= 0f))
                        {
                            float speedY = CalamityWorld.downedPolterghast ? 10f : 6f;
                            NPC.velocity.Y = (player.Center.Y - NPC.Center.Y > 0).ToDirectionInt() * speedY;
                        }
                        if (Math.Abs(NPC.velocity.X) < 6f)
                            NPC.velocity.X *= 1.04f;
                    }
                    else if (!canSwimToPlayer && Math.Abs(NPC.velocity.Y) < 4f)
                    {
                        NPC.velocity.Y *= 0.97f;
                    }
                    // Turn around if we hit a tile on the X axis
                    if (!canSwimToPlayer && NPC.collideX)
                    {
                        NPC.velocity.X *= -1f;
                    }

                    // Check if we can dive
                    if (player.Center.Y < NPC.Top.Y - 10f &&
                        NPC.ai[1] <= 0f)
                    {
                        if (Main.rand.NextBool(10))
                        {
                            NPC.ai[1] = TotalTime;
                        }
                        NPC.ai[2] = (player.Center.X - NPC.Center.X > 0).ToDirectionInt() * 10f;
                    }
                }
                else
                {
                    // Consistently update the enemy.
                    if (NPC.ai[3] % 40f == 39f)
                    {
                        NPC.netUpdate = true;
                    }
                    // Dive upward in an attempt to hit to the player
                    if (NPC.ai[1] > TotalTime - DiveTime)
                    {
                        NPC.velocity.X = NPC.ai[2];
                        if (NPC.ai[1] > TotalTime - DiveTime * 0.5f)
                        {
                            float flySpeed = CalamityWorld.downedAquaticScourge ? 0.115f : 0.085f;
                            if (CalamityWorld.downedPolterghast)
                            {
                                flySpeed = 0.135f;
                            }
                            NPC.velocity.Y -= flySpeed;
                        }
                        else
                        {
                            NPC.ai[1] = TotalTime - DiveTime;
                            NPC.velocity.Y += 0.2f;
                        }
                    }
                    else
                    {
                        // Don't fall too fast because of wings
                        NPC.ai[1] = TotalTime - DiveTime;
                        NPC.velocity.Y += 0.1f;
                        NPC.ai[3]++;
                        if (NPC.ai[3] > 420f)
                        {
                            NPC.ai[0] = NPC.ai[1] = NPC.ai[2] = NPC.ai[3] = 0f;
                            Flying = true;
                            NPC.netUpdate = true;
                        }
                    }
                }
                // If sitting on land, rotate in a way that looks like we're stuck on the ground
                if (!NPC.wet)
                {
                    NPC.velocity.X *= 0.92f;
                }
            }
            else
            {
                NPC.noTileCollide = true;
                NPC.ai[0]++;
                if (NPC.ai[0] % 300f >= 180f)
                {
                    if (NPC.ai[0] % 300f == 205f)
                    {
                        NPC.velocity.Y = -6.5f;
                    }
                    if (NPC.ai[0] % 300f == 235f)
                    {
                        float chargeSpeed = 8f;
                        if (CalamityWorld.downedAquaticScourge)
                        {
                            chargeSpeed = 14f;
                        }
                        if (CalamityWorld.downedPolterghast)
                        {
                            chargeSpeed = 18f;
                        }
                        NPC.velocity = NPC.DirectionTo(player.Center) * chargeSpeed;
                    }
                }
                else
                {
                    if (Math.Abs(player.Center.X - NPC.Center.X) > 320f)
                    {
                        NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, (player.Center.X - NPC.Center.X > 0).ToDirectionInt() * 10f, 0.05f);
                    }
                    if (Math.Abs(player.Center.Y - NPC.Center.Y) > 50f)
                    {
                        NPC.velocity.Y = NPC.DirectionTo(player.Center).Y * 9f;
                    }
                }
            }
            int idealDirection = (NPC.velocity.X > 0).ToDirectionInt();
            NPC.direction = NPC.spriteDirection = idealDirection;
            if (idealDirection != NPC.direction)
            {
                NPC.netUpdate = true;
            }
            NPC.rotation = NPC.velocity.ToRotation() +
                (NPC.direction > 0).ToInt() * MathHelper.Pi;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddIf(() => CalamityWorld.downedAquaticScourge, ModContent.ItemType<SulfuricScale>(), 12, 1, 3);
            npcLoot.AddIf(() => !CalamityWorld.downedAquaticScourge, ModContent.ItemType<SulfuricScale>(), 2, 1, 3);
            npcLoot.AddIf(() => CalamityWorld.downedAquaticScourge, ModContent.ItemType<SkyfinBombers>(), 20);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 8; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SkyfinGore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SkyfinGore2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SkyfinGore3").Type, NPC.scale);
                }
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
        }
    }
}
