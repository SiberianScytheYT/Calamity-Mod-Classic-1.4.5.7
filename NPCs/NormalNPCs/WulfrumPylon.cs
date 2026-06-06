using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader.Utilities;

namespace CalRD.NPCs.NormalNPCs
{
    public class WulfrumPylon : ModNPC
    {
        public bool Charging
        {
            get => NPC.ai[0] != 0f;
            set => NPC.ai[0] = value.ToInt();
        }
        public float ChargeRadius
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public static List<int> SuperchargableEnemies = new List<int>()
        {
            ModContent.NPCType<WulfrumDrone>(),
            ModContent.NPCType<WulfrumGyrator>(),
            ModContent.NPCType<WulfrumHovercraft>()
        };
        public const float ChargeRadiusMax = 200f;
        public const float SuperchargeTime = 600f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wulfrum Pylon");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            AIType = -1;
            NPC.aiStyle = -1;
            NPC.damage = 16;
            NPC.width = 44;
            NPC.height = 44;
            NPC.defense = 4;
            NPC.lifeMax = 49;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 1, 50);
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.buffImmune[BuffID.Confused] = false;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<WulfrumPylonBanner>();
        }

        public override void AI()
        {
            NPC.TargetClosest(false);

            Player player = Main.player[NPC.target];

            if (!Charging && NPC.Distance(player.Center) < ChargeRadiusMax * 0.667f)
            {
                Charging = true;
                NPC.netUpdate = true;
            }
            else if (Charging)
            {
                ChargeRadius = (int)MathHelper.Lerp(ChargeRadius, ChargeRadiusMax, 0.1f);

                if (Main.rand.NextBool(4))
                {
                    float dustCount = MathHelper.TwoPi * ChargeRadius / 5f;
                    for (int i = 0; i < dustCount; i++)
                    {
                        float angle = MathHelper.TwoPi * i / dustCount;
                        Dust dust = Dust.NewDustPerfect(NPC.Center, 229);
                        dust.position = NPC.Center + angle.ToRotationVector2() * ChargeRadius;
                        dust.noGravity = true;
                        dust.velocity = NPC.velocity;
                    }
                }

                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npcAtIndex = Main.npc[i];
                    if (!npcAtIndex.active)
                        continue;

                    // For some strange reason, the Wulfrum Rover is not counted when it's added to a static list.
                    // What I assume is going on is that it hasn't been loaded yet since it's later alphabetically (Pylon is before Rover).
                    // As a result, they are checked separately.
                    if (!SuperchargableEnemies.Contains(npcAtIndex.type) &&
                        npcAtIndex.type != ModContent.NPCType<WulfrumRover>())
                        continue;
                    if (npcAtIndex.ai[3] > 0f)
                        continue;
                    if (NPC.Distance(npcAtIndex.Center) > ChargeRadius)
                        continue;

                    npcAtIndex.ai[3] = SuperchargeTime; // Supercharge the npc for a while if isn't already supercharged.
                    npcAtIndex.netUpdate = true;

                    // And emit some dust.

                    // Dust doesn't need to be spawned for the server.
                    if (Main.dedServ)
                        continue;

                    for (int j = 0; j < 10; j++)
                    {
                        Dust.NewDust(npcAtIndex.position, npcAtIndex.width, npcAtIndex.height, 226);
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            int frame = (int)(NPC.frameCounter / 8) % Main.npcFrameCount[NPC.type];

            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float pylonMult = !NPC.AnyNPCs(ModContent.NPCType<WulfrumPylon>()) ? 1.3f : 1f;

            if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneSulphur)
                return 0f;

            // Spawn less frequently in the inner third of the world.
            if (spawnInfo.PlayerFloorX > Main.maxTilesX * 0.333f && spawnInfo.PlayerFloorX < Main.maxTilesX - Main.maxTilesX * 0.333f)
                return SpawnCondition.OverworldDaySlime.Chance * (Main.hardMode ? 0.01f : 0.06f) * pylonMult;

            return SpawnCondition.OverworldDaySlime.Chance * (Main.hardMode ? 0.06f : 0.15f) * pylonMult;
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

        public override void OnKill()
        {
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<WulfrumShard>(), 2, 3);
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EnergyCore>());
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<WulfrumBattery>(), 0.07f);
        }
    }
}
