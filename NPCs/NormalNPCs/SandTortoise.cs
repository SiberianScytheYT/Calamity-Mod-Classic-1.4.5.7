using CalRD.Items.Placeables.Banners;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class SandTortoise : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sand Tortoise");
            Main.npcFrameCount[NPC.type] = 8;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new FlavorTextBestiaryInfoElement("Like their jungle and tundra cousins, they prove quite the serious threat with their high speed lunges.")
            });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 2f;
            NPC.damage = 70;
            NPC.aiStyle = 39;
            NPC.width = 46;
            NPC.height = 32;
            NPC.defense = 30;
			NPC.DR_NERD(0.25f);
            NPC.scale = 1.5f;
            NPC.lifeMax = 580;
            NPC.knockBackResist = 0.2f;
            AnimationType = NPCID.GiantTortoise;
            NPC.value = Item.buyPrice(0, 0, 15, 0);
            NPC.HitSound = SoundID.NPCHit24;
            NPC.DeathSound = SoundID.NPCDeath27;
            NPC.noGravity = false;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SandTortoiseBanner>();
            NPC.buffImmune[BuffID.Confused] = false;
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

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !Main.hardMode || spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return SpawnCondition.DesertCave.Chance * 0.05f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(ItemID.TurtleShell, 10);
    }
}
