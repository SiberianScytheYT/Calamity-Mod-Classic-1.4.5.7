using CalRD.Buffs.Potions;
using CalRD.Buffs.StatDebuffs;
using CalRD.CalPlayer;
using CalRD.Items.Accessories;
using CalRD.Items.Armor;
using CalRD.Items.DifficultyItems;
using CalRD.Items.Materials;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Placeables.Furniture.Fountains;
using CalRD.Items.Potions;
using CalRD.Items.Tools;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.SupremeCalamitas;
using CalRD.NPCs.TownNPCs;
using CalRD.Projectiles.Magic;
using CalRD.Projectiles.Melee;
using CalRD.Projectiles.Ranged;
using CalRD.Projectiles.Rogue;
using CalRD.Projectiles.Summon;
using CalRD.Projectiles.Typeless;
using CalRD.UI;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRD.Items
{
	public class CalamityGlobalItem : GlobalItem
    {
        #region Instances
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        protected override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        #endregion

		public bool rogue = false;
		public float StealthGenBonus;

		public int timesUsed = 0;

        #region Chargeable Item Variables
        public bool UsesCharge = false;
        public float Charge = 0f;
        public float MaxCharge = 1f;
        public float ChargePerUse = 0f;
        // If left at the default value of -1, ChargePerUse is automatically used for alt fire.
        // If you want a different amount of charge used for alt fire, then set a different value here.
        public float ChargePerAltUse = -1f;
        public float ChargeRatio
		{
            get
			{
                float ratio = Charge / MaxCharge;
				return float.IsNaN(ratio) || float.IsInfinity(ratio) ? 0f : MathHelper.Clamp(ratio, 0f, 1f);
			}
		}
        #endregion

        // Rarity is provided both as the classic int and the new enum.
        public CalamityRarity customRarity = CalamityRarity.NoEffect;
		public int postMoonLordRarity 
		{
			get => (int)customRarity;
			set => customRarity = (CalamityRarity)value;
		}

		///See RogueWeapon.cs for rogue modifier shit
		#region Modifiers
		public CalamityGlobalItem()
		{
			StealthGenBonus = 1f;
		}

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			CalamityGlobalItem myClone = (CalamityGlobalItem)base.Clone(item, itemClone);
			myClone.StealthGenBonus = StealthGenBonus;
			return myClone;
		}

		public override void PreReforge(Item item)/* tModPorter Note: Use CanReforge instead for logic determining if a reforge can happen. */
		{
			StealthGenBonus = 1f;
		}
		#endregion

        #region SetDefaults
        public override void SetDefaults(Item item)
        {
            if (customRarity.IsPostML() && item.rare != ItemRarityID.Red)
                item.rare = ItemRarityID.Red;

            if (item.maxStack == 99 || item.type == ItemID.Dynamite || item.type == ItemID.StickyDynamite ||
                item.type == ItemID.BouncyDynamite || item.type == ItemID.StickyBomb || item.type == ItemID.BouncyBomb)
                item.maxStack = 999;

            if (item.type == ItemID.PirateMap || item.type == ItemID.SnowGlobe)
                item.maxStack = 20;

            if (item.type >= ItemID.GreenSolution && item.type <= ItemID.RedSolution)
                item.value = Item.buyPrice(0, 0, 5, 0);

            if (CalamityLists.weaponAutoreuseList?.Contains(item.type) ?? false)
                item.autoReuse = true;

            if (item.type == ItemID.PsychoKnife || item.type == ItemID.TaxCollectorsStickOfDoom)
                item.damage *= 4;
            else if (item.type == ItemID.SpectreStaff)
                item.damage *= 3;
            else if (CalamityLists.doubleDamageBuffList?.Contains(item.type) ?? false)
                item.damage *= 2;
            else if (item.type == ItemID.RainbowRod)
                item.damage = (int)(item.damage * 1.75);
            else if (CalamityLists.sixtySixDamageBuffList?.Contains(item.type) ?? false)
                item.damage = (int)(item.damage * 1.66);
            else if (CalamityLists.fiftyDamageBuffList?.Contains(item.type) ?? false)
                item.damage = (int)(item.damage * 1.5);
            else if (CalamityLists.thirtyThreeDamageBuffList?.Contains(item.type) ?? false)
                item.damage = (int)(item.damage * 1.33);
            else if (CalamityLists.twentyFiveDamageBuffList?.Contains(item.type) ?? false)
                item.damage = (int)(item.damage * 1.25);
            else if (CalamityLists.twentyDamageBuffList?.Contains(item.type) ?? false)
                item.damage = (int)(item.damage * 1.2);
            else if (CalamityLists.tenDamageBuffList?.Contains(item.type) ?? false)
                item.damage = (int)(item.damage * 1.1);
            else if (CalamityLists.tenDamageNerfList?.Contains(item.type) ?? false)
                item.damage = (int)(item.damage * 0.9);
            else if (CalamityLists.quarterDamageNerfList?.Contains(item.type) ?? false)
                item.damage = (int)(item.damage * 0.75);
            else if (item.type == ItemID.BlizzardStaff)
                item.damage = (int)(item.damage * 0.7);
            else if (item.type == ItemID.LaserMachinegun)
                item.damage = (int)(item.damage * 0.65);
            else if (item.type == ItemID.StardustDragonStaff)
                item.damage = (int)(item.damage * 0.5);

            if (item.type == ItemID.BookStaff)
                item.mana = 10;
            else if (item.type == ItemID.UnholyTrident)
                item.mana = 14;
            else if (item.type == ItemID.FrostStaff)
                item.mana = 9;
            else if (item.type == ItemID.BookofSkulls)
                item.mana = 12;
            else if (item.type == ItemID.BlizzardStaff)
                item.mana = 7;
            else if (item.type == ItemID.SolarFlareHelmet) //total defense pre-buff = 78 post-buff = 94
                item.defense = 29; //5 more defense
            else if (item.type == ItemID.SolarFlareBreastplate)
                item.defense = 41; //7 more defense
            else if (item.type == ItemID.SolarFlareLeggings)
                item.defense = 24; //4 more defense
            else if (item.type == ItemID.GladiatorHelmet) //total defense pre-buff = 7 post-buff = 15
                item.defense = 3; //1 more defense
            else if (item.type == ItemID.GladiatorBreastplate)
                item.defense = 5; //2 more defense
            else if (item.type == ItemID.GladiatorLeggings)
                item.defense = 4; //2 more defense
            else if (item.type == ItemID.HallowedPlateMail) //total defense pre-buff = 31, 50, 35 post-buff = 36, 55, 40
                item.defense = 18; //3 more defense
            else if (item.type == ItemID.HallowedGreaves)
                item.defense = 13; //2 more defense

			if (CalamityLists.noGravityList.Contains(item.type))
				ItemID.Sets.ItemNoGravity[item.type] = true;
			if (CalamityLists.lavaFishList.Contains(item.type))
				ItemID.Sets.CanFishInLava[item.type] = true;

			// not expert because ML drops it in normal so that it can be used with the lore item
            if (item.type == ItemID.GravityGlobe)
			{
				item.expert = false;
				item.rare = 10;
			}
            
            if (item.type == ItemID.SuspiciousLookingTentacle)
                item.expert = true;

            if (item.type == ItemID.PearlwoodHammer)
			{
                item.hammer += 35; //80% hammer power
				item.useAnimation = 20;
				item.useTime = 15;
				item.damage *= 4;
				item.tileBoost += 1;
				item.rare = 4;
			}
            if (item.type == ItemID.PearlwoodBow)
			{
				item.useAnimation += 8; //35
				item.useTime += 8; //35
				item.shootSpeed += 3.4f; //10f
				item.knockBack += 1f; //1f
				item.rare = 4;
				item.damage = (int)(item.damage * 2.1);
			}
            if (item.type == ItemID.PearlwoodSword)
			{
				item.damage *= 4;
				item.rare = 4;
			}
			if (item.type == ItemID.StarCannon)
				item.UseSound = null;
        }
        #endregion

        #region Shoot
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CalamityPlayer modPlayer = player.Calamity();
            if (rogue)
            {
                velocity.X *= modPlayer.throwingVelocity;
                velocity.Y *= modPlayer.throwingVelocity;
                if (modPlayer.gloveOfRecklessness)
                {
                    Vector2 rotated = new Vector2(velocity.X, velocity.Y);
                    rotated = rotated.RotatedByRandom(MathHelper.ToRadians(10f));
                    velocity.X = rotated.X;
                    velocity.Y = rotated.Y;
                }
            }
            if (modPlayer.eArtifact && item.CountsAsClass(DamageClass.Ranged))
            {
                velocity.X *= 1.25f;
                velocity.Y *= 1.25f;
            }
			Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            if (modPlayer.luxorsGift)
            {
                // useTime 9 = 0.9 useTime 2 = 0.2
                double damageMult = 1.0;
                if (item.useTime < 10)
                    damageMult -= (10 - item.useTime) / 10.0;

                double newDamage = damage * damageMult;

                if (player.whoAmI == Main.myPlayer)
                {
                    if (item.CountsAsClass(DamageClass.Melee))
                        Projectile.NewProjectile(source, position, velocity1 * 0.5f, ModContent.ProjectileType<LuxorsGiftMelee>(), CalamityUtils.DamageSoftCap(newDamage * 0.6, 60), 0f, player.whoAmI);

                    else if (rogue)
                        Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<LuxorsGiftRogue>(), CalamityUtils.DamageSoftCap(newDamage * 0.5, 50), 0f, player.whoAmI);

                    else if (item.CountsAsClass(DamageClass.Ranged))
                        Projectile.NewProjectile(source, position, velocity1 * 1.5f, ModContent.ProjectileType<LuxorsGiftRanged>(), CalamityUtils.DamageSoftCap(newDamage * 0.4, 40), 0f, player.whoAmI);

                    else if (item.CountsAsClass(DamageClass.Magic))
                        Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<LuxorsGiftMagic>(), CalamityUtils.DamageSoftCap(newDamage * 0.8, 80), 0f, player.whoAmI);

                    else if (item.CountsAsClass(DamageClass.Summon) && player.ownedProjectileCounts[ModContent.ProjectileType<LuxorsGiftSummon>()] < 1)
                        Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<LuxorsGiftSummon>(), damage, 0f, player.whoAmI);
                }
            }
            if (modPlayer.bloodflareMage && modPlayer.canFireBloodflareMageProjectile)
            {
                if (item.CountsAsClass(DamageClass.Magic))
                {
					modPlayer.canFireBloodflareMageProjectile = false;
					if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<GhostlyBolt>(), CalamityUtils.DamageSoftCap(damage * 1.3, 250), 1f, player.whoAmI);
                    }
                }
            }
            if (modPlayer.bloodflareRanged && modPlayer.canFireBloodflareRangedProjectile)
            {
                if (item.CountsAsClass(DamageClass.Ranged))
                {
					modPlayer.canFireBloodflareRangedProjectile = false;
					if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BloodBomb>(), CalamityUtils.DamageSoftCap(damage * 0.8, 150), 2f, player.whoAmI);
                    }
                }
            }
            if (modPlayer.tarraMage)
            {
                if (modPlayer.tarraCrits >= 5 && player.whoAmI == Main.myPlayer)
                {
                    modPlayer.tarraCrits = 0;
                    int leafAmt = 9 + Main.rand.Next(3);
                    for (int l = 0; l < leafAmt; l++)
                    {
                        float spreadMult = 0.025f * l;
                        float hardar = velocity.X + Main.rand.Next(-25, 26) * spreadMult;
                        float hordor = velocity.Y + Main.rand.Next(-25, 26) * spreadMult;
                        float num84 = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                        num84 = item.shootSpeed / num84;
                        hardar *= num84;
                        hordor *= num84;
                        Projectile.NewProjectile(source, position, new Vector2(hardar, hordor), ProjectileID.Leaf, CalamityUtils.DamageSoftCap(damage * 0.2, 50), item.knockBack, player.whoAmI);
                    }
                }
            }
            if (modPlayer.ataxiaBolt && modPlayer.canFireAtaxiaRangedProjectile)
            {
                if (item.CountsAsClass(DamageClass.Ranged))
                {
					modPlayer.canFireAtaxiaRangedProjectile = false;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, position, velocity * 1.25f, ModContent.ProjectileType<ChaosFlare>(), CalamityUtils.DamageSoftCap(damage * 0.25, 50), 2f, player.whoAmI);
                    }
                }
            }
            if (modPlayer.godSlayerRanged && modPlayer.canFireGodSlayerRangedProjectile)
            {
                if (item.CountsAsClass(DamageClass.Ranged))
                {
					modPlayer.canFireGodSlayerRangedProjectile = false;
					if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, position, velocity * 1.25f, ModContent.ProjectileType<GodSlayerShrapnelRound>(), CalamityUtils.DamageSoftCap(damage, 200), 2f, player.whoAmI);
                    }
                }
            }
            if (modPlayer.ataxiaVolley && modPlayer.canFireAtaxiaRogueProjectile)
            {
                if (rogue)
                {
					modPlayer.canFireAtaxiaRogueProjectile = false;
					if (player.whoAmI == Main.myPlayer)
                    {
                        SoundEngine.PlaySound(SoundID.Item20, player.Center);
                        float spread = 45f * 0.0174f;
                        double startAngle = Math.Atan2(player.velocity.X, player.velocity.Y) - spread / 2;
                        double deltaAngle = spread / 8f;
                        double offsetAngle;
                        for (int i = 0; i < 4; i++)
                        {
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<ChaosFlare2>(), CalamityUtils.DamageSoftCap(damage * 0.5, 100), 1f, player.whoAmI);
                            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<ChaosFlare2>(), CalamityUtils.DamageSoftCap(damage * 0.5, 100), 1f, player.whoAmI);
                        }
                    }
                }
            }
            if (modPlayer.reaverDoubleTap && modPlayer.canFireReaverRangedProjectile)
            {
                if (item.CountsAsClass(DamageClass.Ranged))
                {
					modPlayer.canFireReaverRangedProjectile = false;
					if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, position, velocity * 1.25f, ModContent.ProjectileType<MiniRocket>(), CalamityUtils.DamageSoftCap(damage * 1.3, 150), 2f, player.whoAmI);
                    }
                }
            }
            if (modPlayer.victideSet)
            {
                if ((item.CountsAsClass(DamageClass.Ranged) || item.CountsAsClass(DamageClass.Melee) || item.CountsAsClass(DamageClass.Magic) || item.CountsAsClass(DamageClass.Throwing) || rogue || item.CountsAsClass(DamageClass.Summon)) && item.rare < ItemRarityID.Yellow && Main.rand.NextBool(10))
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, position, velocity * 1.25f, ModContent.ProjectileType<Seashell>(), CalamityUtils.DamageSoftCap(damage * 2, 60), 1f, player.whoAmI);
                    }
                }
            }
            if (modPlayer.dynamoStemCells)
            {
                if (item.CountsAsClass(DamageClass.Ranged) && Main.rand.Next(0, 100) >= 80)
                {
					double damageMult = item.useTime / 30D;
					if (damageMult < 0.35)
						damageMult = 0.35;

					int newDamage = (int)(damage * 2 * damageMult);

                    if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, position, velocity * 1.25f, ModContent.ProjectileType<Minibirb>(), CalamityUtils.DamageSoftCap(newDamage, 1000), 2f, player.whoAmI);
                    }
                }
            }
            if (modPlayer.prismaticRegalia)
            {
                if (item.CountsAsClass(DamageClass.Magic) && Main.rand.Next(0, 100) >= 95)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
						for (int i = -5; i <= 5; i += 5)
						{
							if (i != 0)
							{
								Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
								int rocket = Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<MiniRocket>(), CalamityUtils.DamageSoftCap(damage * 0.8, 200), 2f, player.whoAmI);
								Main.projectile[rocket].Calamity().forceTypeless = true;
							}
						}
                    }
                }
            }
            if (modPlayer.harpyWingBoost && modPlayer.harpyRing)
            {
                if (Main.rand.NextBool(5))
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
						float spreadX = velocity.X + Main.rand.NextFloat(-0.75f, 0.75f);
						float spreadY = velocity.Y + Main.rand.NextFloat(-0.75f, 0.75f);
                        int feather = Projectile.NewProjectile(source, position, new Vector2(spreadX, spreadY) * 1.25f, ModContent.ProjectileType<TradewindsProjectile>(), CalamityUtils.DamageSoftCap(damage * 0.5, 75), 2f, player.whoAmI);
						Main.projectile[feather].usesLocalNPCImmunity = true;
						Main.projectile[feather].localNPCHitCooldown = 10;
						Main.projectile[feather].Calamity().forceTypeless = true;
                    }
                }
            }
			if (item.type == ItemID.StarCannon)
			{
				Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 5f;
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				{
					position += muzzleOffset;
				}
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FallenStarProj>(), damage, item.knockBack, player.whoAmI);
				SoundEngine.PlaySound(SoundID.Item11 with {PitchVariance = (0.05f)}, position);
				return false;
			}
			if (item.type == ItemID.PearlwoodBow)
			{
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && (Main.projectile[i].type == ModContent.ProjectileType<RainbowFront>() || Main.projectile[i].type == ModContent.ProjectileType<RainbowTrail>()) && Main.projectile[i].owner == player.whoAmI)
                    {
                        Main.projectile[i].Kill();
                    }
                }
				for (int i = -8; i <= 8; i += 8)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
					Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<RainbowFront>(), damage, 0f, player.whoAmI);
				}
			}
            return true;
        }
        #endregion

        #region SavingAndLoading
        public override void SaveData(Item item, TagCompound tag)/* tModPorter Suggestion: Edit tag parameter instead of returning new TagCompound */
        {
            tag.Add("rogue", rogue);
            tag.Add("timesUsed", timesUsed);
            //tag.Add("rarity", customRarity);
            tag.Add("charge", Charge);
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            rogue = tag.GetBool("rogue");
            timesUsed = tag.GetInt("timesUsed");
            //customRarity = (CalamityRarity)tag.GetInt("rarity");

            // Changed charge from int to float. If an old charge int is present, load that instead.
            if (tag.ContainsKey("Charge"))
                Charge = tag.GetInt("Charge");
            else
                Charge = tag.GetFloat("charge");
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = rogue;

            writer.Write(flags);
            writer.Write((int)customRarity);
            writer.Write(timesUsed);
            writer.Write(Charge);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            rogue = flags[0];

            customRarity = (CalamityRarity)reader.ReadInt32();
            timesUsed = reader.ReadInt32();
            Charge = reader.ReadSingle();
        }
        #endregion

        #region Pickup Item Changes
        public override bool OnPickup(Item item, Player player)
        {
            if (item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane)
            {
                bool boostedHeart = player.Calamity().photosynthesis;
                if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitas>()))
                {
                    player.statLife -= boostedHeart ? 5 : 10;
                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.HealEffect(boostedHeart ? -5 : -10, true);
                    }
                }
                else if (boostedHeart)
                {
                    player.statLife += 5;
                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.HealEffect(5, true);
                    }
                }
            }
            return true;
        }
        #endregion

        #region Profaned Soul Crystal Attacks

        private bool HandleAttackTransforms(Item item, Player player)
        {
            if (player.whoAmI != Main.myPlayer)
                return false;
            int weaponType = item.CountsAsClass(DamageClass.Melee) ? 1 : item.CountsAsClass(DamageClass.Ranged) ? 2 : item.CountsAsClass(DamageClass.Magic) ? 3 : item.Calamity().rogue ? 4 : -1;
            if (weaponType > 0)
            {
                if (player.Calamity().profanedSoulWeaponType != weaponType || player.Calamity().profanedSoulWeaponUsage >= 300)
                {
                    player.Calamity().profanedSoulWeaponType = weaponType;
                    player.Calamity().profanedSoulWeaponUsage = 0;
                }
                Vector2 correctedVelocity = Main.MouseWorld - player.Center;
                correctedVelocity.Normalize();
                bool shouldNerf = player.Calamity().endoCooper || player.Calamity().magicHat; //No bonkers damage memes thank you very much.
                bool enrage = player.statLife <= (int)(player.statLifeMax2 * 0.5);
                if (item.CountsAsClass(DamageClass.Melee))
                {
                    if (player.Calamity().profanedSoulWeaponUsage % (enrage ? 4 : 6) == 0)
                    {
                        if (player.Calamity().profanedSoulWeaponUsage > 0 && player.Calamity().profanedSoulWeaponUsage % (enrage ? 20 : 30) == 0) //every 5 shots is a shotgun spread
                        {
                            int numProj = 5;

                            correctedVelocity *= 12f;
                            int spread = 3;
                            for (int i = 0; i < numProj; i++)
                            {
                                Vector2 perturbedspeed = new Vector2(correctedVelocity.X, correctedVelocity.Y + Main.rand.Next(-3, 4)).RotatedBy(MathHelper.ToRadians(spread));

                                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y - 10, perturbedspeed.X, perturbedspeed.Y, ModContent.ProjectileType<ProfanedCrystalMeleeSpear>(), (int)((shouldNerf ? 1000 : 1750) * player.MinionDamage()), 1f, player.whoAmI, Main.rand.NextBool(player.Calamity().profanedSoulWeaponUsage == 4 ? 5 : 7) ? 1f : 0f);
                                spread -= Main.rand.Next(2, 4);
                                SoundEngine.PlaySound(SoundID.Item20, player.Center);
                            }
                            player.Calamity().profanedSoulWeaponUsage = 0;
                        }
                        else
                        {
                            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, correctedVelocity * 6.9f, ModContent.ProjectileType<ProfanedCrystalMeleeSpear>(), (int)((shouldNerf ? 500 : 1250) * player.MinionDamage()), 1f, player.whoAmI, Main.rand.NextBool(player.Calamity().profanedSoulWeaponUsage == 4 ? 5 : 7) ? 1f : 0f, 1f);
                            SoundEngine.PlaySound(SoundID.Item20, player.Center);
                        }

                    }
                    player.Calamity().profanedSoulWeaponUsage++;

                }
                else if (item.CountsAsClass(DamageClass.Ranged))
                {
                    if (enrage || Main.rand.NextBool(2)) //100% chance if 50% or lower, else 1 in 2 chance
                    {
                        correctedVelocity *= 20f;
                        Vector2 perturbedspeed = new Vector2(correctedVelocity.X + Main.rand.Next(-3, 4), correctedVelocity.Y + Main.rand.Next(-3, 4)).RotatedBy(MathHelper.ToRadians(3));
                        bool isSmallBoomer = Main.rand.NextDouble() <= (enrage ? 0.2 : 0.3); // 20% chance if enraged, else 30% This is intentional due to literally doubling the amount of projectiles fired.
                        bool isThiccBoomer = isSmallBoomer && Main.rand.NextDouble() <= 0.05; // 5%
                        int projType = isSmallBoomer ? isThiccBoomer ? 1 : 2 : 3;
                        int dam = (int)((shouldNerf ? 500 : 1000) * player.MinionDamage());
                        switch (projType)
                        {
                            case 1: //big boomer
                            case 2: //boomer
                                int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, perturbedspeed, ModContent.ProjectileType<ProfanedCrystalRangedHuges>(), dam, 0f, player.whoAmI, projType == 1 ? 1f : 0f);
                                Main.projectile[proj].Calamity().forceMinion = true;
                                break;
                            case 3: //bab boomer
                                int proj2 = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, perturbedspeed, ModContent.ProjectileType<ProfanedCrystalRangedSmalls>(), dam, 0f, player.whoAmI, 0f);
                                Main.projectile[proj2].Calamity().forceMinion = true;
                                break;
                        }
                        if (projType > 1)
                        {
                            SoundEngine.PlaySound(SoundID.Item20, player.Center);
                        }
                    }
                }
                else if (item.CountsAsClass(DamageClass.Magic))
                {
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<ProfanedCrystalMageFireball>()] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<ProfanedCrystalMageFireballSplit>()] == 0)
                    {
                        player.Calamity().profanedSoulWeaponUsage = 0;
                    }
                    int manaCost = (int)(100 * player.manaCost);
                    if (player.statMana < manaCost && player.Calamity().profanedSoulWeaponUsage == 0)
                    {
                        if (player.manaFlower)
                        {
                            player.QuickMana();
                        }
                    }
                    if (player.statMana >= manaCost && player.Calamity().profanedSoulWeaponUsage == 0 && !player.silence)
                    {
                        player.manaRegenDelay = (int)player.maxRegenDelay;
                        player.statMana -= manaCost;
                        correctedVelocity *= 25f;
                        SoundEngine.PlaySound(SoundID.Item20, player.Center);
                        int dam = (int)((shouldNerf ? 1800 : 4500) * player.MinionDamage());
                        if (player.HasBuff(BuffID.ManaSickness))
                        {
                            int sickPenalty = (int)(dam * (0.05f * ((player.buffTime[player.FindBuffIndex(BuffID.ManaSickness)] + 60) / 60)));
                            dam -= sickPenalty;
                        }
                        int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.position, correctedVelocity, ModContent.ProjectileType<ProfanedCrystalMageFireball>(), dam, 1f, player.whoAmI, enrage ? 1f : 0f);
                        Main.projectile[proj].Calamity().forceMinion = true;
                        player.Calamity().profanedSoulWeaponUsage = enrage ? 20 : 25;
                    }
                    if (player.Calamity().profanedSoulWeaponUsage > 0)
                        player.Calamity().profanedSoulWeaponUsage--;
                }
                else if (item.Calamity().rogue)
                {
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<ProfanedCrystalRogueShard>()] == 0)
                    {
                        player.Calamity().profanedSoulWeaponUsage = 0;
                    }
                    if (player.Calamity().profanedSoulWeaponUsage >= (enrage ? 69 : 180))
                    {
                        float crystalCount = 36f;
                        for (float i = 0; i < crystalCount; i++)
                        {
                            float angle = MathHelper.TwoPi / crystalCount * i;
                            int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, angle.ToRotationVector2() * 8f, ModContent.ProjectileType<ProfanedCrystalRogueShard>(), (int)((shouldNerf ? 300 : 880) * player.MinionDamage()), 1f, player.whoAmI, 0f, 0f);
                            Main.projectile[proj].Calamity().forceMinion = true;
                            SoundEngine.PlaySound(SoundID.Item20, player.Center);
                        }
                        player.Calamity().profanedSoulWeaponUsage = 0;
                    }
                    else if (player.Calamity().profanedSoulWeaponUsage % (enrage ? 5 : 10) == 0)
                    {
                        float angle = MathHelper.TwoPi / (enrage ? 9 : 18) * (player.Calamity().profanedSoulWeaponUsage / (enrage ? 1 : 10));
                        int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, angle.ToRotationVector2() * 8f, ModContent.ProjectileType<ProfanedCrystalRogueShard>(), (int)((shouldNerf ? 400 : 1100) * player.MinionDamage()), 1f, player.whoAmI, 1f, 0f);
                        Main.projectile[proj].Calamity().forceMinion = true;
                        SoundEngine.PlaySound(SoundID.Item20, player.Center);
                    }
                    player.Calamity().profanedSoulWeaponUsage += enrage ? 1 : 2;
                    if (!enrage && player.Calamity().profanedSoulWeaponUsage % 2 != 0)
                        player.Calamity().profanedSoulWeaponUsage--;
                }
            }
            return false;
        }

        #endregion

        #region Andromeda Dev Item Attacks
        private bool PerformAndromedaAttacks(Item item, Player player)
        {
            if (player.whoAmI != Main.myPlayer)
                return false;

            int robotIndex = -1;

            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].active &&
                    Main.projectile[i].type == ModContent.ProjectileType<GiantIbanRobotOfDoom>() &&
                    Main.projectile[i].owner == player.whoAmI)
                {
                    robotIndex = i;
                    break;
                }
            }
            if (robotIndex != -1)
            {
                Projectile robot = Main.projectile[robotIndex];
                GiantIbanRobotOfDoom robotModProjectile = ((GiantIbanRobotOfDoom)robot.ModProjectile);
                if (player.ownedProjectileCounts[ModContent.ProjectileType<AndromedaRegislash>()] <= 0 &&
                    robotModProjectile.TopIconActive &&
                    (robotModProjectile.RightIconCooldown <= GiantIbanRobotOfDoom.RightIconAttackTime ||
                     !robotModProjectile.RightIconActive)) // "Melee" attack
                {
                    int damage = player.Calamity().andromedaState == AndromedaPlayerState.SmallRobot ? GiantIbanRobotOfDoom.RegicideBaseDamageSmall : GiantIbanRobotOfDoom.RegicideBaseDamageLarge;
                    if (item.Calamity().rogue)
                    {
                        damage = (int)(damage * player.RogueDamage());
                    }
                    if (item.CountsAsClass(DamageClass.Melee))
                    {
                        damage = (int)(damage * player.MeleeDamage());
                    }
                    if (item.CountsAsClass(DamageClass.Ranged))
                    {
                        damage = (int)(damage * player.RangedDamage());
                    }
                    if (item.CountsAsClass(DamageClass.Magic))
                    {
                        damage = (int)(damage * player.MagicDamage());
                    }
                    if (item.CountsAsClass(DamageClass.Summon))
                    {
                        damage = (int)(damage * player.MinionDamage());
                    }
                    Projectile blade = Projectile.NewProjectileDirect(player.GetSource_FromThis(), robot.Center + (robot.spriteDirection > 0).ToDirectionInt() * robot.width / 2 * Vector2.UnitX, 
                               Vector2.Zero, ModContent.ProjectileType<AndromedaRegislash>(), damage, 15f, player.whoAmI, Projectile.GetByUUID(robot.owner, robot.whoAmI));

                    if (item.Calamity().rogue)
                    {
                        blade.Calamity().forceRogue = true;
                    }
                    if (item.CountsAsClass(DamageClass.Melee))
                    {
                        blade.Calamity().forceMelee = true;
                    }
                    if (item.CountsAsClass(DamageClass.Ranged))
                    {
                        blade.Calamity().forceRanged = true;
                    }
                    if (item.CountsAsClass(DamageClass.Magic))
                    {
                        blade.Calamity().forceMagic = true;
                    }
                    if (item.CountsAsClass(DamageClass.Summon))
                    {
                        blade.Calamity().forceMinion = true;
                    }
                }

                if (!robotModProjectile.TopIconActive &&
                    (robotModProjectile.LeftBracketActive || robotModProjectile.RightBracketActive) &&
                    !robotModProjectile.BottomBracketActive &&
                    robotModProjectile.LaserCooldown <= 0 &&
                    (robotModProjectile.RightIconCooldown <= GiantIbanRobotOfDoom.RightIconAttackTime ||
                     !robotModProjectile.RightIconActive)) // "Ranged" attack
                {
                    robotModProjectile.LaserCooldown = AndromedaDeathRay.TrueTimeLeft * 2;
                    if (player.Calamity().andromedaState == AndromedaPlayerState.SmallRobot)
                    {
                        robotModProjectile.LaserCooldown = AndromedaDeathRay.TrueTimeLeft + 1;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Use Item Changes
        public override bool? UseItem(Item item, Player player)/* tModPorter Suggestion: Return null instead of false */
        {
			// Give 2 minutes of Honey buff when drinking Bottled Honey.
            if (item.type == ItemID.BottledHoney)
				player.AddBuff(BuffID.Honey, 7200);
            return base.UseItem(item, player);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (player.Calamity().profanedCrystalBuffs && item.pick == 0 && item.axe == 0 && item.hammer == 0 && item.autoReuse && (item.Calamity().rogue || item.CountsAsClass(DamageClass.Magic) || item.CountsAsClass(DamageClass.Ranged) || item.CountsAsClass(DamageClass.Melee)))
            {
                NPC closest = Main.MouseWorld.ClosestNPCAt(1000f, true);
                if (closest != null)
                {
                    player.MinionAttackTargetNPC = closest.whoAmI;
                    player.UpdateMinionTarget();
                }
                return false;
            }
            if (player.ActiveItem().type == ModContent.ItemType<IgneousExaltation>())
            {
                bool hasBlades = false;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<IgneousBlade>() && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].localAI[1] == 0f)
                    {
                        hasBlades = true;
                        break;
                    }
                }
                if (hasBlades)
                {
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        if (Main.projectile[i].ModProjectile is IgneousBlade)
                        {
                            if ((Main.projectile[i].ModProjectile as IgneousBlade).Firing)
                                continue;
                        }
                        if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<IgneousBlade>() && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].localAI[1] == 0f)
                        {
                            Main.projectile[i].rotation = MathHelper.PiOver2 + MathHelper.PiOver4;
                            Main.projectile[i].velocity = Main.projectile[i].DirectionTo(Main.MouseWorld) * 22f;
                            Main.projectile[i].rotation += Main.projectile[i].velocity.ToRotation();
                            Main.projectile[i].ai[0] = 180f;
                            (Main.projectile[i].ModProjectile as IgneousBlade).Firing = true;
                            Main.projectile[i].tileCollide = true;
                            Main.projectile[i].netUpdate = true;
                        }
                    }
                }
                return false;
            }
            if (player.ActiveItem().type == ModContent.ItemType<ColdDivinity>())
            {
                bool canContinue = true;
                int count = 0;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<ColdDivinityPointyThing>() && Main.projectile[i].owner == player.whoAmI)
                    {
                        if (Main.projectile[i].ai[1] > 1f)
                        {
                            canContinue = false;
                            break;
                        }
                        else if (Main.projectile[i].ai[1] == 0f)
                        {
                            if (((ColdDivinityPointyThing)Main.projectile[i].ModProjectile).circlingPlayer)
                                count++;
                        }
                    }
                }
                if (canContinue && count > 0)
                {
                    NPC unluckyTarget = CalamityUtils.MinionHoming(Main.MouseWorld, 1000f, player);
                    if (unluckyTarget != null)
                    {
                        int height = unluckyTarget.getRect().Height;
                        int width = unluckyTarget.getRect().Width;
                        int pointyThingyAmount = count;
                        float angleVariance = MathHelper.TwoPi / pointyThingyAmount;
                        float angle = 0f;

                        for (int i = 0; i < pointyThingyAmount; i++)
                        {
                            if (Main.projectile.Length == Main.maxProjectiles)
                                break;
                            int projj = Projectile.NewProjectile(player.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<ColdDivinityPointyThing>(), (int)(80 * player.MinionDamage()), 1f, player.whoAmI, angle, 2f);
                            angle += angleVariance;
                            for (int j = 0; j < 22; j++)
                            {
                                Dust dust = Dust.NewDustDirect(Main.projectile[projj].position, Main.projectile[projj].width, Main.projectile[projj].height, DustID.Ice);
                                dust.velocity = Vector2.UnitY * Main.rand.NextFloat(3f, 5.5f) * Main.rand.NextBool(2).ToDirectionInt();
                                dust.noGravity = true;
                            }
                        }
                    }
                }
                return false;
            }
            return base.AltFunctionUse(item, player);
        }

        public override bool CanUseItem(Item item, Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityGlobalItem modItem = item.Calamity();

            // Restrict behavior when reading Dreadon's Log.
            if (PopupGUIManager.AnyGUIsActive)
                return false;

            if (player.ownedProjectileCounts[ModContent.ProjectileType<RelicOfDeliveranceSpear>()] > 0 &&
                (item.damage > 0 || item.ammo != AmmoID.None))
            {
                return false; // Don't use weapons if you're charging with a spear
            }

            // Conversion for Andromeda
            if (player.ownedProjectileCounts[ModContent.ProjectileType<GiantIbanRobotOfDoom>()] > 0)
            {
				if (item.type == ItemID.WireKite)
					return false;
                if (item.pick > 0 || item.axe > 0 || item.hammer > 0 || item.fishingPole > 0)
                    return false;
                if (item.Calamity().rogue || item.CountsAsClass(DamageClass.Magic) || item.CountsAsClass(DamageClass.Ranged) || item.CountsAsClass(DamageClass.Melee))
                {
                    if (player.altFunctionUse == 0)
                        return PerformAndromedaAttacks(item, player);
                    else return false;
                }
            }

            // Conversion for Profaned Soul Crystal
            if (modPlayer.profanedCrystalBuffs && item.pick == 0 && item.axe == 0 && item.hammer == 0 && item.autoReuse && (modItem.rogue || item.CountsAsClass(DamageClass.Magic) || item.CountsAsClass(DamageClass.Ranged) || item.CountsAsClass(DamageClass.Melee)))
            {   
                if (player.altFunctionUse == 0)
                {
                    return HandleAttackTransforms(item, player);
                }
                else
                {
                    return AltFunctionUse(item, player);
                }
            }

            // Check for sufficient charge if this item uses charge.
            if (item.type >= ItemID.Count && modItem.UsesCharge)
            {
                // If attempting to use alt fire, and alt fire charge is defined, require that charge. Otherwise require normal charge per use.
                float chargeNeeded = (player.altFunctionUse == 2 && modItem.ChargePerAltUse != -1f) ? modItem.ChargePerAltUse : modItem.ChargePerUse;

                // If the amount of charge needed is zero or less, ignore the charge requirement entirely (e.g. summon staff right click).
                if (chargeNeeded > 0f)
				{
                    if (modItem.Charge < chargeNeeded)
                        return false;

                    // If you have enough charge, decrement charge on the spot because this hook runs exactly once every time you use an item.
                    // Mana has to be checked separately or you'll fail to use the weapon on a mana check later and still have consumed charge.
                    if (player.CheckMana(item))
                        Charge -= chargeNeeded;
                }
            }

            if (item.type == ItemID.MonkStaffT1)
            {
                return player.ownedProjectileCounts[item.shoot] <= 0;
            }
            if (item.type == ItemID.InvisibilityPotion && player.FindBuffIndex(ModContent.BuffType<ShadowBuff>()) > -1)
            {
                return false;
            }
            if ((item.type == ItemID.RegenerationPotion || item.type == ItemID.LifeforcePotion) && player.FindBuffIndex(ModContent.BuffType<Cadence>()) > -1)
            {
                return false;
            }
            if (item.type == ModContent.ItemType<CrumblingPotion>() && player.FindBuffIndex(ModContent.BuffType<ArmorShattering>()) > -1)
            {
                return false;
            }
            if (item.type == ItemID.WrathPotion && player.FindBuffIndex(ModContent.BuffType<HolyWrathBuff>()) > -1)
            {
                return false;
            }
            if (item.type == ItemID.RagePotion && player.FindBuffIndex(ModContent.BuffType<ProfanedRageBuff>()) > -1)
            {
                return false;
            }
            if ((item.type == ItemID.ShinePotion || item.type == ItemID.NightOwlPotion) && (modPlayer.etherealExtorter && modPlayer.ZoneAbyss))
            {
                return false;
            }
            if ((item.type == ItemID.SuperAbsorbantSponge || item.type == ItemID.EmptyBucket) && modPlayer.ZoneAbyss)
            {
                return false;
            }
            if (item.type == ItemID.MagicMirror || item.type == ItemID.IceMirror || item.type == ItemID.CellPhone || item.type == ItemID.RecallPotion)
            {
                return !NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitas>());
            }
            if (item.type == ItemID.RodofDiscord)
            {
				if (player.chaosState)
					return false;

				Vector2 teleportLocation;
				teleportLocation.X = (float)Main.mouseX + Main.screenPosition.X;
				if (player.gravDir == 1f)
				{
					teleportLocation.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;
				}
				else
				{
					teleportLocation.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
				}
				teleportLocation.X -= (float)(player.width / 2);
				if (teleportLocation.X > 50f && teleportLocation.X < (float)(Main.maxTilesX * 16 - 50) && teleportLocation.Y > 50f && teleportLocation.Y < (float)(Main.maxTilesY * 16 - 50))
				{
					int x = (int)teleportLocation.X / 16;
					int y = (int)teleportLocation.Y / 16;
					bool templeCheck = Main.tile[x, y].WallType != WallID.LihzahrdBrickUnsafe || y <= Main.worldSurface || NPC.downedPlantBoss;
					if (templeCheck && !Collision.SolidCollision(teleportLocation, player.width, player.height))
					{
						int duration = CalamityPlayer.chaosStateDuration;
						if (CalamityPlayer.areThereAnyDamnBosses || CalamityPlayer.areThereAnyDamnEvents)
							duration = CalamityPlayer.chaosStateDurationBoss;
						if (modPlayer.eScarfCooldown)
							duration = (int)(duration * 1.5);
						else if (modPlayer.scarfCooldown)
							duration *= 2;
						player.AddBuff(BuffID.ChaosState, duration, true);
					}
				}
			}
            return true;
        }
        #endregion

        #region Modify Weapon Damage
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (item.type < ItemID.Count)
                return;

            // Summon weapons specifically do not have their damage affected by charge. They still require charge to function however.
            CalamityGlobalItem modItem = item.Calamity();
            if (!item.CountsAsClass(DamageClass.Summon) && (modItem?.UsesCharge ?? false))
            {
                // At exactly zero charge, do not perform any multiplication.
                // This makes charge-using weapons show up at full damage when previewed in crafting, Recipe Browser, etc.
                if (Charge == 0f)
                    return;
                damage *= ChargeDamageFormula();
            }
        }

        // This formula gives a slightly higher value than 1.0 above 85% charge, and a slightly lower value than 0.0 at 0% charge.
        // Specifically, it gives 0.0 or less at 0.36% charge or lower. This is fine because the result is immediately clamped.
        internal float ChargeDamageFormula()
        {
            float x = MathHelper.Clamp(ChargeRatio, 0f, 1f);
            float y = 1.087f - 0.08f / (x + 0.07f);
            return MathHelper.Clamp(y, 0f, 1f);
        }
        #endregion

        #region Modify Tooltips
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
			#region Custom Rarities#
            TooltipLine tt2 = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.Mod == "Terraria");
            if (tt2 != null)
            {
                // The special color in the tooltip were overlapping. Placing everything in here, above other things, was the only solution I could find that worked.
                if (item.type == ModContent.ItemType<Eternity>())
                {
                    List<Color> colorSet = new List<Color>()
                    {
                        new Color(188, 192, 193), // white
                        new Color(157, 100, 183), // purple
                        new Color(249, 166, 77), // honey-ish orange
                        new Color(255, 105, 234), // pink
                        new Color(67, 204, 219), // sky blue
                        new Color(249, 245, 99), // bright yellow
                        new Color(236, 168, 247), // purplish pink
                    };
                    if (tt2 != null)
                    {
                        int colorIndex = (int)(Main.GlobalTimeWrappedHourly / 2 % colorSet.Count);
                        Color currentColor = colorSet[colorIndex];
                        Color nextColor = colorSet[(colorIndex + 1) % colorSet.Count];
                        tt2.OverrideColor = Color.Lerp(currentColor, nextColor, Main.GlobalTimeWrappedHourly % 2f > 1f ? 1f : Main.GlobalTimeWrappedHourly % 1f);
                    }
                    return;
                }
                switch (customRarity)
                {
                    default:
                        break;
                    case CalamityRarity.Turquoise:
                        tt2.OverrideColor = new Color(0, 255, 200);
                        break;
                    case CalamityRarity.PureGreen:
                        tt2.OverrideColor = new Color(0, 255, 0);
                        break;
                    case CalamityRarity.DarkBlue:
                        tt2.OverrideColor = new Color(43, 96, 222);
                        break;
                    case CalamityRarity.Violet:
                        tt2.OverrideColor = new Color(108, 45, 199);
                        break;
                    case CalamityRarity.Developer:
                        tt2.OverrideColor = new Color(255, 0, 255);
                        break;

                    case CalamityRarity.Rainbow:
                        tt2.OverrideColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                        break;
                    case CalamityRarity.DraedonRust:
                        tt2.OverrideColor = new Color(204, 71, 35);
                        break;
                    case CalamityRarity.RareVariant:
                        tt2.OverrideColor = new Color(255, 140, 0);
                        break;
                    case CalamityRarity.Dedicated:
                        tt2.OverrideColor = new Color(139, 0, 0);
                        break;

                    case CalamityRarity.ItemSpecific:
                        // Uniquely colored developer items
                        if (item.type == ModContent.ItemType<Fabstaff>())
                            tt2.OverrideColor = new Color(Main.DiscoR, 100, 255);
                        if (item.type == ModContent.ItemType<BlushieStaff>())
                            tt2.OverrideColor = new Color(0, 0, 255);
                        if (item.type == ModContent.ItemType<Judgement>())
                            tt2.OverrideColor = Judgement.GetSyncedLightColor();
                        if (item.type == ModContent.ItemType<NanoblackReaperMelee>() || item.type == ModContent.ItemType<NanoblackReaperRogue>())
                            tt2.OverrideColor = new Color(0.34f, 0.34f + 0.66f * Main.DiscoG / 255f, 0.34f + 0.5f * Main.DiscoG / 255f);
                        if (item.type == ModContent.ItemType<ProfanedSoulCrystal>())
                            tt2.OverrideColor = CalamityUtils.ColorSwap(new Color(255, 166, 0), new Color(25, 250, 25), 4f); //alternates between emerald green and amber (BanditHueh)
                        if (item.type == ModContent.ItemType<BensUmbrella>())
                            tt2.OverrideColor = CalamityUtils.ColorSwap(new Color(210, 0, 255), new Color(255, 248, 24), 4f);
                        if (item.type == ModContent.ItemType<Endogenesis>())
                            tt2.OverrideColor = CalamityUtils.ColorSwap(new Color(131, 239, 255), new Color(36, 55, 230), 4f);
                        if (item.type == ModContent.ItemType<DraconicDestruction>())
                            tt2.OverrideColor = CalamityUtils.ColorSwap(new Color(255, 69, 0), new Color(139, 0, 0), 4f);
                        if (item.type == ModContent.ItemType<ScarletDevil>())
                            tt2.OverrideColor = CalamityUtils.ColorSwap(new Color(191, 45, 71), new Color(185, 187, 253), 4f);
                        if (item.type == ModContent.ItemType<RedSun>())
                            tt2.OverrideColor = CalamityUtils.ColorSwap(new Color(204, 86, 80), new Color(237, 69, 141), 4f);
                        if (item.type == ModContent.ItemType<GaelsGreatsword>())
                            tt2.OverrideColor = new Color(146, 0, 0);
                        if (item.type == ModContent.ItemType<CrystylCrusher>())
                            tt2.OverrideColor = new Color(129, 29, 149);
                        if (item.type == ModContent.ItemType<Svantechnical>())
                            tt2.OverrideColor = new Color(220, 20, 60);
                        if (item.type == ModContent.ItemType<SomaPrime>())
                            tt2.OverrideColor = new Color(254, 253, 235);
                        if (item.type == ModContent.ItemType<Contagion>())
                            tt2.OverrideColor = new Color(207, 17, 117);
                        if (item.type == ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>() || item.type == ModContent.ItemType<TriactisTruePaladinianMageHammerofMight>())
                            tt2.OverrideColor = new Color(227, 226, 180);
                        if (item.type == ModContent.ItemType<RoyalKnivesMelee>() || item.type == ModContent.ItemType<RoyalKnives>())
                            tt2.OverrideColor = CalamityUtils.ColorSwap(new Color(154, 255, 151), new Color(228, 151, 255), 4f);
                        if (item.type == ModContent.ItemType<DemonshadeHelm>() || item.type == ModContent.ItemType<DemonshadeBreastplate>() || item.type == ModContent.ItemType<DemonshadeGreaves>())
                            tt2.OverrideColor = CalamityUtils.ColorSwap(new Color(255, 132, 22), new Color(221, 85, 7), 4f);
                        if (item.type == ModContent.ItemType<PrototypeAndromechaRing>())
                        {
                            if (Main.GlobalTimeWrappedHourly % 1f < 0.6f)
                            {
                                tt2.OverrideColor = new Color(89, 229, 255);
                            }
                            else if (Main.GlobalTimeWrappedHourly % 1f < 0.8f)
                            {
                                tt2.OverrideColor = Color.Lerp(new Color(89, 229, 255), Color.White, (Main.GlobalTimeWrappedHourly % 1f - 0.6f) / 0.2f);
                            }
                            else
                            {
                                tt2.OverrideColor = Color.Lerp(Color.White, new Color(89, 229, 255), (Main.GlobalTimeWrappedHourly % 1f - 0.8f) / 0.2f);
                            }
                        }
                        if (item.type == ModContent.ItemType<Earth>())
						{
							List<Color> earthColors = new List<Color>()
							{
								new Color(255, 99, 146),
								new Color(255, 228, 94),
								new Color(127, 200, 248)
							};
							if (tt2 != null)
							{
								int colorIndex = (int)(Main.GlobalTimeWrappedHourly / 2 % earthColors.Count);
								Color currentColor = earthColors[colorIndex];
								Color nextColor = earthColors[(colorIndex + 1) % earthColors.Count];
								tt2.OverrideColor = Color.Lerp(currentColor, nextColor, Main.GlobalTimeWrappedHourly % 2f > 1f ? 1f : Main.GlobalTimeWrappedHourly % 1f);
							}
						}

                        // Uniquely colored legendary weapons and Yharim's Crystal
                        if (item.type == ModContent.ItemType<AegisBlade>() || item.type == ModContent.ItemType<YharimsCrystal>())
                            tt2.OverrideColor = new Color(255, Main.DiscoG, 53);
                        if (item.type == ModContent.ItemType<BlossomFlux>())
                            tt2.OverrideColor = new Color(Main.DiscoR, 203, 103);
                        if (item.type == ModContent.ItemType<BrinyBaron>() || item.type == ModContent.ItemType<ColdDivinity>())
                            tt2.OverrideColor = new Color(53, Main.DiscoG, 255);
                        if (item.type == ModContent.ItemType<CosmicDischarge>())
                            tt2.OverrideColor = new Color(150, Main.DiscoG, 255);
                        if (item.type == ModContent.ItemType<Malachite>())
                            tt2.OverrideColor = new Color(Main.DiscoR, 203, 103);
                        if (item.type == ModContent.ItemType<SeasSearing>())
                            tt2.OverrideColor = new Color(60, Main.DiscoG, 190);
                        if (item.type == ModContent.ItemType<SHPC>())
                            tt2.OverrideColor = new Color(255, Main.DiscoG, 155);
                        if (item.type == ModContent.ItemType<Vesuvius>())
                            tt2.OverrideColor = new Color(255, Main.DiscoG, 0);
                        if (item.type == ModContent.ItemType<PristineFury>())
							tt2.OverrideColor = CalamityUtils.ColorSwap(new Color(255, 168, 53), new Color(255, 249, 0), 2f);
                        if (item.type == ModContent.ItemType<LeonidProgenitor>())
							tt2.OverrideColor = CalamityUtils.ColorSwap(LeonidProgenitor.blueColor, LeonidProgenitor.purpleColor, 3f);
                        break;
                }
            }
			#endregion
            
			if (item.type == ItemID.BottledHoney)
			{
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.Mod == "Terraria" && line2.Name == "HealLife")
					{
						line2.Text += "\n" + CalamityUtils.GetTextValue("Vanilla.ItemTooltips.BottledHoneyExtra");
					}
				}
			}
            if (item.type == ItemID.RodofDiscord)
			{
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
					{
						line2.Text += "\n" + CalamityUtils.GetTextValue("Vanilla.ItemTooltips.RodofDiscordExtra");
					}
				}
			}
			if (item.type == ItemID.SuperAbsorbantSponge)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.NoAbyss");
                    }
                }
            }
            if (item.type == ItemID.EmptyBucket)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Defense")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.NoAbyss");
                    }
                }
            }
            if (item.type == ItemID.CrimsonHeart)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.SmallLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.ShadowOrb)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.SmallLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.MagicLantern)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.SmallLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.ArcticDivingGear)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.SmallLightAbyss") + "\n" +
                            CalamityUtils.GetTextValue("Misc.ModerateBreathLoss");
                    }
                }
            }
            if (item.type == ItemID.JellyfishNecklace)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.SmallLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.JellyfishDivingGear)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.SmallLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.FairyBell)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.ModerateLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.DD2PetGhost)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.ModerateLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.ShinePotion)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "BuffTime")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.ModerateLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.WispinaBottle)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.LargeLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.SuspiciousLookingTentacle)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.LargeLightAbyss");
                    }
                }
            }
            if (item.type == ItemID.GillsPotion)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "BuffTime")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.LargeBreathLoss");
                    }
                }
            }
            if (item.type == ItemID.DivingHelmet)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.ModerateBreathLoss");
                    }
                }
            }
            if (item.type == ItemID.NeptunesShell || item.type == ItemID.MoonShell)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.LargeBreathLoss");
                    }
                }
            }
            if (item.type == ItemID.CelestialShell)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.LargeBreathLoss");
                    }
                }
            }
            if (item.type == ItemID.WarmthPotion)
            {
				if (CalamityWorld.death)
				{
					foreach (TooltipLine line2 in tooltips)
					{
						if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
						{
							line2.Text += "\n" + CalamityUtils.GetTextValue("Vanilla.ItemTooltips.WarmthExtra") + "\n" +
								CalamityUtils.GetTextValue("Misc.ColdProtection");
						}
					}
				}
				else
				{
					foreach (TooltipLine line2 in tooltips)
					{
						if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
						{
							line2.Text += "\n" + CalamityUtils.GetTextValue("Vanilla.ItemTooltips.WarmthExtra");
						}
					}
				}
            }
            if (item.type == ItemID.FlaskofVenom)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.FlaskofVenom");
                    }
                }
            }
            if (item.type == ItemID.FlaskofCursedFlames)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.FlaskofCursedFlames");
                    }
                }
            }
            if (item.type == ItemID.FlaskofFire)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.FlaskofFire");
                    }
                }
            }
            if (item.type == ItemID.FlaskofGold)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.FlaskofGold");
                    }
                }
            }
            if (item.type == ItemID.FlaskofIchor)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.FlaskofIchor");
                    }
                }
            }
            if (item.type == ItemID.FlaskofNanites)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.FlaskofNanites");
                    }
                }
            }
            if (item.type == ItemID.FlaskofParty)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.BuffDescription.WeaponImbueConfetti");
                    }
                }
            }
            if (item.type == ItemID.FlaskofPoison)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.FlaskofPoison");
                    }
                }
            }
            if (item.type == ItemID.WormScarf)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = line2.Text.Replace("17", "10");
                    }
                }
            }
			if (item.type == ItemID.TitanGlove)
			{
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
					{
						line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.TenPercentTrueMelee");
					}
				}
			}
			if (item.type == ItemID.PowerGlove)
			{
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
					{
						line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.TenPercentTrueMelee");
					}
				}
			}
			if (item.type == ItemID.MechanicalGlove)
			{
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
					{
						line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.TenPercentTrueMelee");
					}
				}
			}
			if (item.type == ItemID.FireGauntlet)
            {
				if (CalamityWorld.death)
				{
					foreach (TooltipLine line2 in tooltips)
					{
						if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
						{
							line2.Text = line2.Text.Replace("12", "14") + "\n" + 
                                CalamityUtils.GetTextValue("Misc.TenPercentTrueMelee") + "\n" +
								CalamityUtils.GetTextValue("Misc.HeatColdProtection");
						}
					}
				}
				else
				{
					foreach (TooltipLine line2 in tooltips)
					{
						if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
						{
							line2.Text = line2.Text.Replace("12", "14") + "\n" + 
                                CalamityUtils.GetTextValue("Misc.TenPercentTrueMelee");
						}
					}
				}
            }
            if (item.type == ItemID.SpectreHood)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SpectreHood");
                    }
                }
            }
            if (item.type == ItemID.ObsidianSkinPotion)
            {
				string heatImmunity = CalamityWorld.death ? "\n" + CalamityUtils.GetTextValue("Misc.HeatProtection") : "";
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
					{
						line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.ObsidianSkin") + heatImmunity;
					}
				}
            }
            if (item.type == ItemID.ObsidianRose)
            {
				string heatImmunity = CalamityWorld.death ? "\n" + CalamityUtils.GetTextValue("Misc.HeatProtection") : "";
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
					{
						line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.ObsidianRose") + heatImmunity;
					}
				}
            }
            if (item.type == ItemID.MagmaStone && CalamityWorld.death)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.HeatColdProtection");
                    }
                }
            }
            if (item.type == ItemID.LavaCharm && CalamityWorld.death)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.HeatProtection");
                    }
                }
            }
            if (item.type == ItemID.LavaWaders && CalamityWorld.death)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.HeatProtection");
                    }
                }
            }
            if (item.type == ItemID.InvisibilityPotion)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text += "\n" + CalamityUtils.GetTextValue("Vanilla.ItemTooltips.InvisibilityPotionExtra");
                    }
                }
            }
            if (item.type == ItemID.GoldenFishingRod)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "NeedsBait")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GoldenFishingRod");
                    }
                }
            }
            if (item.type == ItemID.Picksaw)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
						// I'm leaving this intentionally ambiguous so that people have to search for Scoria Ore
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Picksaw");
                    }
                }
            }
            if (item.type == ItemID.SolarFlarePickaxe)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Material")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.MineUelibloom");
                    }
                }
            }
            if (item.type == ItemID.VortexPickaxe)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Material")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.MineUelibloom");
                    }
                }
            }
            if (item.type == ItemID.NebulaPickaxe)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Material")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.MineUelibloom");
                    }
                }
            }
            if (item.type == ItemID.StardustPickaxe)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Material")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.MineUelibloom");
                    }
                }
            }

			if (Main.player[Main.myPlayer].Calamity().trueMeleeDamage > 0D)
			{
				if (item.CountsAsClass(DamageClass.Melee) && item.damage > 0)
				{
					foreach (TooltipLine line2 in tooltips)
					{
						if (line2.Mod == "Terraria" && line2.Name == "Damage")
						{
							line2.Text += " : " + string.Concat((int)(Main.player[Main.myPlayer].Calamity().actualMeleeDamageStat * item.damage * 
								(1D + Main.player[Main.myPlayer].Calamity().trueMeleeDamage) + 5E-06f)) + CalamityUtils.GetTextValue("Misc.TrueMeleeDamage");
						}
					}
				}
			}

			if (item.type == ItemID.MeteorHelmet || item.type == ItemID.MeteorSuit || item.type == ItemID.MeteorLeggings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Meteor");
                    }
                }
            }
            if (item.type == ItemID.CopperHelmet || item.type == ItemID.CopperChainmail || item.type == ItemID.CopperGreaves)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Copper");
                    }
                }
            }
            if (item.type == ItemID.TinHelmet || item.type == ItemID.TinChainmail || item.type == ItemID.TinGreaves)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Tin");
                    }
                }
            }
            if (item.type == ItemID.AncientIronHelmet || item.type == ItemID.IronHelmet || item.type == ItemID.IronChainmail || item.type == ItemID.IronGreaves)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.AncientIron");
                    }
                }
            }
            if (item.type == ItemID.LeadHelmet || item.type == ItemID.LeadChainmail || item.type == ItemID.LeadGreaves)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Lead");
                    }
                }
            }
            if (item.type == ItemID.SilverHelmet || item.type == ItemID.SilverChainmail || item.type == ItemID.SilverGreaves)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Silver");
                    }
                }
            }
            if (item.type == ItemID.TungstenHelmet || item.type == ItemID.TungstenChainmail || item.type == ItemID.TungstenGreaves)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Tungsten");
                    }
                }
            }
            if (item.type == ItemID.AncientGoldHelmet || item.type == ItemID.GoldHelmet || item.type == ItemID.GoldChainmail || item.type == ItemID.GoldGreaves)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.AncientGold");
                    }
                }
            }
            if (item.type == ItemID.PlatinumHelmet || item.type == ItemID.PlatinumChainmail || item.type == ItemID.PlatinumGreaves)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Platinum");
                    }
                }
            }
			if (item.type == ItemID.AncientBattleArmorHat || item.type == ItemID.AncientBattleArmorShirt || item.type == ItemID.AncientBattleArmorPants)
            {
				if (!Main.player[Main.myPlayer].Calamity().forbiddenCirclet)
				{
					foreach (TooltipLine line2 in tooltips)
					{
						if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
						{
							line2.Text += "\n" + CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Forbidden");
						}
					}
				}
            }
            if (item.type == ItemID.GladiatorHelmet)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Defense")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GladiatorHelmet");
                    }
                }
            }
            if (item.type == ItemID.GladiatorBreastplate)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Defense")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GladiatorBreastplate");
                    }
                }
            }
            if (item.type == ItemID.GladiatorLeggings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Defense")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GladiatorLeggings");
                    }
                }
            }
            if (item.type == ItemID.ObsidianHelm)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Defense")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.ObsidianHelm");
                    }
                }
            }
            if (item.type == ItemID.ObsidianShirt)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Defense")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.ObsidianShirt");
                    }
                }
            }
            if (item.type == ItemID.ObsidianPants)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Defense")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.ObsidianPants");
                    }
                }
            }
			if (item.type == ItemID.MoltenHelmet || item.type == ItemID.MoltenBreastplate || item.type == ItemID.MoltenGreaves)
            {
				if (CalamityWorld.death)
				{
					foreach (TooltipLine line2 in tooltips)
					{
						if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
						{
							line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Molten") + "\n" + CalamityUtils.GetTextValue("Misc.HeatColdProtection");
						}
					}
                }
				else
				{
					foreach (TooltipLine line2 in tooltips)
					{
						if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
						{
							line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Molten");
						}
					}
                }
            }
            if (item.type == ItemID.FrostHelmet || item.type == ItemID.FrostBreastplate || item.type == ItemID.FrostLeggings)
            {
				if (CalamityWorld.death)
				{
					foreach (TooltipLine line2 in tooltips)
					{
						if (line2.Mod == "Terraria" && line2.Name == "SetBonus")
						{
							line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.HeatColdProtection");
						}
					}
				}
            }
            if (item.type == ItemID.MagicQuiver)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.MagicQuiver");
                    }
                }
            }
            if (item.type == ItemID.MiningHelmet)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.MiningHelmet");
                    }
                }
            }
            if (item.type == ItemID.HandWarmer)
            {
				string coldImmunity = CalamityWorld.death ? "\n" + CalamityUtils.GetTextValue("Misc.ColdProtection") : "";
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.HandWarmer") + coldImmunity;
                    }
                }
            }
            if (item.type == ItemID.AngelWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Angel");
                    }
                }
            }
            if (item.type == ItemID.DemonWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Demon");
                    }
                }
            }
            if (item.type == ItemID.Jetpack)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Jetpack");
                    }
                }
            }
            if (item.type == ItemID.ButterflyWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Butterfly");
                    }
                }
            }
            if (item.type == ItemID.FairyWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Fairy");
                    }
                }
            }
            if (item.type == ItemID.BeeWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Bee");
                    }
                }
            }
            if (item.type == ItemID.HarpyWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Harpy");
                    }
                }
            }
            if (item.type == ItemID.BoneWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Bone");
                    }
                }
            }
            if (item.type == ItemID.FlameWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Flame");
                    }
                }
            }
            if (item.type == ItemID.FrozenWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Frozen");
                    }
                }
            }
            if (item.type == ItemID.GhostWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Ghost");
                    }
                }
            }
            if (item.type == ItemID.BeetleWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Beetle");
                    }
                }
            }
            if (item.type == ItemID.FinWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Fin");
                    }
                }
            }
            if (item.type == ItemID.FishronWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Fishron");
                    }
                }
            }
            if (item.type == ItemID.SteampunkWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Steampunk");
                    }
                }
            }
            if (item.type == ItemID.LeafWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Leaf");
                    }
                }
            }
            if (item.type == ItemID.BatWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Bat");
                    }
                }
            }
            if (item.type == ItemID.Yoraiz0rWings || item.type == ItemID.JimsWings || item.type == ItemID.SkiphsWings ||
                item.type == ItemID.LokisWings || item.type == ItemID.ArkhalisWings || item.type == ItemID.LeinforsWings ||
                item.type == ItemID.BejeweledValkyrieWing || item.type == ItemID.RedsWings || item.type == ItemID.DTownsWings ||
                item.type == ItemID.WillsWings || item.type == ItemID.CrownosWings || item.type == ItemID.CenxsWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Dev");
                    }
                }
            }
            if (item.type == ItemID.TatteredFairyWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.TatteredFairy");
                    }
                }
            }
            if (item.type == ItemID.SpookyWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Spooky");
                    }
                }
            }
            if (item.type == ItemID.Hoverboard)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Hoverboard");
                    }
                }
            }
            if (item.type == ItemID.FestiveWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Festive");
                    }
                }
            }
            if (item.type == ItemID.MothronWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Mothron");
                    }
                }
            }
            if (item.type == ItemID.WingsSolar)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Solar");
                    }
                }
            }
            if (item.type == ItemID.WingsStardust)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Stardust");
                    }
                }
            }
            if (item.type == ItemID.WingsVortex)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Vortex");
                    }
                }
            }
            if (item.type == ItemID.WingsNebula)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Nebula");
                    }
                }
            }
            if (item.type == ItemID.BetsyWings)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.Wings.Betsy");
                    }
                }
            }
            if (item.type == ItemID.GrapplingHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Basic");
                    }
                }
            }
            if (item.type == ItemID.AmethystHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Amethyst");
                    }
                }
            }
            if (item.type == ItemID.TopazHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Topaz");
                    }
                }
            }
            if (item.type == ItemID.SapphireHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Sapphire");
                    }
                }
            }
            if (item.type == ItemID.EmeraldHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Emerald");
                    }
                }
            }
            if (item.type == ItemID.RubyHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Ruby");
                    }
                }
            }
            if (item.type == ItemID.DiamondHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Diamond");
                    }
                }
            }
            if (item.type == ItemID.WebSlinger)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.WebSlinger");
                    }
                }
            }
            if (item.type == ItemID.SkeletronHand)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.SkeletronHand");
                    }
                }
            }
            if (item.type == ItemID.SlimeHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Slime");
                    }
                }
            }
            if (item.type == ItemID.FishHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Fish");
                    }
                }
            }
            if (item.type == ItemID.IvyWhip)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.IvyWhip");
                    }
                }
            }
            if (item.type == ItemID.BatHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Bat");
                    }
                }
            }
            if (item.type == ItemID.CandyCaneHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.CandyCane");
                    }
                }
            }
            if (item.type == ItemID.DualHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Dual");
                    }
                }
            }
            if (item.type == ItemID.ThornHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Thorn");
                    }
                }
            }
            if (item.type == ItemID.WormHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Worm");
                    }
                }
            }
            if (item.type == ItemID.TendonHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Tendon");
                    }
                }
            }
            if (item.type == ItemID.IlluminantHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Illuminant");
                    }
                }
            }
            if (item.type == ItemID.AntiGravityHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.AntiGravity");
                    }
                }
            }
            if (item.type == ItemID.SpookyHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Spooky");
                    }
                }
            }
            if (item.type == ItemID.ChristmasHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Christmas");
                    }
                }
            }
            if (item.type == ItemID.LunarHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Lunar");
                    }
                }
            }
            if (item.type == ItemID.StaticHook)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.Mod == "Terraria" && line2.Name == "Equipable")
                    {
                        line2.Text = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.GrapplingHooks.Static");
                    }
                }
            }
            if (item.accessory)
            {
                if (item.prefix == PrefixID.Precise)
                {
                    foreach (TooltipLine line2 in tooltips)
                    {
                        if (line2.Mod == "Terraria" && line2.Name == "PrefixAccCritChance")
                        {
                            line2.Text = line2.Text.Replace("2", "1");
                        }
                    }
                }
                if (item.prefix == PrefixID.Lucky)
                {
                    foreach (TooltipLine line2 in tooltips)
                    {
                        if (line2.Mod == "Terraria" && line2.Name == "PrefixAccCritChance")
                        {
                            line2.Text = line2.Text.Replace("4", "3");
                        }
                    }
                }
                if (item.prefix == PrefixID.Hard)
                {
                    foreach (TooltipLine line2 in tooltips)
                    {
                        if (line2.Mod == "Terraria" && line2.Name == "PrefixAccDefense")
                        {
                            if (NPC.downedMoonlord)
                                line2.Text = line2.Text.Replace("1", "3");
                            else if (Main.hardMode)
                                line2.Text = line2.Text.Replace("1", "2");
                            line2.Text += "\n" + CalamityUtils.GetText("Vanilla.ItemTooltips.DR").Format(0.25f);
                        }
                    }
                }
                if (item.prefix == PrefixID.Guarding)
                {
					foreach (TooltipLine line2 in tooltips)
                    {
                        if (line2.Mod == "Terraria" && line2.Name == "PrefixAccDefense")
                        {
                            if (NPC.downedMoonlord)
                                line2.Text = line2.Text.Replace("2", "4");
                            else if (Main.hardMode)
                                line2.Text = line2.Text.Replace("2", "3");
                            line2.Text += "\n" + CalamityUtils.GetText("Vanilla.ItemTooltips.DR").Format(0.5f);
                        }
                    }
                }
                if (item.prefix == PrefixID.Armored)
                {
					foreach (TooltipLine line2 in tooltips)
                    {
                        if (line2.Mod == "Terraria" && line2.Name == "PrefixAccDefense")
                        {
                            if (NPC.downedMoonlord)
                                line2.Text = line2.Text.Replace("3", "6");
                            else if (Main.hardMode)
                                line2.Text = line2.Text.Replace("3", "4");
                            line2.Text += "\n" + CalamityUtils.GetText("Vanilla.ItemTooltips.DR").Format(0.75f);
                        }
                    }
                }
                if (item.prefix == PrefixID.Warding)
                {
					foreach (TooltipLine line2 in tooltips)
                    {
                        if (line2.Mod == "Terraria" && line2.Name == "PrefixAccDefense")
                        {
                            if (NPC.downedMoonlord)
                                line2.Text = line2.Text.Replace("4", "8");
                            else if (Main.hardMode)
                                line2.Text = line2.Text.Replace("4", "6");
                            line2.Text += "\n" + CalamityUtils.GetText("Vanilla.ItemTooltips.DR").Format(1f);
                        }
                    }
                }
            }

            ModLoader.TryGetMod("Fargowiltas", out Mod fargos);
			if (fargos != null)
			{
				//Fargo's fountain effects
				if (item.type == ModContent.ItemType<SunkenSeaFountain>())
				{
					TooltipLine line = new TooltipLine(Mod, "Tooltip0", CalamityUtils.GetTextValue("Misc.Fargos.SunkenSeaFountain"));
					tooltips.Add(line);
				}
				if (item.type == ModContent.ItemType<SulphurousFountainItem>())
				{
					TooltipLine line = new TooltipLine(Mod, "Tooltip0", CalamityUtils.GetTextValue("Misc.Fargos.SulphurousFountain"));
					tooltips.Add(line);
				}
				if (item.type == ModContent.ItemType<AstralFountainItem>())
				{
					TooltipLine line = new TooltipLine(Mod, "Tooltip0", CalamityUtils.GetTextValue("Misc.Fargos.AstralFountain"));
					tooltips.Add(line);
				}
            }

            if (item.type < ItemID.Count)
                return;

            CalamityGlobalItem modItem = item.Calamity();
            if (modItem?.UsesCharge ?? false)
            {
                // Convert current charge ratio into a percentage.
                float displayedPercent = ChargeRatio * 100f;
                TooltipLine line = new TooltipLine(Mod, "Tooltip0", CalamityUtils.GetText("Misc.Charge").Format(displayedPercent.ToString("N1")));
                tooltips.Add(line);
            }
        }
		#endregion

        #region Boss Bag Changes
		public override void ModifyItemLoot(Item item, ItemLoot loot)
        {
            // Give a chance for Laudanum, Stress Pills and Heart of Darkness from every boss bag
            
            if (ItemID.Sets.BossBag[item.type])
                loot.AddRevBagAccessories();
            
			switch (item.type)
				{
					case ItemID.WoodenCrate:
                        loot.Add(new CommonDrop(ModContent.ItemType<WulfrumShard>(), 4, 3, 5));
                        break;

					case ItemID.IronCrate:
                        loot.Add(new CommonDrop(ModContent.ItemType<WulfrumShard>(), 4, 5, 8));
                        loot.Add(new CommonDrop(ModContent.ItemType<AncientBoneDust>(), 4, 5, 8));
                        break;

					case ItemID.CorruptFishingCrate:
                        loot.Add(new CommonDrop(ModContent.ItemType<EbonianGel>(), 4, 5, 8));
                        loot.Add(new CommonDrop(ModContent.ItemType<MurkySludge>(), 5, 1, 3));
                        break;

					case ItemID.CrimsonFishingCrate:
                        loot.Add(new CommonDrop(ModContent.ItemType<EbonianGel>(), 4, 5, 8));
                        loot.Add(new CommonDrop(ModContent.ItemType<MurkySludge>(), 5, 1, 3));
                        break;

					case ItemID.HallowedFishingCrate:
                        loot.AddIf(() => NPC.downedMoonlord, ModContent.ItemType<UnholyEssence>(),  5, 5, 10);
                        loot.AddIf(() => CalamityWorld.downedProvidence, (WorldGen.crimson ? ModContent.ItemType<ProfanedRagePotion>() : ModContent.ItemType<HolyWrathPotion>()), 5, 1, 3);
                        break;

					case ItemID.DungeonFishingCrate:
                        loot.AddIf(() => NPC.downedPlantBoss, ItemID.Ectoplasm, 3, 5, 10);
                        loot.AddIf(() => NPC.downedMoonlord, ModContent.ItemType<Phantoplasm>(), 5, 5, 10);
                        break;

					case ItemID.JungleFishingCrate:
                        loot.Add(new CommonDrop(ModContent.ItemType<MurkyPaste>(), 5, 1, 3));
                        loot.AddIf(() => Main.hardMode, ModContent.ItemType<BeetleJuice>(), 5, 1, 3);
                        loot.AddIf(() => Main.hardMode, ModContent.ItemType<TrapperBulb>(), 5, 1, 3);
                        loot.AddIf(() => (CalamityWorld.downedCalamitas || NPC.downedPlantBoss), ItemID.ChlorophyteBar, 4, 5, 10);
                        loot.AddIf(() => NPC.downedPlantBoss, ModContent.ItemType<DraedonBar>(), 4, 5, 10);
                        loot.AddIf(() => NPC.downedGolemBoss, ModContent.ItemType<PlagueCellCluster>(), 5, 3, 6);
                        loot.AddIf(() => CalamityWorld.downedProvidence, ModContent.ItemType<UeliaceBar>(), 4, 5, 10);
                        break;

					case ItemID.FloatingIslandFishingCrate:
                        loot.AddIf(() => (CalamityWorld.downedHiveMind || CalamityWorld.downedPerforator), ModContent.ItemType<AerialiteBar>(), 4, 5, 10);
                        loot.AddIf(() => Main.hardMode, ModContent.ItemType<EssenceofCinder>(), 5, 5, 15);
                        loot.AddIf(() => NPC.downedMoonlord, ModContent.ItemType<GalacticaSingularity>(), 10, 1, 3);
                        break;
                    
                    // King Slime
                    case ItemID.KingSlimeBossBag:
                        var ksRev = loot.DefineConditionalDropSet(DropHelper.If(() => CalamityWorld.revenge));
                        ksRev.Add(ModContent.ItemType<CrownJewel>());
                        break;

                    // Eye of Cthulhu
                    case ItemID.EyeOfCthulhuBossBag:
                        var EoCRev = loot.DefineConditionalDropSet(DropHelper.If(() => CalamityWorld.revenge));
                        loot.Add(new CommonDrop(ModContent.ItemType<VictoryShard>(), 1, 3, 5));
                        loot.Add(new CommonDrop(ModContent.ItemType<TeardropCleaver>(), 3));
                        EoCRev.Add(ModContent.ItemType<CounterScarf>());
                        break;

                    // Queen Bee
                    case ItemID.QueenBeeBossBag:
                        loot.Add(new CommonDrop(ItemID.Stinger, 1, 8, 12));
                        loot.Add(new CommonDrop(ModContent.ItemType<HardenedHoneycomb>(), 1, 50, 75));
                        break;

                    // Skeletron
                    case ItemID.SkeletronBossBag:
                        loot.Add(new CommonDrop(ModContent.ItemType<ClothiersWrath>(), DropHelper.RareVariantDropRateInt));
                        break;

                    // Wall of Flesh
                    case ItemID.WallOfFleshBossBag:
                        var WoFDT = loot.DefineConditionalDropSet(DropHelper.If(() => !CalamityWorld.demonMode));
                        loot.Add(new CommonDrop(ModContent.ItemType<Meowthrower>(), 3));
                        loot.Add(new CommonDrop(ModContent.ItemType<BlackHawkRemote>(), 3));
                        loot.Add(new CommonDrop(ModContent.ItemType<BlastBarrel>(), 3));
                        loot.Add(new CommonDrop(ModContent.ItemType<RogueEmblem>(), 4));
                        loot.Add(ItemDropRule.Common(ItemID.CorruptionKey, 5));
                        loot.Add(ItemDropRule.Common(ItemID.CrimsonKey, 5));
                        WoFDT.Add(ModContent.ItemType<MLGRune>()); // Demon Trophy
                        break;

                    // Destroyer
                    case ItemID.DestroyerBossBag:
                        var destroRev = loot.DefineConditionalDropSet(DropHelper.If(() => CalamityWorld.revenge));
                        int shpcChance = DropHelper.LegendaryDropRateInt;
                        destroRev.Add(ModContent.ItemType<SHPC>(), shpcChance);
                        break;

                    // Plantera
                    case ItemID.PlanteraBossBag:
                        var plantRev = loot.DefineConditionalDropSet(DropHelper.If(() => CalamityWorld.revenge));
                        loot.Add(new CommonDrop(ModContent.ItemType<LivingShard>(), 1, 16, 22));
                        int bFluxChance = DropHelper.LegendaryDropRateInt;
                        plantRev.Add(ModContent.ItemType<BlossomFlux>(), bFluxChance);
                        loot.Add(new CommonDrop(ItemID.JungleKey, 5));
                        break;

                    // Golem
                    case ItemID.GolemBossBag:
                        var golemRev = loot.DefineConditionalDropSet(DropHelper.If(() => CalamityWorld.revenge));
                        int aegisChance = DropHelper.LegendaryDropRateInt;
                        golemRev.Add(ModContent.ItemType<AegisBlade>(), aegisChance);
                        loot.Add(new CommonDrop(ModContent.ItemType<EssenceofCinder>(), 1, 8, 13));
						loot.Add(new CommonDrop(ModContent.ItemType<LeadWizard>(), DropHelper.RareVariantDropRateInt));
                        break;

                    // Duke Fishron
                    case ItemID.FishronBossBag:
                        var fishronRev = loot.DefineConditionalDropSet(DropHelper.If(() => CalamityWorld.revenge));
                        int baronChance = DropHelper.LegendaryDropRateInt;
                        fishronRev.Add(ModContent.ItemType<BrinyBaron>(), baronChance);
                        loot.Add(new CommonDrop(ModContent.ItemType<DukesDecapitator>(), 4));
                        break;

                    // Betsy
                    case ItemID.BossBagBetsy:
                        var betsyRev = loot.DefineConditionalDropSet(DropHelper.If(() => CalamityWorld.revenge));
                        int vesuviusChance = DropHelper.LegendaryDropRateInt;
                        betsyRev.Add(ModContent.ItemType<Vesuvius>(), vesuviusChance);
                        break;

                    // Moon Lord
                    case ItemID.MoonLordBossBag:
                        loot.Add(new CommonDrop(ItemID.LunarOre, 1, 50, 50));
                        loot.Add(new CommonDrop(ModContent.ItemType<MLGRune2>(), 1)); // Celestial Onion
                        loot.Add(new CommonDrop(ModContent.ItemType<UtensilPoker>(), 8));
                        loot.Add(new CommonDrop(ModContent.ItemType<GrandDad>(), DropHelper.RareVariantDropRateInt));
                        loot.Add(new CommonDrop(ModContent.ItemType<Infinity>(), DropHelper.RareVariantDropRateInt));
                        break;
                }
        }
        #endregion
        #region Armor Set Changes
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.CopperHelmet && body.type == ItemID.CopperChainmail && legs.type == ItemID.CopperGreaves)
                return "Copper";
            if (head.type == ItemID.TinHelmet && body.type == ItemID.TinChainmail && legs.type == ItemID.TinGreaves)
                return "Tin";
            if ((head.type == ItemID.IronHelmet || head.type == ItemID.AncientIronHelmet) && body.type == ItemID.IronChainmail && legs.type == ItemID.IronGreaves)
                return "Iron";
            if (head.type == ItemID.LeadHelmet && body.type == ItemID.LeadChainmail && legs.type == ItemID.LeadGreaves)
                return "Lead";
            if (head.type == ItemID.SilverHelmet && body.type == ItemID.SilverChainmail && legs.type == ItemID.SilverGreaves)
                return "Silver";
            if (head.type == ItemID.TungstenHelmet && body.type == ItemID.TungstenChainmail && legs.type == ItemID.TungstenGreaves)
                return "Tungsten";
            if ((head.type == ItemID.GoldHelmet || head.type == ItemID.AncientGoldHelmet) && body.type == ItemID.GoldChainmail && legs.type == ItemID.GoldGreaves)
                return "Gold";
            if (head.type == ItemID.PlatinumHelmet && body.type == ItemID.PlatinumChainmail && legs.type == ItemID.PlatinumGreaves)
                return "Platinum";
            if (head.type == ItemID.GladiatorHelmet && body.type == ItemID.GladiatorBreastplate && legs.type == ItemID.GladiatorLeggings)
                return "Gladiator";
            if (head.type == ItemID.ObsidianHelm && body.type == ItemID.ObsidianShirt && legs.type == ItemID.ObsidianPants)
                return "Obsidian";
            if (head.type == ItemID.MoltenHelmet && body.type == ItemID.MoltenBreastplate && legs.type == ItemID.MoltenGreaves)
                return "Molten";
			// Normal and Pink Eskimo set can be mixed and matched
            if ((head.type == ItemID.EskimoHood || head.type == ItemID.PinkEskimoHood) && (body.type == ItemID.EskimoCoat || body.type == ItemID.PinkEskimoCoat) && (legs.type == ItemID.EskimoPants || legs.type == ItemID.PinkEskimoPants))
                return "Eskimo";
			if (head.type == ItemID.MeteorHelmet && body.type == ItemID.MeteorSuit && legs.type == ItemID.MeteorLeggings)
				return "Meteor";
			if (head.type == ItemID.PearlwoodHelmet && body.type == ItemID.PearlwoodBreastplate && legs.type == ItemID.PearlwoodGreaves)
				return "Pearlwood";
            return "";
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (set == "Copper")
                player.pickSpeed -= 0.15f;
            else if (set == "Tin")
                player.pickSpeed -= 0.1f;
            else if (set == "Iron")
                player.pickSpeed -= 0.25f;
            else if (set == "Lead")
                player.pickSpeed -= 0.2f;
            else if (set == "Silver")
                player.pickSpeed -= 0.35f;
            else if (set == "Tungsten")
                player.pickSpeed -= 0.3f;
            else if (set == "Gold")
                player.pickSpeed -= 0.45f;
            else if (set == "Platinum")
                player.pickSpeed -= 0.4f;
            else if (set == "Gladiator")
            {
                modPlayer.rogueStealthMax += 0.7f;
                modPlayer.wearingRogueArmor = true;
                player.Calamity().throwingDamage += 0.05f;
                player.Calamity().throwingVelocity += 0.1f;
                player.statDefense += 3;
                player.setBonus = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Gladiator");
            }
            else if (set == "Obsidian")
            {
                modPlayer.rogueStealthMax += 0.8f;
                modPlayer.wearingRogueArmor = true;
                player.Calamity().throwingDamage += 0.05f;
                player.Calamity().throwingCrit += 5;
                player.statDefense += 2;
                player.fireWalk = true;
                player.lavaMax += 180;
				string heatImmunity = CalamityWorld.death ? "\n" + CalamityUtils.GetTextValue("Misc.HeatProtection") : "";
				player.setBonus = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Obsidian") + heatImmunity;
            }
            else if (set == "Molten")
            {
                player.fireWalk = true;
                player.lavaMax += 300;
            }
            else if (set == "Eskimo")
            {
				modPlayer.eskimoSet = true;
				player.buffImmune[BuffID.Frostburn] = true;
				player.buffImmune[ModContent.BuffType<GlacialState>()] = true;
				string coldImmunity = CalamityWorld.death ? "\n" + CalamityUtils.GetTextValue("Misc.ColdProtection") : "";
				player.setBonus = CalamityUtils.GetTextValue("Vanilla.ItemTooltips.SetBonus.Eskimo") + coldImmunity;
            }
            else if (set == "Meteor")
            {
                player.spaceGun = false;
                modPlayer.meteorSet = true;
            }
        }
        #endregion

        #region Equip Changes
        public override void UpdateEquip(Item item, Player player)
        {
            #region Head
            if (item.type == ItemID.SpectreHood)
                player.GetDamage(DamageClass.Magic) += 0.2f;
            else if (item.type == ItemID.GladiatorHelmet || item.type == ItemID.ObsidianHelm)
                player.Calamity().throwingDamage += 0.03f;
            #endregion

            #region Body
            if (item.type == ItemID.GladiatorBreastplate || item.type == ItemID.ObsidianShirt)
                player.Calamity().throwingCrit += 3;
            #endregion

            #region Legs
            if (item.type == ItemID.GladiatorLeggings || item.type == ItemID.ObsidianPants)
                player.Calamity().throwingVelocity += 0.03f;
            #endregion
        }
        #endregion

        #region Accessory Changes
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();

			if (item.prefix > 0)
			{
				float stealthGenBoost = item.Calamity().StealthGenBonus - 1f;
				if (stealthGenBoost > 0)
				{
					modPlayer.accStealthGenBoost += stealthGenBoost;
				}
			}

            if (item.type == ItemID.FireGauntlet)
            {
                player.GetDamage(DamageClass.Melee) += 0.02f;
                player.GetAttackSpeed(DamageClass.Melee) += 0.02f;
            }

            if (item.type == ItemID.AngelWings) // Boost to max life, defense, and life regen
            {
                player.statLifeMax2 += 20;
                player.statDefense += 15;
                player.lifeRegen += 3;
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.DemonWings) // Boost to all damage and crit
            {
                player.GetDamage(DamageClass.Generic) += 0.1f;
                modPlayer.AllCritBoost(10);
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.FinWings) // Boosted water abilities, faster fall in water
            {
                player.moveSpeed += 0.2f;
                player.jumpSpeedBoost += 1.8f;
                player.gills = true;
                player.ignoreWater = true;
                player.noFallDmg = true;
                if (!player.mount.Active)
                {
                    if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
                        player.maxFallSpeed = 12f;
                }
            }
            else if (item.type == ItemID.BeeWings) // Honey buff
            {
                player.AddBuff(BuffID.Honey, 2);
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.ButterflyWings) // Boost to magic stats
            {
                player.statManaMax2 += 50;
                player.GetDamage(DamageClass.Magic) += 0.1f;
                player.manaCost *= 0.95f;
                player.GetCritChance(DamageClass.Magic) += 5;
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.FairyWings) // Boost to max life
            {
                player.statLifeMax2 += 80;
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.BatWings) // Stronger at night
            {
                player.noFallDmg = true;
                if (!Main.dayTime || Main.eclipse)
                {
					player.jumpSpeedBoost += 1.0f;
                    player.statDefense += 15;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                    modPlayer.AllCritBoost(5);
                    player.moveSpeed += 0.1f;
                }
            }
            else if (item.type == ItemID.HarpyWings)
            {
				modPlayer.harpyWingBoost = true;
                player.moveSpeed += 0.3f;
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.BoneWings) // Bonus to ranged and defense stats while wearing necro armor
            {
                player.noFallDmg = true;
                if ((player.head == ArmorIDs.Head.NecroHelmet || player.head == ArmorIDs.Head.AncientNecroHelmet) &&
                    player.body == ArmorIDs.Body.NecroBreastplate && player.legs == ArmorIDs.Legs.NecroGreaves)
                {
                    player.moveSpeed += 0.15f;
                    player.GetDamage(DamageClass.Ranged) += 0.12f;
                    player.GetCritChance(DamageClass.Ranged) += 16;
                    player.statDefense += 30;
                }
            }
            else if (item.type == ItemID.MothronWings) // Spawn baby mothrons over time to attack enemies, max of 3
            {
                player.statDefense += 5;
                player.GetDamage(DamageClass.Generic) += 0.05f;
                player.moveSpeed += 0.1f;
                player.jumpSpeedBoost += 1.2f;
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.FrozenWings) // Bonus to melee and ranged stats while wearing frost armor
            {
                player.noFallDmg = true;
                if (player.head == ArmorIDs.Head.FrostHelmet && player.body == ArmorIDs.Body.FrostBreastplate && player.legs == ArmorIDs.Legs.FrostLeggings)
                {
                    player.GetDamage(DamageClass.Melee) += 0.02f;
                    player.GetDamage(DamageClass.Ranged) += 0.02f;
                    player.GetCritChance(DamageClass.Melee) += 1;
                    player.GetCritChance(DamageClass.Ranged) += 1;
                }
            }
            else if (item.type == ItemID.FlameWings) // Bonus to melee stats
            {
                player.GetDamage(DamageClass.Melee) += 0.1f;
                player.GetCritChance(DamageClass.Melee) += 5;
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.GhostWings) // Bonus to mage stats while wearing spectre armor
            {
                player.noFallDmg = true;
                if (player.body == ArmorIDs.Body.SpectreRobe && player.legs == ArmorIDs.Legs.SpectrePants)
                {
                    if (player.head == ArmorIDs.Head.SpectreHood)
                    {
                        player.statDefense += 10;
                        player.endurance += 0.05f;
                    }
                    else if (player.head == ArmorIDs.Head.SpectreMask)
                    {
                        player.statManaMax2 += 20;
                        player.GetDamage(DamageClass.Magic) += 0.05f;
                        player.manaCost *= 0.95f;
                        player.GetCritChance(DamageClass.Magic) += 5;
                    }
                }
            }
            else if (item.type == ItemID.BeetleWings) // Boosted defense and melee stats while wearing beetle armor
            {
                player.noFallDmg = true;
                if (player.head == ArmorIDs.Head.BeetleHelmet && player.legs == ArmorIDs.Legs.BeetleLeggings)
                {
                    if (player.body == ArmorIDs.Body.BeetleShell)
                    {
                        player.statDefense += 15;
                        player.endurance += 0.1f;
                    }
                    else if (player.body == ArmorIDs.Body.BeetleScaleMail)
                    {
                        player.GetDamage(DamageClass.Melee) += 0.1f;
                        player.GetCritChance(DamageClass.Melee) += 10;
                    }
                }
            }
            else if (item.type == ItemID.Hoverboard) // Boosted ranged stats while wearing shroomite armor
            {
                player.noFallDmg = true;
                if (player.body == ArmorIDs.Body.ShroomiteBreastplate && player.legs == ArmorIDs.Legs.ShroomiteLeggings)
                {
                    if (player.head == ArmorIDs.Head.ShroomiteHeadgear) //arrows
                    {
                        player.arrowDamage += 0.05f;
                    }
                    else if (player.head == ArmorIDs.Head.ShroomiteMask) //bullets
                    {
                        player.bulletDamage += 0.05f;
                    }
                    else if (player.head == ArmorIDs.Head.ShroomiteHelmet) //rockets
                    {
                        player.specialistDamage += 0.05f;
                    }
					else if (player.Calamity().flamethrowerBoost) //flamethrowers
					{
						player.Calamity().hoverboardBoost = true;
					}
                }
            }
            else if (item.type == ItemID.LeafWings) // Bonus to defensive stats while wearing tiki armor
            {
                player.noFallDmg = true;
                if (player.head == ArmorIDs.Head.TikiMask && player.body == ArmorIDs.Body.TikiShirt && player.legs == ArmorIDs.Legs.TikiPants)
                {
					player.statDefense += 10;
                    player.endurance += 0.1f;
					player.AddBuff(BuffID.DryadsWard, 5, true); // Dryad's Blessing
                }
            }
            else if (item.type == ItemID.FestiveWings) // Drop powerful homing christmas tree bulbs while in flight
            {
                player.noFallDmg = true;
                player.statLifeMax2 += 50;
				if (modPlayer.icicleCooldown <= 0)
				{
					if (player.controlJump &&  !VanillaExtraJump.CloudInABottle.CanStart(player) && player.jump == 0 && player.velocity.Y != 0f && !player.mount.Active && !player.mount.Cart)
					{
						int p = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y, player.velocity.X * 0f, 2f, ProjectileID.OrnamentFriendly, (int)(100 * player.AverageDamage()), 5f, player.whoAmI);
						Main.projectile[p].Calamity().forceTypeless = true;
						Main.projectile[p].Calamity().lineColor = 1;
						modPlayer.icicleCooldown = 10;
					}
				}
            }
            else if (item.type == ItemID.SpookyWings) // Bonus to summon stats while wearing spooky armor
            {
                player.noFallDmg = true;
                if (player.head == ArmorIDs.Head.SpookyHelmet && player.body == ArmorIDs.Body.SpookyBreastplate && player.legs == ArmorIDs.Legs.SpookyLeggings)
                {
                    player.GetKnockback(DamageClass.Summon) += 2f;
                    player.GetDamage(DamageClass.Summon) += 0.05f;
                }
            }
            else if (item.type == ItemID.TatteredFairyWings)
            {
                player.GetDamage(DamageClass.Generic) += 0.05f;
                modPlayer.AllCritBoost(5);
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.SteampunkWings)
            {
                player.statDefense += 8;
                player.GetDamage(DamageClass.Generic) += 0.04f;
                modPlayer.AllCritBoost(2);
                player.moveSpeed += 0.1f;
                player.noFallDmg = true;
            }
            else if (item.type == ItemID.WingsSolar) // Bonus to melee stats while wearing solar flare armor
            {
                player.noFallDmg = true;
                if (player.head == ArmorIDs.Head.SolarFlareHelmet && player.body == ArmorIDs.Body.SolarFlareBreastplate && player.legs == ArmorIDs.Legs.SolarFlareLeggings)
                {
                    player.GetDamage(DamageClass.Melee) += 0.07f;
                    player.GetCritChance(DamageClass.Melee) += 3;
                }
            }
            else if (item.type == ItemID.WingsVortex) // Bonus to ranged stats while wearing vortex armor
            {
                player.noFallDmg = true;
                if (player.head == ArmorIDs.Head.VortexHelmet && player.body == ArmorIDs.Body.VortexBreastplate && player.legs == ArmorIDs.Legs.VortexLeggings)
                {
                    player.GetDamage(DamageClass.Ranged) += 0.03f;
                    player.GetCritChance(DamageClass.Ranged) += 7;
                }
            }
            else if (item.type == ItemID.WingsNebula) // Bonus to magic stats while wearing nebula armor
            {
                player.noFallDmg = true;
                if (player.head == ArmorIDs.Head.NebulaHelmet && player.body == ArmorIDs.Body.NebulaBreastplate && player.legs == ArmorIDs.Legs.NebulaLeggings)
                {
                    player.GetDamage(DamageClass.Magic) += 0.05f;
                    player.GetCritChance(DamageClass.Magic) += 5;
                    player.statManaMax2 += 20;
                    player.manaCost *= 0.95f;
                }
            }
            else if (item.type == ItemID.WingsStardust) // Bonus to summon stats while wearing stardust armor
            {
                player.noFallDmg = true;
                if (player.head == ArmorIDs.Head.StardustHelmet && player.body == ArmorIDs.Body.StardustPlate && player.legs == ArmorIDs.Legs.StardustLeggings)
                {
                    player.maxMinions++;
                    player.GetDamage(DamageClass.Summon) += 0.05f;
                }
            }
            else if (item.type == ItemID.FishronWings || item.type == ItemID.BetsyWings || item.type == ItemID.Yoraiz0rWings ||
				item.type == ItemID.JimsWings || item.type == ItemID.SkiphsWings || item.type == ItemID.LokisWings ||
				item.type == ItemID.ArkhalisWings || item.type == ItemID.LeinforsWings || item.type == ItemID.BejeweledValkyrieWing ||
				item.type == ItemID.RedsWings || item.type == ItemID.DTownsWings || item.type == ItemID.WillsWings ||
				item.type == ItemID.CrownosWings || item.type == ItemID.CenxsWings)
            {
                player.noFallDmg = true;
            }

            if (item.type == ItemID.JellyfishNecklace || item.type == ItemID.JellyfishDivingGear || item.type == ItemID.ArcticDivingGear)
                modPlayer.jellyfishNecklace = true;

            if (item.type == ItemID.WormScarf)
                player.endurance -= 0.07f;

            if (item.type == ItemID.RoyalGel)
                modPlayer.royalGel = true;

            if (item.type == ItemID.HandWarmer)
                modPlayer.handWarmer = true;

			// Hard / Guarding / Armored / Warding give 0.25% / 0.5% / 0.75% / 1% DR
			if (item.prefix == PrefixID.Hard)
			{
				if (NPC.downedMoonlord)
					player.statDefense += 2;
				else if (Main.hardMode)
					player.statDefense += 1;

				player.endurance += 0.0025f;
			}
			if (item.prefix == PrefixID.Guarding)
			{
				if (NPC.downedMoonlord)
					player.statDefense += 2;
				else if (Main.hardMode)
					player.statDefense += 1;

				player.endurance += 0.005f;
			}
			if (item.prefix == PrefixID.Armored)
			{
				if (NPC.downedMoonlord)
					player.statDefense += 3;
				else if (Main.hardMode)
					player.statDefense += 1;

				player.endurance += 0.0075f;
			}
			if (item.prefix == PrefixID.Warding)
			{
				if (NPC.downedMoonlord)
					player.statDefense += 4;
				else if (Main.hardMode)
					player.statDefense += 2;

				player.endurance += 0.01f;
			}

            // Precise only gives 1% crit and Lucky only gives 3% crit
            if ((item.prefix == PrefixID.Precise) || (item.prefix == PrefixID.Lucky))
            {
                player.GetCritChance(DamageClass.Melee) -= 1;
                player.GetCritChance(DamageClass.Ranged) -= 1;
                player.GetCritChance(DamageClass.Magic) -= 1;
                player.GetCritChance(DamageClass.Throwing) -= 1;
            }
        }
        #endregion

        #region WingChanges
        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            CalamityPlayer modPlayer = player.Calamity();
            float flightSpeedMult = 1f +
                (modPlayer.soaring ? 0.1f : 0f) +
                (modPlayer.holyWrath ? 0.05f : 0f) +
                (modPlayer.profanedRage ? 0.05f : 0f) +
                (modPlayer.draconicSurge ? 0.15f : 0f) +
                (modPlayer.etherealExtorter && modPlayer.ZoneAstral ? 0.05f : 0f);
            if (flightSpeedMult > 1.2f)
                flightSpeedMult = 1.2f;

            speed *= flightSpeedMult;

            float flightAccMult = 1f +
                (modPlayer.draconicSurge ? 0.15f : 0f);
            if (flightAccMult > 1.2f)
                flightAccMult = 1.2f;

            acceleration *= flightAccMult;
        }
        #endregion

        #region GrabChanges
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            CalamityPlayer modPlayer = player.Calamity();
            int itemGrabRangeBoost = 0 +
                (modPlayer.wallOfFleshLore ? 10 : 0) +
                (modPlayer.planteraLore ? 20 : 0) +
                (modPlayer.polterghastLore ? 30 : 0);

            grabRange += itemGrabRangeBoost;
        }
        #endregion

        #region The Horseman's Blade
        public static void HorsemansBladeOnHit(Player player, int targetIdx, int damage, float knockback, bool hasExtraUpdates)
        {
            int x = Main.rand.Next(100, 300);
            int y = Main.rand.Next(100, 300);

            // Pick a random side: left or right
            if (Main.rand.NextBool(2))
                x -= Main.LogicCheckScreenWidth / 2 + x;
            else
                x += Main.LogicCheckScreenWidth / 2 - x;

            // Pick a random side: top or bottom
            if (Main.rand.NextBool(2))
                y -= Main.LogicCheckScreenHeight / 2 + y;
            else
                y += Main.LogicCheckScreenHeight / 2 - y;

            x += (int)player.position.X;
            y += (int)player.position.Y;
            float speed = 8f;
            Vector2 vector = new Vector2((float)x, (float)y);
            float dx = Main.npc[targetIdx].position.X - vector.X;
            float dy = Main.npc[targetIdx].position.Y - vector.Y;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);
            dist = speed / dist;
            dx *= dist;
            dy *= dist;
            int projectile = Projectile.NewProjectile(player.GetSource_FromThis(), x, y, dx, dy, ProjectileID.FlamingJack, damage, knockback, player.whoAmI, targetIdx, 0f);
            if (hasExtraUpdates)
                Main.projectile[projectile].extraUpdates += 1;
        }
        #endregion

        #region Consume Additional Ammo
        public static bool HasEnoughAmmo(Player player, Item item, int ammoConsumed)
        {
            bool flag = false;
            bool canShoot = false;

            for (int i = 54; i < Main.InventorySlotsTotal; i++)
            {
                if (player.inventory[i].ammo == item.useAmmo && (player.inventory[i].stack >= ammoConsumed || !player.inventory[i].consumable))
                {
                    canShoot = true;
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                for (int j = 0; j < 54; j++)
                {
                    if (player.inventory[j].ammo == item.useAmmo && (player.inventory[j].stack >= ammoConsumed || !player.inventory[j].consumable))
                    {
                        canShoot = true;
                        break;
                    }
                }
            }
            return canShoot;
        }

        public static void ConsumeAdditionalAmmo(Player player, Item item, int ammoConsumed)
        {
            Item itemAmmo = new Item();
            bool flag = false;
            bool dontConsumeAmmo = false;

            for (int i = 54; i < Main.InventorySlotsTotal; i++)
            {
                if (player.inventory[i].ammo == item.useAmmo && (player.inventory[i].stack >= ammoConsumed || !player.inventory[i].consumable))
                {
                    itemAmmo = player.inventory[i];
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                for (int j = 0; j < 54; j++)
                {
                    if (player.inventory[j].ammo == item.useAmmo && (player.inventory[j].stack >= ammoConsumed || !player.inventory[j].consumable))
                    {
                        itemAmmo = player.inventory[j];
                        break;
                    }
                }
            }

            if (player.ammoBox && Main.rand.NextBool(5))
                dontConsumeAmmo = true;
            if (player.ammoPotion && Main.rand.NextBool(5))
                dontConsumeAmmo = true;
            if (player.ammoCost80 && Main.rand.NextBool(5))
                dontConsumeAmmo = true;
            if (player.ammoCost75 && Main.rand.NextBool(4))
                dontConsumeAmmo = true;

            if (!dontConsumeAmmo && itemAmmo.consumable)
            {
                itemAmmo.stack -= ammoConsumed;
                if (itemAmmo.stack <= 0)
                {
                    itemAmmo.active = false;
                    itemAmmo.TurnToAir();
                }
            }
        }
        #endregion

		#region PostUpdate
		public override void PostUpdate(Item item)
		{
			if (CalamityLists.forceItemList?.Contains(item.type) ?? false)
			{
				CalamityUtils.ForceItemIntoWorld(item);
			}
		}
		#endregion

        #region Goblin Money Theft (PostReforge)
        public override void PostReforge(Item item)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<THIEF>()))
            {
                int value = item.value;
                ItemLoader.ReforgePrice(item, ref value, ref Main.LocalPlayer.discountAvailable);
                CalamityWorld.MoneyStolenByBandit += value / 5;
                CalamityWorld.Reforges++;
            }
        }
        #endregion

        #region Money From Rarity
        public static readonly int Rarity0BuyPrice = Item.buyPrice(0, 0, 50, 0);
        public static readonly int Rarity1BuyPrice = Item.buyPrice(0, 1, 0, 0);
        public static readonly int Rarity2BuyPrice = Item.buyPrice(0, 2, 0, 0);
        public static readonly int Rarity3BuyPrice = Item.buyPrice(0, 4, 0, 0);
        public static readonly int Rarity4BuyPrice = Item.buyPrice(0, 12, 0, 0);
        public static readonly int Rarity5BuyPrice = Item.buyPrice(0, 36, 0, 0);
        public static readonly int Rarity6BuyPrice = Item.buyPrice(0, 48, 0, 0);
        public static readonly int Rarity7BuyPrice = Item.buyPrice(0, 60, 0, 0);
        public static readonly int Rarity8BuyPrice = Item.buyPrice(0, 80, 0, 0);
        public static readonly int Rarity9BuyPrice = Item.buyPrice(0, 95, 0, 0);
        public static readonly int Rarity10BuyPrice = Item.buyPrice(1, 0, 0, 0);
        public static readonly int RarityTurquoiseBuyPrice = Item.buyPrice(1, 20, 0, 0);
        public static readonly int RarityPureGreenBuyPrice = Item.buyPrice(1, 40, 0, 0);
        public static readonly int RarityDarkBlueBuyPrice = Item.buyPrice(1, 80, 0, 0);
        public static readonly int RarityVioletBuyPrice = Item.buyPrice(2, 50, 0, 0);
        public static readonly int RarityHotPinkBuyPrice = Item.buyPrice(5, 0, 0, 0);

		//These duplicates are for my sanity...
        public static readonly int Rarity12BuyPrice = Item.buyPrice(1, 20, 0, 0);
        public static readonly int Rarity13BuyPrice = Item.buyPrice(1, 40, 0, 0);
        public static readonly int Rarity14BuyPrice = Item.buyPrice(1, 80, 0, 0);
        public static readonly int Rarity15BuyPrice = Item.buyPrice(2, 50, 0, 0);
        public static readonly int Rarity16BuyPrice = Item.buyPrice(5, 0, 0, 0);

        public static int GetBuyPrice(int rarity)
        {
            switch (rarity)
            {
                case 0:
                    return Rarity0BuyPrice;
                case 1:
                    return Rarity1BuyPrice;
                case 2:
                    return Rarity2BuyPrice;
                case 3:
                    return Rarity3BuyPrice;
                case 4:
                    return Rarity4BuyPrice;
                case 5:
                    return Rarity5BuyPrice;
                case 6:
                    return Rarity6BuyPrice;
                case 7:
                    return Rarity7BuyPrice;
                case 8:
                    return Rarity8BuyPrice;
                case 9:
                    return Rarity9BuyPrice;
                case 10:
                    return Rarity10BuyPrice;
                case (int)CalamityRarity.Turquoise:
                    return RarityTurquoiseBuyPrice;
                case (int)CalamityRarity.PureGreen:
                    return RarityPureGreenBuyPrice;
                case (int)CalamityRarity.DarkBlue:
                    return RarityDarkBlueBuyPrice;
                case (int)CalamityRarity.Violet:
                    return RarityVioletBuyPrice;
                case (int)CalamityRarity.Developer:
                    return RarityHotPinkBuyPrice;
            }
            return 0;
        }
        public static int GetBuyPrice(Item item)
        {
            return GetBuyPrice(item.rare);
        }
        #endregion

        /// <summary>
        /// Dust helper to spawn dust for an item. Allows you to specify where on the item to spawn the dust, essentially. (ONLY WORKS FOR SWINGING WEAPONS?)
        /// </summary>
        /// <param name="player">The player using the item.</param>
        /// <param name="dustType">The type of dust to use.</param>
        /// <param name="chancePerFrame">The chance per frame to spawn the dust (0f-1f)</param>
        /// <param name="minDistance">The minimum distance between the player and the dust</param>
        /// <param name="maxDistance">The maximum distance between the player and the dust</param>
        /// <param name="minRandRot">The minimum random rotation offset for the dust</param>
        /// <param name="maxRandRot">The maximum random rotation offset for the dust</param>
        /// <param name="minSpeed">The minimum speed that the dust should travel</param>
        /// <param name="maxSpeed">The maximum speed that the dust should travel</param>
        public static Dust MeleeDustHelper(Player player, int dustType, float chancePerFrame, float minDistance, float maxDistance, float minRandRot = -0.2f, float maxRandRot = 0.2f, float minSpeed = 0.9f, float maxSpeed = 1.1f)
        {
            if (Main.rand.NextFloat(1f) < chancePerFrame)
            {
                //Calculate values
                //distance from player,
                //the vector offset from the player center
                //the vector between the pos and the player
                float distance = Main.rand.NextFloat(minDistance, maxDistance);
                Vector2 offset = (player.itemRotation - (MathHelper.PiOver4 * player.direction) + Main.rand.NextFloat(minRandRot, maxRandRot)).ToRotationVector2() * distance * player.direction;
                Vector2 pos = player.Center + offset;
                Vector2 vec = pos - player.Center;
                //spawn the dust
                Dust d = Dust.NewDustPerfect(pos, dustType);
                //normalise vector and multiply by velocity magnitude
                vec.Normalize();
                d.velocity = vec * Main.rand.NextFloat(minSpeed, maxSpeed);
                return d;
            }
            return null;
        }
    }
}
