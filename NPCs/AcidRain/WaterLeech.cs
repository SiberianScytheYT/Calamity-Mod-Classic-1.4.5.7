using CalRD.Dusts;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Magic;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.StatDebuffs;
using System.IO;
using CalRD.BiomeManagers;
using CalRD.Buffs.DamageOverTime;
using Terraria.GameContent.Bestiary;

namespace CalRD.NPCs.AcidRain
{
    public class WaterLeech : ModNPC
    {
        public const float ChasePromptDistance = 55f;
        public const float ChaseMaxDistance = 140f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Water Leech");
            Main.npcFrameCount[NPC.type] = 4;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("They eagerly swim to any prey they can find thanks to their great sense of smell, sucking the blood out of their host and at once also injecting venom onto them.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 14;

            NPC.lifeMax = 30;

            if (CalamityWorld.downedPolterghast)
            {
                NPC.lifeMax = 2250;
                NPC.defense = 10;
            }
            else if (CalamityWorld.downedAquaticScourge)
            {
                NPC.lifeMax = 90;
            }

            NPC.value = Item.buyPrice(0, 0, 2, 5);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit33;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<WaterLeechBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
        }

        public override void AI()
        {
            if (NPC.localAI[0] == 0f)
            {
                NPC.TargetClosest(false);
                NPC.localAI[0] = 1f;
            }

            // Anti-sticky movement and player targeting detection (only one leech and attack the player at once)
            Player player = Main.player[NPC.target];
            bool playerAlreadyTargeted = false;
            float antiStickyAcceleration = 0.25f;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].type == NPC.type &&
                    Main.npc[i].whoAmI != NPC.whoAmI &&
                    Main.npc[i].active)
                {
                    if (Main.npc[i].target == NPC.target && !playerAlreadyTargeted)
                    {
                        playerAlreadyTargeted = true;
                    }
                    if (Main.npc[i].Hitbox.Intersects(NPC.Hitbox))
                    {
                        NPC.velocity += NPC.DirectionFrom(Main.npc[i].Center) * antiStickyAcceleration;
                    }
                }
            }

            if (!playerAlreadyTargeted || player.Calamity().waterLeechTarget == -1)
            {
                player.Calamity().waterLeechTarget = NPC.whoAmI;
            }

            // Latch onto player
            if (NPC.ai[0] == 1f)
            {
                if (NPC.dontTakeDamage)
                {
                    NPC.dontTakeDamage = false;
                    NPC.netUpdate = true;
                }
                if (NPC.Distance(player.Top) >= 140f)
                {
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                    return;
                }
                Vector2 destination = ((player.gravDir == 1f) ? player.Top : player.Bottom) + player.direction * 4 * Vector2.UnitX;
                float speed = Utils.SmoothStep(10f, ChaseMaxDistance, NPC.Distance(destination)) * 16f;
                NPC.velocity = NPC.DirectionTo(destination) * (speed + 7f);
                if (NPC.Distance(destination) < 45f)
                {
                    player.AddBuff(BuffID.Bleeding, 180, true);
                    player.AddBuff(ModContent.BuffType<HeavyBleeding>(), 30, true);
                }
                else if (!NPC.wet && NPC.Distance(destination) > 85f)
                {
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                }
                return;
            }
            NPC.direction = NPC.spriteDirection = (NPC.velocity.X > 0).ToDirectionInt();
            if (!NPC.wet)
            {
                NPC.velocity.X *= 0.97f;
                NPC.velocity.Y += 0.5f;
                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= 300f)
                {
                    NPC.life = 0;
                    NPC.HitEffect();
                    NPC.active = false;
                    NPC.netUpdate = true;
                }
            }
            else if (player.wet && player.Calamity().waterLeechTarget == NPC.whoAmI)
            {
                float speed = player.wet ? 23f : 17f;
                float swimIntertia = 24f;
                if (CalamityWorld.downedPolterghast)
                {
                    speed *= 1.6f;
                    swimIntertia = 17f;
                }
                NPC.velocity = (NPC.velocity * (swimIntertia - 1f) + NPC.DirectionTo(player.Center) * speed) / swimIntertia;
            }
            else if (!player.wet)
            {
                NPC.velocity *= 0.9f;
            }

            if (NPC.Distance(player.Top) < ChasePromptDistance && player.active && !player.dead)
            {
                NPC.ai[0] = 1f;
                NPC.netUpdate = true;
            }
            NPC.dontTakeDamage = !player.wet;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !spawnInfo.Player.Calamity().ZoneSulphur || !Main.raining)
            {
                return 0f;
            }
            return 0.115f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 8; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
            }
        }

        public override void OnKill()
        {
			float dropChance = CalamityWorld.downedAquaticScourge ? 0.01f : 0.05f;
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ParasiticSceptor>(), dropChance);
        }
    }
}
