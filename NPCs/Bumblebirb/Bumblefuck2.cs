using CalRD.Buffs.StatDebuffs;
using CalRD.Buffs.DamageOverTime;
using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRD.NPCs.Bumblebirb
{
    public class Bumblefuck2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Draconic Swarmer");
            Main.npcFrameCount[NPC.type] = 5;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

        public override string Texture => "CalRD/NPCs/Bumblebirb/BumbleFolly";

        public override void SetDefaults()
        {
            NPC.npcSlots = 1f;
            NPC.aiStyle = -1;
            AIType = -1;
			NPC.GetNPCDamage();
			NPC.width = 120;
            NPC.height = 80;
            NPC.defense = 20;
            NPC.LifeMaxNERB(12000, 15000, 50000);
            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
			NPC.buffImmune[BuffID.Daybreak] = false;
			NPC.buffImmune[BuffID.BetsysCurse] = false;
            NPC.buffImmune[ModContent.BuffType<ExoFreeze>()] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit51;
            NPC.DeathSound = SoundID.NPCDeath46;
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe || !NPC.downedMoonlord || spawnInfo.Player.Calamity().ZoneSunkenSea || NPC.AnyNPCs(NPC.type))
			{
				return 0f;
			}
			return SpawnCondition.SurfaceJungle.Chance * 0.09f;
		}

		public override void AI()
        {
            Player player = Main.player[NPC.target];
            Vector2 vector = NPC.Center;

			bool increasedAggression = CalamityPlayer.areThereAnyDamnBosses;

			float rotationMult = 4f;
			float rotationAmt = 0.04f;

			if (Vector2.Distance(player.Center, vector) > 5600f)
            {
                if (NPC.timeLeft > 5)
                {
                    NPC.timeLeft = 5;
                }
            }

            NPC.noTileCollide = false;
            NPC.noGravity = true;

            NPC.rotation = (NPC.rotation * rotationMult + NPC.velocity.X * rotationAmt * 1.25f) / 10f;

            if (NPC.ai[0] == 0f || NPC.ai[0] == 1f)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (i != NPC.whoAmI && Main.npc[i].active && Main.npc[i].type == NPC.type)
                    {
                        Vector2 value42 = Main.npc[i].Center - NPC.Center;
                        if (value42.Length() < (NPC.width + NPC.height))
                        {
                            value42.Normalize();
                            value42 *= -0.1f;
                            NPC.velocity += value42;
                            NPC nPC6 = Main.npc[i];
                            nPC6.velocity -= value42;
                        }
                    }
                }
            }

            if (NPC.target < 0 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
                Vector2 vector240 = Main.player[NPC.target].Center - NPC.Center;
                if (Main.player[NPC.target].dead || vector240.Length() > (increasedAggression ? 5600f : 2800f))
                {
                    NPC.ai[0] = -1f;
                }
            }
            else
            {
                Vector2 vector241 = Main.player[NPC.target].Center - NPC.Center;
                if (NPC.ai[0] > 1f && vector241.Length() > 3600f)
                {
                    NPC.ai[0] = 1f;
                }
            }

            if (NPC.ai[0] == -1f)
            {
                Vector2 value43 = new Vector2(0f, -8f);
                NPC.velocity = (NPC.velocity * 21f + value43) / 10f;
                NPC.noTileCollide = true;
                NPC.dontTakeDamage = true;
                return;
            }

            if (NPC.ai[0] == 0f)
            {
                NPC.TargetClosest(true);
                NPC.spriteDirection = NPC.direction;
                if (NPC.collideX)
                {
                    NPC.velocity.X = NPC.velocity.X * (-NPC.oldVelocity.X * 0.5f);
                    if (NPC.velocity.X > 4f)
                    {
                        NPC.velocity.X = 4f;
                    }
                    if (NPC.velocity.X < -4f)
                    {
                        NPC.velocity.X = -4f;
                    }
                }
                if (NPC.collideY)
                {
                    NPC.velocity.Y = NPC.velocity.Y * (-NPC.oldVelocity.Y * 0.5f);
                    if (NPC.velocity.Y > 4f)
                    {
                        NPC.velocity.Y = 4f;
                    }
                    if (NPC.velocity.Y < -4f)
                    {
                        NPC.velocity.Y = -4f;
                    }
                }
                Vector2 value44 = Main.player[NPC.target].Center - NPC.Center;
                if (value44.Length() > 2800f)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                }
                else if (value44.Length() > 400f)
                {
                    float scaleFactor20 = (increasedAggression ? 9f : 7f) + value44.Length() / 100f + NPC.ai[1] / 15f;
                    float num1377 = 30f;
                    value44.Normalize();
                    value44 *= scaleFactor20;
                    NPC.velocity = (NPC.velocity * (num1377 - 1f) + value44) / num1377;
                }
                else if (NPC.velocity.Length() > 2f)
                {
                    NPC.velocity *= 0.95f;
                }
                else if (NPC.velocity.Length() < 1f)
                {
                    NPC.velocity *= 1.05f;
                }
                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= (increasedAggression ? 90f : 105f))
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 2f;
                }
            }
            else
            {
                if (NPC.ai[0] == 1f)
                {
                    NPC.collideX = false;
                    NPC.collideY = false;
                    NPC.noTileCollide = true;
                    if (NPC.target < 0 || !Main.player[NPC.target].active || Main.player[NPC.target].dead)
                    {
                        NPC.TargetClosest(true);
                    }
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.direction = -1;
                    }
                    else if (NPC.velocity.X > 0f)
                    {
                        NPC.direction = 1;
                    }
                    NPC.spriteDirection = NPC.direction;
                    NPC.rotation = (NPC.rotation * rotationMult + NPC.velocity.X * rotationAmt) / 10f;
                    Vector2 value45 = Main.player[NPC.target].Center - NPC.Center;
                    if (value45.Length() < 800f && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                    {
                        NPC.ai[0] = 0f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                    }
                    NPC.ai[2] += 0.0166666675f;
                    float scaleFactor21 = (increasedAggression ? 12f : 9f) + NPC.ai[2] + value45.Length() / 150f;
                    float num1378 = 25f;
                    value45.Normalize();
                    value45 *= scaleFactor21;
                    NPC.velocity = (NPC.velocity * (num1378 - 1f) + value45) / num1378;
                    NPC.netSpam = 5;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                    }
                    return;
                }
                if (NPC.ai[0] == 2f)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.direction = -1;
                    }
                    else if (NPC.velocity.X > 0f)
                    {
                        NPC.direction = 1;
                    }
                    NPC.spriteDirection = NPC.direction;
                    NPC.rotation = (NPC.rotation * rotationMult * 0.75f + NPC.velocity.X * rotationAmt * 1.25f) / 8f;
                    NPC.noTileCollide = true;
                    Vector2 vector242 = Main.player[NPC.target].Center - NPC.Center;
                    vector242.Y -= 8f;
                    float scaleFactor22 = increasedAggression ? 18f : 14f;
                    float num1379 = 8f;
                    vector242.Normalize();
                    vector242 *= scaleFactor22;
                    NPC.velocity = (NPC.velocity * (num1379 - 1f) + vector242) / num1379;
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.direction = -1;
                    }
                    else
                    {
                        NPC.direction = 1;
                    }
                    NPC.spriteDirection = NPC.direction;
                    NPC.ai[1] += 1f;
                    if (NPC.ai[1] > 10f)
                    {
                        NPC.velocity = vector242;
                        if (NPC.velocity.X < 0f)
                        {
                            NPC.direction = -1;
                        }
                        else
                        {
                            NPC.direction = 1;
                        }
                        NPC.ai[0] = 2.1f;
                        NPC.ai[1] = 0f;
                    }
                }
                else if (NPC.ai[0] == 2.1f)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.direction = -1;
                    }
                    else if (NPC.velocity.X > 0f)
                    {
                        NPC.direction = 1;
                    }
                    NPC.spriteDirection = NPC.direction;
                    NPC.velocity *= 1.01f;
                    NPC.noTileCollide = true;
                    NPC.ai[1] += 1f;
                    int num1380 = 30;
                    if (NPC.ai[1] > num1380)
                    {
                        if (!Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                        {
                            NPC.ai[0] = 0f;
                            NPC.ai[1] = 0f;
                            NPC.ai[2] = 0f;
                            return;
                        }
                        if (NPC.ai[1] > (num1380 * 2))
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.ai[2] = 0f;
                        }
                    }
                }
            }
        }

		public override bool PreKill() => !CalamityPlayer.areThereAnyDamnBosses;

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(ModContent.ItemType<EffulgentFeather>(), 1, 2, 4);

		public override void FindFrame(int frameHeight)
        {
			NPC.frameCounter += NPC.ai[0] == 2.1f ? 1.5 : 1D;
			if (NPC.frameCounter > 4D) //iban said the time between frames was 5 so using that as a base
			{
				NPC.frameCounter = 0D;
				NPC.frame.Y += frameHeight;
			}
			if (NPC.frame.Y >= frameHeight * 4)
			{
				NPC.frame.Y = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
			Color color36 = Color.Gold;
			float amount9 = 0.5f;
			int num153 = NPC.ai[0] == 2.1f ? 7 : 0;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			cooldownSlot = 1;
			return true;
		}

		public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 244, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 50; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 244, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
