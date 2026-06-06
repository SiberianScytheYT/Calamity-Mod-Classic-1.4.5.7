using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Ranged;
using CalRD.NPCs.AquaticScourge;
using CalRD.Projectiles.Enemy;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.SulphurousSea
{
	public class Mauler : ModNPC
    {
        private bool hasBeenHit = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mauler");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.damage = 135;
            NPC.width = 180;
            NPC.height = 90;
            NPC.defense = 50;
			NPC.DR_NERD(0.05f);
            NPC.lifeMax = 165000;
            NPC.aiStyle = -1;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.value = Item.buyPrice(0, 25, 0, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath60;
            NPC.knockBackResist = 0f;
            NPC.rarity = 2;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<MaulerBanner>();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hasBeenHit);
            writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hasBeenHit = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();
        }

        public override void AI()
        {
            NPC.spriteDirection = (NPC.direction > 0) ? 1 : -1;
            NPC.noGravity = true;
            if (NPC.direction == 0)
            {
                NPC.TargetClosest(true);
            }
            if (NPC.justHit)
            {
                hasBeenHit = true;
            }
            NPC.chaseable = hasBeenHit;
            if (NPC.wet)
            {
                bool flag14 = hasBeenHit;
                NPC.TargetClosest(false);
                if (Main.player[NPC.target].wet && !Main.player[NPC.target].dead &&
                    Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) &&
                    (Main.player[NPC.target].Center - NPC.Center).Length() < 300f)
                {
                    flag14 = true;
                }
                if (Main.player[NPC.target].dead && flag14)
                {
                    flag14 = false;
                }
                if (!flag14)
                {
                    if (NPC.collideX)
                    {
                        NPC.velocity.X = NPC.velocity.X * -1f;
                        NPC.direction *= -1;
                        NPC.netUpdate = true;
                    }
                    if (NPC.collideY)
                    {
                        NPC.netUpdate = true;
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y) * -1f;
                            NPC.directionY = -1;
                            NPC.ai[0] = -1f;
                        }
                        else if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y);
                            NPC.directionY = 1;
                            NPC.ai[0] = 1f;
                        }
                    }
                }
                if (flag14)
                {
                    if (!Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                    {
                        NPC.localAI[1] += 1f;
                    }
                    else
                    {
                        if (NPC.localAI[1] > 0f)
                        {
                            NPC.localAI[1] -= 1f;
                        }
                    }
                    NPC.localAI[2] += 1f;
                    NPC.TargetClosest(true);
                    NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.5f;
                    NPC.velocity.Y = NPC.velocity.Y + (float)NPC.directionY * 0.25f;
                    if (NPC.velocity.X > 22f)
                    {
                        NPC.velocity.X = 22f;
                    }
                    if (NPC.velocity.X < -22f)
                    {
                        NPC.velocity.X = -22f;
                    }
                    if (NPC.velocity.Y > 13f)
                    {
                        NPC.velocity.Y = 13f;
                    }
                    if (NPC.velocity.Y < -13f)
                    {
                        NPC.velocity.Y = -13f;
                    }
                }
                else
                {
                    NPC.velocity.X += (float)NPC.direction * 0.2f;
                    if (NPC.velocity.X < -4f || NPC.velocity.X > 4f)
                    {
                        NPC.velocity.X *= 0.95f;
                    }
                    if (NPC.ai[0] == -1f)
                    {
						NPC.velocity.Y -= 0.01f;
						if (NPC.velocity.Y < -0.3f)
						{
							NPC.ai[0] = 1f;
						}
					}
					else
					{
						NPC.velocity.Y += 0.01f;
						if (NPC.velocity.Y > 0.3f)
						{
							NPC.ai[0] = -1f;
                        }
                    }
                }
                int num258 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                int num259 = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
                if (Main.tile[num258, num259 - 1].LiquidAmount > 128)
                {
                    if (Main.tile[num258, num259 + 1].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                    else if (Main.tile[num258, num259 + 2].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                }
                if (NPC.velocity.Y > 0.4f || NPC.velocity.Y < -0.4f)
                {
                    NPC.velocity.Y *= 0.95f;
                }
            }
            else
            {
                NPC.localAI[1] += 1f;
                NPC.localAI[0] += 1f;
                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] >= 30f)
                {
                    NPC.localAI[0] = 0f;
                    if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                    {
                        float speed = 12f;
                        Vector2 vector = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)(NPC.height / 2));
                        float num6 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector.X + (float)Main.rand.Next(-20, 21);
                        float num7 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector.Y + (float)Main.rand.Next(-20, 21);
                        float num8 = (float)Math.Sqrt((double)(num6 * num6 + num7 * num7));
                        num8 = speed / num8;
                        num6 *= num8;
                        num7 *= num8;
                        int damage = 50;
                        if (Main.expertMode)
                        {
                            damage = 45;
                        }
                        int beam = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + (NPC.spriteDirection == 1 ? 60f : -60f), NPC.Center.Y, num6, num7, ModContent.ProjectileType<SulphuricAcidMist>(), damage, 0f, Main.myPlayer, 0f, 0f);
                    }
                }
                if (NPC.velocity.Y == 0f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.velocity.Y = (float)Main.rand.Next(-200, -150) * 0.1f; //50 20
                        NPC.velocity.X = (float)Main.rand.Next(-20, 20) * 0.1f; //20 20
                        NPC.netUpdate = true;
                    }
                }
                NPC.velocity.Y = NPC.velocity.Y + 0.4f; //0.4
                if (NPC.velocity.Y > 16f)
                {
                    NPC.velocity.Y = 16f;
                }
                NPC.ai[0] = 1f;
            }
            if (hasBeenHit)
            {
                if (Main.rand.NextBool(300))
                {
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/MaulerRoar"), NPC.position);
                }
            }
            if (NPC.localAI[1] >= 255f || NPC.localAI[2] >= 600f)
            {
                NPC.localAI[2] = 0f;
                BlowUp();
                return;
            }
            else if (NPC.localAI[1] > 0f)
            {
                for (int k = 0; k < (int)((double)NPC.localAI[1] * 0.05); k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 0f, 0f, 0, default, 1f);
                }
            }
            NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.1f;
            if (NPC.rotation < -0.2f)
            {
                NPC.rotation = -0.2f;
            }
            if (NPC.rotation > 0.2f)
            {
                NPC.rotation = 0.2f;
                return;
            }
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.minion && !projectile.Calamity().overridesMinionDamagePrevention)
            {
                return hasBeenHit;
            }
            return null;
        }

        public void BlowUp()
        {
            SoundEngine.PlaySound(SoundID.NPCDeath60, NPC.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 valueBoom = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float spreadBoom = 15f * 0.0174f;
                double startAngleBoom = Math.Atan2(NPC.velocity.X, NPC.velocity.Y) - spreadBoom / 2;
                double deltaAngleBoom = spreadBoom / 8f;
                double offsetAngleBoom;
                int iBoom;
                int damageBoom = NPC.localAI[1] >= 255f ? 200 : 50;
                for (iBoom = 0; iBoom < 25; iBoom++)
                {
                    int projectileType = Main.rand.NextBool(2) ? ModContent.ProjectileType<SulphuricAcidMist>() : ModContent.ProjectileType<SulphuricAcidBubble>();
                    offsetAngleBoom = startAngleBoom + deltaAngleBoom * (iBoom + iBoom * iBoom) / 2f + 32f * iBoom;
                    int boom1 = Projectile.NewProjectile(NPC.GetSource_FromThis(), valueBoom.X, valueBoom.Y, (float)(Math.Sin(offsetAngleBoom) * 6f), (float)(Math.Cos(offsetAngleBoom) * 6f), projectileType, damageBoom, 0f, Main.myPlayer, 0f, 0f);
                    int boom2 = Projectile.NewProjectile(NPC.GetSource_FromThis(), valueBoom.X, valueBoom.Y, (float)(-Math.Sin(offsetAngleBoom) * 6f), (float)(-Math.Cos(offsetAngleBoom) * 6f), projectileType, damageBoom, 0f, Main.myPlayer, 0f, 0f);
                }
            }
            for (int num621 = 0; num621 < 25; num621++)
            {
                int num622 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 31, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
                Main.dust[num622].noGravity = true;
            }
            for (int num623 = 0; num623 < 50; num623++)
            {
                int num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
                Main.dust[num624].noGravity = true;
            }
            if (NPC.localAI[1] >= 255f)
            {
                NPC.active = false;
            }
            NPC.netUpdate = true;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += hasBeenHit ? 0.15f : 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 300, true);
            target.AddBuff(BuffID.Venom, 300, true);
            target.AddBuff(BuffID.Rabies, 300, true);
        }

        public override void OnKill()
        {
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<SulphuricAcidCannon>(), CalamityWorld.downedPolterghast, 3, 1, 1);
            int item = Item.NewItem(NPC.GetSource_FromThis(), NPC.Center, NPC.Size, ItemID.SharkFin, Main.rand.Next(2, 5), false, 0, false, false);
            Main.item[item].color = new Color(151, 115, 57, 255);
            NetMessage.SendData(MessageID.ItemTweaker, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            if (spawnInfo.Player.Calamity().ZoneSulphur && spawnInfo.Water && !NPC.AnyNPCs(ModContent.NPCType<Mauler>()) &&
                !NPC.AnyNPCs(ModContent.NPCType<AquaticScourgeHead>()))
            {
                if (!Main.hardMode)
                {
                    return 0.001f;
                }
                if (!NPC.downedMoonlord)
                {
                    return 0.01f;
                }
                return 0.1f;
            }
            return 0f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 30; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }

                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Mauler").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Mauler2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Mauler3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Mauler4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Mauler5").Type, 1f);
                }
            }
        }
    }
}
