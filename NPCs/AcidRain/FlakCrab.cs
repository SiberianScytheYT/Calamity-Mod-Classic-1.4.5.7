using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Ranged;
using CalRD.Projectiles.Enemy;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.AcidRain
{
    public class FlakCrab : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flak Crab");
            Main.npcFrameCount[NPC.type] = 7;
        }

        public override void SetDefaults()
        {
            NPC.width = 28;
            NPC.height = 70;

            NPC.damage = 10;
            NPC.lifeMax = 300;

            NPC.aiStyle = AIType = -1;

            if (CalamityWorld.downedPolterghast)
            {
                NPC.lifeMax = 7500;
				NPC.DR_NERD(0.2f);
            }

            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.value = Item.buyPrice(0, 0, 5, 55);
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.DD2_WitherBeastDeath;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<FlakCrabBanner>();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
			// Enables expert scaling, if damage is 0 in set defaults expert scaling will not happen
			NPC.damage = 0;

            Player closest = Main.player[Player.FindClosest(NPC.Top, 0, 0)];
            NPC.ai[0]++;
            NPC.defense = NPC.localAI[1] < 10f ? 999999 : 20;

            if (NPC.justHit)
            {
                NPC.localAI[0] = 240;
                NPC.netUpdate = true;
            }
            if (NPC.localAI[0] == 0f || NPC.localAI[1] < 10f)
            {
                if (NPC.ai[0] < 300f)
                {
                    NPC.chaseable = false;
                    NPC.knockBackResist = 0f;
                }
                if (Math.Abs(closest.Center.X - NPC.Center.X) < 320f &&
                    closest.Center.Y - NPC.Top.Y < -60f &&
                    NPC.ai[1]++ >= Main.rand.Next(90, 135))
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        float speed = CalamityWorld.downedPolterghast ? 29f : 17f;
                        speed *= Main.rand.NextFloat(0.8f, 1.2f);
                        int damage = Main.expertMode ? CalamityWorld.downedPolterghast ? 32 : 18 : CalamityWorld.downedPolterghast ? 42 : 23;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Top + Vector2.UnitY * 6f, NPC.DirectionTo(closest.Center).RotatedByRandom(0.25f) * speed,
                            ModContent.ProjectileType<FlakAcid>(), damage, 2f);
                        NPC.ai[1] = 0;
                    }
                }
            }
            else
            {
                NPC.localAI[0]--;
                NPC.chaseable = true;
                if (NPC.velocity.Y == 0f)
                {
                    NPC.knockBackResist = 0.6f;
                    NPC.TargetClosest(true);
                    NPC.velocity.X *= 0.85f;
                    NPC.ai[2]++;
                    float hopRate = 10f + 15f * (NPC.life / (float)NPC.lifeMax);
                    float lungeForwardSpeed = 10f;
                    float jumpSpeed = 9f;
                    if (Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
                    {
                        lungeForwardSpeed *= 1.5f;
                    }
                    if (NPC.ai[2] > hopRate)
                    {
                        NPC.ai[3] += 1f;
                        if (NPC.ai[3] >= 3f)
                        {
                            NPC.ai[3] = 0f;
                            lungeForwardSpeed *= 1.5f;
                        }
                        NPC.ai[2] = 0f;
                        NPC.velocity.Y -= jumpSpeed;
                        NPC.velocity.X = lungeForwardSpeed * -NPC.direction;
                        NPC.netUpdate = true;
                    }
                }
                else
                {
                    NPC.knockBackResist = 0.2f;
                    NPC.velocity.X *= 0.995f;
                }
            }
            if (NPC.ai[0] >= 300f && !NPC.chaseable)
            {
                NPC.chaseable = true;
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            // Don't draw the bar if in stealth mode
            if (NPC.localAI[0] == 0f || NPC.localAI[1] < 10f)
                return false;
            return null;
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CorrodedFossil>(), 3 * (CalamityWorld.downedPolterghast ? 5 : 1), 1, 3);
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<FlakToxicannon>(), 0.05f);
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.localAI[1] < 10f)
            {
                NPC.frame.Y = 0;
                return;
            }
            if (NPC.localAI[0] > 0f)
            {
                if (NPC.frameCounter++ % 6 == 5)
                {
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                {
                    NPC.frame.Y = frameHeight * 3; // Frames 1 and 2 are for transitioning. Frame 0 is sitting still, and the rest are walking frames
                }
                if (NPC.localAI[0] <= 8)
                    NPC.frame.Y = frameHeight;
            }
            else
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
                    for (int k = 0; k < 15; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
                    }
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("FlakCrab").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("FlakCrab2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("FlakCrab3").Type, 1f);
                }
            }
            NPC.localAI[1]++;
        }
    }
}
