using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Ravager
{
    public class RockPillar : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rock Pillar");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
	            Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.width = 60;
            NPC.height = 300;
			NPC.DR_NERD(0.5f);
			NPC.chaseable = false;
			NPC.lifeMax = CalamityWorld.downedProvidence ? 35000 : 5000;
            NPC.alpha = 255;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
			NPC.HitSound = SoundID.NPCHit41;
			NPC.DeathSound = SoundID.NPCDeath14;
		}

        public override void AI()
        {
            if (CalamityGlobalNPC.scavenger < 0 || !Main.npc[CalamityGlobalNPC.scavenger].active)
            {
                NPC.life = 0;
                NPC.HitEffect(NPC.direction, 9999);
                NPC.netUpdate = true;
                return;
            }

            if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

            if (NPC.alpha > 0)
            {
				NPC.damage = 0;

				NPC.alpha -= 10;
                if (NPC.alpha < 0)
                    NPC.alpha = 0;
            }
            else
            {
                if (CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive)
                    NPC.damage = NPC.defDamage * 2;
                else
                    NPC.damage = NPC.defDamage;
            }                

            if (NPC.ai[0] == 0f)
            {
                if (NPC.velocity.Y == 0f)
                {
                    if (NPC.ai[1] == -1f)
                    {
						SoundEngine.PlaySound(SoundID.Item62, NPC.position);

						for (int num621 = 0; num621 < 10; num621++)
						{
							int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Iron, 0f, 0f, 100, default, 2f);
							Main.dust[num622].velocity *= 3f;
							if (Main.rand.NextBool(2))
							{
								Main.dust[num622].scale = 0.5f;
								Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
							}
						}
						for (int num623 = 0; num623 < 10; num623++)
						{
							int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 3f);
							Main.dust[num624].noGravity = true;
							Main.dust[num624].velocity *= 5f;
							num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Iron, 0f, 0f, 100, default, 2f);
							Main.dust[num624].velocity *= 2f;
						}

						NPC.noTileCollide = true;
						NPC.velocity.X = 12 * NPC.direction;
                        NPC.velocity.Y = -28.5f;
                        NPC.ai[0] = 1f;
                        NPC.ai[1] = 0f;
                    }
                }
            }
            else
            {
                if (NPC.velocity.Y == 0f || Vector2.Distance(NPC.Center, Main.npc[CalamityGlobalNPC.scavenger].Center) > 2800f)
                {
                    SoundEngine.PlaySound(SoundID.Item14, NPC.position);
                    NPC.ai[0] = 0f;
                    NPC.life = 0;
                    NPC.HitEffect(NPC.direction, 9999);
                    NPC.netUpdate = true;
                    return;
                }
                else
                {
                    NPC.velocity.Y += 0.2f;

					if (NPC.velocity.Y >= 0f && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
						NPC.noTileCollide = false;
				}
            }
        }

		public override bool CheckActive()
		{
			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 180, true);
			SoundEngine.PlaySound(SoundID.Item14, NPC.position);
			NPC.ai[0] = 0f;
			NPC.life = 0;
			NPC.HitEffect(NPC.direction, 9999);
			NPC.netUpdate = true;
		}

		public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 80;
                NPC.height = 360;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 30; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Iron, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 30; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Iron, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
			else
			{
				for (int num621 = 0; num621 < 2; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Iron, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.NextBool(2))
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 2; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Iron, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
        }
    }
}
