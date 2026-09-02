using CalRD.BiomeManagers;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Crags
{
    public class CultistAssassin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cultist Assassin");
            Main.npcFrameCount[NPC.type] = 4;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("A devotee brought to madness by the power of the brimstone flame.")
            });
        }

        public override void SetDefaults()
        {
            NPC.lavaImmune = true;
            NPC.aiStyle = 3;
            NPC.damage = 50;
            NPC.width = 18;
            NPC.height = 40;
            NPC.defense = 25;
            NPC.lifeMax = 80;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.ZombieXmas;
            AIType = NPCID.ChaosElemental;
            NPC.value = Item.buyPrice(0, 0, 10, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath50;
            if (CalamityWorld.downedProvidence)
            {
                NPC.damage = 100;
                NPC.defense = 40;
                NPC.lifeMax = 3000;
            }
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CultistAssassinBanner>();
			NPC.buffImmune[BuffID.Confused] = false;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Crag>().Type };
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.Player.Calamity().ZoneCalamity || spawnInfo.Player.ZoneDungeon) && Main.hardMode ? 0.04f : 0f;
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
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Bloodstone>(), CalamityWorld.downedProvidence, 2, 1, 1);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofChaos>(), Main.hardMode, 3, 1, 1);
        }
    }
}
