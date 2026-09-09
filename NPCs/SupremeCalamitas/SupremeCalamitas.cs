using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Items.Accessories;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Potions;
using CalRD.Items.Tools;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Boss;
using CalRD.Projectiles.Summon;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Events;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace CalRD.NPCs.SupremeCalamitas
{
    [AutoloadBossHead]
    public class SupremeCalamitas : ModNPC
    {
        private float bossLife;
        private float uDieLul = 1f;
        private float passedVar = 0f;

        private bool protectionBoost = false;
        private bool canDespawn = false;
        private bool despawnProj = false;
        private bool startText = false;
        private bool startBattle = false; //100%
        private bool startSecondAttack = false; //80%
        private bool startThirdAttack = false; //60%
        private bool halfLife = false; //40%
        private bool startFourthAttack = false; //30%
        private bool secondStage = false; //20%
        private bool startFifthAttack = false; //10%
        private bool gettingTired = false; //8%
        private bool gettingTired2 = false; //6%
        private bool gettingTired3 = false; //4%
        private bool gettingTired4 = false; //2%
        private bool gettingTired5 = false; //1%
        private bool willCharge = false;
        private bool canFireSplitingFireball = true;
        private bool spawnArena = false;

        private int giveUpCounter = 1200;
        private int lootTimer = 0; //900 * 5 = 4500
        private int phaseChange = 0;
        private int spawnX = 0;
        private int spawnX2 = 0;
        private int spawnXReset = 0;
        private int spawnXReset2 = 0;
        private int spawnXAdd = 200;
        private int spawnY = 0;
        private int spawnYReset = 0;
        private int spawnYAdd = 0;
        private int bulletHellCounter = 0;
        private int bulletHellCounter2 = 0;

        private Rectangle safeBox = default;

        public static float normalDR = 0.7f;
        public static float deathDR = 0.75f;
        public static float bossRushDR = 0.6f;
        public static float enragedDR = 0.99f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Supreme Calamitas");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			NPCID.Sets.BossBestiaryPriority.Add(Type);
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Scale = 0.65f,
				PortraitScale = 0.65f
			};
			value.Position.Y -= 10f;
			NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
		}
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
	        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
	        {
		        new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
		        new FlavorTextBestiaryInfoElement("The Witch herself in the flesh. Her brimstone magic is not something to be taken lightly.")
	        });
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 50f;
            NPC.width = 120;
            NPC.height = 120;
            NPC.defense = 120;
			NPC.DR_NERD(normalDR, normalDR, deathDR, bossRushDR, true);
			CalamityGlobalNPC global = NPC.Calamity();
            global.multDRReductions.Add(BuffID.CursedInferno, 0.9f);
            NPC.value = Item.buyPrice(10, 0, 0, 0);
			NPC.LifeMaxNERB(5000000, 5500000, 2100000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.dontTakeDamage = false;
            NPC.chaseable = true;
            NPC.boss = true;
            NPC.canGhostHeal = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/SCG");
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(protectionBoost);
            writer.Write(canDespawn);
            writer.Write(despawnProj);
            writer.Write(startText);
            writer.Write(startBattle);
            writer.Write(startSecondAttack);
            writer.Write(startThirdAttack);
            writer.Write(startFourthAttack);
            writer.Write(startFifthAttack);
            writer.Write(halfLife);
            writer.Write(secondStage);
            writer.Write(gettingTired);
            writer.Write(gettingTired2);
            writer.Write(gettingTired3);
            writer.Write(gettingTired4);
            writer.Write(gettingTired5);
            writer.Write(willCharge);
            writer.Write(canFireSplitingFireball);
            writer.Write(spawnArena);
            writer.Write(NPC.dontTakeDamage);
            writer.Write(NPC.chaseable);

            writer.Write(giveUpCounter);
            writer.Write(lootTimer);
            writer.Write(phaseChange);
            writer.Write(spawnX);
            writer.Write(spawnX2);
            writer.Write(spawnXReset);
            writer.Write(spawnXReset2);
            writer.Write(spawnXAdd);
            writer.Write(spawnY);
            writer.Write(spawnYReset);
            writer.Write(spawnYAdd);
            writer.Write(bulletHellCounter);
            writer.Write(bulletHellCounter2);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            protectionBoost = reader.ReadBoolean();
            canDespawn = reader.ReadBoolean();
            despawnProj = reader.ReadBoolean();
            startText = reader.ReadBoolean();
            startBattle = reader.ReadBoolean();
            startSecondAttack = reader.ReadBoolean();
            startThirdAttack = reader.ReadBoolean();
            startFourthAttack = reader.ReadBoolean();
            startFifthAttack = reader.ReadBoolean();
            halfLife = reader.ReadBoolean();
            secondStage = reader.ReadBoolean();
            gettingTired = reader.ReadBoolean();
            gettingTired2 = reader.ReadBoolean();
            gettingTired3 = reader.ReadBoolean();
            gettingTired4 = reader.ReadBoolean();
            gettingTired5 = reader.ReadBoolean();
            willCharge = reader.ReadBoolean();
            canFireSplitingFireball = reader.ReadBoolean();
            spawnArena = reader.ReadBoolean();
            NPC.dontTakeDamage = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();

            giveUpCounter = reader.ReadInt32();
            lootTimer = reader.ReadInt32();
            phaseChange = reader.ReadInt32();
            spawnX = reader.ReadInt32();
            spawnX2 = reader.ReadInt32();
            spawnXReset = reader.ReadInt32();
            spawnXReset2 = reader.ReadInt32();
            spawnXAdd = reader.ReadInt32();
            spawnY = reader.ReadInt32();
            spawnYReset = reader.ReadInt32();
            spawnYAdd = reader.ReadInt32();
            bulletHellCounter = reader.ReadInt32();
            bulletHellCounter2 = reader.ReadInt32();
        }

        public override void AI()
        {
            #region StartUp
            CalamityGlobalNPC.SCal = NPC.whoAmI;

            lootTimer++;

            bool wormAlive = false;
            if (CalamityGlobalNPC.SCalWorm != -1)
            {
                wormAlive = Main.npc[CalamityGlobalNPC.SCalWorm].active;
            }

            bool cataclysmAlive = false;
            if (CalamityGlobalNPC.SCalCataclysm != -1)
            {
                cataclysmAlive = Main.npc[CalamityGlobalNPC.SCalCataclysm].active;
            }

            bool catastropheAlive = false;
            if (CalamityGlobalNPC.SCalCatastrophe != -1)
            {
                catastropheAlive = Main.npc[CalamityGlobalNPC.SCalCatastrophe].active;
            }

            if (Main.slimeRain)
            {
                Main.StopSlimeRain(true);
                CalamityNetcode.SyncWorld();
            }

            CalRD.StopRain();

            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
			bool enraged = NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive);

			// Projectile damage values
			int bulletHellblastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneHellblast2>());
			int firstBulletHellblastDamage = (int)Math.Round(bulletHellblastDamage * 1.25);
			int barrageDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneBarrage>());
			int gigablastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneGigaBlast>());
			int fireblastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneFireblast>());
			int monsterDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneMonster>());
			int waveDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneWave>());
			int hellblastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneHellblast>());

			Vector2 vectorCenter = NPC.Center;

			// Get a target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			Player player = Main.player[NPC.target];

            if (!startText)
            {
                if (Main.LocalPlayer.Calamity().sCalKillCount == 4)
                {
                    string key = "Don't get me wrong, I like pain too, but you're just ridiculous."; //kill SCal 4 times
                    Color messageColor = Color.Orange;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }
                else if (Main.LocalPlayer.Calamity().sCalKillCount == 1)
                {
                    string key = "Do you enjoy going through hell?"; //kill SCal once
                    Color messageColor = Color.Orange;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }

                if (Main.LocalPlayer.Calamity().sCalDeathCount < 51)
                {
                    if (Main.LocalPlayer.Calamity().sCalDeathCount == 50)
                    {
                        string key = "Alright, I'm done counting. You probably died this much just to see what I'd say."; //die 50 or more times
                        Color messageColor = Color.Orange;
                        CalamityUtils.DisplayLocalizedText(key, messageColor);
                    }
                    else if (Main.LocalPlayer.Calamity().sCalDeathCount > 19)
                    {
                        string key = "Do you have a fetish for getting killed or something?"; //die 20 or more times
                        Color messageColor = Color.Orange;
                        CalamityUtils.DisplayLocalizedText(key, messageColor);
                    }
                    else if (Main.LocalPlayer.Calamity().sCalDeathCount > 4)
                    {
                        string key = "You must enjoy dying more than most people, huh?"; //die 5 or more times
                        Color messageColor = Color.Orange;
                        CalamityUtils.DisplayLocalizedText(key, messageColor);
                    }
                }
                startText = true;
            }
            #endregion
            #region ArenaCreation
            if (!spawnArena)
            {
                spawnArena = true;
                Vector2 vectorPlayer = new Vector2(player.position.X, player.position.Y);
                if (death)
                {
                    safeBox.X = spawnX = spawnXReset = (int)(vectorPlayer.X - 1000f);
                    spawnX2 = spawnXReset2 = (int)(vectorPlayer.X + 1000f);
                    safeBox.Y = spawnY = spawnYReset = (int)(vectorPlayer.Y - 1000f);
                    safeBox.Width = 2000;
                    safeBox.Height = 2000;
                    spawnYAdd = 100;
                }
                else
                {
                    safeBox.X = spawnX = spawnXReset = (int)(vectorPlayer.X - 1250f);
                    spawnX2 = spawnXReset2 = (int)(vectorPlayer.X + 1250f);
                    safeBox.Y = spawnY = spawnYReset = (int)(vectorPlayer.Y - 1250f);
                    safeBox.Width = 2500;
                    safeBox.Height = 2500;
                    spawnYAdd = 125;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int num52 = (int)(safeBox.X + (float)(safeBox.Width / 2)) / 16;
                    int num53 = (int)(safeBox.Y + (float)(safeBox.Height / 2)) / 16;
                    int num54 = safeBox.Width / 2 / 16 + 1;
                    for (int num55 = num52 - num54; num55 <= num52 + num54; num55++)
                    {
                        for (int num56 = num53 - num54; num56 <= num53 + num54; num56++)
                        {
                            if ((num55 == num52 - num54 || num55 == num52 + num54 || num56 == num53 - num54 || num56 == num53 + num54) && !Main.tile[num55, num56].HasTile)
                            {
                                Main.tile[num55, num56].TileType = (ushort)ModContent.TileType<Tiles.ArenaTile>();
                                Main.tile[num55, num56].Get<TileWallWireStateData>().HasTile = true;
                            }
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendTileSquare(-1, num55, num56, 1, TileChangeType.None);
                            }
                            else
                            {
                                WorldGen.SquareTileFrame(num55, num56, true);
                            }
                        }
                    }
                }
            }
            #endregion
            #region Enrage and DR
            if (!player.Hitbox.Intersects(safeBox))
            {
                if (uDieLul < 3f)
                {
                    uDieLul *= 1.01f;
                }
                else if (uDieLul > 3f)
                {
                    uDieLul = 3f;
                }
                protectionBoost = true;
            }
            else
            {
                if (uDieLul > 1f)
                {
                    uDieLul *= 0.99f;
                }
                else if (uDieLul < 1f)
                {
                    uDieLul = 1f;
                }
                protectionBoost = false;
            }

            // Set DR to be 99% and unbreakable if enraged. Boost DR during the 5th attack.
            CalamityGlobalNPC global = NPC.Calamity();
            if (protectionBoost && !gettingTired5)
            {
                global.DR = enragedDR;
                global.unbreakableDR = true;
            }
            else
            {
                global.DR = BossRushEvent.BossRushActive ? bossRushDR : CalamityWorld.death ? deathDR : normalDR;
                global.unbreakableDR = false;
                if (startFifthAttack)
                    global.DR *= 1.2f;
            }
			#endregion
			#region Despawn
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
					canDespawn = true;

					float num740 = player.Center.X - vectorCenter.X;
					float num741 = player.Center.Y - vectorCenter.Y;
					NPC.rotation = (float)Math.Atan2(num741, num740) - MathHelper.PiOver2;

					if (NPC.velocity.Y > 3f)
						NPC.velocity.Y = 3f;
					NPC.velocity.Y -= 0.2f;
					if (NPC.velocity.Y < -12f)
						NPC.velocity.Y = -12f;

					if (NPC.timeLeft > 60)
						NPC.timeLeft = 60;
                }
            }
            else
                canDespawn = false;
            #endregion
            #region FirstAttack
            if (bulletHellCounter2 < 900)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

				if (!canDespawn)
					NPC.velocity *= 0.95f;

                float num740 = player.Center.X - vectorCenter.X;
                float num741 = player.Center.Y - vectorCenter.Y;
                NPC.rotation = (float)Math.Atan2(num741, num740) - MathHelper.PiOver2;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    bulletHellCounter += 1;
                    if (bulletHellCounter > (enraged ? 4 : 6))
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 < 300) //blasts from above
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else if (bulletHellCounter2 < 600) //blasts from left and right
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else //blasts from above, left, and right
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 3f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                return;
            }
            else if (!startBattle)
            {
                string key = "Alright, let's get started. Not sure why you're bothering.";
                Color messageColor = Color.Orange;
                CalamityUtils.DisplayLocalizedText(key, messageColor);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    spawnY += 250;
                    if (death)
                    {
                        spawnY -= 50;
                    }
                    for (int x = 0; x < 5; x++)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), spawnX + 50, spawnY, ModContent.NPCType<SCalWormHeart>(), 0, 0f, 0f, 0f, 0f, 255);
                        spawnX += spawnXAdd;
                        NPC.NewNPC(NPC.GetSource_FromThis(), spawnX2 - 50, spawnY, ModContent.NPCType<SCalWormHeart>(), 0, 0f, 0f, 0f, 0f, 255);
                        spawnX2 -= spawnXAdd;
                        spawnY += spawnYAdd;
                    }
                    spawnX = spawnXReset;
                    spawnX2 = spawnXReset2;
                    spawnY = spawnYReset;
                    NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), ModContent.NPCType<SCalWormHead>());
                }
                startBattle = true;
            }
            #endregion
            #region SecondAttack
            if (bulletHellCounter2 < 1800 && startSecondAttack)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

				if (!canDespawn)
					NPC.velocity *= 0.95f;

                float num740 = player.Center.X - vectorCenter.X;
                float num741 = player.Center.Y - vectorCenter.Y;
                NPC.rotation = (float)Math.Atan2(num741, num740) - MathHelper.PiOver2;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (bulletHellCounter2 < 1200)
                    {
                        if (bulletHellCounter2 % 180 == 0) //blasts from top
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer, 0f, 0f);
                    }
                    else if (bulletHellCounter2 < 1500 && bulletHellCounter2 > 1200)
                    {
                        if (bulletHellCounter2 % 180 == 0) //blasts from right
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer, 0f, 0f);
                    }
                    else if (bulletHellCounter2 > 1500)
                    {
                        if (bulletHellCounter2 % 180 == 0) //blasts from top
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer, 0f, 0f);
                    }
                    bulletHellCounter += 1;
                    if (bulletHellCounter > (enraged ? 7 : 9))
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 < 1200) //blasts from below
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y + 1000f, 0f, -4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else if (bulletHellCounter2 < 1500) //blasts from left
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else //blasts from left and right
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                return;
            }
            if (!startSecondAttack && (NPC.life <= NPC.lifeMax * 0.75))
            {
                string key = "You seem so confident, even though you are painfully ignorant of what has yet to transpire.";
                Color messageColor = Color.Orange;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
                startSecondAttack = true;
                return;
            }
            #endregion
            #region ThirdAttack
            if (bulletHellCounter2 < 2700 && startThirdAttack)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

				if (!canDespawn)
					NPC.velocity *= 0.95f;

                float num740 = player.Center.X - vectorCenter.X;
                float num741 = player.Center.Y - vectorCenter.Y;
                NPC.rotation = (float)Math.Atan2(num741, num740) - MathHelper.PiOver2;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (bulletHellCounter2 % 180 == 0) //blasts from top
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer, 0f, 0f);

                    if (bulletHellCounter2 % 240 == 0) //fireblasts from above
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 10f * uDieLul, ModContent.ProjectileType<BrimstoneFireblast>(), fireblastDamage, 0f, Main.myPlayer, 0f, 0f);

                    bulletHellCounter += 1;
                    if (bulletHellCounter > (enraged ? 9 : 11))
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 < 2100) //blasts from above
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else if (bulletHellCounter2 < 2400) //blasts from right
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else //blasts from left and right
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                return;
            }
            if (!startThirdAttack && (NPC.life <= NPC.lifeMax * 0.5))
            {
                Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/SCL");
                string key = "Everything was going well until you came along.";
                Color messageColor = Color.Orange;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
                startThirdAttack = true;
                return;
            }
            #endregion
            #region FourthAttack
            if (bulletHellCounter2 < 3600 && startFourthAttack)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

				if (!canDespawn)
					NPC.velocity *= 0.95f;

                float num740 = player.Center.X - vectorCenter.X;
                float num741 = player.Center.Y - vectorCenter.Y;
                NPC.rotation = (float)Math.Atan2(num741, num740) - MathHelper.PiOver2;

                if (Main.netMode != NetmodeID.MultiplayerClient) //more clustered attack
                {
                    if (bulletHellCounter2 % 180 == 0) //blasts from top
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer, 0f, 0f);

                    if (bulletHellCounter2 % 240 == 0) //fireblasts from above
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 10f * uDieLul, ModContent.ProjectileType<BrimstoneFireblast>(), fireblastDamage, 0f, Main.myPlayer, 0f, 0f);

					int divisor = revenge ? 225 : expertMode ? 450 : 675;
                    if (bulletHellCounter2 % divisor == 0 && expertMode) //giant homing fireballs
                    {
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 1f * uDieLul, ModContent.ProjectileType<BrimstoneMonster>(), monsterDamage, 0f, Main.myPlayer, 0f, passedVar);
                        passedVar += 1f;
                    }

                    bulletHellCounter += 1;
                    if (bulletHellCounter > (enraged ? 10 : 12))
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 < 3000) //blasts from below
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y + 1000f, 0f, -4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else if (bulletHellCounter2 < 3300) //blasts from left
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else //blasts from left and right
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                return;
            }
            if (!startFourthAttack && (NPC.life <= NPC.lifeMax * 0.3))
            {
                Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/SCE");
                string key = "Hmm...perhaps I should let the little ones out to play for a while.";
                Color messageColor = Color.Orange;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
                startFourthAttack = true;
                return;
            }
            #endregion
            #region FifthAttack
            if (bulletHellCounter2 < 4500 && startFifthAttack)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

				if (!canDespawn)
					NPC.velocity *= 0.95f;

                float num740 = player.Center.X - vectorCenter.X;
                float num741 = player.Center.Y - vectorCenter.Y;
                NPC.rotation = (float)Math.Atan2(num741, num740) - MathHelper.PiOver2;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (bulletHellCounter2 % 240 == 0) //blasts from top
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer, 0f, 0f);

                    if (bulletHellCounter2 % 360 == 0) //fireblasts from above
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 10f * uDieLul, ModContent.ProjectileType<BrimstoneFireblast>(), fireblastDamage, 0f, Main.myPlayer, 0f, 0f);

                    if (bulletHellCounter2 % 30 == 0) //projectiles that move in wave pattern
                    {
						int random = Main.rand.Next(-500, 501);
						Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + random, -5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneWave>(), waveDamage, 0f, Main.myPlayer, 0f, 0f);
						Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - 1000f, player.position.Y - random, 5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneWave>(), waveDamage, 0f, Main.myPlayer, 0f, 0f);
					}

                    bulletHellCounter += 1;
                    if (bulletHellCounter > (enraged ? 12 : 14))
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 < 3900) //blasts from above
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else if (bulletHellCounter2 < 4200) //blasts from left and right
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else //blasts from above, left, and right
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 3f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                return;
            }
            if (!startFifthAttack && (NPC.life <= NPC.lifeMax * 0.1))
            {
                string key = "I'm just getting started!";
                Color messageColor = Color.Orange;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
                startFifthAttack = true;
                return;
            }
            #endregion
            #region EndSections
            if (startFifthAttack)
            {
                if (gettingTired5)
                {
                    Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/SCA");
                    NPC.noGravity = false;
                    NPC.noTileCollide = false;
                    NPC.damage = 0;

					if (!canDespawn)
						NPC.velocity.X *= 0.98f;

                    float num = player.Center.X - vectorCenter.X;
                    float num1 = player.Center.Y - vectorCenter.Y;
                    NPC.rotation = (float)Math.Atan2(num1, num) - MathHelper.PiOver2;

                    if (CalamityWorld.downedSCal) //after first time you kill her
                    {
                        if (giveUpCounter == 900)
                        {
                            string key = "Perhaps one of these times I'll change my mind...";
                            Color messageColor = Color.Orange;
                            CalamityUtils.DisplayLocalizedText(key, messageColor);
                        }
                        giveUpCounter--;
						bool canBeHit = giveUpCounter < 900;
                        NPC.chaseable = canBeHit;
                        NPC.dontTakeDamage = !canBeHit;
                        return;
                    }
                    if (giveUpCounter == 600)
                    {
                        string key = "He has grown far stronger since we last fought...you stand no chance.";
                        Color messageColor = Color.Orange;
                        CalamityUtils.DisplayLocalizedText(key, messageColor);
                    }
                    if (giveUpCounter == 300)
                    {
                        string key = "Well...I suppose this is the end...";
                        Color messageColor = Color.Orange;
                        CalamityUtils.DisplayLocalizedText(key, messageColor);
                    }
                    if (giveUpCounter <= 0)
                    {
                        NPC.chaseable = true;
                        NPC.dontTakeDamage = false;
                        return;
                    }
                    giveUpCounter--;
                    NPC.chaseable = false;
                    NPC.dontTakeDamage = true;
                    return;
                }
                if (!gettingTired5 && (NPC.life <= NPC.lifeMax * 0.01))
                {
					for (int x = 0; x < Main.maxProjectiles; x++)
					{
						Projectile projectile = Main.projectile[x];
						if (projectile.active && projectile.type == ModContent.ProjectileType<BrimstoneMonster>())
						{
							if (projectile.timeLeft > 90)
								projectile.timeLeft = 90;
						}
					}

					string key = "Not even I could defeat him! What hope do you have!?";
                    Color messageColor = Color.Orange;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                    gettingTired5 = true;
                    return;
                }
                else if (!gettingTired4 && (NPC.life <= NPC.lifeMax * 0.02))
                {
                    string key = "He has never lost a battle!";
                    Color messageColor = Color.Orange;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                    gettingTired4 = true;
                    return;
                }
                else if (!gettingTired3 && (NPC.life <= NPC.lifeMax * 0.04))
                {
                    string key = "Even if you defeat me you would still have the lord to contend with!";
                    Color messageColor = Color.Orange;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                    gettingTired3 = true;
                    return;
                }
                else if (!gettingTired2 && (NPC.life <= NPC.lifeMax * 0.06))
                {
                    string key = "Just stop!";
                    Color messageColor = Color.Orange;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                    gettingTired2 = true;
                    return;
                }
                else if (!gettingTired && (NPC.life <= NPC.lifeMax * 0.08))
                {
                    string key = "How are you still alive!?";
                    Color messageColor = Color.Orange;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        spawnY += 250;
                        if (death)
                        {
                            spawnY -= 50;
                        }
                        for (int x = 0; x < 5; x++)
                        {
                            NPC.NewNPC(NPC.GetSource_FromThis(), spawnX + 50, spawnY, ModContent.NPCType<SCalWormHeart>(), 0, 0f, 0f, 0f, 0f, 255);
                            spawnX += spawnXAdd;
                            NPC.NewNPC(NPC.GetSource_FromThis(), spawnX2 - 50, spawnY, ModContent.NPCType<SCalWormHeart>(), 0, 0f, 0f, 0f, 0f, 255);
                            spawnX2 -= spawnXAdd;
                            spawnY += spawnYAdd;
                        }
                        spawnX = spawnXReset;
                        spawnX2 = spawnXReset2;
                        spawnY = spawnYReset;
                        NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), ModContent.NPCType<SCalWormHead>());
                    }
                    gettingTired = true;
                    return;
                }
            }
            #endregion
            #region DespawnProjectiles
            if (bulletHellCounter2 % 900 == 0 && despawnProj)
            {
                for (int x = 0; x < Main.maxProjectiles; x++)
                {
                    Projectile projectile = Main.projectile[x];
                    if (projectile.active)
                    {
                        if (projectile.type == ModContent.ProjectileType<BrimstoneHellblast2>() ||
                            projectile.type == ModContent.ProjectileType<BrimstoneBarrage>() ||
                            projectile.type == ModContent.ProjectileType<BrimstoneWave>())
                        {
							if (projectile.timeLeft > 60)
								projectile.timeLeft = 60;
                        }
                        else if (projectile.type == ModContent.ProjectileType<BrimstoneGigaBlast>() || projectile.type == ModContent.ProjectileType<BrimstoneFireblast>())
                        {
							projectile.ai[1] = 1f;

							if (projectile.timeLeft > 60)
								projectile.timeLeft = 60;
						}
                    }
                }
                despawnProj = false;
            }
            #endregion
            #region TransformSeekerandBrotherTriggers
            if (!halfLife && (NPC.life <= NPC.lifeMax * 0.4))
            {
                string key = "Don't worry, I still have plenty of tricks left.";
                Color messageColor = Color.Orange;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
                halfLife = true;
            }

            if (NPC.life <= NPC.lifeMax * 0.2)
            {
                if (secondStage == false)
                {
                    string key = "Impressive...but still not good enough!";
                    Color messageColor = Color.Orange;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        SoundEngine.PlaySound(SoundID.Item74, NPC.position);
                        for (int I = 0; I < 20; I++)
                        {
                            int FireEye = NPC.NewNPC(NPC.GetSource_FromThis(), (int)(vectorCenter.X + (Math.Sin(I * 18) * 300)), (int)(vectorCenter.Y + (Math.Cos(I * 18) * 300)), ModContent.NPCType<SoulSeekerSupreme>(), NPC.whoAmI, 0, 0, 0, -1);
                            NPC Eye = Main.npc[FireEye];
                            Eye.ai[0] = I * 18;
                            Eye.ai[3] = I * 18;
                        }
                    }
                    secondStage = true;
                }
            }

            if (bossLife == 0f && NPC.life > 0)
            {
                bossLife = NPC.lifeMax;
            }
            if (NPC.life > 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int num660 = (int)(NPC.lifeMax * 0.55);
                    if ((NPC.life + num660) < bossLife)
                    {
                        bossLife = NPC.life;
                        NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), ModContent.NPCType<SupremeCataclysm>());
                        NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), ModContent.NPCType<SupremeCatastrophe>());
                        string key = "Brothers, could you assist me for a moment? This ordeal is growing tiresome.";
                        Color messageColor = Color.Orange;
                        CalamityUtils.DisplayLocalizedText(key, messageColor);
                    }
                }
            }

            #endregion
            #region TargetandRotation
            float num801 = NPC.position.X + (NPC.width / 2) - player.position.X - (player.width / 2);
            float num802 = NPC.position.Y + NPC.height - 59f - player.position.Y - (player.height / 2);
            float num803 = (float)Math.Atan2(num802, num801) + MathHelper.PiOver2;

            if (num803 < 0f)
                num803 += MathHelper.TwoPi;
            else if (num803 > MathHelper.TwoPi)
                num803 -= MathHelper.TwoPi;

            float num804 = 0.1f;
			if (NPC.rotation < num803)
			{
				if ((num803 - NPC.rotation) > MathHelper.Pi)
					NPC.rotation -= num804;
				else
					NPC.rotation += num804;
			}
			else if (NPC.rotation > num803)
			{
				if ((NPC.rotation - num803) > MathHelper.Pi)
					NPC.rotation += num804;
				else
					NPC.rotation -= num804;
			}

            if (NPC.rotation > num803 - num804 && NPC.rotation < num803 + num804)
                NPC.rotation = num803;

            if (NPC.rotation < 0f)
                NPC.rotation += MathHelper.TwoPi;
            else if (NPC.rotation > MathHelper.TwoPi)
                NPC.rotation -= MathHelper.TwoPi;

            if (NPC.rotation > num803 - num804 && NPC.rotation < num803 + num804)
                NPC.rotation = num803;
            #endregion
            #region FirstStage
            if (NPC.ai[0] == 0f)
            {
                NPC.damage = NPC.defDamage;
                if (wormAlive)
                {
                    NPC.dontTakeDamage = true;
                    NPC.chaseable = false;
                }
                else
                {
                    if (cataclysmAlive || catastropheAlive)
                    {
                        NPC.dontTakeDamage = true;
                        NPC.chaseable = false;
                        NPC.damage = 0;

						if (!canDespawn)
							NPC.velocity *= 0.95f;

                        float num740 = player.Center.X - vectorCenter.X;
                        float num741 = player.Center.Y - vectorCenter.Y;
                        NPC.rotation = (float)Math.Atan2(num741, num740) - MathHelper.PiOver2;
                        return;
                    }
                    else
                    {
                        NPC.dontTakeDamage = false;
                        NPC.chaseable = true;
                    }
                }

				if (NPC.ai[1] == -1f)
				{
					phaseChange++;
					if (phaseChange > 23)
						phaseChange = 0;

					int phase = 0; //0 = shots above 1 = charge 2 = nothing 3 = hellblasts 4 = fireblasts
					switch (phaseChange)
					{
						case 0:
							phase = 0;
							willCharge = false;
							break; //0341
						case 1:
							phase = 3;
							break;
						case 2:
							phase = 4;
							willCharge = true;
							break;
						case 3:
							phase = 1;
							break;
						case 4:
							phase = 1;
							break; //1430
						case 5:
							phase = 4;
							willCharge = false;
							break;
						case 6:
							phase = 3;
							break;
						case 7:
							phase = 0;
							willCharge = true;
							break;
						case 8:
							phase = 1;
							break; //1034
						case 9:
							phase = 0;
							willCharge = false;
							break;
						case 10:
							phase = 3;
							break;
						case 11:
							phase = 4;
							break;
						case 12:
							phase = 4;
							break; //4310
						case 13:
							phase = 3;
							willCharge = true;
							break;
						case 14:
							phase = 1;
							break;
						case 15:
							phase = 0;
							willCharge = false;
							break;
						case 16:
							phase = 4;
							break; //4411
						case 17:
							phase = 4;
							willCharge = true;
							break;
						case 18:
							phase = 1;
							break;
						case 19:
							phase = 1;
							break;
						case 20:
							phase = 0;
							break; //0101
						case 21:
							phase = 1;
							break;
						case 22:
							phase = 0;
							break;
						case 23:
							phase = 1;
							break;
					}

					NPC.ai[1] = phase;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
				}
				else
				{
					if (NPC.ai[1] == 0f)
					{
						float num823 = 12f;
						float num824 = 0.12f;

						// Reduce acceleration if target is holding a true melee weapon
						Item targetSelectedItem = player.inventory[player.selectedItem];
						if (targetSelectedItem.CountsAsClass(DamageClass.Melee) && (targetSelectedItem.shoot == 0 || CalamityLists.trueMeleeProjectileList.Contains(targetSelectedItem.shoot)))
						{
							num824 *= 0.5f;
						}

						Vector2 vector82 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float num825 = player.position.X + (player.width / 2) - vector82.X;
						float num826 = player.position.Y + (player.height / 2) - 550f - vector82.Y;
						float num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);

						num827 = num823 / num827;
						num825 *= num827;
						num826 *= num827;

						if (!canDespawn)
						{
							if (NPC.velocity.X < num825)
							{
								NPC.velocity.X += num824;
								if (NPC.velocity.X < 0f && num825 > 0f)
									NPC.velocity.X += num824;
							}
							else if (NPC.velocity.X > num825)
							{
								NPC.velocity.X -= num824;
								if (NPC.velocity.X > 0f && num825 < 0f)
									NPC.velocity.X -= num824;
							}
							if (NPC.velocity.Y < num826)
							{
								NPC.velocity.Y += num824;
								if (NPC.velocity.Y < 0f && num826 > 0f)
									NPC.velocity.Y += num824;
							}
							else if (NPC.velocity.Y > num826)
							{
								NPC.velocity.Y -= num824;
								if (NPC.velocity.Y > 0f && num826 < 0f)
									NPC.velocity.Y -= num824;
							}
						}

						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 300f)
						{
							NPC.ai[1] = -1f;
							NPC.TargetClosest(true);
							NPC.netUpdate = true;
						}

						vector82 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						num825 = player.position.X + (player.width / 2) - vector82.X;
						num826 = player.position.Y + (player.height / 2) - vector82.Y;
						NPC.rotation = (float)Math.Atan2(num826, num825) - MathHelper.PiOver2;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NPC.localAI[1] += wormAlive ? 0.5f : 1f;
							if (NPC.localAI[1] > 90f)
							{
								NPC.localAI[1] = 0f;

								float num828 = 10f * uDieLul;
								Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
								float num180 = player.position.X + player.width * 0.5f - value9.X;
								float num181 = Math.Abs(num180) * 0.1f;
								float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
								float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);

								num183 = num828 / num183;
								num180 *= num183;
								num182 *= num183;
								value9.X += num180;
								value9.Y += num182;

								int randomShot = Main.rand.Next(6);
								if (randomShot == 0 && canFireSplitingFireball)
								{
									canFireSplitingFireball = false;
									randomShot = ModContent.ProjectileType<BrimstoneFireblast>();
									SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
									num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
									num827 = num828 / num827;
									num825 *= num827;
									num826 *= num827;
									vector82.X += num825 * 8f;
									vector82.Y += num826 * 8f;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), vector82.X, vector82.Y, num825, num826, randomShot, fireblastDamage, 0f, Main.myPlayer, 0f, 0f);
								}
								else if (randomShot == 1 && canFireSplitingFireball)
								{
									canFireSplitingFireball = false;
									randomShot = ModContent.ProjectileType<BrimstoneGigaBlast>();
									SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneShoot"), NPC.Center);
									num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
									num827 = num828 / num827;
									num825 *= num827;
									num826 *= num827;
									vector82.X += num825 * 8f;
									vector82.Y += num826 * 8f;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), vector82.X, vector82.Y, num825, num826, randomShot, gigablastDamage, 0f, Main.myPlayer, 0f, 0f);
								}
								else
								{
									canFireSplitingFireball = true;
									randomShot = ModContent.ProjectileType<BrimstoneBarrage>();
									SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
									for (int num186 = 0; num186 < 8; num186++)
									{
										num180 = player.position.X + player.width * 0.5f - value9.X;
										num182 = player.position.Y + player.height * 0.5f - value9.Y;
										num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
										float speedBoost = num186 > 3 ? -(num186 - 3) : num186;
										num183 = (8f + speedBoost) / num183;
										num180 *= num183;
										num182 *= num183;
										Projectile.NewProjectile(NPC.GetSource_FromThis(), value9.X, value9.Y, num180 + speedBoost, num182 + speedBoost, randomShot, barrageDamage, 0f, Main.myPlayer, 0f, 0f);
									}
								}
							}
						}
					}
					else if (NPC.ai[1] == 1f)
					{
						NPC.rotation = num803;
						float num383 = wormAlive ? 26f : 30f;
						if (NPC.life < NPC.lifeMax * 0.95)
							num383 += 1f;
						if (NPC.life < NPC.lifeMax * 0.85)
							num383 += 1f;
						if (NPC.life < NPC.lifeMax * 0.7)
							num383 += 1f;
						if (NPC.life < NPC.lifeMax * 0.6)
							num383 += 1f;
						if (NPC.life < NPC.lifeMax * 0.5)
							num383 += 1f;

						Vector2 vector37 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float num384 = player.position.X + (player.width / 2) - vector37.X;
						float num385 = player.position.Y + (player.height / 2) - vector37.Y;
						float num386 = (float)Math.Sqrt(num384 * num384 + num385 * num385);
						num386 = num383 / num386;

						if (!canDespawn)
						{
							NPC.velocity.X = num384 * num386;
							NPC.velocity.Y = num385 * num386;
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/SCalDash"), NPC.Center);
						}

						NPC.ai[1] = 2f;
					}
					else if (NPC.ai[1] == 2f)
					{
						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 25f)
						{
							if (!canDespawn)
							{
								NPC.velocity *= 0.96f;

								if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
									NPC.velocity.X = 0f;
								if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
									NPC.velocity.Y = 0f;
							}
						}
						else
							NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - MathHelper.PiOver2;

						if (NPC.ai[2] >= 70f)
						{
							NPC.ai[3] += 1f;
							NPC.ai[2] = 0f;
							NPC.target = 255;
							NPC.rotation = num803;

							if (NPC.ai[3] >= 2f)
								NPC.ai[1] = -1f;
							else
								NPC.ai[1] = 1f;
						}
					}
					else if (NPC.ai[1] == 3f)
					{
						float num412 = 32f;
						float num413 = 1.2f;

						// Reduce acceleration if target is holding a true melee weapon
						Item targetSelectedItem = player.inventory[player.selectedItem];
						if (targetSelectedItem.CountsAsClass(DamageClass.Melee) && (targetSelectedItem.shoot == 0 || CalamityLists.trueMeleeProjectileList.Contains(targetSelectedItem.shoot)))
						{
							num413 *= 0.5f;
						}

						int num414 = 1;
						if (NPC.position.X + (NPC.width / 2) < player.position.X + player.width)
							num414 = -1;

						Vector2 vector40 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float num415 = player.position.X + (player.width / 2) + (num414 * 600) - vector40.X;
						float num416 = player.position.Y + (player.height / 2) - vector40.Y;
						float num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);

						num417 = num412 / num417;
						num415 *= num417;
						num416 *= num417;

						if (!canDespawn)
						{
							if (NPC.velocity.X < num415)
							{
								NPC.velocity.X += num413;
								if (NPC.velocity.X < 0f && num415 > 0f)
									NPC.velocity.X += num413;
							}
							else if (NPC.velocity.X > num415)
							{
								NPC.velocity.X -= num413;
								if (NPC.velocity.X > 0f && num415 < 0f)
									NPC.velocity.X -= num413;
							}
							if (NPC.velocity.Y < num416)
							{
								NPC.velocity.Y += num413;
								if (NPC.velocity.Y < 0f && num416 > 0f)
									NPC.velocity.Y += num413;
							}
							else if (NPC.velocity.Y > num416)
							{
								NPC.velocity.Y -= num413;
								if (NPC.velocity.Y > 0f && num416 < 0f)
									NPC.velocity.Y -= num413;
							}
						}

						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 480f)
						{
							NPC.TargetClosest(true);
							NPC.ai[1] = -1f;
							NPC.target = 255;
							NPC.netUpdate = true;
						}
						else
						{
							if (!player.dead)
								NPC.ai[3] += wormAlive ? 0.5f : 1f;

							if (NPC.ai[3] >= 20f)
							{
								NPC.ai[3] = 0f;
								vector40 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
								num415 = player.position.X + (player.width / 2) - vector40.X;
								num416 = player.position.Y + (player.height / 2) - vector40.Y;
								SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneHellblastSound"), NPC.Center);

								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									float num418 = 10f * uDieLul;
									int num420 = ModContent.ProjectileType<BrimstoneHellblast>();
									num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);
									num417 = num418 / num417;
									num415 *= num417;
									num416 *= num417;
									vector40.X += num415 * 4f;
									vector40.Y += num416 * 4f;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), vector40.X, vector40.Y, num415, num416, num420, hellblastDamage, 0f, Main.myPlayer, 0f, 0f);
								}
							}
						}
					}
					else if (NPC.ai[1] == 4f)
					{
						int num831 = 1;
						if (NPC.position.X + (NPC.width / 2) < player.position.X + player.width)
							num831 = -1;

						float num832 = 32f;
						float num833 = 1.2f;

						// Reduce acceleration if target is holding a true melee weapon
						Item targetSelectedItem = player.HeldItem;
						if (targetSelectedItem.CountsAsClass(DamageClass.Melee) && (targetSelectedItem.shoot == 0 || CalamityLists.trueMeleeProjectileList.Contains(targetSelectedItem.shoot)))
						{
							num833 *= 0.5f;
						}

						Vector2 vector83 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float num834 = player.position.X + (player.width / 2) + (num831 * 750) - vector83.X; //600
						float num835 = player.position.Y + (player.height / 2) - vector83.Y;
						float num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);

						num836 = num832 / num836;
						num834 *= num836;
						num835 *= num836;

						if (!canDespawn)
						{
							if (NPC.velocity.X < num834)
							{
								NPC.velocity.X += num833;
								if (NPC.velocity.X < 0f && num834 > 0f)
									NPC.velocity.X += num833;
							}
							else if (NPC.velocity.X > num834)
							{
								NPC.velocity.X -= num833;
								if (NPC.velocity.X > 0f && num834 < 0f)
									NPC.velocity.X -= num833;
							}
							if (NPC.velocity.Y < num835)
							{
								NPC.velocity.Y += num833;
								if (NPC.velocity.Y < 0f && num835 > 0f)
									NPC.velocity.Y += num833;
							}
							else if (NPC.velocity.Y > num835)
							{
								NPC.velocity.Y -= num833;
								if (NPC.velocity.Y > 0f && num835 < 0f)
									NPC.velocity.Y -= num833;
							}
						}

						vector83 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						num834 = player.position.X + (player.width / 2) - vector83.X;
						num835 = player.position.Y + (player.height / 2) - vector83.Y;
						NPC.rotation = (float)Math.Atan2(num835, num834) - MathHelper.PiOver2;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NPC.localAI[1] += wormAlive ? 0.5f : 1f;
							if (NPC.localAI[1] > 140f)
							{
								SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
								NPC.localAI[1] = 0f;
								float num837 = 5f * uDieLul;
								int num839 = ModContent.ProjectileType<BrimstoneFireblast>();
								num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);
								num836 = num837 / num836;
								num834 *= num836;
								num835 *= num836;
								vector83.X += num834 * 8f;
								vector83.Y += num835 * 8f;
								Projectile.NewProjectile(NPC.GetSource_FromThis(), vector83.X, vector83.Y, num834, num835, num839, fireblastDamage, 0f, Main.myPlayer, 0f, 0f);
							}
						}

						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 300f)
						{
							NPC.ai[1] = -1f;
							NPC.TargetClosest(true);
							NPC.netUpdate = true;
						}
					}
				}

                if (NPC.life < NPC.lifeMax * 0.4)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    NPC.netUpdate = true;
                }
            }
            #endregion
            #region Transition
            else if (NPC.ai[0] == 1f || NPC.ai[0] == 2f)
            {
                NPC.dontTakeDamage = true;
                NPC.chaseable = false;

                if (NPC.ai[0] == 1f)
                {
                    NPC.ai[2] += 0.005f;
                    if (NPC.ai[2] > 0.5)
                        NPC.ai[2] = 0.5f;
                }
                else
                {
                    NPC.ai[2] -= 0.005f;
                    if (NPC.ai[2] < 0f)
                        NPC.ai[2] = 0f;
                }

                NPC.rotation += NPC.ai[2];

                NPC.ai[1] += 1f;
                if (NPC.ai[1] == 100f)
                {
                    NPC.ai[0] += 1f;
                    NPC.ai[1] = 0f;

                    if (NPC.ai[0] == 3f)
                        NPC.ai[2] = 0f;
                    else
                    {
                        for (int num388 = 0; num388 < 50; num388++)
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f, 0, default, 1f);

                        SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                    }
                }

                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f, 0, default, 1f);

				if (!canDespawn)
				{
					NPC.velocity *= 0.98f;

					if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
						NPC.velocity.X = 0f;
					if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
						NPC.velocity.Y = 0f;
				}
            }
            #endregion
            #region LastStage
            else
            {
                NPC.damage = NPC.defDamage;
                if (wormAlive)
                {
                    NPC.dontTakeDamage = true;
                    NPC.chaseable = false;
                }
                else
                {
                    if (NPC.AnyNPCs(ModContent.NPCType<SoulSeekerSupreme>()))
                    {
                        NPC.dontTakeDamage = true;
                        NPC.chaseable = false;
                    }
                    else
                    {
                        NPC.dontTakeDamage = false;
                        NPC.chaseable = true;
                    }
                }

				if (NPC.ai[1] == -1f)
				{
					phaseChange++;
					if (phaseChange > 23)
						phaseChange = 0;

					int phase = 0; //0 = shots above 1 = charge 2 = nothing 3 = hellblasts 4 = fireblasts
					switch (phaseChange)
					{
						case 0:
							phase = 0;
							willCharge = false;
							break; //0341
						case 1:
							phase = 3;
							break;
						case 2:
							phase = 4;
							willCharge = true;
							break;
						case 3:
							phase = 1;
							break;
						case 4:
							phase = 1;
							break; //1430
						case 5:
							phase = 4;
							willCharge = false;
							break;
						case 6:
							phase = 3;
							break;
						case 7:
							phase = 0;
							willCharge = true;
							break;
						case 8:
							phase = 1;
							break; //1034
						case 9:
							phase = 0;
							willCharge = false;
							break;
						case 10:
							phase = 3;
							break;
						case 11:
							phase = 4;
							break;
						case 12:
							phase = 4;
							break; //4310
						case 13:
							phase = 3;
							willCharge = true;
							break;
						case 14:
							phase = 1;
							break;
						case 15:
							phase = 0;
							willCharge = false;
							break;
						case 16:
							phase = 4;
							break; //4411
						case 17:
							phase = 4;
							willCharge = true;
							break;
						case 18:
							phase = 1;
							break;
						case 19:
							phase = 1;
							break;
						case 20:
							phase = 0;
							break; //0101
						case 21:
							phase = 1;
							break;
						case 22:
							phase = 0;
							break;
						case 23:
							phase = 1;
							break;
					}

					NPC.ai[1] = phase;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
				}
				else
				{
					if (NPC.ai[1] == 0f)
					{
						float num823 = 12f;
						float num824 = 0.12f;

						// Reduce acceleration if target is holding a true melee weapon
						Item targetSelectedItem = player.inventory[player.selectedItem];
						if (targetSelectedItem.CountsAsClass(DamageClass.Melee) && (targetSelectedItem.shoot == 0 || CalamityLists.trueMeleeProjectileList.Contains(targetSelectedItem.shoot)))
						{
							num824 *= 0.5f;
						}

						Vector2 vector82 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float num825 = player.position.X + (player.width / 2) - vector82.X;
						float num826 = player.position.Y + (player.height / 2) - 550f - vector82.Y;
						float num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);

						num827 = num823 / num827;
						num825 *= num827;
						num826 *= num827;

						if (!canDespawn)
						{
							if (NPC.velocity.X < num825)
							{
								NPC.velocity.X += num824;
								if (NPC.velocity.X < 0f && num825 > 0f)
									NPC.velocity.X += num824;
							}
							else if (NPC.velocity.X > num825)
							{
								NPC.velocity.X -= num824;
								if (NPC.velocity.X > 0f && num825 < 0f)
									NPC.velocity.X -= num824;
							}

							if (NPC.velocity.Y < num826)
							{
								NPC.velocity.Y += num824;
								if (NPC.velocity.Y < 0f && num826 > 0f)
									NPC.velocity.Y += num824;
							}
							else if (NPC.velocity.Y > num826)
							{
								NPC.velocity.Y -= num824;
								if (NPC.velocity.Y > 0f && num826 < 0f)
									NPC.velocity.Y -= num824;
							}
						}

						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 240f)
						{
							NPC.ai[1] = -1f;
							NPC.TargetClosest(true);
							NPC.netUpdate = true;
						}

						vector82 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						num825 = player.position.X + (player.width / 2) - vector82.X;
						num826 = player.position.Y + (player.height / 2) - vector82.Y;
						NPC.rotation = (float)Math.Atan2(num826, num825) - MathHelper.PiOver2;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NPC.localAI[1] += wormAlive ? 0.5f : 1f;
							if (NPC.localAI[1] > 60f)
							{
								NPC.localAI[1] = 0f;

								float num828 = 10f * uDieLul;
								Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
								float num180 = player.position.X + player.width * 0.5f - value9.X;
								float num181 = Math.Abs(num180) * 0.1f;
								float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
								float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);

								num183 = num828 / num183;
								num180 *= num183;
								num182 *= num183;
								value9.X += num180;
								value9.Y += num182;

								int randomShot = Main.rand.Next(6);
								if (randomShot == 0 && canFireSplitingFireball)
								{
									SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
									canFireSplitingFireball = false;
									randomShot = ModContent.ProjectileType<BrimstoneFireblast>();
									num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
									num827 = num828 / num827;
									num825 *= num827;
									num826 *= num827;
									vector82.X += num825 * 8f;
									vector82.Y += num826 * 8f;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), vector82.X, vector82.Y, num825, num826, randomShot, fireblastDamage, 0f, Main.myPlayer, 0f, 0f);
								}
								else if (randomShot == 1 && canFireSplitingFireball)
								{
									SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneShoot"), NPC.Center);
									canFireSplitingFireball = false;
									randomShot = ModContent.ProjectileType<BrimstoneGigaBlast>();
									num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
									num827 = num828 / num827;
									num825 *= num827;
									num826 *= num827;
									vector82.X += num825 * 8f;
									vector82.Y += num826 * 8f;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), vector82.X, vector82.Y, num825, num826, randomShot, gigablastDamage, 0f, Main.myPlayer, 0f, 0f);
								}
								else
								{
									SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
									canFireSplitingFireball = true;
									randomShot = ModContent.ProjectileType<BrimstoneBarrage>();
									for (int num186 = 0; num186 < 8; num186++)
									{
										num180 = player.position.X + player.width * 0.5f - value9.X;
										num182 = player.position.Y + player.height * 0.5f - value9.Y;
										num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
										float speedBoost = num186 > 3 ? -(num186 - 3) : num186;
										num183 = (8f + speedBoost) / num183;
										num180 *= num183;
										num182 *= num183;
										Projectile.NewProjectile(NPC.GetSource_FromThis(), value9.X, value9.Y, num180 + speedBoost, num182 + speedBoost, randomShot, barrageDamage, 0f, Main.myPlayer, 0f, 0f);
									}
								}
							}
						}
					}
					else if (NPC.ai[1] == 1f)
					{
						NPC.rotation = num803;
						float num383 = wormAlive ? 31f : 35f;
						if (NPC.life < NPC.lifeMax * 0.3)
							num383 += 1f;
						if (NPC.life < NPC.lifeMax * 0.2)
							num383 += 1f;
						if (NPC.life < NPC.lifeMax * 0.1)
							num383 += 1f;

						Vector2 vector37 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float num384 = player.position.X + (player.width / 2) - vector37.X;
						float num385 = player.position.Y + (player.height / 2) - vector37.Y;
						float num386 = (float)Math.Sqrt(num384 * num384 + num385 * num385);
						num386 = num383 / num386;

						if (!canDespawn)
						{
							NPC.velocity.X = num384 * num386;
							NPC.velocity.Y = num385 * num386;
						}

						NPC.ai[1] = 2f;
					}
					else if (NPC.ai[1] == 2f)
					{
						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 25f)
						{
							if (!canDespawn)
							{
								NPC.velocity *= 0.96f;

								if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
									NPC.velocity.X = 0f;
								if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
									NPC.velocity.Y = 0f;
							}
						}
						else
							NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - MathHelper.PiOver2;

						if (NPC.ai[2] >= 70f)
						{
							NPC.ai[3] += 1f;
							NPC.ai[2] = 0f;
							NPC.target = 255;
							NPC.rotation = num803;

							if (NPC.ai[3] >= 1f)
								NPC.ai[1] = -1f;
							else
								NPC.ai[1] = 1f;
						}
					}
					else if (NPC.ai[1] == 3f)
					{
						float num412 = 32f;
						float num413 = 1.2f;

						// Reduce acceleration if target is holding a true melee weapon
						Item targetSelectedItem = player.inventory[player.selectedItem];
						if (targetSelectedItem.CountsAsClass(DamageClass.Melee) && (targetSelectedItem.shoot == 0 || CalamityLists.trueMeleeProjectileList.Contains(targetSelectedItem.shoot)))
						{
							num413 *= 0.5f;
						}

						int num414 = 1;
						if (NPC.position.X + (NPC.width / 2) < player.position.X + player.width)
							num414 = -1;

						Vector2 vector40 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float num415 = player.position.X + (player.width / 2) + (num414 * 600) - vector40.X;
						float num416 = player.position.Y + (player.height / 2) - vector40.Y;
						float num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);

						num417 = num412 / num417;
						num415 *= num417;
						num416 *= num417;

						if (!canDespawn)
						{
							if (NPC.velocity.X < num415)
							{
								NPC.velocity.X += num413;
								if (NPC.velocity.X < 0f && num415 > 0f)
									NPC.velocity.X += num413;
							}
							else if (NPC.velocity.X > num415)
							{
								NPC.velocity.X -= num413;
								if (NPC.velocity.X > 0f && num415 < 0f)
									NPC.velocity.X -= num413;
							}
							if (NPC.velocity.Y < num416)
							{
								NPC.velocity.Y += num413;
								if (NPC.velocity.Y < 0f && num416 > 0f)
									NPC.velocity.Y += num413;
							}
							else if (NPC.velocity.Y > num416)
							{
								NPC.velocity.Y -= num413;
								if (NPC.velocity.Y > 0f && num416 < 0f)
									NPC.velocity.Y -= num413;
							}
						}

						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 300f)
						{
							NPC.TargetClosest(true);
							NPC.ai[1] = -1f;
							NPC.target = 255;
							NPC.netUpdate = true;
						}
						else
						{
							if (!player.dead)
								NPC.ai[3] += wormAlive ? 0.5f : 1f;

							if (NPC.ai[3] >= 24f)
							{
								NPC.ai[3] = 0f;
								vector40 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
								num415 = player.position.X + (player.width / 2) - vector40.X;
								num416 = player.position.Y + (player.height / 2) - vector40.Y;
								SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneHellblastSound"), NPC.Center);

								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									float num418 = 10f * uDieLul;
									int num420 = ModContent.ProjectileType<BrimstoneHellblast>();
									num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);
									num417 = num418 / num417;
									num415 *= num417;
									num416 *= num417;
									vector40.X += num415 * 4f;
									vector40.Y += num416 * 4f;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), vector40.X, vector40.Y, num415, num416, num420, hellblastDamage, 0f, Main.myPlayer, 0f, 0f);
								}
							}
						}
					}
					else if (NPC.ai[1] == 4f)
					{
						int num831 = 1;
						if (NPC.position.X + (NPC.width / 2) < player.position.X + player.width)
							num831 = -1;

						float num832 = 32f;
						float num833 = 1.2f;

						// Reduce acceleration if target is holding a true melee weapon
						Item targetSelectedItem = player.inventory[player.selectedItem];
						if (targetSelectedItem.CountsAsClass(DamageClass.Melee) && (targetSelectedItem.shoot == 0 || CalamityLists.trueMeleeProjectileList.Contains(targetSelectedItem.shoot)))
						{
							num833 *= 0.5f;
						}

						Vector2 vector83 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float num834 = player.position.X + (player.width / 2) + (num831 * 750) - vector83.X; //600
						float num835 = player.position.Y + (player.height / 2) - vector83.Y;
						float num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);

						num836 = num832 / num836;
						num834 *= num836;
						num835 *= num836;

						if (!canDespawn)
						{
							if (NPC.velocity.X < num834)
							{
								NPC.velocity.X += num833;
								if (NPC.velocity.X < 0f && num834 > 0f)
									NPC.velocity.X += num833;
							}
							else if (NPC.velocity.X > num834)
							{
								NPC.velocity.X -= num833;
								if (NPC.velocity.X > 0f && num834 < 0f)
									NPC.velocity.X -= num833;
							}
							if (NPC.velocity.Y < num835)
							{
								NPC.velocity.Y += num833;
								if (NPC.velocity.Y < 0f && num835 > 0f)
									NPC.velocity.Y += num833;
							}
							else if (NPC.velocity.Y > num835)
							{
								NPC.velocity.Y -= num833;
								if (NPC.velocity.Y > 0f && num835 < 0f)
									NPC.velocity.Y -= num833;
							}
						}

						vector83 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						num834 = player.position.X + (player.width / 2) - vector83.X;
						num835 = player.position.Y + (player.height / 2) - vector83.Y;
						NPC.rotation = (float)Math.Atan2(num835, num834) - MathHelper.PiOver2;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NPC.localAI[1] += wormAlive ? 0.5f : 1f;
							if (NPC.localAI[1] > 100f)
							{
								SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
								NPC.localAI[1] = 0f;
								float num837 = 5f * uDieLul;
								int num839 = ModContent.ProjectileType<BrimstoneFireblast>();
								num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);
								num836 = num837 / num836;
								num834 *= num836;
								num835 *= num836;
								vector83.X += num834 * 8f;
								vector83.Y += num835 * 8f;
								int shot = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector83.X, vector83.Y, num834, num835, num839, fireblastDamage, 0f, Main.myPlayer, 0f, 0f);
							}
						}

						NPC.ai[2] += 1f;
						if (NPC.ai[2] >= 240f)
						{
							NPC.ai[1] = -1f;
							NPC.TargetClosest(true);
							NPC.netUpdate = true;
						}
					}
				}
            }
            #endregion
        }

        #region Loot
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ModContent.ItemType<OmegaHealingPotion>();
        }

        // If SCal is killed too quickly, cancel all drops and chastise the player
        public override bool SpecialOnKill()
        {
			//75 seconds for bullet hells + 25 seconds for normal phases.
			//Does not occur in Boss Rush due to weakened SCal + stronger weapons (rarely occurs with just Cal gear)
            if ((lootTimer < 6000) && !BossRushEvent.BossRushActive)
            {
                string key = "Go to hell.";
                Color messageColor = Color.Orange;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
                return true;
            }

            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Materials
            npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<CalamitousEssence>(), 1, 30, 40);
            npcLoot.AddConditionalPerPlayer(() => !Main.expertMode, ModContent.ItemType<CalamitousEssence>(), 1, 20, 30);

            // Weapons

			// All non-hybrid weapons are listed twice so that the drop rates are actually equal between each unique weapon
			int[] weapons = new int[]
			{
				ModContent.ItemType<Animus>(), ModContent.ItemType<Animus>(),
				ModContent.ItemType<Azathoth>(), ModContent.ItemType<Azathoth>(),
				ModContent.ItemType<Contagion>(), ModContent.ItemType<Contagion>(),
				ModContent.ItemType<CrystylCrusher>(), ModContent.ItemType<CrystylCrusher>(),
				ModContent.ItemType<DraconicDestruction>(), ModContent.ItemType<DraconicDestruction>(),
				ModContent.ItemType<Earth>(), ModContent.ItemType<Earth>(),
				ModContent.ItemType<Fabstaff>(), ModContent.ItemType<Fabstaff>(),
				ModContent.ItemType<RoyalKnivesMelee>(), ModContent.ItemType<RoyalKnives>(), // Illustrious Knives
				ModContent.ItemType<NanoblackReaperMelee>(), ModContent.ItemType<NanoblackReaperRogue>(),
				ModContent.ItemType<RedSun>(), ModContent.ItemType<RedSun>(),
				ModContent.ItemType<ScarletDevil>(), ModContent.ItemType<ScarletDevil>(),
				ModContent.ItemType<SomaPrime>(), ModContent.ItemType<SomaPrime>(),
				ModContent.ItemType<BlushieStaff>(), ModContent.ItemType<BlushieStaff>(), // Staff of Blushie
				ModContent.ItemType<Svantechnical>(), ModContent.ItemType<Svantechnical>(),
				ModContent.ItemType<Judgement>(), ModContent.ItemType<Judgement>(),
				ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>(), ModContent.ItemType<TriactisTruePaladinianMageHammerofMight>(),
				ModContent.ItemType<Megafleet>(), ModContent.ItemType<Megafleet>(), // Voidragon
				ModContent.ItemType<Endogenesis>(), ModContent.ItemType<Endogenesis>(),
				ModContent.ItemType<BensUmbrella>(), ModContent.ItemType<BensUmbrella>(), //Temporal Umbrella
				ModContent.ItemType<PrototypeAndromechaRing>(), ModContent.ItemType<PrototypeAndromechaRing>()
			};
			// slight inaccuracy: scal's drops are instanced per player.
			npcLoot.Add(ItemDropRule.OneFromOptions(1, weapons));
			
            npcLoot.AddConditionalPerPlayer(() => CalamityWorld.revenge, ModContent.ItemType<Vehemenc>(), 1);

            // Vanity
            npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<BrimstoneJewel>(), Main.expertMode);
            npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<BrimstoneJewel>(), 1, 1, 1, !Main.expertMode);
            npcLoot.AddConditionalPerPlayer(() => CalamityWorld.death, ModContent.ItemType<Levi>(), 1);

            // Other
            npcLoot.Add(ModContent.ItemType<SupremeCalamitasTrophy>(), 10);
            npcLoot.AddIf(() => !CalamityWorld.downedSCal, ModContent.ItemType<KnowledgeCalamitas>());
            npcLoot.AddResidentEvilAmmo(CalamityWorld.downedSCal, 6, 3, 2);
            npcLoot.AddIf(() =>
            {
	            if (Main.player[NPC.target].Calamity().sCalKillCount == 0 &&
	                Main.LocalPlayer.Calamity().sCalDeathCount == 3) // Three deaths exactly rewards Lul
		            return true;
	            return false;
            }, ModContent.ItemType<CheatTestThing>());
        }
        
        public override void OnKill()
        {
            DeathMessage();

            // Incrase the player's SCal kill count
            if (Main.player[NPC.target].Calamity().sCalKillCount < 5)
                Main.player[NPC.target].Calamity().sCalKillCount++;
			
            // Mark Supreme Calamitas as dead
            CalamityWorld.downedSCal = true;
            CalamityNetcode.SyncWorld();
        }
        #endregion

        private void DeathMessage()
        {
            Color messageColor = Color.Orange;
            string key;

            // If the player has never killed SCal before, comment on how many attempts it took
            if (Main.player[NPC.target].Calamity().sCalKillCount == 0)
            {
                switch (Main.LocalPlayer.Calamity().sCalDeathCount)
                {
                    case 0:
                        key = "You didn't die at all huh? Welp, you probably cheated. Do it again, for real this time...but here's your reward I guess.";
                        break;
                    case 1:
                        key = "One death? That's it? ...I guess you earned this then.";
                        break;
                    case 2:
                        key = "Two deaths, nice job. Here's your reward.";
                        break;
                    case 3: // Three deaths exactly rewards Lul
                        key = "Third time's the charm. Here's a special reward.";
                        break;
                    default: // Four or more deaths: Lul is permanently missed
                        key = "At long last I am free...for a time. I'll keep coming back, just like you. Until we meet again, farewell.";
                        break;
                }
            }
            else
            {
                // If SCal has been killed before, instead comment on her respawning
                key = "At long last I am free...for a time. I'll keep coming back, just like you. Until we meet again, farewell.";
            }

            CalamityUtils.DisplayLocalizedText(key, messageColor);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ModContent.ProjectileType<SonOfYharon>())
            {
                projectile.damage /= 2;
            }
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
	        modifiers.ModifyHitInfo += NoDamage;
        }
        
        public void NoDamage(ref NPC.HitInfo hit)
        {
	        if (hit.Damage >= NPC.lifeMax * 0.5f)
	        {
		        hit.Damage = 0;
	        }
        }

        public override bool CheckActive()
        {
            return canDespawn;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

		public override Color? GetAlpha(Color drawColor)
		{
			if (willCharge)
				return new Color(0, 0, 0, NPC.alpha);
			return null;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = NPC.ai[0] > 1f ? ModContent.Request<Texture2D>("CalRD/NPCs/SupremeCalamitas/SupremeCalamitas2").Value : TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 7;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
					vector41 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - screenPos;
			vector43 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = NPC.ai[0] > 1f || NPC.IsABestiaryIconDummy ? ModContent.Request<Texture2D>("CalRD/NPCs/SupremeCalamitas/SupremeCalamitas2Glow").Value : ModContent.Request<Texture2D>("CalRD/NPCs/SupremeCalamitas/SupremeCalamitasGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num163 = 1; num163 < num153; num163++)
				{
					Color color41 = color37;
					color41 = Color.Lerp(color41, color36, amount9);
					color41 *= (num153 - num163) / 15f;
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
					vector44 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 100;
                NPC.height = 100;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 600, true);
        }
    }
}
