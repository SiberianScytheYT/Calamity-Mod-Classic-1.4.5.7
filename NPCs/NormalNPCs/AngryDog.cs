using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Materials;
using CalRD.World;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.NormalNPCs
{
	public class AngryDog : ModNPC
    {
        private bool reset = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Angry Dog");
            Main.npcFrameCount[NPC.type] = 9;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.7f,
                PortraitPositionXOverride = 10f,
                Velocity = 1.2f
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement("very angy boi")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 10;
            NPC.width = 56;
            NPC.height = 56;
            NPC.defense = 4;
            NPC.lifeMax = 50;
            if (CalamityWorld.downedCryogen)
            {
                NPC.damage = 60;
                NPC.defense = 10;
                NPC.lifeMax = 1000;
            }
            NPC.knockBackResist = 0.3f;
            AnimationType = NPCID.Hellhound;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 3, 0);
            NPC.HitSound = new SoundStyle("CalRD/Sounds/NPCHit/AngryDogHit");
            NPC.DeathSound = SoundID.NPCDeath5;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<AngryDogBanner>();
			NPC.coldDamage = true;
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(reset);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            reset = reader.ReadBoolean();
        }

        public override void AI()
        {
            if (Main.rand.NextBool(900))
            {
                SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/AngryDogGrowl"), NPC.position);
            }
            bool phase2 = (double)NPC.life <= (double)NPC.lifeMax * (CalamityWorld.death ? 0.9 : 0.5);
            if (phase2)
            {
                if (!reset)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[3] = 0f;
                    reset = true;
                    NPC.netUpdate = true;
                }
                if (NPC.ai[1] < 7f)
                {
                    NPC.ai[1] += 1f;
                }
				CalamityAI.UnicornAI(NPC, Mod, true, CalamityWorld.death ? 6f : 4f, 5f, 0.2f);
                return;
            }
			CalamityAI.UnicornAI(NPC, Mod, false, CalamityWorld.death ? 6f : 4f, 6f, CalamityWorld.death ? 0.1f : 0.07f);
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[1] < 7f && NPC.ai[1] > 0f)
            {
                NPC.frame.Y = frameHeight * 7;
                NPC.frameCounter = 0.0;
                return;
            }
            if (NPC.ai[1] >= 7f)
            {
                NPC.frame.Y = frameHeight * 8;
                NPC.frameCounter = 0.0;
                return;
            }
            if (NPC.velocity.Y > 0f || NPC.velocity.Y < 0f)
            {
                NPC.frame.Y = frameHeight * 5;
                NPC.frameCounter = 0.0;
            }
            else
            {
                NPC.spriteDirection = NPC.direction;
                NPC.frameCounter += (double)(NPC.velocity.Length() / 16f);
                if (NPC.frameCounter > 12.0)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y >= frameHeight * 6)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneSnow &&
                !spawnInfo.Player.PillarZone() &&
                !spawnInfo.Player.ZoneDungeon &&
                !spawnInfo.Player.InSunkenSea() &&
                !spawnInfo.PlayerInTown && !spawnInfo.Player.ZoneOldOneArmy && !Main.snowMoon && !Main.pumpkinMoon ? 0.012f : 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 180, true);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ItemID.Leather, 1, 1, 2);
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Cryophobia>(), 100);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofEleum>(), CalamityWorld.downedCryogen, 3, 1, 1);
        }
    }
}
