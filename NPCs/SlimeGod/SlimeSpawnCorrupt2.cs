using CalRD.Events;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.SlimeGod
{
    public class SlimeSpawnCorrupt2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Corrupt Slime Spawn");
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 1;
			NPC.GetNPCDamage();
			NPC.width = 40;
            NPC.height = 30;
            NPC.defense = 4;
            NPC.lifeMax = 90;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 50000;
            }
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.CorruptSlime;
            NPC.alpha = 55;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.buffImmune[BuffID.OnFire] = true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ItemID.Vitamins, Main.expertMode ? 50 : 100);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Weak, 90, true);
        }
    }
}
