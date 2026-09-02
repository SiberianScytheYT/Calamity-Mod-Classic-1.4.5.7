using CalRD.Items.Placeables.Banners;
using CalRD.Items.Placeables.Ores;
using CalRD.World;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class AeroSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aero Slime");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers();
            value.Position.Y -= 6;
            value.PortraitPositionYOverride = -36f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("The essence of Aerialite Ore has allowed this slime to take to the skies!")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 14;
            NPC.damage = 18;
            NPC.width = 40;
            NPC.height = 30;
            NPC.defense = 6;
            NPC.lifeMax = 50;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.Slimer;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.alpha = 50;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<AeroSlimeBanner>();
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || (!CalamityWorld.downedHiveMind && !CalamityWorld.downedPerforator) || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return SpawnCondition.Cavern.Chance * 0.05f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 59, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 59, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnKill()
        {
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AerialiteOre>(), 10, 26);
        }
    }
}
