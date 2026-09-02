using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class SunBat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sun Bat");
            Main.npcFrameCount[NPC.type] = 7;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("A light in the dark.")
            });
        }

        public override void SetDefaults()
        {
            NPC.lavaImmune = true;
            NPC.aiStyle = 14;
            AIType = NPCID.Lavabat;
            NPC.damage = 35;
            NPC.width = 26;
            NPC.height = 20;
            NPC.defense = 10;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0.65f;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath4;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SunBatBanner>();
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            NPC.spriteDirection = (NPC.direction > 0) ? 1 : -1;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !Main.hardMode || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return SpawnCondition.Underground.Chance * 0.12f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire, 120, true);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120, true);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 64, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 64, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnKill()
        {
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofCinder>(), 3);
        }
    }
}
