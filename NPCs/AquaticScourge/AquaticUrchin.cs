using CalRD.BiomeManagers;
using CalRD.Events;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Rogue;
using CalRD.World;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.AquaticScourge
{
	public class AquaticUrchin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquatic Urchin");
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A spiny animal native to the ocean that flings itself around relentlessly in defense of its territory, though mostly helpless when outside water, given enough time they can make it back home.")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = Main.hardMode ? 50 : 25;
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 10;
            NPC.lifeMax = Main.hardMode ? 300 : 50;
            NPC.knockBackResist = 0.8f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.value = Item.buyPrice(0, 0, 0, 80);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath15;
            NPC.behindTiles = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<AquaticUrchinBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type };
        }

        public override void AI()
        {
            CalamityAI.UnicornAI(NPC, Mod, true, NPC.wet ? 12f : 4f, NPC.wet ? 7f : 3f, NPC.wet ? 0.12f : 0.05f, NPC.wet ? -16f : -7.5f, NPC.wet ? -14f : -6.5f, NPC.wet ? -13f : -6f, NPC.wet ? -12f : -5f, NPC.wet ? -15f : -7f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Venom, 120, true);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            if (spawnInfo.Player.Calamity().ZoneSulphur && spawnInfo.Water && NPC.CountNPCS(ModContent.NPCType<AquaticUrchin>()) < 2)
            {
                return 0.2f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(ModContent.ItemType<UrchinStinger>(), 1, 15, 25);

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
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AquaticUrchin").Type, 1f);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AquaticUrchin2").Type, 1f);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AquaticUrchin3").Type, 1f);
            }
        }
    }
}
