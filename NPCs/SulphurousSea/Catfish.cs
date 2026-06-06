using CalRD.Items.Placeables.Banners;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.SulphurousSea
{
	public class Catfish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Catfish");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.damage = 30;
            NPC.width = 98;
            NPC.height = 40;
            NPC.defense = 12;
            NPC.lifeMax = 120;
            NPC.aiStyle = -1;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath40;
            NPC.knockBackResist = 0.8f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CatfishBanner>();
            NPC.chaseable = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.chaseable = reader.ReadBoolean();
        }

        public override void AI()
        {
            CalamityAI.PassiveSwimmingAI(NPC, Mod, 0, Main.player[NPC.target].Calamity().GetAbyssAggro(200f, 150f), 0.25f, 0.15f, 8f, 8f, 0.1f);
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.minion && !projectile.Calamity().overridesMinionDamagePrevention)
            {
                return NPC.chaseable;
            }
            return null;
        }

        public override void FindFrame(int frameHeight)
        {
            if (!NPC.wet)
            {
                NPC.frameCounter = 0.0;
                return;
            }
            NPC.frameCounter += NPC.chaseable ? 0.15f : 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 180, true);
            target.AddBuff(BuffID.Venom, 180, true);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            if (spawnInfo.Player.Calamity().ZoneSulphur && spawnInfo.Water)
            {
                return 0.2f;
            }
            return 0f;
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ItemID.DivingHelmet, 20);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 25; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
