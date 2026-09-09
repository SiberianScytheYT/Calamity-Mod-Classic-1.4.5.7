using CalRD.BiomeManagers;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.StatDebuffs;
using CalRD.Buffs.DamageOverTime;
using Terraria.GameContent.Bestiary;

namespace CalRD.NPCs.AcidRain
{
	public class Radiator : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Radiator");
            Main.npcFrameCount[NPC.type] = 4;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("It ingests the potent toxins found in its home and displays them on its back, touching them would be a bad idea.")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 67;
            NPC.damage = 10;
            NPC.width = 24;
            NPC.height = 24;
            NPC.defense = 5;
            NPC.lifeMax = 50;

            if (CalamityWorld.downedPolterghast)
            {
                NPC.damage = 60;
                NPC.lifeMax = 3250;
                NPC.defense = 20;
            }
            else if (CalamityWorld.downedAquaticScourge)
            {
                NPC.damage = 30;
                NPC.lifeMax = 130;
                NPC.defense = 10;
            }

            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			AIType = NPCID.GlowingSnail;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<RadiatorBanner>();
            NPC.catchItem = (short)ModContent.ItemType<RadiatingCrystal>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };
        }

        public override void AI()
        {			
            Lighting.AddLight(NPC.Center, 0.3f, 1.5f, 0.3f);

            int auraSize = 200; //roughly 12 blocks (half the size of Wither Beast aura)
			Player player = Main.player[Main.myPlayer];
			if (!player.dead && player.active && (double) (player.Center - NPC.Center).Length() < auraSize)
			{
                player.AddBuff(ModContent.BuffType<Irradiated>(), 3, false);
                player.AddBuff(BuffID.Poisoned, 2, false);
				if (CalamityWorld.downedPolterghast)
				{
                    player.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 3, false);
                    player.AddBuff(BuffID.Venom, 2, false);
				}
			}
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 8)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y > frameHeight * 2)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddIf(() => CalamityWorld.downedAquaticScourge, ModContent.ItemType<SulfuricScale>(), 12, 1, 3);
            npcLoot.AddIf(() => !CalamityWorld.downedAquaticScourge, ModContent.ItemType<SulfuricScale>(), 2, 1, 3);
        }
    }
}
