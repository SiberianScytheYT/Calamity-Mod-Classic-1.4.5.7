using CalRD.Events;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.DesertScourge
{
    public class DriedSeekerHead : ModNPC
    {
        bool TailSpawned = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dried Seeker");
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.7f,
                PortraitScale = 0.7f,
                CustomTexturePath = "CalRD/ExtraTextures/Bestiary/DriedSeeker_Bestiary",
                PortraitPositionXOverride = 40f,
                PortraitPositionYOverride = 40f
            };
            value.Position.X += 50f;
            value.Position.Y += 35f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
		
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("A small sea serpent that suffered much the same fate as the Desert Scourge, it serves as a companion in its endless search for food.")
            });
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 18;
            NPC.height = 18;
            NPC.lifeMax = 100;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 50000;
            }
            NPC.aiStyle = 6;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
        }

        public override void AI()
        {
            if (!TailSpawned)
            {
                int Previous = NPC.whoAmI;
                for (int num36 = 0; num36 < 4; num36++)
                {
                    int lol;
                    if (num36 >= 0 && num36 < 3)
                    {
                        lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<DriedSeekerBody>(), NPC.whoAmI);
                    }
                    else
                    {
                        lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<DriedSeekerTail>(), NPC.whoAmI);
                    }
                    Main.npc[lol].realLife = NPC.whoAmI;
                    Main.npc[lol].ai[2] = (float)NPC.whoAmI;
                    Main.npc[lol].ai[1] = (float)Previous;
                    Main.npc[Previous].ai[0] = (float)lol;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, lol, 0f, 0f, 0f, 0);
                    Previous = lol;
                }
                TailSpawned = true;
            }
            if (Main.player[NPC.target].dead)
            {
                NPC.velocity.Y = NPC.velocity.Y + 1f;
                if ((double)NPC.position.Y > Main.worldSurface * 16.0)
                {
                    NPC.velocity.Y = NPC.velocity.Y + 1f;
                }
                if ((double)NPC.position.Y > Main.rockLayer * 16.0)
                {
                    for (int num957 = 0; num957 < 200; num957++)
                    {
                        if (Main.npc[num957].aiStyle == NPC.aiStyle)
                        {
                            Main.npc[num957].active = false;
                        }
                    }
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override bool PreKill()
        {
            return false;
        }
    }
}
