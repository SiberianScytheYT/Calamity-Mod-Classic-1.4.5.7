using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.NormalNPCs
{
	public class Cryon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryon");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = 42;
            NPC.width = 50;
            NPC.height = 64;
            NPC.defense = 10;
			NPC.DR_NERD(0.1f);
            NPC.lifeMax = 300;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CryonBanner>();
			NPC.coldDamage = true;
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override void AI()
        {
            CalamityAI.UnicornAI(NPC, Mod, false, CalamityWorld.death ? 6f : 4f, 5f, CalamityWorld.death ? 0.15f : 0.1f);
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y > 0f || NPC.velocity.Y < 0f)
            {
                NPC.spriteDirection = NPC.direction;
                NPC.frame.Y = frameHeight * 5;
                NPC.frameCounter = 0.0;
            }
            else
            {
                NPC.spriteDirection = NPC.direction;
                NPC.frameCounter += (double)(NPC.velocity.Length() / 2f);
                if (NPC.frameCounter > 12.0)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y >= frameHeight * 4)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneSnow &&
                !spawnInfo.Player.PillarZone() &&
                !spawnInfo.Player.ZoneDungeon &&
                !spawnInfo.Player.InSunkenSea() &&
                Main.hardMode && !spawnInfo.PlayerInTown && !spawnInfo.Player.ZoneOldOneArmy && !Main.snowMoon && !Main.pumpkinMoon ? 0.015f : 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Frostburn, 300, true);
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(ModContent.BuffType<GlacialState>(), 30, true);
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 92, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 15; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 92, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnKill()
        {
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofEleum>(), 0.5f);
        }
    }
}
