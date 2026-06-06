using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Crags
{
	public class DespairStone : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Despair Stone");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = 40;
            NPC.width = 72;
            NPC.height = 72;
            NPC.defense = 38;
			NPC.DR_NERD(0.35f);
            NPC.lifeMax = 120;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.behindTiles = true;
            NPC.lavaImmune = true;
            if (CalamityWorld.downedProvidence)
            {
                NPC.damage = 80;
                NPC.defense = 50;
                NPC.lifeMax = 3000;
            }
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<DespairStoneBanner>();
			NPC.buffImmune[BuffID.Confused] = false;
        }

        public override void AI()
        {
            CalamityAI.UnicornAI(NPC, Mod, true, CalamityWorld.death ? 6f : 4f, 5f, 0.2f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.Calamity().ZoneCalamity ? 0.25f : 0f;
        }

        public override void OnKill()
        {
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Bloodstone>(), CalamityWorld.downedProvidence, 2, 1, 1);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofChaos>(), Main.hardMode, 3, 1, 1);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 40; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
