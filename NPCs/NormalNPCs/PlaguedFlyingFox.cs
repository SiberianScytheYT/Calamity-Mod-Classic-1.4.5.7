using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class PlaguedFlyingFox : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Melter");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers();
            value.PortraitPositionYOverride = -32f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("A flying fox overtaken by plague nanomachines, deadly plague oozes from its mouth.")
            });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.5f;
            NPC.aiStyle = 14;
            AIType = NPCID.GiantFlyingFox;
            AnimationType = NPCID.GiantFlyingFox;
            NPC.damage = 55;
            NPC.width = 38;
            NPC.height = 34;
            NPC.defense = 15;
            NPC.lifeMax = 500;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 10, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath4;
            NPC.buffImmune[BuffID.ShadowFlame] = true;
            NPC.buffImmune[BuffID.Venom] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<MelterBanner>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !NPC.downedGolemBoss || spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return SpawnCondition.HardmodeJungle.Chance * 0.09f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 300, true);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Plague, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Plague, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnKill()
        {
			DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<PlagueCellCluster>(), 1, 2);
        }
    }
}
