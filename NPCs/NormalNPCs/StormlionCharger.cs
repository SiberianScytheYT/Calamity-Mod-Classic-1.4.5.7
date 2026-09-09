using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Summon;
using CalRD.World;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class StormlionCharger : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stormlion");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1.2f
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new FlavorTextBestiaryInfoElement("This variant of antlion is very unusual with its feeding; As it takes to the surface and reaches upwards with its mandibles to act as lightning rods, feeding off the storms' lightning.")
            });
        }

        public override void SetDefaults()
        {
            NPC.damage = 20;
            NPC.aiStyle = 3;
            NPC.width = 33;
            NPC.height = 31;
            NPC.defense = 8;
            NPC.lifeMax = 80;
            NPC.knockBackResist = 0.2f;
            AnimationType = NPCID.WalkingAntlion;
            NPC.value = Item.buyPrice(0, 0, 2, 0);
            NPC.HitSound = SoundID.NPCHit31;
            NPC.DeathSound = SoundID.NPCDeath34;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<StormlionBanner>();
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
            if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return SpawnCondition.DesertCave.Chance * 0.2f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Electrified, 90, true);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<StormjawStaff>(), 5);
            //100% chance if DS isn't dead yet, otherwise 50%
            npcLoot.AddIf(() => !CalamityWorld.downedDesertScourge, ModContent.ItemType<StormlionMandible>());
            npcLoot.AddIf(() => CalamityWorld.downedDesertScourge, ModContent.ItemType<StormlionMandible>(), 2);
        }
    }
}
