using CalRD.Buffs.Summon;
using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Summon
{
	public class SquirrelSquireMinion : ModProjectile
    {
        public float dust = 0f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Squirrel Squire");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 17;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
			CalamityGlobalProjectile modProj = Projectile.Calamity();
            if (dust == 0f)
            {
                modProj.spawnedPlayerMinionDamageValue = player.MinionDamage();
                modProj.spawnedPlayerMinionProjectileDamageValue = Projectile.damage;
                int dustAmt = 36;
                for (int d = 0; d < dustAmt; d++)
                {
                    Vector2 source = Vector2.Normalize(Projectile.velocity) * new Vector2((float)Projectile.width / 2f, (float)Projectile.height) * 0.75f;
                    source = source.RotatedBy((double)((float)(d - (dustAmt / 2 - 1)) * MathHelper.TwoPi / (float)dustAmt), default) + Projectile.Center;
                    Vector2 dustVel = source - Projectile.Center;
                    int dusty = Dust.NewDust(source + dustVel, 0, 0, 7, dustVel.X * 1.1f, dustVel.Y * 1.1f, 100, default, 1.4f);
                    Main.dust[dusty].noGravity = true;
                    Main.dust[dusty].noLight = true;
                    Main.dust[dusty].velocity = dustVel;
                }
                dust += 1f;
            }
            if (player.MinionDamage() != modProj.spawnedPlayerMinionDamageValue)
            {
                int damage2 = (int)((float)modProj.spawnedPlayerMinionProjectileDamageValue /
                    modProj.spawnedPlayerMinionDamageValue *
                    player.MinionDamage());
                Projectile.damage = damage2;
            }
            bool projTypeCheck = Projectile.type == ModContent.ProjectileType<SquirrelSquireMinion>();
            player.AddBuff(ModContent.BuffType<SquirrelSquireBuff>(), 3600);
            if (projTypeCheck)
            {
                if (player.dead)
                {
                    modPlayer.squirrel = false;
                }
                if (modPlayer.squirrel)
                {
                    Projectile.timeLeft = 2;
                }
            }
			bool leftofPlayer = false;
			bool rightofPlayer = false;
			bool flag3 = false;
			bool flag4 = false;
			if (Projectile.lavaWet)
			{
				Projectile.ai[0] = 1f;
				Projectile.ai[1] = 0f;
			}
			float minionOffset = 40f * (Projectile.minionPos + 1f) * player.direction;
			if (player.Center.X < Projectile.Center.X - 10f + minionOffset)
				leftofPlayer = true;
			else if (player.Center.X > Projectile.Center.X + 10f + minionOffset)
				rightofPlayer = true;

			if (Projectile.ai[1] == 0f)
			{
				float playerDist = (player.Center - Projectile.Center).Length();
				if (playerDist > 1000f)
				{
					Projectile.ai[0] = 1f;
				}
				if (playerDist > 2000f) //teleport to player if too far
				{
					Projectile.position = player.position;
					Projectile.netUpdate = true;
				}
			}
			if (Projectile.ai[0] != 0f) //flying back to the player
			{
				Projectile.tileCollide = false;
				float npcDetectRange = 800f;
				bool npcFound = false;
				int targetIndex = -1;
				for (int index = 0; index < Main.maxNPCs; ++index)
				{
					NPC npc = Main.npc[index];
					if (npc.CanBeChasedBy(Projectile, false))
					{
						float npcDist = Vector2.Distance(npc.Center, player.Center);
						if (npcDist < npcDetectRange)
						{
							if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
								targetIndex = index;
							npcFound = true;
							break;
						}
					}
				}

				//return to normal if npc found
				if (npcFound && targetIndex >= 0)
					Projectile.ai[0] = 0f;

				Vector2 homeBase = player.Center - Projectile.Center;
				homeBase.X -= 40f * player.direction;
				if (!npcFound)
					homeBase.X -= 40f * Projectile.minionPos * player.direction;
				homeBase.Y -= 60f;
				float playerDist = homeBase.Length();
				float speed = playerDist;
				float acceleration = 0.4f;

				//if close enough to the player and has tile to stand on, return to normal
				if (playerDist < 100f && player.velocity.Y == 0f && Projectile.Bottom.Y <= player.Bottom.Y && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					if (Projectile.velocity.Y < -6f)
						Projectile.velocity.Y = -6f;
				}
				if (playerDist > 2000f)
				{
					Projectile.position = player.position;
					Projectile.netUpdate = true;
				}
				if (playerDist < 50f)
				{
					if (Math.Abs(Projectile.velocity.X) > 2f || Math.Abs(Projectile.velocity.Y) > 2f)
					{
						Projectile.velocity *= 0.99f;
					}
					acceleration = 0.01f;
				}
				else
				{
					if (playerDist < 100f)
					{
						acceleration = 0.1f;
					}
					if (playerDist > 300f)
					{
						acceleration = 1f;
					}
					playerDist = speed / playerDist;
					homeBase *= playerDist;
				}
				if (Projectile.velocity.X < homeBase.X)
				{
					Projectile.velocity.X += acceleration;
					if (acceleration > 0.05f && Projectile.velocity.X < 0f)
					{
						Projectile.velocity.X += acceleration;
					}
				}
				if (Projectile.velocity.X > homeBase.X)
				{
					Projectile.velocity.X -= acceleration;
					if (acceleration > 0.05f && Projectile.velocity.X > 0f)
					{
						Projectile.velocity.X -= acceleration;
					}
				}
				if (Projectile.velocity.Y < homeBase.Y)
				{
					Projectile.velocity.Y += acceleration;
					if (acceleration > 0.05f && Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y += acceleration * 2f;
					}
				}
				if (Projectile.velocity.Y > homeBase.Y)
				{
					Projectile.velocity.Y -= acceleration;
					if (acceleration > 0.05f && Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y -= acceleration * 2f;
					}
				}
				if (Projectile.frame < 12 || Projectile.frame == 16)
				{
					Projectile.frame = 12;
				}
				else
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 3)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame >= 16)
					{
						Projectile.frame = 13;
					}
				}
				if (Projectile.velocity.X > 0.5f)
					Projectile.spriteDirection = 1;
				else if (Projectile.velocity.X < -0.5f)
					Projectile.spriteDirection = -1;
			}
			else
			{
				float minionOffset2 = (float)(40 * Projectile.minionPos);
				float attackCooldown = 30f;
				float directionCooldown = 60f;
				--Projectile.localAI[0];
				if (Projectile.localAI[0] < 0f)
					Projectile.localAI[0] = 0f;
				if (Projectile.ai[1] > 0f)
				{
					--Projectile.ai[1];
				}
				else
				{
					Vector2 targetPos = Projectile.position;
					float range = 100000f;
					float maxDist = range;
					int targetIndex = -1;
					NPC target = Projectile.OwnerMinionAttackTargetNPC;
					if (target != null && target.CanBeChasedBy(Projectile, false))
					{
						float npcDist = Vector2.Distance(target.Center, Projectile.Center);
						if (npcDist < range)
						{
							if (targetIndex == -1 && npcDist <= maxDist)
							{
								maxDist = npcDist;
								targetPos = target.Center;
							}
							if (Collision.CanHit(Projectile.Center, Projectile.width, Projectile.height, target.Center, target.width, target.height))
							{
								range = npcDist;
								targetPos = target.Center;
								targetIndex = target.whoAmI;
							}
						}
					}
					if (targetIndex == -1)
					{
						for (int index = 0; index < Main.maxNPCs; ++index)
						{
							NPC npc = Main.npc[index];
							if (npc.CanBeChasedBy(Projectile, false))
							{
								float npcDist = Vector2.Distance(npc.Center, Projectile.Center);
								if (npcDist < range)
								{
									if (targetIndex == -1 && npcDist <= maxDist)
									{
										maxDist = npcDist;
										targetPos = npc.Center;
									}
									if (Collision.CanHit(Projectile.Center, Projectile.width, Projectile.height, npc.Center, npc.width, npc.height))
									{
										range = npcDist;
										targetPos = npc.Center;
										targetIndex = index;
									}
								}
							}
						}
					}
					if (targetIndex == -1 && maxDist < range)
						range = maxDist;
					float num13 = 400f;
					if ((double)Projectile.position.Y > Main.worldSurface * 16D)
						num13 = 200f;
					if (range < num13 + minionOffset2 && targetIndex == -1)
					{
						float xDist = targetPos.X - Projectile.Center.X;
						if (xDist < -5f)
						{
							leftofPlayer = true;
							rightofPlayer = false;
						}
						else if (xDist > 5f)
						{
							rightofPlayer = true;
							leftofPlayer = false;
						}
					}
					else if (targetIndex >= 0 && range < 800f + minionOffset2)
					{
						Projectile.localAI[0] = directionCooldown;
						float xDist = targetPos.X - Projectile.Center.X;
						if (Math.Abs(xDist) > 300f)
						{
							if (xDist < -50f)
							{
								leftofPlayer = true;
								rightofPlayer = false;
							}
							else if (xDist > 50f)
							{
								rightofPlayer = true;
								leftofPlayer = false;
							}
						}
						else if (Projectile.owner == Main.myPlayer)
						{
							Projectile.ai[1] = attackCooldown;
							float speed = 12f;
							Vector2 source = Projectile.Center - Vector2.UnitY * 8f;
							Vector2 projVel = targetPos - source;
							projVel.X += Main.rand.NextFloat(-10f, 10f);
							projVel.Y += Main.rand.NextFloat(-10f, 10f) - Math.Abs(projVel.X) * Main.rand.NextFloat(0.0001f, 0.01f);
							projVel.Normalize();
							projVel *= speed;
							int damage = Projectile.damage;
							int projType = ModContent.ProjectileType<SquirrelSquireAcorn>();
							int index = Projectile.NewProjectile(Entity.GetSource_FromThis(), source, projVel, projType, damage, Projectile.knockBack, Projectile.owner);
							if (projVel.X < 0f)
								Projectile.direction = Projectile.spriteDirection = -1;
							else if (projVel.X > 0f)
								Projectile.direction = Projectile.spriteDirection = 1;
							Projectile.netUpdate = true;
						}
					}
				}
				if (Projectile.ai[1] != 0f) //If on attack cooldown
				{
					leftofPlayer = false;
					rightofPlayer = false;
				}
				else if (Projectile.localAI[0] == 0f)
					Projectile.direction = Projectile.spriteDirection = player.direction;
				Projectile.tileCollide = true;
				float num18 = 0.2f;
				float num19 = 6f;
				if (num19 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
				{
					num19 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
					num18 = 0.3f;
				}
				if (leftofPlayer)
				{
					if (Projectile.velocity.X > -3.5f)
						Projectile.velocity.X -= num18;
					else
						Projectile.velocity.X -= num18 * 0.25f;
				}
				else if (rightofPlayer)
				{
					if (Projectile.velocity.X < 3.5f)
						Projectile.velocity.X += num18;
					else
						Projectile.velocity.X += num18 * 0.25f;
				}
				else
				{
					Projectile.velocity.X *= 0.9f;
					if (Projectile.velocity.X >= -num18 && Projectile.velocity.X <= num18)
						Projectile.velocity.X = 0f;
				}
				if (leftofPlayer | rightofPlayer)
				{
					int i = (int)Projectile.Center.X / 16;
					int j = (int)Projectile.Center.Y / 16;
					if (leftofPlayer)
						--i;
					if (rightofPlayer)
						++i;
					if (WorldGen.SolidTile(i + (int)Projectile.velocity.X, j))
						flag4 = true;
				}
				if (player.position.Y + player.height - 8f > Projectile.position.Y + Projectile.height)
					flag3 = true;
				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY, 1, false, 0);
				if (Projectile.velocity.Y == 0f)
				{
					if (!flag3 && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f))
					{
						int i = (int)Projectile.Center.X / 16;
						int j = (int)Projectile.Center.Y / 16 + 1;
						if (leftofPlayer)
							--i;
						if (rightofPlayer)
							++i;
						WorldGen.SolidTile(i, j);
					}
					if (flag4)
					{
						int i = (int)Projectile.Center.X / 16;
						int j = (int)Projectile.Center.Y / 16 + 1;
						if (WorldGen.SolidTile(i, j) || Main.tile[i, j].IsHalfBlock || (int)Main.tile[i, j].Slope > 0)
						{
							try
							{
								int i2 = (int)Projectile.Center.X / 16;
								int j2 = (int)Projectile.Center.Y / 16;
								if (leftofPlayer)
									--i2;
								if (rightofPlayer)
									++i2;
								i2 += (int)Projectile.velocity.X;
								if (!WorldGen.SolidTile(i2, j2 - 1) && !WorldGen.SolidTile(i2, j2 - 2))
									Projectile.velocity.Y = -5.1f;
								else if (!WorldGen.SolidTile(i2, j2 - 2))
									Projectile.velocity.Y = -7.1f;
								else if (WorldGen.SolidTile(i2, j2 - 5))
									Projectile.velocity.Y = -11.1f;
								else if (WorldGen.SolidTile(i2, j2 - 4))
									Projectile.velocity.Y = -10.1f;
								else
									Projectile.velocity.Y = -9.1f;
							}
							catch
							{
								Projectile.velocity.Y = -9.1f;
							}
						}
					}
				}
				if (Projectile.velocity.X > num19)
					Projectile.velocity.X = num19;
				if (Projectile.velocity.X < -num19)
					Projectile.velocity.X = -num19;
				if (Projectile.velocity.X < -0.05f)
					Projectile.direction = Projectile.spriteDirection = -1;
				if (Projectile.velocity.X > 0.05f)
					Projectile.direction = Projectile.spriteDirection = 1;
				if (Projectile.ai[1] > 0f)
				{
					if (Projectile.localAI[1] == 0f)
					{
						Projectile.localAI[1] = 1f;
						Projectile.frame = 8;
					}
					if (Projectile.frame >= 8 && Projectile.frame <= 11)
					{
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 8)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame == 11)
							Projectile.frame = 8;
					}
				}
				else if (Projectile.velocity.Y == 0f)
				{
					Projectile.localAI[1] = 0f;
					if (Projectile.velocity.X == 0f)
					{
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 4)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame >= 4)
						{
							Projectile.frame = 0;
						}
					}
					else if (Math.Abs(Projectile.velocity.X) > 0.8f)
					{
						Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 20)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame < 4)
							Projectile.frame = 4;
						if (Projectile.frame >= 8)
							Projectile.frame = 4;
					}
					else
					{
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 4)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame >= 4)
						{
							Projectile.frame = 0;
						}
					}
				}
				else if (Projectile.velocity.Y < 0f)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = 16;
				}
				else if (Projectile.velocity.Y > 0f)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = 16;
				}
				Projectile.velocity.Y += 0.4f;
				if (Projectile.velocity.Y > 10f)
					Projectile.velocity.Y = 10f;
				Vector2 velocity = Projectile.velocity;
			}
		}

        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */ => false;

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void OnKill(int timeLeft)
        {
			int index = Gore.NewGore(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Main.rand.Next(61, 64), Projectile.scale);
			Main.gore[index].velocity *= 0.1f;
        }
    }
}
