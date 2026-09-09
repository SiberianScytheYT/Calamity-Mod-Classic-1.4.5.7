using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;

namespace CalRD.NPCs.NormalNPCs
{
    internal enum DroneAIState
    {
        Searching = 0,
        Charging = 1
    }
    public class WulfrumDrone : ModNPC
    {
        internal DroneAIState AIState
        {
            get => (DroneAIState)(int)NPC.ai[0];
            set => NPC.ai[0] = (int)value;
        }
        public float HorizontalChargeTime
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float Time
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float SuperchargeTimer
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        public bool Supercharged => SuperchargeTimer > 0;
        public ref float FlyAwayTimer => ref NPC.localAI[0];

        public const float TotalHorizontalChargeTime = 75f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wulfrum Drone");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                SpriteDirection = 1
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A strange little mechanism of unknown origin, they try to keep away intruders from their territory by swooping down at them.")
            });
        }

        public override void SetDefaults()
        {
            AIType = -1;
            NPC.aiStyle = -1;
            NPC.damage = 16;
            NPC.width = 32;
            NPC.height = 32;
            NPC.defense = 4;
            NPC.lifeMax = 21;
            NPC.knockBackResist = 0.35f;
            NPC.value = Item.buyPrice(0, 0, 1, 20);
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<WulfrumDroneBanner>();
        }

        public override void SendExtraAI(BinaryWriter writer) => writer.Write(FlyAwayTimer);

        public override void ReceiveExtraAI(BinaryReader reader) => FlyAwayTimer = reader.ReadSingle();

        public override void AI()
        {
            NPC.TargetClosest(false);

            Player player = Main.player[NPC.target];

            bool farFromPlayer = NPC.Distance(player.Center) > 640f;
            bool obstanceInFrontOfPlayer = !Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);

            if (NPC.target < 0 || NPC.target >= 255 || farFromPlayer || obstanceInFrontOfPlayer || player.dead || !player.active)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                farFromPlayer = NPC.Distance(player.Center) > 640f;
                obstanceInFrontOfPlayer = !Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
                // Fly away if there is no living target, or the closest target is too far away.
                if (player.dead || !player.active || farFromPlayer || obstanceInFrontOfPlayer)
                {
                    if (FlyAwayTimer > 480)
					{
                        NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.UnitY * -8f, 0.1f);
                        NPC.rotation = NPC.rotation.AngleTowards(0f, MathHelper.ToRadians(15f));
                        NPC.noTileCollide = true;
                    }
                    else
                    {
                        NPC.velocity *= 0.96f;
                        NPC.rotation = NPC.rotation.AngleTowards(0f, MathHelper.ToRadians(15f));
                        FlyAwayTimer++;
                    }
                    return;
                }
            }

            FlyAwayTimer = Utils.Clamp(FlyAwayTimer - 3, 0, 180);

            NPC.noTileCollide = !farFromPlayer;

            if (Supercharged)
            {
                SuperchargeTimer--;
            }

            if (AIState == DroneAIState.Searching)
            {
                if (NPC.direction == 0)
                    NPC.direction = 1;
                Vector2 destination = player.Center + new Vector2(300f * NPC.direction, -90f);
                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(destination) * 8f, 0.1f);
                if (NPC.Distance(destination) < 40f)
                {
                    Time++;
                    NPC.velocity *= 0.95f;
                    if (Time >= 40f)
                    {
                        AIState = DroneAIState.Charging;
                        NPC.netUpdate = true;
                    }
                }
            }
            else
            {
                if (HorizontalChargeTime < 25)
                {
                    NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 8f, 0.1f);
                }
                if (Supercharged && Main.netMode != NetmodeID.MultiplayerClient && HorizontalChargeTime % 30f == 29f)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + Vector2.UnitX * 6f * NPC.spriteDirection, NPC.DirectionTo(player.Center) * 6f, ProjectileID.MartianTurretBolt, 14, 0f);
                }
                HorizontalChargeTime++;
                if (HorizontalChargeTime > TotalHorizontalChargeTime)
                {
                    AIState = DroneAIState.Searching;
                    HorizontalChargeTime = 0f;
                    NPC.direction = (player.Center.X - NPC.Center.X < 0).ToDirectionInt();
                    NPC.netUpdate = true;
                }
            }
            NPC.spriteDirection = (NPC.velocity.X < 0).ToDirectionInt();
            NPC.rotation = NPC.velocity.X / 25f;

            // Generate idle dust
            if (!Main.dedServ)
            {
                Dust dust = Dust.NewDustPerfect(NPC.Bottom, 229);
                dust.color = Color.Green;
                dust.scale = 0.675f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            int frame = (int)(NPC.frameCounter / 5) % (Main.npcFrameCount[NPC.type] / 2);
            if (Supercharged)
                frame += Main.npcFrameCount[NPC.type] / 2;

            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float pylonMult = NPC.AnyNPCs(ModContent.NPCType<WulfrumPylon>()) ? 3f : 1f;
            if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneSulphur)
                return 0f;

            return SpawnCondition.OverworldDaySlime.Chance * (Main.hardMode ? 0.025f : 0.1f) * pylonMult;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 15; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<WulfrumShard>(), 1, 1, 3);
            npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<WulfrumShard>());
            npcLoot.Add(ModContent.ItemType<WulfrumBattery>(), 14);
			npcLoot.AddIf(() => Supercharged, ModContent.ItemType<EnergyCore>());
        }
    }
}
