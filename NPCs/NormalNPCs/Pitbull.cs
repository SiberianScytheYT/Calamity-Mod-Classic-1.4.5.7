using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class Pitbull : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pitbull");
            Main.npcFrameCount[NPC.type] = 10;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 26;
            NPC.damage = 18;
            NPC.width = 46;
            NPC.height = 30;
            NPC.defense = 4;
            NPC.lifeMax = 60;
            NPC.knockBackResist = 0.3f;
            AnimationType = NPCID.Hellhound;
            AIType = NPCID.Wolf;
            NPC.value = Item.buyPrice(0, 0, 2, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath5;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<PitbullBanner>();
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !NPC.downedBoss1 || spawnInfo.Player.Calamity().ZoneSulphur)
            {
                return 0f;
            }
            return SpawnCondition.OverworldNightMonster.Chance * 0.045f;
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
            int bandageDropRate = CalamityWorld.defiled ? DropHelper.DefiledDropRateInt : Main.expertMode ? 50 : 100;
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ItemID.AdhesiveBandage, bandageDropRate, 1, 1);
        }
    }
}
