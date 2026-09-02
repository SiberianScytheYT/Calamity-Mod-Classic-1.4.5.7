using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class EbonianBlightSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ebonian Blight Slime");
            Main.npcFrameCount[NPC.type] = 4;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement("A slime blessed by a higher being, and one of the few of its kind not to consume for itself.")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 1;
			AIType = NPCID.DungeonSlime;
			NPC.damage = 30;
            NPC.width = 60;
            NPC.height = 42;
            NPC.defense = 8;
            NPC.lifeMax = 130;
            NPC.knockBackResist = 0.3f;
            AnimationType = NPCID.RainbowSlime;
            NPC.value = Item.buyPrice(0, 0, 2, 0);
            NPC.alpha = 105;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Confused] = false;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<EbonianBlightSlimeBanner>();
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 14, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 40; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 14, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneAbyss)
            {
                return 0f;
            }
            return SpawnCondition.Corruption.Chance * 0.15f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.ManaSickness, 120, true);
            target.AddBuff(BuffID.Weak, 120, true);
        }

        public override void OnKill()
        {
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EbonianGel>(), 15, 20);
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.Gel, 10, 14);
        }
    }
}
