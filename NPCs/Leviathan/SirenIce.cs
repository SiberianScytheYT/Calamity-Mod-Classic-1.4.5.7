using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Leviathan
{
	public class SirenIce : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ice Shield");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
	            Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.canGhostHeal = false;
            NPC.noTileCollide = true;
			NPC.GetNPCDamage();
			NPC.width = 100;
            NPC.height = 100;
            NPC.defense = 10;
			NPC.DR_NERD(0.5f);
            NPC.lifeMax = 650;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 10000;
            }
            NPC.alpha = 255;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
        }

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.dontTakeDamage);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.dontTakeDamage = reader.ReadBoolean();
		}

		public override void AI()
        {
			int num989 = (int)NPC.ai[0];
			if (Main.npc[num989].active && Main.npc[num989].type == ModContent.NPCType<Siren>())
            {
				if (NPC.alpha > 100 && NPC.ai[1] == 0f)
					NPC.alpha -= 2;

				if (Main.npc[num989].damage == 0)
					NPC.ai[1] = 1f;
				else
					NPC.ai[1] = 0f;

				if (NPC.ai[1] == 1f)
					NPC.alpha = Main.npc[num989].alpha;

				NPC.dontTakeDamage = Main.npc[num989].damage == 0;
				NPC.rotation = Main.npc[num989].rotation;
                NPC.spriteDirection = Main.npc[num989].direction;
                NPC.velocity = Vector2.Zero;
                NPC.position = Main.npc[num989].Center;
                NPC.position.X = NPC.position.X - (NPC.width / 2) + ((NPC.spriteDirection == 1) ? -20f : 20f);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2) - 30;
                NPC.gfxOffY = Main.npc[num989].gfxOffY;
                Lighting.AddLight((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16, 0f, 0.8f, 1.1f);
                return;
            }

			NPC.dontTakeDamage = false;
            NPC.life = 0;
            NPC.HitEffect(0, 10.0);
            NPC.active = false;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (CalamityLists.projectileDestroyExceptionList.TrueForAll(x => projectile.type != x))
            {
                if (projectile.penetrate == -1 && !projectile.minion)
                {
                    projectile.penetrate = 1;
                }
                else if (projectile.penetrate >= 1)
                {
                    projectile.penetrate = 1;
                }
            }
        }

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => NPC.ai[1] == 0f;

		public override Color? GetAlpha(Color drawColor)
        {
			if (NPC.ai[1] == 1f)
				return Color.Transparent;

			return new Color(200, 200, 200, NPC.alpha);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
			if (NPC.ai[1] == 1f)
				return;

			target.AddBuff(BuffID.Frostburn, 240, true);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 67, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 25; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 67, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
