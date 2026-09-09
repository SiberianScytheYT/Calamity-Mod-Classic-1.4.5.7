using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
	public class Bohldohr : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bohldohr");
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple,
                new FlavorTextBestiaryInfoElement("He is here.")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = 150;
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 18;
            NPC.lifeMax = 300;
            NPC.knockBackResist = 0.95f;
            NPC.value = Item.buyPrice(0, 0, 10, 0);
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath35;
            NPC.behindTiles = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<BOHLDOHRBanner>();
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override void AI()
        {
            CalamityAI.UnicornAI(NPC, Mod, true, CalamityWorld.death ? 6f : 4f, 5f, 0.2f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            return SpawnCondition.JungleTemple.Chance * 0.05f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 155, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 155, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        { 
            // RIP LORDE
            // npcLoot.AddIf(() => CalamityWorld.downedSCal, ModContent.ItemType<NO>());
            npcLoot.Add(ItemID.LihzahrdBrick, 1, 10, 30);
            npcLoot.Add(ItemID.LunarTabletFragment, 7, 1, 3); //solar tablet fragment
            npcLoot.Add(ItemID.LihzahrdPowerCell, 50);
        }
    }
}
