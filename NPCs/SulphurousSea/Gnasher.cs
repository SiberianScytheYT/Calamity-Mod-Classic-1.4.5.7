using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.SulphurousSea
{
	public class Gnasher : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gnasher");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.damage = 25;
            NPC.width = 50;
            NPC.height = 36;
            NPC.defense = 30;
			NPC.DR_NERD(0.15f);
            NPC.lifeMax = 35;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 3;
            AIType = 67;
            NPC.value = Item.buyPrice(0, 0, 0, 60);
            NPC.HitSound = SoundID.NPCHit50;
            NPC.DeathSound = SoundID.NPCDeath54;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<GnasherBanner>();
        }

        public override void AI()
        {
            NPC.spriteDirection = (NPC.direction > 0) ? -1 : 1;
            float num79 = (Main.player[NPC.target].Center - NPC.Center).Length();
            num79 *= 0.0025f;
            if ((double)num79 > 1.5)
            {
                num79 = 1.5f;
            }
            float num78;
            if (Main.expertMode)
            {
                num78 = 2.5f - num79;
            }
            else
            {
                num78 = 2.25f - num79;
            }
            num78 *= (CalamityWorld.death ? 1.2f : 0.8f);
            if (NPC.velocity.X < -num78 || NPC.velocity.X > num78)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity *= 0.8f;
                }
            }
            else if (NPC.velocity.X < num78 && NPC.direction == 1)
            {
                NPC.velocity.X = NPC.velocity.X + 1f;
                if (NPC.velocity.X > num78)
                {
                    NPC.velocity.X = num78;
                }
            }
            else if (NPC.velocity.X > -num78 && NPC.direction == -1)
            {
                NPC.velocity.X = NPC.velocity.X - 1f;
                if (NPC.velocity.X < -num78)
                {
                    NPC.velocity.X = -num78;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 120, true);
            target.AddBuff(BuffID.Venom, 120, true);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            if (spawnInfo.Player.Calamity().ZoneSulphur)
            {
                return 0.2f;
            }
            return 0f;
        }

        public override void OnKill()
        {
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ItemID.TurtleShell, Main.hardMode, 10, 1, 1);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 15; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gnasher").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gnasher2").Type, 1f);
                }
            }
        }
    }
}
