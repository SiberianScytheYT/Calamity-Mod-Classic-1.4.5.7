using CalRD.Events;
using CalRD.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Cryogen
{
	public class CryogenIce : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryogen's Shield");
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
			NPC.width = 190;
            NPC.height = 190;
			NPC.DR_NERD(0.4f);
            NPC.lifeMax = 1400;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 100000;
            }
            NPC.alpha = 255;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
        }

        public override void AI()
        {
			NPC.alpha -= 3;
            if (NPC.alpha < 0)
                NPC.alpha = 0;

            NPC.rotation += 0.15f;

            if (NPC.type == ModContent.NPCType<CryogenIce>())
            {
                int num989 = (int)NPC.ai[0];
                if (Main.npc[num989].active && Main.npc[num989].type == ModContent.NPCType<Cryogen>())
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.position = Main.npc[num989].Center;
                    NPC.position.X = NPC.position.X - (NPC.width / 2);
                    NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                    NPC.gfxOffY = Main.npc[num989].gfxOffY + 14;
                    return;
                }
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.alpha == 0;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Frostburn, 90, true);
            target.AddBuff(BuffID.Chilled, 60, true);
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * balance);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 67, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int num621 = 0; num621 < 25; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 67, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }

                for (int num623 = 0; num623 < 50; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 67, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 67, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int totalProjectiles = 4;
					float radians = MathHelper.TwoPi / totalProjectiles;
					int type = ModContent.ProjectileType<IceBlast>();
					int damage2 = NPC.GetProjectileDamage(type);
					float velocity = BossRushEvent.BossRushActive ? 12f : 8f;
					Vector2 spinningPoint = Main.rand.NextBool(2) ? new Vector2(0f, -velocity) : Vector2.Normalize(new Vector2(-velocity, -velocity)) * velocity;
					for (int k = 0; k < totalProjectiles; k++)
					{
						Vector2 vector255 = spinningPoint.RotatedBy(radians * k);
						int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vector255, type, damage2, 0f, Main.myPlayer, 0f, 0f);
						Main.projectile[proj].timeLeft = 300;
					}
				}

                float randomSpread;
                for (int spike = 0; spike < 4; spike++)
                {
                    randomSpread = Main.rand.Next(-200, 200) / 100;
                    for (int x = 1; x <= 4; x++)
                    {
                        if (Main.netMode != NetmodeID.Server)
                            Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity * randomSpread, Mod.Find<ModGore>("CryoShieldGore" + x).Type, 1f);
                    }
                    
                }
            }
        }
    }
}
