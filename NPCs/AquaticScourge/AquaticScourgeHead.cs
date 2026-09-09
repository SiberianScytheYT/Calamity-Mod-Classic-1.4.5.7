using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.Items.Accessories;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Summon;
using CalRD.Items.Weapons.Rogue;
using CalRD.NPCs.TownNPCs;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.IO;
using CalRD.BiomeManagers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalRD.Items.Armor.Vanity;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace CalRD.NPCs.AquaticScourge
{
	[AutoloadBossHead]
    public class AquaticScourgeHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquatic Scourge");
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.6f,
                PortraitScale = 0.6f,
                CustomTexturePath = "CalRD/ExtraTextures/Bestiary/AquaticScourge_Bestiary"
            };
            value.Position.X += 40f;
            value.Position.Y += 20f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
		
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("A distant relative of the Desert Scourge, its appearance has been changed heavily by the sulphurous waters; Due to the ample food supply and its adaptions to its environment, it's far less aggressive than its kin.")
            });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 16f;
			NPC.GetNPCDamage();
            NPC.width = 90;
            NPC.height = 90;
            NPC.defense = 10;
			NPC.DR_NERD(0.1f);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.LifeMaxNERB(73000, 85000, 10000000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 12, 0, 0);
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type };
			if (CalamityWorld.death || BossRushEvent.BossRushActive)
				NPC.scale = 1.2f;
			else if (CalamityWorld.revenge)
				NPC.scale = 1.15f;
			else if (Main.expertMode)
				NPC.scale = 1.1f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.chaseable);
			writer.Write(NPC.localAI[1]);
		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.chaseable = reader.ReadBoolean();
			NPC.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
			if (NPC.justHit || NPC.life <= NPC.lifeMax * 0.99 || BossRushEvent.BossRushActive)
			{
				Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/AquaticScourge");
			}
			CalamityAI.AquaticScourgeAI(NPC, Mod, true);
		}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            Rectangle targetHitbox = target.Hitbox;

            float dist1 = Vector2.Distance(NPC.Center, targetHitbox.TopLeft());
            float dist2 = Vector2.Distance(NPC.Center, targetHitbox.TopRight());
            float dist3 = Vector2.Distance(NPC.Center, targetHitbox.BottomLeft());
            float dist4 = Vector2.Distance(NPC.Center, targetHitbox.BottomRight());

            float minDist = dist1;
            if (dist2 < minDist)
                minDist = dist2;
            if (dist3 < minDist)
                minDist = dist3;
            if (dist4 < minDist)
                minDist = dist4;

            return minDist <= 50f;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.minion && !projectile.Calamity().overridesMinionDamagePrevention)
            {
                return NPC.Calamity().newAI[0] == 1f;
            }
            return null;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            if (spawnInfo.Player.Calamity().ZoneSulphur && spawnInfo.Water)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<AquaticScourgeHead>()))
                    return 0.01f;
            }
            return 0f;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ModContent.ItemType<SulphurousSand>();
        }

        public override bool SpecialOnKill()
        {
            int closestSegmentID = DropHelper.FindClosestWormSegment(NPC.GetSource_FromThis(), NPC,
                ModContent.NPCType<AquaticScourgeHead>(),
                ModContent.NPCType<AquaticScourgeBody>(),
                ModContent.NPCType<AquaticScourgeBodyAlt>(),
                ModContent.NPCType<AquaticScourgeTail>());
            NPC.position = Main.npc[closestSegmentID].position;
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<AquaticScourgeBag>()));

            npcLoot.Add(ItemID.GreaterHealingPotion, 1, 8, 14);
			npcLoot.Add(ModContent.ItemType<AquaticScourgeTrophy>(), 10);
			npcLoot.AddConditionalPerPlayer(() => !CalamityWorld.downedAquaticScourge, ModContent.ItemType<KnowledgeAquaticScourge>(), true, desc: null);
            npcLoot.AddConditionalPerPlayer(() => !CalamityWorld.downedAquaticScourge, ModContent.ItemType<KnowledgeSulphurSea>(), true, desc: null); 
            npcLoot.AddResidentEvilAmmo(info => !CalamityWorld.downedAquaticScourge, 4, 2, 1);

			// All other drops are contained in the bag, so they only drop directly on Normal
            var normalOnly = npcLoot.DefineNormalOnlyDropSet();
            {
                // Materials
                normalOnly.Add(ModContent.ItemType<VictoryShard>(), 1, 11, 20);
                normalOnly.Add(ItemID.Coral, 1, 5, 9);
                normalOnly.Add(ItemID.Seashell, 1, 5, 9);
                normalOnly.Add(ItemID.Starfish, 1, 5, 9);

                // Weapons
                int[] weapons = new int[]
                {
                    ModContent.ItemType<SubmarineShocker>(),
                    ModContent.ItemType<Barinautical>(),
                    ModContent.ItemType<Downpour>(),
                    ModContent.ItemType<DeepseaStaff>(),
                    ModContent.ItemType<ScourgeoftheSeas>()
                };
                normalOnly.Add(DropHelper.CalamityStyle(DropHelper.DirectWeaponDropRateFraction, weapons));

                // Equipment
                normalOnly.Add(ModContent.ItemType<AeroStone>(), 9);

                // Vanity
                normalOnly.Add(ModContent.ItemType<AquaticScourgeMask>(), 7);

                // Fishing
                normalOnly.Add(ModContent.ItemType<BleachedAnglingKit>());
            }
        }

        public override void OnKill()
        {
            CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<SEAHOE>() }, CalamityWorld.downedAquaticScourge);
            
            // If Aquatic Scourge has not yet been killed, notify players of buffed Acid Rain
            if (!CalamityWorld.downedAquaticScourge)
            {
                if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/MaulerRoar"), Main.player[Main.myPlayer].position);

                string sulfSeaBoostKey = "The sulphuric sky darkens...";
                Color sulfSeaBoostColor = AcidRainEvent.TextColor;

                CalamityUtils.DisplayLocalizedText(sulfSeaBoostKey, sulfSeaBoostColor);
                //set a timer for acid rain to start after 10 seconds
                CalamityWorld.forceRainTimer = 601;
            }

            // Mark Aquatic Scourge as dead
            CalamityWorld.downedAquaticScourge = true;
            CalamityNetcode.SyncWorld();
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
			NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int k = 0; k < 15; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                    }
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ASHead").Type, 1f);
                }
            }
        }

        public override bool CheckActive()
        {
            if (NPC.Calamity().newAI[0] == 1f && !Main.player[NPC.target].dead && NPC.Calamity().newAI[1] != 1f)
            {
                return false;
            }
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 360, true);
            target.AddBuff(BuffID.Venom, 360, true);
        }
    }
}
