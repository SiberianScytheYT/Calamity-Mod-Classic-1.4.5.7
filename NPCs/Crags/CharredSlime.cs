using CalRD.BiomeManagers;
using CalRD.World;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Placeables.Ores;
using CalRD.Items.Materials;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Crags
{
    public class CharredSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Charred Slime");
            Main.npcFrameCount[NPC.type] = 2;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("A slime infused with Charred Ore, it's a perfect match!")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 1;
			AIType = NPCID.LavaSlime;
            NPC.damage = 40;
            NPC.width = 40;
            NPC.height = 30;
            NPC.defense = 10;
            NPC.lifeMax = 250;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.CorruptSlime;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.alpha = 50;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            if (CalamityWorld.downedProvidence)
            {
                NPC.damage = 80;
                NPC.defense = 20;
                NPC.lifeMax = 3500;
            }
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CharredSlimeBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Crag>().Type };
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!CalamityWorld.downedBrimstoneElemental)
            {
                return 0f;
            }
            return spawnInfo.Player.Calamity().ZoneCalamity ? 0.08f : 0f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnKill()
        {
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CharredOre>(), 10, 26);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Bloodstone>(), CalamityWorld.downedProvidence, 2, 1, 1);
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofChaos>(), 3, 1, 1);
        }
    }
}
