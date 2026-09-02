using CalRD.BiomeManagers;
using CalRD.Items.SummonItems;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.AcidRain
{
    public class BloodwormNormal : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodworm");
            Main.npcFrameCount[NPC.type] = 7;
            Main.npcCatchable[NPC.type] = true;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("An unnaturally large worm... and a great feast for any fish mad or wise enough to go for it.")
            });
        }

        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 36;
            NPC.height = 16;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.catchItem = (short)ModContent.ItemType<BloodwormItem>();
            NPC.dontTakeDamageFromHostiles = true;
			NPC.rarity = 4;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };
        }

        public override void AI()
        {
			if (NPC.collideY)
            {
                if (NPC.ai[0] == 0f)
                {
                    NPC.ai[0] = Main.rand.NextBool(2).ToDirectionInt();
                    NPC.netUpdate = true;
                }
                if (NPC.collideX)
                {
                    NPC.ai[0] *= -1;
                }
            }
            float xSpeed = 3f;
            NPC.velocity.X = xSpeed * NPC.ai[0];
            NPC.spriteDirection = (int)NPC.ai[0];
			bool flee = false;
			for (int i = 0; i < Main.player.Length; i++)
			{
				Player player = Main.player[i];
				if (player.active && !player.dead && Vector2.Distance(player.Center, NPC.Center) <= 220f)
				{
					flee = true;
					break;
				}
			}
			int timeBeforeFlee = 60;
			if (flee && NPC.ai[1] < timeBeforeFlee)
			{
				NPC.ai[1] += 1f;
			}
			if (NPC.ai[1] == timeBeforeFlee && Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.position.Y += 16f;
				NPC.Transform(ModContent.NPCType<BloodwormFleeing>());
				NPC.netUpdate = true;
				return;
			}
		}

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.Player.Calamity().ZoneSulphur && CalamityWorld.encounteredOldDuke && !CalamityWorld.rainingAcid) ? SpawnCondition.WormCritter.Chance * 2.569f : 0f;
        }
    }
}
