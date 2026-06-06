using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRD.NPCs.Abyss
{
    public class EidolonWyrmHeadHuge : ModNPC
    {
		private Vector2 patrolSpot = Vector2.Zero;
		public bool detectsPlayer = false;
        public const int minLength = 40;
        public const int maxLength = 41;
        public float speed = 7.5f; //10
        public float turnSpeed = 0.15f; //0.15
        bool TailSpawned = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eidolon Wyrm");
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 24f;
            NPC.damage = 1500;
            NPC.width = 254; //36
            NPC.height = 138; //20
            NPC.defense = 3000;
            CalamityGlobalNPC global = NPC.Calamity();
            global.DR = 0.999999f;
            global.unbreakableDR = true;
            NPC.lifeMax = 1000000;
            NPC.aiStyle = -1;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(10, 0, 0, 0);
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.WriteVector2(patrolSpot);
			writer.Write(detectsPlayer);
            writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			patrolSpot = reader.ReadVector2();
			detectsPlayer = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();
        }

        public override void AI()
        {
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(true);
			}
			if (NPC.justHit || detectsPlayer || Main.player[NPC.target].chaosState)
            {
				if (!detectsPlayer)
				{
					if (Main.netMode != NetmodeID.Server)
					{
						SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/Scare"), Main.player[NPC.target].position);
					}
					detectsPlayer = true;
				}
                NPC.damage = 1500;
            }
            else
            {
                NPC.damage = 0;
            }
            NPC.chaseable = detectsPlayer;
            if (detectsPlayer)
            {
                if (NPC.soundDelay <= 0 && Main.netMode != NetmodeID.Server)
                {
                    NPC.soundDelay = 420;
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/EidolonWyrmRoarClose") with {Volume = 2.5f}, NPC.position);
                }
            }
            else
            {
                if (Main.rand.NextBool(900) && Main.netMode != NetmodeID.Server)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/EidolonWyrmRoarClose") with {Volume = 2.5f}, NPC.position);
                }
            }
            if (NPC.ai[3] > 0f)
            {
                NPC.realLife = (int)NPC.ai[3];
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!TailSpawned && NPC.ai[0] == 0f)
                {
                    int Previous = NPC.whoAmI;
                    for (int num36 = 0; num36 < maxLength; num36++)
                    {
                        int lol;
                        if (num36 >= 0 && num36 < minLength)
                        {
                            if (num36 % 2 == 0)
                            {
                                lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<EidolonWyrmBodyHuge>(), NPC.whoAmI);
                            }
                            else
                            {
                                lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<EidolonWyrmBodyAltHuge>(), NPC.whoAmI);
                            }
                        }
                        else
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<EidolonWyrmTailHuge>(), NPC.whoAmI);
                        }
                        Main.npc[lol].realLife = NPC.whoAmI;
                        Main.npc[lol].ai[2] = (float)NPC.whoAmI;
                        Main.npc[lol].ai[1] = (float)Previous;
                        Main.npc[Previous].ai[0] = (float)lol;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, lol, 0f, 0f, 0f, 0);
                        Previous = lol;
                    }
                    TailSpawned = true;
                }
                if (detectsPlayer)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] >= 300f)
                    {
                        NPC.localAI[0] = 0f;
                        NPC.TargetClosest(true);
                        NPC.netUpdate = true;
                        int damage = Main.expertMode ? 300 : 400;
                        float xPos = Main.rand.NextBool(2) ? NPC.position.X + 200f : NPC.position.X - 200f;
                        Vector2 vector2 = new Vector2(xPos, NPC.position.Y + Main.rand.Next(-200, 201));
                        int random = Main.rand.Next(3);
                        if (random == 0)
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), vector2.X, vector2.Y, 0f, 0f, ProjectileID.CultistBossLightningOrb, damage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), -vector2.X, -vector2.Y, 0f, 0f, ProjectileID.CultistBossLightningOrb, damage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else if (random == 1)
                        {
                            Vector2 vec = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center);
                            vec = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center + Main.player[NPC.target].velocity * 20f);
                            if (vec.HasNaNs())
                            {
                                vec = new Vector2((float)NPC.direction, 0f);
                            }
                            for (int n = 0; n < 1; n++)
                            {
                                Vector2 vector4 = vec * 4f;
                                Projectile.NewProjectile(Entity.GetSource_FromThis(), vector2.X, vector2.Y, vector4.X, vector4.Y, ProjectileID.CultistBossIceMist, damage, 0f, Main.myPlayer, 0f, 1f);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), -vector2.X, -vector2.Y, -vector4.X, -vector4.Y, ProjectileID.CultistBossIceMist, damage, 0f, Main.myPlayer, 0f, 1f);
                            }
                        }
                        else
                        {
                            if (Math.Abs(Main.player[NPC.target].velocity.X) > 0.1f || Math.Abs(Main.player[NPC.target].velocity.Y) > 0.1f)
                            {
                                SoundEngine.PlaySound(SoundID.Item117, Main.player[NPC.target].position);
                                for (int num621 = 0; num621 < 20; num621++)
                                {
                                    int num622 = Dust.NewDust(new Vector2(Main.player[NPC.target].position.X, Main.player[NPC.target].position.Y),
                                        Main.player[NPC.target].width, Main.player[NPC.target].height, 185, 0f, 0f, 100, default, 2f);
                                    Main.dust[num622].velocity *= 0.6f;
                                    if (Main.rand.NextBool(2))
                                    {
                                        Main.dust[num622].scale = 0.5f;
                                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                                    }
                                }
                                for (int num623 = 0; num623 < 30; num623++)
                                {
                                    int num624 = Dust.NewDust(new Vector2(Main.player[NPC.target].position.X, Main.player[NPC.target].position.Y),
                                        Main.player[NPC.target].width, Main.player[NPC.target].height, 185, 0f, 0f, 100, default, 3f);
                                    Main.dust[num624].noGravity = true;
                                    num624 = Dust.NewDust(new Vector2(Main.player[NPC.target].position.X, Main.player[NPC.target].position.Y),
                                        Main.player[NPC.target].width, Main.player[NPC.target].height, 185, 0f, 0f, 100, default, 2f);
                                    Main.dust[num624].velocity *= 0.2f;
                                }
                                if (Math.Abs(Main.player[NPC.target].velocity.X) > 0.1f)
                                {
                                    Main.player[NPC.target].velocity.X = -Main.player[NPC.target].velocity.X * 2f;
                                }
                                if (Math.Abs(Main.player[NPC.target].velocity.Y) > 0.1f)
                                {
                                    Main.player[NPC.target].velocity.Y = -Main.player[NPC.target].velocity.Y * 2f;
                                }
                            }
                        }
                    }
                }
            }

            if (NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = -1;
            }
            else if (NPC.velocity.X > 0f)
            {
                NPC.spriteDirection = 1;
            }

            if (Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(false);

				NPC.velocity.Y += 3f;
				if (NPC.position.Y > Main.worldSurface * 16.0)
					NPC.velocity.Y += 3f;

				if (NPC.position.Y > (Main.maxTilesY - 200) * 16.0)
				{
					for (int a = 0; a < Main.maxNPCs; a++)
					{
						if (Main.npc[a].type == NPC.type || Main.npc[a].type == ModContent.NPCType<EidolonWyrmBodyAltHuge>() || Main.npc[a].type == ModContent.NPCType<EidolonWyrmBodyHuge>() || Main.npc[a].type == ModContent.NPCType<EidolonWyrmTailHuge>())
							Main.npc[a].active = false;
					}
				}
			}

            NPC.alpha -= 42;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }

			if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 6400f || !NPC.AnyNPCs(ModContent.NPCType<EidolonWyrmTailHuge>()))
            {
                NPC.active = false;
            }

            float num188 = speed;
            float num189 = turnSpeed;
			Vector2 vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);

			if (patrolSpot == Vector2.Zero)
				patrolSpot = Main.player[NPC.target].Center;

			float num191 = detectsPlayer ? Main.player[NPC.target].Center.X : patrolSpot.X;
			float num192 = detectsPlayer ? Main.player[NPC.target].Center.Y : patrolSpot.Y;

			if (!detectsPlayer)
			{
				num192 += 800;
				if (Math.Abs(NPC.Center.X - num191) < 400f) //500
				{
					if (NPC.velocity.X > 0f)
					{
						num191 += 500f;
					}
					else
					{
						num191 -= 500f;
					}
				}
			}
            else
            {
                num188 = 10f;
                num189 = 0.175f;
                if (!Main.player[NPC.target].wet)
                {
                    num188 = 20f;
                    num189 = 0.25f;
                }
            }
            float num48 = num188 * 1.3f;
            float num49 = num188 * 0.7f;
            float num50 = NPC.velocity.Length();
            if (num50 > 0f)
            {
                if (num50 > num48)
                {
                    NPC.velocity.Normalize();
                    NPC.velocity *= num48;
                }
                else if (num50 < num49)
                {
                    NPC.velocity.Normalize();
                    NPC.velocity *= num49;
                }
            }
            num191 = (float)((int)(num191 / 16f) * 16);
            num192 = (float)((int)(num192 / 16f) * 16);
            vector18.X = (float)((int)(vector18.X / 16f) * 16);
            vector18.Y = (float)((int)(vector18.Y / 16f) * 16);
            num191 -= vector18.X;
            num192 -= vector18.Y;
            float num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
            float num196 = System.Math.Abs(num191);
            float num197 = System.Math.Abs(num192);
            float num198 = num188 / num193;
            num191 *= num198;
            num192 *= num198;
            if ((NPC.velocity.X > 0f && num191 > 0f) || (NPC.velocity.X < 0f && num191 < 0f) || (NPC.velocity.Y > 0f && num192 > 0f) || (NPC.velocity.Y < 0f && num192 < 0f))
            {
                if (NPC.velocity.X < num191)
                {
                    NPC.velocity.X = NPC.velocity.X + num189;
                }
                else
                {
                    if (NPC.velocity.X > num191)
                    {
                        NPC.velocity.X = NPC.velocity.X - num189;
                    }
                }
                if (NPC.velocity.Y < num192)
                {
                    NPC.velocity.Y = NPC.velocity.Y + num189;
                }
                else
                {
                    if (NPC.velocity.Y > num192)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num189;
                    }
                }
                if ((double)System.Math.Abs(num192) < (double)num188 * 0.2 && ((NPC.velocity.X > 0f && num191 < 0f) || (NPC.velocity.X < 0f && num191 > 0f)))
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + num189 * 2f;
                    }
                    else
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num189 * 2f;
                    }
                }
                if ((double)System.Math.Abs(num191) < (double)num188 * 0.2 && ((NPC.velocity.Y > 0f && num192 < 0f) || (NPC.velocity.Y < 0f && num192 > 0f)))
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 2f; //changed from 2
                    }
                    else
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 2f; //changed from 2
                    }
                }
            }
            else
            {
                if (num196 > num197)
                {
                    if (NPC.velocity.X < num191)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 1.1f; //changed from 1.1
                    }
                    else if (NPC.velocity.X > num191)
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 1.1f; //changed from 1.1
                    }
                    if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + num189;
                        }
                        else
                        {
                            NPC.velocity.Y = NPC.velocity.Y - num189;
                        }
                    }
                }
                else
                {
                    if (NPC.velocity.Y < num192)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + num189 * 1.1f;
                    }
                    else if (NPC.velocity.Y > num192)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num189 * 1.1f;
                    }
                    if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X + num189;
                        }
                        else
                        {
                            NPC.velocity.X = NPC.velocity.X - num189;
                        }
                    }
                }
            }
            NPC.rotation = (float)System.Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.minion && !projectile.Calamity().overridesMinionDamagePrevention)
            {
                return detectsPlayer;
            }
            return null;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Vector2 center = new Vector2(NPC.Center.X, NPC.Center.Y);
            Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
            Vector2 vector = center - Main.screenPosition;
            vector -= new Vector2((float)ModContent.Request<Texture2D>("CalRD/NPCs/Abyss/EidolonWyrmHeadGlowHuge").Value.Width, (float)(ModContent.Request<Texture2D>("CalRD/NPCs/Abyss/EidolonWyrmHeadGlowHuge").Value.Height / Main.npcFrameCount[NPC.type])) * 1f / 2f;
            vector += vector11 * 1f + new Vector2(0f, 0f + 4f + NPC.gfxOffY);
            Color color = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.LightYellow);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/NPCs/Abyss/EidolonWyrmHeadGlowHuge").Value, vector,
                new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, vector11, 1f, spriteEffects, 0f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.Calamity().ZoneAbyssLayer4 && spawnInfo.Water && !NPC.AnyNPCs(ModContent.NPCType<EidolonWyrmHeadHuge>()) && CalamityWorld.downedPolterghast)
            {
                return SpawnCondition.CaveJellyfish.Chance * 0.012f;
            }
            return 0f;
        }

        public override void OnKill()
        {
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Voidstone>(), 80, 100);
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EidolicWail>());
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<SoulEdge>());
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<HalibutCannon>(), CalamityWorld.revenge);

            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Lumenite>(), CalamityWorld.downedCalamitas, 1, 50, 108);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Lumenite>(), CalamityWorld.downedCalamitas && Main.expertMode, 2, 15, 27);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ItemID.Ectoplasm, NPC.downedPlantBoss, 1, 21, 32);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (detectsPlayer)
            {
                for (int k = 0; k < 5; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hit.HitDirection, -1f, 0, default, 1f);
                }
                if (NPC.life <= 0)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        for (int k = 0; k < 15; k++)
                        {
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hit.HitDirection, -1f, 0, default, 1f);
                        }
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WyrmAdult").Type, 1f);
                    }
                }
            }
        }

        public override bool CheckActive()
        {
            if (detectsPlayer && !Main.player[NPC.target].dead)
            {
                return false;
            }
            if (NPC.timeLeft <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int k = (int)NPC.ai[0]; k > 0; k = (int)Main.npc[k].ai[0])
                {
                    if (Main.npc[k].active)
                    {
                        Main.npc[k].active = false;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            Main.npc[k].life = 0;
                            Main.npc[k].netSkip = -1;
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, k, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
            }
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 1200, true);
        }
    }
}
