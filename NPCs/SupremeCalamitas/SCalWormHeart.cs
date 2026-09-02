using CalRD.Projectiles.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.SupremeCalamitas
{
	public class SCalWormHeart : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Heart");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 24;
            NPC.height = 24;
            NPC.defense = 0;
			NPC.LifeMaxNERB(160000, 180000, 90000);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.alpha = 255;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            if (CalamityGlobalNPC.SCal < 0 || !Main.npc[CalamityGlobalNPC.SCal].active)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }
            NPC.alpha -= 42;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ModContent.ProjectileType<Celestus2>())
            {
                projectile.damage = (int)(projectile.damage * 0.66);
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreKill()
        {
            return false;
        }
    }
}
