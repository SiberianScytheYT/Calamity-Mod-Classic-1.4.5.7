using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.Items.Accessories;
using CalRD.Items.Accessories.Wings;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Dyes;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.SummonItems;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.TownNPCs;
using CalRD.Projectiles.Boss;
using CalRD.Projectiles.Summon;
using CalRD.Projectiles.Typeless;
using CalRD.Tiles.FurnitureProfaned;
using CalRD.Tiles.Ores;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.NPCs.Providence
{
	[AutoloadBossHead]
	public class Providence : ModNPC
	{
		private enum Phase
		{
			PhaseChange = -1,
			HolyBlast = 0,
			HolyFire = 1,
			FlameCocoon = 2,
			MoltenBlobs = 3,
			HolyBomb = 4,
			SpearCocoon = 5,
			Crystal = 6,
			Laser = 7
		}

		private float AIState
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}

		private bool text = false;
        private bool useDefenseFrames = false;
        private float bossLife;
        private int biomeType = 0;
        private int flightPath = 0;
        private int phaseChange = 0;
        private int immuneTimer = 300;
        private int frameUsed = 0;
        private int healTimer = 0;
        internal bool challenge = Main.expertMode/* && Main.netMode == NetmodeID.SinglePlayer*/; //Used to determine if Profaned Soul Crystal should drop, couldn't figure out mp mems always dropping it so challenge is singleplayer only.
		internal bool hasTakenDaytimeDamage = false;

		public static float normalDR = 0.35f;
        public static float cocoonDR = 0.9f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Providence, the Profaned Goddess");
            Main.npcFrameCount[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

        public override void SetDefaults()
        {
            NPC.npcSlots = 36f;
            NPC.damage = 100;
            NPC.width = 600;
            NPC.height = 450;
            NPC.defense = 50;
			NPC.DR_NERD(normalDR, null, null, null, true);
			CalamityGlobalNPC global = NPC.Calamity();
            global.flatDRReductions.Add(BuffID.CursedInferno, 0.05f);
            NPC.LifeMaxNERB(440000, 500000, 12500000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 50, 0, 0);
            NPC.boss = true;
			NPC.Opacity = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.buffImmune[BuffID.StardustMinionBleed] = false;
            NPC.buffImmune[BuffID.BetsysCurse] = false;
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.chaseable = true;
            NPC.canGhostHeal = false;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/ProvidenceTheme");
            NPC.DeathSound = new SoundStyle("CalRD/Sounds/NPCKilled/ProvidenceDeath");
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(text);
            writer.Write(useDefenseFrames);
            writer.Write(biomeType);
            writer.Write(phaseChange);
            writer.Write(immuneTimer);
            writer.Write(frameUsed);
            writer.Write(healTimer);
            writer.Write(flightPath);
            writer.Write(NPC.dontTakeDamage);
            writer.Write(NPC.chaseable);
            writer.Write(NPC.canGhostHeal);
			writer.Write(NPC.localAI[2]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            text = reader.ReadBoolean();
            useDefenseFrames = reader.ReadBoolean();
            biomeType = reader.ReadInt32();
            phaseChange = reader.ReadInt32();
            immuneTimer = reader.ReadInt32();
            frameUsed = reader.ReadInt32();
            healTimer = reader.ReadInt32();
            flightPath = reader.ReadInt32();
            NPC.dontTakeDamage = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();
            NPC.canGhostHeal = reader.ReadBoolean();
			NPC.localAI[2] = reader.ReadSingle();
        }

        public override void AI()
        {
            CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();

            // whoAmI variable for Guardians and other things
            CalamityGlobalNPC.holyBoss = NPC.whoAmI;

            // Rotation
            NPC.rotation = NPC.velocity.X * 0.004f;

			// Target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				NPC.TargetClosest(true);

			// Target variable and boss center
			Player player = Main.player[NPC.target];
            Vector2 vector = NPC.Center;

            // Target's current biome
            bool isHoly = player.ZoneHallow;
            bool isHell = player.ZoneUnderworldHeight;

            // Fire projectiles at normal rate or not
            bool normalAttackRate = true;

			// Is in spawning animation
			float spawnAnimationTime = 180f;
			bool spawnAnimation = calamityGlobalNPC.newAI[3] < spawnAnimationTime;

			// Percent life remaining
			float lifeRatio = NPC.life / (float)NPC.lifeMax;

			// Night bool
			bool nightTime = !Main.dayTime;

			// Play enrage animation if night starts
			if (nightTime && calamityGlobalNPC.newAI[3] == spawnAnimationTime)
			{
				AIState = (int)Phase.HolyBlast;
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
				NPC.ai[3] = 0f;
				calamityGlobalNPC.newAI[1] = 0f;
				calamityGlobalNPC.newAI[2] = 0f;
				calamityGlobalNPC.newAI[3] = 0f;
			}

			// Difficulty bools
			bool death = CalamityWorld.death || BossRushEvent.BossRushActive || nightTime;
			bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive || nightTime;
			bool expertMode = Main.expertMode || BossRushEvent.BossRushActive || nightTime;
			bool enraged = NPC.Calamity().enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && BossRushEvent.BossRushActive);

			// Increase all projectile damage at night
			int projectileDamageMult = 1;
			if (nightTime)
				projectileDamageMult = 2;

			// Projectile damage values
			int holyLaserDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<ProvidenceHolyRay>()) * projectileDamageMult;
			int crystalDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<ProvidenceCrystal>()) * projectileDamageMult;
			int holySpearDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<HolySpear>()) * projectileDamageMult;
			int holyBombDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<HolyBomb>()) * projectileDamageMult;
			int moltenBlastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<MoltenBlast>()) * projectileDamageMult;
			int holyFireDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<HolyFire>()) * projectileDamageMult;
			int holyBlastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<HolyBlast>()) * projectileDamageMult;

			// Change dust type at night
			int dustType = Main.dayTime ? (int)CalamityDusts.ProfanedFire : (int)CalamityDusts.Nightwither;

			// Phase times
			float phaseTime = nightTime ? (300f - 120f * (1f - lifeRatio)) : 300f;
			float crystalPhaseTime = nightTime ? (float)Math.Round(60f * lifeRatio) : death ? 60f : 120f;
			int nightCrystalTime = 210;
			float attackDelayAfterCocoon = 90f;

			// Phases
			bool ignoreGuardianAmt = lifeRatio < (death ? 0.2f : 0.15f);
            bool phase2 = lifeRatio < 0.75f && !nightTime;
			bool delayAttacks = NPC.localAI[2] > 0f;

			// Spear phase
			float spearRateIncrease = 1f - lifeRatio;
			float enragedSpearRateIncrease = 0.5f;
			float bossRushSpearRateIncrease = 0.25f;
			float baseSpearRate = 18f;
			float spearRate = 1f + spearRateIncrease;

			if (enraged)
				spearRate += enragedSpearRateIncrease;

			if (BossRushEvent.BossRushActive)
				spearRate += bossRushSpearRateIncrease;

			// Projectile fire rate multiplier
			double attackRateMult = 1D;

			// Where projectiles are fired from during cocoon phases
			Vector2 fireFrom = new Vector2(vector.X, vector.Y + 20f);

			// Cocoon projectile initial velocity
			float cocoonProjVelocity = 3f + (death ? 2f * (1f - lifeRatio) : 0f);

			// Distance X needed from target in order to fire holy or molten blasts
			float distanceNeededToShoot = death ? 300f : revenge ? 360f : 420f;

			// X distance from target
			float distanceX = Math.Abs(vector.X - player.Center.X);

			// Inflict Holy Inferno if target is too far away
			float baseDistance = 2800f;
			float shorterFlameCocoonDistance = CalamityWorld.death ? 2200f : CalamityWorld.revenge ? 2400f : Main.expertMode ? 2600f : baseDistance;
			float shorterSpearCocoonDistance = CalamityWorld.death ? 1800f : CalamityWorld.revenge ? 2150f : Main.expertMode ? 2500f : baseDistance;
			float shorterDistance = AIState == (int)Phase.FlameCocoon ? shorterFlameCocoonDistance : shorterSpearCocoonDistance;
			float maxDistance = (AIState == (int)Phase.FlameCocoon || AIState == (int)Phase.SpearCocoon) ? shorterDistance : baseDistance;
			if (Vector2.Distance(player.Center, vector) > maxDistance)
            {
				if (!player.dead && player.active)
					player.AddBuff(ModContent.BuffType<HolyInferno>(), 2);
            }

            // Count the remaining Guardians, healer especially because it allows the boss to heal
            int guardianAmt = 0;
            bool healerAlive = false;
            if (CalamityGlobalNPC.holyBossAttacker != -1)
            {
                if (Main.npc[CalamityGlobalNPC.holyBossAttacker].active)
                    guardianAmt++;
            }
            if (CalamityGlobalNPC.holyBossDefender != -1)
            {
                if (Main.npc[CalamityGlobalNPC.holyBossDefender].active)
                    guardianAmt++;
            }
            if (CalamityGlobalNPC.holyBossHealer != -1)
            {
                if (Main.npc[CalamityGlobalNPC.holyBossHealer].active)
                {
                    guardianAmt++;
                    healerAlive = true;
                }
            }

            // Change projectile fire rate depending on Guardian amount
            if (guardianAmt > 0)
            {
                normalAttackRate = ignoreGuardianAmt;
                if (!normalAttackRate)
                {
                    switch (guardianAmt)
                    {
                        case 1:
                            attackRateMult = 1.25;
                            break;
                        case 2:
                            attackRateMult = 1.5;
                            break;
                        case 3:
                            attackRateMult = 2D;
                            break;
                        default:
                            break;
                    }
                }
            }

            // Whether the boss can be homed in on or healed off of
            NPC.chaseable = normalAttackRate && AIState != (int)Phase.FlameCocoon && AIState != (int)Phase.SpearCocoon && AIState != (int)Phase.Laser;
            NPC.canGhostHeal = NPC.chaseable;

            // Prevent lag by stopping rain
            CalRD.StopRain();

            // Set target biome type
            if (biomeType == 0)
            {
                if (isHoly)
                    biomeType = 1;
                else if (isHell)
                    biomeType = 2;
            }

            // Become immune over time if target isn't in hell or hallow
            if (!isHoly && !isHell && !BossRushEvent.BossRushActive)
            {
                if (immuneTimer > 0)
                    immuneTimer--;
            }
            else
                immuneTimer = 300;

            // Take damage or not
            NPC.dontTakeDamage = immuneTimer <= 0;

            // Heal
            if (healerAlive)
            {
                float heal = revenge ? 90f : 120f;
                switch (guardianAmt)
                {
                    case 1:
                        heal *= 2f;
                        break;
                    case 2:
                        break;
                    case 3:
                        heal *= 0.5f;
                        break;
                    default:
                        break;
                }

                healTimer++;
                if (healTimer >= heal)
                {
                    healTimer = 0;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int healAmt = NPC.lifeMax / 200;
                        if (healAmt > NPC.lifeMax - NPC.life)
                            healAmt = NPC.lifeMax - NPC.life;

                        if (healAmt > 0)
                        {
                            NPC.life += healAmt;
                            NPC.HealEffect(healAmt, true);
                            NPC.netUpdate = true;
                        }
                    }
                }
            }

			// Despawn
			bool targetDead = false;
            if (!player.active || player.dead)
            {
				if (!player.active || player.dead)
				{
					NPC.TargetClosest(false);
					player = Main.player[NPC.target];
				}
				if (!player.active || player.dead)
				{
					targetDead = true;

					if (NPC.timeLeft > 60)
						NPC.timeLeft = 60;

					if (NPC.velocity.X > 0f)
						NPC.velocity.X += 0.2f;
					else
						NPC.velocity.X -= 0.2f;

					NPC.velocity.Y -= 0.2f;
				}
            }
            else if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

			// Guardian spawn
			if (!nightTime)
			{
				if (bossLife == 0f && NPC.life > 0)
					bossLife = NPC.lifeMax;
				if (NPC.life > 0)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num660 = (int)(NPC.lifeMax * 0.66);
						if ((NPC.life + num660) < bossLife)
						{
							bossLife = NPC.life;
							int x = (int)(NPC.position.X + Main.rand.Next(NPC.width - 32));
							int y = (int)(NPC.position.Y + Main.rand.Next(NPC.height - 32));
							NPC.NewNPC(NPC.GetSource_FromThis(), x - 100, y - 100, ModContent.NPCType<ProvSpawnDefense>());
							NPC.NewNPC(NPC.GetSource_FromThis(), x + 100, y - 100, ModContent.NPCType<ProvSpawnHealer>());
							NPC.NewNPC(NPC.GetSource_FromThis(), x, y + 100, ModContent.NPCType<ProvSpawnOffense>());
						}
					}
				}
			}

            // Set DR based on current attack phase
            NPC.Calamity().DR = (AIState == (int)Phase.FlameCocoon || AIState == (int)Phase.SpearCocoon || AIState == (int)Phase.Laser || spawnAnimation) ?
				cocoonDR : delayAttacks ?
				MathHelper.Lerp(normalDR, cocoonDR, NPC.localAI[2] / attackDelayAfterCocoon) : normalDR;

			// Movement
			if (AIState != (int)Phase.FlameCocoon && AIState != (int)Phase.SpearCocoon)
            {
                // Firing holy ray or not
                bool firingLaser = AIState == (int)Phase.Laser;

                // Change X direction of movement
                if (flightPath == 0)
                {
                    NPC.TargetClosest(true);
                    if (vector.X < player.Center.X)
                    {
                        flightPath = 1;
                        calamityGlobalNPC.newAI[0] = 0f;
                    }
                    else
                    {
                        flightPath = -1;
                        calamityGlobalNPC.newAI[0] = 0f;
                    }
                }

                // Get a target
                NPC.TargetClosest(true);

                // Increase speed over time if flying in same direction for too long
                if (revenge)
                    calamityGlobalNPC.newAI[0] += 1f;

                // Distance needed from target to change direction
                float num851 = 800f;

                // Increase distance from target when firing molten blasts or holy bombs
                bool stayAwayFromTarget = AIState == (int)Phase.MoltenBlobs || AIState == (int)Phase.HolyBomb;
                if (stayAwayFromTarget)
                    num851 += death ? 240f : revenge ? 180f : 120f;

                // Change X movement path if far enough away from target
                if (vector.X < player.Center.X && flightPath < 0 && distanceX > num851)
                    flightPath = 0;
                if (vector.X > player.Center.X && flightPath > 0 && distanceX > num851)
                    flightPath = 0;

				// Velocity and acceleration
				float speedIncreaseTimer = nightTime ? 90f : death ? 120f : 150f;
                bool increaseSpeed = calamityGlobalNPC.newAI[0] > speedIncreaseTimer;
				float accelerationBoost = death ? 0.3f * (1f - lifeRatio) : 0.2f * (1f - lifeRatio);
				float velocityBoost = death ? 6f * (1f - lifeRatio) : 4f * (1f - lifeRatio);
                float acceleration = (expertMode ? 1.1f : 1.05f) + accelerationBoost;
                float velocity = (expertMode ? 16f : 15f) + velocityBoost;
                if (BossRushEvent.BossRushActive || nightTime)
                {
                    acceleration = 1.3f;
                    velocity = 20f;
                }
                if (firingLaser)
                {
                    acceleration *= normalAttackRate ? 0.4f : 0.2f;
                    velocity *= normalAttackRate ? 0.4f : 0.2f;
                }
                else if (increaseSpeed)
                {
                    velocity += (calamityGlobalNPC.newAI[0] - speedIncreaseTimer) * 0.04f;
                    if (velocity > 30f)
                        velocity = 30f;
                }

				if (!targetDead)
				{
					NPC.velocity.X += flightPath * acceleration;
					if (NPC.velocity.X > velocity)
						NPC.velocity.X = velocity;
					if (NPC.velocity.X < -velocity)
						NPC.velocity.X = -velocity;

					float num855 = player.position.Y - (NPC.position.Y + NPC.height);
					if (num855 < (firingLaser ? 150f : 200f)) // 150
						NPC.velocity.Y -= 0.2f;
					if (num855 > (firingLaser ? 200f : 250f)) // 200
						NPC.velocity.Y += 0.2f;

					float speedVariance = normalAttackRate ? 2f : 1f;
					if (NPC.velocity.Y > (firingLaser ? speedVariance : 6f))
						NPC.velocity.Y = firingLaser ? speedVariance : 6f;
					if (NPC.velocity.Y < (firingLaser ? -speedVariance : -6f))
						NPC.velocity.Y = firingLaser ? -speedVariance : -6f;
				}

				// Slowly drift down when spawning
				if (spawnAnimation)
				{
					float minSpawnVelocity = 0.4f;
					float maxSpawnVelocity = 4f;
					float velocityY = maxSpawnVelocity - MathHelper.Lerp(minSpawnVelocity, maxSpawnVelocity, calamityGlobalNPC.newAI[3] / spawnAnimationTime);
					NPC.velocity = new Vector2(0f, velocityY);
				}
			}

			// Phase switch
			switch ((int)AIState)
			{
				case (int)Phase.PhaseChange:

					phaseChange++;
					if (phaseChange > 14)
						phaseChange = 0;

					int phase = 0;

					// Holy ray in hallow, Crystal in hell
					bool useLaser = (phase2 && biomeType == 1) || BossRushEvent.BossRushActive;
					bool useCrystal = (phase2 && biomeType == 2) || BossRushEvent.BossRushActive;

					// Unique pattern for Death Mode and Boss Rush
					if (death)
					{
						switch (phaseChange)
						{
							case 0:
								phase = (int)Phase.MoltenBlobs;
								break;
							case 1:
								phase = (int)Phase.SpearCocoon;
								break;
							case 2:
								phase = (int)Phase.HolyBlast;
								break;
							case 3:
								phase = (useCrystal || nightTime) ? (int)Phase.Crystal : (int)Phase.MoltenBlobs;
								break;
							case 4:
								phase = useCrystal ? (int)Phase.MoltenBlobs : (int)Phase.FlameCocoon;
								break;
							case 5:
								phase = useCrystal ? (int)Phase.FlameCocoon : (int)Phase.HolyFire;
								break;
							case 6:
								phase = (useLaser || nightTime) ? (int)Phase.Laser : (int)Phase.HolyBomb;
								if (useLaser || nightTime)
								{
									NPC.TargetClosest(false);
								}
								break;
							case 7:
								phase = (useLaser || nightTime) ? (int)Phase.HolyBomb : (int)Phase.MoltenBlobs;
								break;
							case 8:
								phase = (useLaser || nightTime) ? (int)Phase.MoltenBlobs : (int)Phase.SpearCocoon;
								break;
							case 9:
								phase = (int)Phase.HolyBlast;
								break;
							case 10:
								phase = (useCrystal || nightTime) ? (int)Phase.Crystal : (int)Phase.FlameCocoon;
								break;
							case 11:
								phase = nightTime ? (int)Phase.FlameCocoon : (int)Phase.MoltenBlobs;
								break;
							case 12:
								phase = (useLaser || nightTime) ? (int)Phase.Laser : (int)Phase.HolyBomb;
								if (useLaser || nightTime)
								{
									NPC.TargetClosest(false);
								}
								break;
							case 13:
								phase = (int)Phase.SpearCocoon;
								break;
							case 14:
								phase = (useLaser || nightTime) ? (int)Phase.HolyBomb : (int)Phase.HolyBlast;
								break;
							default:
								break;
						}
					}
					else
					{
						switch (phaseChange)
						{
							case 0:
								phase = (int)Phase.HolyBlast;
								break;
							case 1:
								phase = useLaser ? (int)Phase.Laser : (int)Phase.HolyFire;
								if (useLaser)
								{
									NPC.TargetClosest(false);
								}
								break;
							case 2:
								phase = (int)Phase.HolyBomb;
								break;
							case 3:
								phase = (int)Phase.MoltenBlobs;
								break;
							case 4:
								phase = (int)Phase.SpearCocoon;
								break;
							case 5:
								phase = useCrystal ? (int)Phase.Crystal : (int)Phase.HolyBomb;
								break;
							case 6:
								phase = (int)Phase.HolyFire;
								break;
							case 7:
								phase = (int)Phase.HolyBlast;
								break;
							case 8:
								phase = (int)Phase.MoltenBlobs;
								break;
							case 9:
								phase = (int)Phase.FlameCocoon;
								break;
							case 10:
								phase = (int)Phase.HolyBomb;
								break;
							case 11:
								phase = useLaser ? (int)Phase.Laser : (int)Phase.HolyBlast;
								if (useLaser)
								{
									NPC.TargetClosest(false);
								}
								break;
							case 12:
								phase = (int)Phase.HolyFire;
								break;
							case 13:
								phase = (int)Phase.MoltenBlobs;
								break;
							case 14:
								phase = (int)Phase.SpearCocoon;
								break;
							default:
								break;
						}
					}

					// Pick a target
					NPC.TargetClosest(true);

					// If too far from target, set phase to 0
					if (Math.Abs(vector.X - player.Center.X) > 5600f)
						phase = (int)Phase.HolyBlast;

					// Reset attack delay for laser
					if (phase == (int)Phase.Laser)
						NPC.localAI[2] = 0f;

					// Reset arrays
					AIState = phase;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					calamityGlobalNPC.newAI[1] = 0f;
					calamityGlobalNPC.newAI[2] = 0f;
					break;

				case (int)Phase.HolyBlast:

					if (spawnAnimation)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient && calamityGlobalNPC.newAI[3] == 0f)
							Projectile.NewProjectile(NPC.GetSource_FromThis(), vector + new Vector2(0f, -80f), Vector2.Zero, ModContent.ProjectileType<HolyAura>(), 0, 0f, Main.myPlayer, biomeType, 0f);

						if (calamityGlobalNPC.newAI[3] == 10f && nightTime)
							SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/ProvidenceHolyRay"), NPC.position);

						if (calamityGlobalNPC.newAI[3] > 10f && calamityGlobalNPC.newAI[3] < 150f)
						{
							int dustAmt = (int)MathHelper.Lerp(4f, 8f, calamityGlobalNPC.newAI[3] / spawnAnimationTime);
							for (int m = 0; m < dustAmt; m++)
							{
								float fade = MathHelper.Lerp(1.3f, 0.7f, NPC.Opacity) * CalamityUtils.GetLerpValue(0f, 120f, calamityGlobalNPC.newAI[3], clamped: true);
								Color newColor = Main.hslToRgb(calamityGlobalNPC.newAI[3] / 180f, 1f, 0.5f);

								if (!nightTime)
								{
									newColor.R = 255;
									if (biomeType == 2)
										newColor.B = 0;
								}
								else
								{
									newColor.B = 255;
									if (biomeType == 2)
										newColor.G = 0;
									else
										newColor.R = 0;
								}

								int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 267, 0f, 0f, 0, newColor);
								Main.dust[dust].position = NPC.Center + Main.rand.NextVector2Circular(NPC.width * 2f, NPC.height * 2f) + new Vector2(0f, -150f);
								Main.dust[dust].velocity *= Main.rand.NextFloat() * 0.8f;
								Main.dust[dust].noGravity = true;
								Main.dust[dust].fadeIn = 0.6f + Main.rand.NextFloat() * 0.7f * fade;
								Main.dust[dust].velocity += Vector2.UnitY * 3f;
								Main.dust[dust].scale = 1.2f;

								if (dust != 6000)
								{
									Dust dust2 = Dust.CloneDust(dust);
									dust2.scale /= 2f;
									dust2.fadeIn *= 0.85f;
									dust2.color = new Color(255, 255, 0, 255);
								}
							}
						}

						NPC.Opacity = MathHelper.Clamp(calamityGlobalNPC.newAI[3] / spawnAnimationTime, 0f, 1f);

						calamityGlobalNPC.newAI[3] += 1f;

						if (nightTime && calamityGlobalNPC.newAI[3] >= spawnAnimationTime)
							calamityGlobalNPC.newAI[3] += 1f;

						return;
					}

					// Attack delay after cocoon phase
					if (delayAttacks)
					{
						NPC.localAI[2] -= 1f;
						return;
					}

					if (distanceX > distanceNeededToShoot && NPC.position.Y < player.position.Y)
					{
						NPC.ai[3] += 1f;

						int shootBoost = death ? (int)Math.Round(5f * (1f - lifeRatio)) : (int)Math.Round(4f * (1f - lifeRatio));
						int num856 = (expertMode ? 24 : 26) - shootBoost;
						if (enraged)
							num856 = 20;

						num856 = (int)(num856 * attackRateMult);

						if (NPC.ai[3] >= num856)
							NPC.ai[3] = -num856;

						if (NPC.ai[3] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
						{
							vector.X += NPC.velocity.X * 7f;
							float num857 = player.position.X + player.width * 0.5f - vector.X;
							float num858 = player.Center.Y - vector.Y;
							float num859 = (float)Math.Sqrt(num857 * num857 + num858 * num858);

							float velocityBoost = death ? 4f * (1f - lifeRatio) : 2.5f * (1f - lifeRatio);
							float num860 = (expertMode ? 10.25f : 9f) + velocityBoost;
							if (enraged)
								num860 = 12.75f;

							if (revenge)
								num860 *= 1.15f;

							num859 = num860 / num859;
							num857 *= num859;
							num858 *= num859;

							Projectile.NewProjectile(NPC.GetSource_FromThis(), vector.X, vector.Y, num857, num858, ModContent.ProjectileType<HolyBlast>(), holyBlastDamage, 0f, Main.myPlayer, 0f, 0f);
						}
					}
					else if (NPC.ai[3] < 0f)
						NPC.ai[3] += 1f;

					NPC.ai[1] += 1f;
					if (NPC.ai[1] >= phaseTime)
						AIState = (int)Phase.PhaseChange;

					break;

				case (int)Phase.HolyFire:

					// Attack delay after cocoon phase
					if (delayAttacks)
					{
						NPC.localAI[2] -= 1f;
						return;
					}

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.ai[3] += 1f;

						int shootBoost = death ? (int)Math.Round(6f * (1f - lifeRatio)) : (int)Math.Round(5f * (1f - lifeRatio));
						int num864 = (expertMode ? 36 : 39) - shootBoost;
						if (BossRushEvent.BossRushActive)
							num864 = 31;

						num864 = (int)(num864 * attackRateMult);

						if (NPC.ai[3] >= num864)
						{
							NPC.ai[3] = 0f;

							Vector2 vector113 = new Vector2(vector.X, NPC.position.Y + NPC.height - 14f);

							float num865 = NPC.velocity.Y;
							if (num865 < 0f)
								num865 = 0f;

							num865 += expertMode ? 4f : 3f;

							Projectile.NewProjectile(NPC.GetSource_FromThis(), vector113.X, vector113.Y, NPC.velocity.X * 0.25f, num865, ModContent.ProjectileType<HolyFire>(), holyFireDamage, 0f, Main.myPlayer, 0f, 0f);
						}
					}

					NPC.ai[1] += 1f;
					if (NPC.ai[1] >= phaseTime)
						AIState = (int)Phase.PhaseChange;

					break;

				case (int)Phase.FlameCocoon:

					NPC.TargetClosest(true);

					if (!targetDead)
					{
						if (NPC.velocity.Length() <= 2f)
							NPC.velocity = Vector2.Zero;
						if (NPC.velocity.Length() > 2f)
						{
							NPC.velocity *= 0.9f;
							return;
						}
					}

					float divisor = (expertMode ? 2f : 3f) + (float)Math.Floor(3f * lifeRatio) + (attackRateMult > 1D ? (float)Math.Ceiling(attackRateMult * 1.6) : 0f);
					int totalFlameProjectiles = 36;
					int chains = 4;
					float interval = totalFlameProjectiles / chains * divisor;
					double patternInterval = Math.Floor(NPC.ai[3] / interval);

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						if (patternInterval % 2 == 0)
						{
							if (NPC.ai[3] % divisor == 0f)
							{
								bool normalSpread = calamityGlobalNPC.newAI[1] % 2f == 0f;
								Vector2 spinningPoint = normalSpread ? new Vector2(0f, -cocoonProjVelocity) : Vector2.Normalize(new Vector2(-cocoonProjVelocity, -cocoonProjVelocity));
								double radians = MathHelper.TwoPi / chains;
								SoundEngine.PlaySound(SoundID.Item20, NPC.position);
								for (int i = 0; i < chains; i++)
								{
									Vector2 vector2 = spinningPoint.RotatedBy(radians * i + MathHelper.ToRadians(NPC.ai[2]));

									if (!normalSpread)
										vector2 *= cocoonProjVelocity;

									int projectileType = ModContent.ProjectileType<HolyBurnOrb>();
									if (Main.rand.NextBool(4) && !death)
										projectileType = ModContent.ProjectileType<HolyLight>();

									int dmgAmt = nightTime ? -300 : NPC.GetProjectileDamageNoScaling(projectileType);

									Projectile.NewProjectile(Entity.GetSource_FromThis(), fireFrom, vector2, projectileType, 0, 0f, Main.myPlayer, 0f, dmgAmt);
								}

								// Radial offset
								NPC.ai[2] += 10f;
							}
						}
						else
						{
							NPC.ai[2] = 0f;

							totalFlameProjectiles = 16;
							if (NPC.ai[3] % (divisor * totalFlameProjectiles) == 0f)
							{
								calamityGlobalNPC.newAI[1] += 1f;
								double radians = MathHelper.TwoPi / totalFlameProjectiles;
								SoundEngine.PlaySound(SoundID.Item20, NPC.position);
								for (int i = 0; i < totalFlameProjectiles; i++)
								{
									Vector2 vector2 = new Vector2(0f, -cocoonProjVelocity).RotatedBy(radians * i);

									int projectileType = ModContent.ProjectileType<HolyBurnOrb>();
									if (Main.rand.NextBool(4) && !death)
										projectileType = ModContent.ProjectileType<HolyLight>();

									int dmgAmt = nightTime ? -300 : NPC.GetProjectileDamageNoScaling(projectileType);

									Projectile.NewProjectile(Entity.GetSource_FromThis(), fireFrom, vector2, projectileType, 0, 0f, Main.myPlayer, 0f, dmgAmt);
								}

								Vector2 velocity2 = Vector2.Normalize(player.Center - fireFrom) * cocoonProjVelocity;
								int type = ModContent.ProjectileType<HolyBurnOrb>();
								Projectile.NewProjectile(Entity.GetSource_FromThis(), fireFrom, velocity2, type, 0, 0f, Main.myPlayer, 0f, nightTime ? -300 : NPC.GetProjectileDamageNoScaling(type));
							}
						}
					}

					if (NPC.ai[3] == 0f)
						DespawnSpecificProjectiles();

					// Air is burning text
					NPC.ai[3] += 1f;
					if (NPC.ai[3] >= (phaseTime * 1.5f) && !text)
					{
						text = true;
						string key = "The air is burning...";
						Color messageColor = Color.Orange;

						CalamityUtils.DisplayLocalizedText(key, messageColor);
					}

					// Inflict Icarus Folly
					if (NPC.ai[3] >= (phaseTime * 2f))
					{
						if (Main.netMode != NetmodeID.Server)
						{
							Player player2 = Main.player[Main.myPlayer];
							bool inLiquid = (player2.wet || player2.honeyWet) && !player2.lavaWet;

							if (!player2.dead && player2.active && Vector2.Distance(player2.Center, vector) < 2800f && !inLiquid)
							{
								SoundEngine.PlaySound(SoundID.Item20, player2.position);
								player2.AddBuff(ModContent.BuffType<ExtremeGravity>(), 3000, true);

								for (int num621 = 0; num621 < 40; num621++)
								{
									int num622 = Dust.NewDust(new Vector2(player2.position.X, player2.position.Y),
										player2.width, player2.height, dustType, 0f, 0f, 100, default, 2f);
									Main.dust[num622].velocity *= 3f;
									if (Main.rand.NextBool(2))
									{
										Main.dust[num622].scale = 0.5f;
										Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
									}
								}

								for (int num623 = 0; num623 < 60; num623++)
								{
									int num624 = Dust.NewDust(new Vector2(player2.position.X, player2.position.Y),
										player2.width, player2.height, dustType, 0f, 0f, 100, default, 3f);
									Main.dust[num624].noGravity = true;
									Main.dust[num624].velocity *= 5f;
									num624 = Dust.NewDust(new Vector2(player2.position.X, player2.position.Y),
										player2.width, player2.height, dustType, 0f, 0f, 100, default, 2f);
									Main.dust[num624].velocity *= 2f;
								}
							}
						}

						text = false;
						AIState = (int)Phase.PhaseChange;
						NPC.localAI[2] = attackDelayAfterCocoon;
					}

					break;

				case (int)Phase.MoltenBlobs:

					// Attack delay after cocoon phase
					if (delayAttacks)
					{
						NPC.localAI[2] -= 1f;
						return;
					}

					if (distanceX > distanceNeededToShoot && NPC.position.Y < player.position.Y)
					{
						NPC.ai[3] += 1f;

						int shootBoost = death ? (int)Math.Round(5f * (1f - lifeRatio)) : (int)Math.Round(4f * (1f - lifeRatio));
						int num856 = (expertMode ? 24 : 26) - shootBoost;
						if (BossRushEvent.BossRushActive)
							num856 = 20;

						num856 = (int)(num856 * attackRateMult);

						if (NPC.ai[3] >= num856)
							NPC.ai[3] = -num856;

						if (NPC.ai[3] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
						{
							vector.X += NPC.velocity.X * 7f;
							float num857 = player.position.X + player.width * 0.5f - vector.X;
							float num858 = player.Center.Y - vector.Y;
							float num859 = (float)Math.Sqrt(num857 * num857 + num858 * num858);

							float shootBoost2 = death ? 4f * (1f - lifeRatio) : 2.5f * (1f - lifeRatio);
							float num860 = (expertMode ? 10.25f : 9f) + shootBoost2;
							if (BossRushEvent.BossRushActive)
								num860 = 12.75f;

							if (revenge)
								num860 *= 1.15f;

							num859 = num860 / num859;
							num857 *= num859;
							num858 *= num859;

							Projectile.NewProjectile(NPC.GetSource_FromThis(), vector.X, vector.Y, num857 * 0.1f, num858, ModContent.ProjectileType<MoltenBlast>(), moltenBlastDamage, 0f, Main.myPlayer, 0f, 0f);
						}
					}
					else if (NPC.ai[3] < 0f)
						NPC.ai[3] += 1f;

					NPC.ai[1] += 1f;
					if (NPC.ai[1] >= phaseTime)
						AIState = (int)Phase.PhaseChange;

					break;

				case (int)Phase.HolyBomb:

					// Attack delay after cocoon phase
					if (delayAttacks)
					{
						NPC.localAI[2] -= 1f;
						return;
					}

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.ai[3] += 1f;

						int shootBoost = death ? (int)Math.Round(12f * (1f - lifeRatio)) : (int)Math.Round(10f * (1f - lifeRatio));
						int num864 = (expertMode ? 73 : 77) - shootBoost;
						if (enraged)
							num864 = 63;

						num864 = (int)(num864 * attackRateMult);

						if (NPC.ai[3] >= num864)
						{
							NPC.ai[3] = 0f;

							Vector2 vector113 = new Vector2(vector.X, NPC.position.Y + NPC.height - 14f);

							float num865 = NPC.velocity.Y;
							if (num865 < 0f)
								num865 = 0f;

							num865 += expertMode ? 4f : 3f;

							Projectile.NewProjectile(NPC.GetSource_FromThis(), vector113.X, vector113.Y, NPC.velocity.X * 0.25f, num865, ModContent.ProjectileType<HolyBomb>(), holyBombDamage, 0f, Main.myPlayer, 0f, 0f);
						}
					}

					NPC.ai[1] += 1f;
					if (NPC.ai[1] >= phaseTime)
						AIState = (int)Phase.PhaseChange;

					break;

				case (int)Phase.SpearCocoon:

					NPC.TargetClosest(true);

					if (!targetDead)
					{
						if (NPC.velocity.Length() <= 2f)
							NPC.velocity = Vector2.Zero;
						if (NPC.velocity.Length() > 2f)
						{
							NPC.velocity *= 0.9f;
							return;
						}
					}

					if (NPC.ai[1] == 0f)
						DespawnSpecificProjectiles();

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.ai[2] += spearRate;
						if (NPC.ai[2] >= (float)(baseSpearRate * attackRateMult))
						{
							NPC.ai[2] = 0f;

							SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, fireFrom);

							int projectileType = ModContent.ProjectileType<HolySpear>();

							if (calamityGlobalNPC.newAI[2] % 2f == 0f)
							{
								int totalSpearProjectiles = 12;
								double radians = MathHelper.TwoPi / totalSpearProjectiles;
								Vector2 spinningPoint = Vector2.Normalize(new Vector2(-calamityGlobalNPC.newAI[1], -cocoonProjVelocity));

								for (int i = 0; i < totalSpearProjectiles; i++)
								{
									Vector2 vector2 = spinningPoint.RotatedBy(radians * i) * cocoonProjVelocity;
									Projectile.NewProjectile(Entity.GetSource_FromThis(), fireFrom, vector2, projectileType, holySpearDamage, 0f, Main.myPlayer, 0f, 0f);
								}

								if (spearRateIncrease > 1f)
									spearRateIncrease = 1f;

								float radialOffset = MathHelper.Lerp(0.2f, 0.4f, spearRateIncrease);
								calamityGlobalNPC.newAI[1] += radialOffset;
							}

							calamityGlobalNPC.newAI[2] += 1f;

							cocoonProjVelocity = death ? 14f : revenge ? 13f : expertMode ? 12f : 10f;
							Vector2 velocity2 = Vector2.Normalize(player.Center - fireFrom) * cocoonProjVelocity;
							Projectile.NewProjectile(Entity.GetSource_FromThis(), fireFrom, velocity2, projectileType, holySpearDamage, 0f, Main.myPlayer, 1f, 0f);
						}
					}

					NPC.ai[3] += 1f;
					if (NPC.ai[3] >= phaseTime)
					{
						AIState = (int)Phase.PhaseChange;
						NPC.localAI[2] = attackDelayAfterCocoon;
					}

					break;

				case (int)Phase.Crystal:

					NPC.TargetClosest(true);

					if (!targetDead)
						NPC.velocity *= 0.9f;

					NPC.ai[1] += 1f;
					if (NPC.ai[1] >= crystalPhaseTime)
					{
						if (NPC.ai[1] == crystalPhaseTime && Main.netMode != NetmodeID.MultiplayerClient)
						{
							int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center.X, player.Center.Y - 360f, 0f, 0f, ModContent.ProjectileType<ProvidenceCrystal>(), crystalDamage, 0f, player.whoAmI, lifeRatio, 0f);

							if (nightTime)
								Main.projectile[proj].timeLeft = nightCrystalTime;
						}

						if (NPC.ai[1] >= crystalPhaseTime + nightCrystalTime || !nightTime)
							AIState = (int)Phase.PhaseChange;
					}

					break;

				case (int)Phase.Laser:

					Vector2 value19 = new Vector2(27f, 59f);

					float rotation = 450f + (guardianAmt * 18);

					NPC.ai[2] += 1f;
					if (NPC.ai[2] < 120f)
					{
						if (NPC.ai[2] >= 40f)
						{
							int num1220 = 0;
							if (NPC.ai[2] >= 80f)
								num1220 = 1;

							for (int d = 0; d < 1 + num1220; d++)
							{
								float scalar = 1.2f;
								if (d % 2 == 1)
									scalar = 2.8f;

								Vector2 vector199 = new Vector2(vector.X, vector.Y + 32f) + ((float)Main.rand.NextDouble() * MathHelper.TwoPi).ToRotationVector2() * value19 / 2f;
								int index = Dust.NewDust(vector199 - Vector2.One * 8f, 16, 16, dustType, NPC.velocity.X / 2f, NPC.velocity.Y / 2f, 0, default, 1f);
								Main.dust[index].velocity = Vector2.Normalize(vector - vector199) * 3.5f * (10f - num1220 * 2f) / 10f;
								Main.dust[index].noGravity = true;
								Main.dust[index].scale = scalar;
							}
						}
					}
					else if (NPC.ai[2] < (revenge ? 220f : 300f))
					{
						if (NPC.ai[2] == 120f)
						{
							if (Main.player[Main.myPlayer].active && !Main.player[Main.myPlayer].dead && Vector2.Distance(Main.player[Main.myPlayer].Center, vector) < 2800f)
							{
								SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/ProvidenceHolyRay"),
									Main.player[Main.myPlayer].position);
							}

							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								NPC.TargetClosest(false);

								Vector2 velocity = player.Center - vector;
								velocity.Normalize();

								float num1225 = -1f;
								if (velocity.X < 0f)
									num1225 = 1f;

								// 60 degrees offset
								velocity = velocity.RotatedBy(-(double)num1225 * MathHelper.TwoPi / 6f);
								Projectile.NewProjectile(NPC.GetSource_FromThis(), vector.X, vector.Y + 32f, velocity.X, velocity.Y, ModContent.ProjectileType<ProvidenceHolyRay>(), holyLaserDamage, 0f, Main.myPlayer, num1225 * MathHelper.TwoPi / rotation, NPC.whoAmI);

								// -60 degrees offset
								if (revenge)
									Projectile.NewProjectile(NPC.GetSource_FromThis(), vector.X, vector.Y + 32f, -velocity.X, -velocity.Y, ModContent.ProjectileType<ProvidenceHolyRay>(), holyLaserDamage, 0f, Main.myPlayer, -num1225 * MathHelper.TwoPi / rotation, NPC.whoAmI);

								if (nightTime && lifeRatio < 0.5f)
								{
									rotation *= 0.33f;
									velocity = velocity.RotatedBy(-(double)num1225 * MathHelper.TwoPi / 2f);
									Projectile.NewProjectile(NPC.GetSource_FromThis(), vector.X, vector.Y + 32f, velocity.X, velocity.Y, ModContent.ProjectileType<ProvidenceHolyRay>(), holyLaserDamage, 0f, Main.myPlayer, num1225 * MathHelper.TwoPi / rotation, NPC.whoAmI);

									if (revenge)
										Projectile.NewProjectile(NPC.GetSource_FromThis(), vector.X, vector.Y + 32f, -velocity.X, -velocity.Y, ModContent.ProjectileType<ProvidenceHolyRay>(), holyLaserDamage, 0f, Main.myPlayer, -num1225 * MathHelper.TwoPi / rotation, NPC.whoAmI);
								}

								NPC.netUpdate = true;
							}
						}
					}

					NPC.ai[1] += 1f;
					if (NPC.ai[1] >= (revenge ? 235f : 315f))
						AIState = (int)Phase.PhaseChange;

					break;
			}
        }

		private void DespawnSpecificProjectiles()
		{
			for (int x = 0; x < Main.maxProjectiles; x++)
			{
				Projectile projectile = Main.projectile[x];
				if (projectile.active)
				{
					if (projectile.type == ModContent.ProjectileType<HolyFire2>() || projectile.type == ModContent.ProjectileType<HolyFlare>())
						projectile.Kill();
					else if (projectile.type == ModContent.ProjectileType<HolyBlast>() || projectile.type == ModContent.ProjectileType<HolyFire>())
						projectile.active = false;
				}
			}
		}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void OnKill()
        {
            DropHelper.DropBags(ModContent.ItemType<ProvidenceBag>(), NPC);
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ProvidenceTrophy>(), 10);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeProvidence>(), true, !CalamityWorld.downedProvidence);
            DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedProvidence, 5, 2, 1);

            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<RuneofCos>(), true, !CalamityWorld.downedProvidence);

			CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<THIEF>() }, CalamityWorld.downedProvidence);

			// Accessories clientside only in Expert. Both drop if she is defeated at night.
			DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ElysianWings>(), Main.expertMode, biomeType != 2 || !hasTakenDaytimeDamage);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ElysianAegis>(), Main.expertMode, biomeType == 2 || !hasTakenDaytimeDamage);

			// Drops pre-scal, cannot be sold, does nothing aka purely vanity. Requires at least expert for consistency with other post scal dev items.
			bool shouldDrop = challenge/* || (Main.expertMode && Main.rand.NextBool(CalamityWorld.downedSCal ? 10 : 200))*/;
			DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ProfanedSoulCrystal>(), true, shouldDrop);

			// Special drop for defeating her at night
			DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ProfanedMoonlightDye>(), true, !hasTakenDaytimeDamage, 3, 4);

			// All other drops are contained in the bag, so they only drop directly on Normal
			if (!Main.expertMode)
            {
                // Materials
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<UnholyEssence>(), 20, 30);
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DivineGeode>(), 15, 20);

				// Weapons
				float w = DropHelper.DirectWeaponDropRateFloat;
				DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC,
					DropHelper.WeightStack<HolyCollider>(w),
					DropHelper.WeightStack<SolarFlare>(w),
					DropHelper.WeightStack<TelluricGlare>(w),
					DropHelper.WeightStack<BlissfulBombardier>(w),
					DropHelper.WeightStack<PurgeGuzzler>(w),
					DropHelper.WeightStack<DazzlingStabberStaff>(w),
					DropHelper.WeightStack<MoltenAmputator>(w)
				);

				// Equipment
				DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<SamuraiBadge>(), 40);

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ProvidenceMask>(), 7);
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                SpawnLootBox();
            }

            // If Providence has not been killed, notify players of Uelibloom Ore
            if (!CalamityWorld.downedProvidence)
            {
                string key2 = "The calamitous beings have been inundated with bloodstone.";
                Color messageColor2 = Color.Orange;
                string key3 = "Fossilized tree bark is bursting through the jungle's mud.";
                Color messageColor3 = Color.LightGreen;

                WorldGenerationMethods.SpawnOre(ModContent.TileType<UelibloomOre>(), 15E-05, .4f, .8f);

				CalamityUtils.DisplayLocalizedText(key2, messageColor2);
				CalamityUtils.DisplayLocalizedText(key3, messageColor3);
			}

			if (challenge)
			{
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.NewText(Language.GetTextValue("The Profaned Goddess has recognised your devotion to purity!"), Color.DarkOrange);
				} 
			}

            // Mark Providence as dead
            CalamityWorld.downedProvidence = true;
            CalamityNetcode.SyncWorld();
        }

        private void SpawnLootBox()
        {
            int tileCenterX = (int)NPC.Center.X / 16;
            int tileCenterY = (int)NPC.Center.Y / 16;
            int halfBox = NPC.width / 2 / 16 + 1;
            for (int x = tileCenterX - halfBox; x <= tileCenterX + halfBox; x++)
            {
                for (int y = tileCenterY - halfBox; y <= tileCenterY + halfBox; y++)
                {
                    if ((x == tileCenterX - halfBox || x == tileCenterX + halfBox || y == tileCenterY - halfBox || y == tileCenterY + halfBox)
                        && !Main.tile[x, y].HasTile)
                    {
                        Main.tile[x, y].TileType = (ushort)ModContent.TileType<ProfanedRock>();
                        Main.tile[x, y].Get<TileWallWireStateData>().HasTile = true;
                    }
                    Main.tile[x, y].LiquidAmount = 0;

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendTileSquare(-1, x, y, 1, TileChangeType.None);
                    else
                        WorldGen.SquareTileFrame(x, y, true);
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			bool nightTime = !Main.dayTime;

			string baseTextureString = "CalRD/NPCs/Providence/";
			string baseGlowTextureString = baseTextureString + "Glowmasks/";

			string getTextureString = baseTextureString + "Providence";
			string getTextureGlowString = baseGlowTextureString + "ProvidenceGlow";
			string getTextureGlow2String = baseGlowTextureString + "ProvidenceGlow2";

			if (AIState == (int)Phase.FlameCocoon || AIState == (int)Phase.SpearCocoon)
            {
				if (!useDefenseFrames)
				{
					getTextureString = baseTextureString + "ProvidenceDefense";
					getTextureGlowString = baseGlowTextureString + "ProvidenceDefenseGlow";
					getTextureGlow2String = baseGlowTextureString + "ProvidenceDefenseGlow2";
				}
				else
				{
					getTextureString = baseTextureString + "ProvidenceDefenseAlt";
					getTextureGlowString = baseGlowTextureString + "ProvidenceDefenseAltGlow";
					getTextureGlow2String = baseGlowTextureString + "ProvidenceDefenseAltGlow2";
				}
            }
            else
            {
				if (frameUsed == 0)
				{
					getTextureGlowString = baseGlowTextureString + "ProvidenceGlow";
					getTextureGlow2String = baseGlowTextureString + "ProvidenceGlow2";
				}
				else if (frameUsed == 1)
				{
					getTextureString = baseTextureString + "ProvidenceAlt";
					getTextureGlowString = baseGlowTextureString + "ProvidenceAltGlow";
					getTextureGlow2String = baseGlowTextureString + "ProvidenceAltGlow2";
				}
				else if (frameUsed == 2)
				{
					getTextureString = baseTextureString + "ProvidenceAttack";
					getTextureGlowString = baseGlowTextureString + "ProvidenceAttackGlow";
					getTextureGlow2String = baseGlowTextureString + "ProvidenceAttackGlow2";
				}
				else
				{
					getTextureString = baseTextureString + "ProvidenceAttackAlt";
					getTextureGlowString = baseGlowTextureString + "ProvidenceAttackAltGlow";
					getTextureGlow2String = baseGlowTextureString + "ProvidenceAttackAltGlow2";
				}
            }

			if (nightTime)
			{
				getTextureString += "Night";
				getTextureGlowString += "Night";
				getTextureGlow2String += "Night";
			}

			Texture2D texture = ModContent.Request<Texture2D>(getTextureString).Value;
			Texture2D textureGlow = ModContent.Request<Texture2D>(getTextureGlowString).Value;
			Texture2D textureGlow2 = ModContent.Request<Texture2D>(getTextureGlow2String).Value;

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 5;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			Color color37 = Color.Lerp(Color.White, nightTime ? Color.Cyan : Color.Yellow, 0.5f) * NPC.Opacity;
			Color color42 = Color.Lerp(Color.White, nightTime ? Color.BlueViolet : Color.Violet, 0.5f) * NPC.Opacity;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num163 = 1; num163 < num153; num163++)
				{
					Color color41 = color37;
					color41 = Color.Lerp(color41, color36, amount9);
					color41 = NPC.GetAlpha(color41);
					color41 *= (num153 - num163) / 15f;
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector44 -= new Vector2(textureGlow.Width, textureGlow.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(textureGlow, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

					Color color43 = color42;
					color43 = Color.Lerp(color43, color36, amount9);
					color43 = NPC.GetAlpha(color43);
					color43 *= (num153 - num163) / 15f;
					spriteBatch.Draw(textureGlow2, vector44, NPC.frame, color43, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(textureGlow, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			spriteBatch.Draw(textureGlow2, vector43, NPC.frame, color42, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
        }

        public override void FindFrame(int frameHeight) //9 total frames
        {
            if (AIState == (int)Phase.FlameCocoon || AIState == (int)Phase.SpearCocoon)
            {
                if (!useDefenseFrames)
                {
                    NPC.frameCounter += 1.0;
                    if (NPC.frameCounter > 8.0)
                    {
                        NPC.frame.Y = NPC.frame.Y + frameHeight;
                        NPC.frameCounter = 0.0;
                    }
                    if (NPC.frame.Y >= frameHeight * 3)
                    {
                        NPC.frame.Y = 0;
                        useDefenseFrames = true;
                    }
                }
                else
                {
                    NPC.frameCounter += 1.0;
                    if (NPC.frameCounter > 8.0)
                    {
                        NPC.frame.Y = NPC.frame.Y + frameHeight;
                        NPC.frameCounter = 0.0;
                    }
                    if (NPC.frame.Y >= frameHeight * 2)
                        NPC.frame.Y = frameHeight * 2;
                }
            }
            else
            {
                if (useDefenseFrames)
                    useDefenseFrames = false;

                NPC.frameCounter += 1.0;
                if (NPC.frameCounter > (NPC.Calamity().newAI[3] < 180f ? 8.0 : 5.0))
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * 3) //6
                {
                    NPC.frame.Y = 0;
                    frameUsed++;
                }
                if (frameUsed > 3)
                    frameUsed = 0;
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            return null;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * 0.8f);
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
			if (!hasTakenDaytimeDamage)
			{
				if (Main.dayTime)
				{
					hasTakenDaytimeDamage = true;

					if (Main.netMode != NetmodeID.SinglePlayer)
					{
						var netMessage = Mod.GetPacket();
						netMessage.Write((byte)CalRDMessageType.ProvidenceDyeConditionSync);
						netMessage.Write((byte)NPC.whoAmI);
						netMessage.Write(hasTakenDaytimeDamage);
						netMessage.Send();
					}
				}
			}

			if (challenge)
			{
				List<int> exceptionList = new List<int>()
				{
					ModContent.ProjectileType<GoldenGunProj>(),
					ModContent.ProjectileType<MiniGuardianDefense>(),
					ModContent.ProjectileType<MiniGuardianAttack>(),
					ModContent.ProjectileType<SilvaCrystalExplosion>(),
					ModContent.ProjectileType<GhostlyMine>()
				};

				bool allowedClass = projectile.IsSummon() || (!projectile.CountsAsClass(DamageClass.Melee) && !projectile.CountsAsClass(DamageClass.Ranged) && !projectile.CountsAsClass(DamageClass.Magic) && !projectile.CountsAsClass(DamageClass.Throwing) && !projectile.Calamity().rogue);
				bool allowedDamage = allowedClass && hit.Damage <= 75; //Flat 75 regardless of difficulty.
				//Absorber on-hit effects likely won't proc this but Deific Amulet and Astral Bulwark stars will proc this.
				bool allowedBabs = Main.player[projectile.owner].Calamity().pArtifact && !Main.player[projectile.owner].Calamity().profanedCrystalBuffs;

				if ((exceptionList.TrueForAll(x => projectile.type != x) && !allowedDamage) || !allowedBabs)
				{
					challenge = false;

					if (Main.netMode != NetmodeID.SinglePlayer)
					{
						var netMessage = Mod.GetPacket();
						netMessage.Write((byte)CalRDMessageType.PSCChallengeSync);
						netMessage.Write((byte)NPC.whoAmI);
						netMessage.Write(challenge);
						netMessage.Send();
					}
				}
			}
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (!hasTakenDaytimeDamage)
			{
				if (Main.dayTime)
				{
					hasTakenDaytimeDamage = true;

					if (Main.netMode != NetmodeID.SinglePlayer)
					{
						var netMessage = Mod.GetPacket();
						netMessage.Write((byte)CalRDMessageType.ProvidenceDyeConditionSync);
						netMessage.Write((byte)NPC.whoAmI);
						netMessage.Write(hasTakenDaytimeDamage);
						netMessage.Send();
					}
				}
			}

			if (challenge)
			{
				challenge = false;

				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					var netMessage = Mod.GetPacket();
					netMessage.Write((byte)CalRDMessageType.PSCChallengeSync);
					netMessage.Write((byte)NPC.whoAmI);
					netMessage.Write(challenge);
					netMessage.Send();
				}
			}
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 8;
                SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/ProvidenceHurt"), NPC.Center);
            }

			int dustType = Main.dayTime ? (int)CalamityDusts.ProfanedFire : (int)CalamityDusts.Nightwither;
			for (int k = 0; k < 15; k++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType, hit.HitDirection, -1f, 0, default, 1f);

            if (NPC.life <= 0)
            {
	            if (Main.netMode != NetmodeID.Server)
	            {
		            float randomSpread = Main.rand.Next(-50, 50) / 100;
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("Providence").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("Providence2").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("Providence3").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread * Main.rand.NextFloat(), Mod.Find<ModGore>("Providence4").Type, 1f);
                }
                NPC.position = NPC.Center;
                NPC.width = 400;
                NPC.height = 350;
				NPC.position -= NPC.Size * 0.5f;
                for (int d = 0; d < 60; d++)
                {
                    int fire = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType, 0f, 0f, 100, default, 2f);
                    Main.dust[fire].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[fire].scale = 0.5f;
                        Main.dust[fire].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int d = 0; d < 90; d++)
                {
                    int fire = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType, 0f, 0f, 100, default, 3f);
                    Main.dust[fire].noGravity = true;
                    Main.dust[fire].velocity *= 5f;
                    fire = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType, 0f, 0f, 100, default, 2f);
                    Main.dust[fire].velocity *= 2f;
                }
            }
        }
    }
}
