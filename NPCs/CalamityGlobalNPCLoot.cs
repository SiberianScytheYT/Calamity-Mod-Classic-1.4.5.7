using CalRD.Buffs.DamageOverTime;
using CalRD.CalPlayer;
using CalRD.Events;
using CalRD.Items.Accessories;
using CalRD.Items.DifficultyItems;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.AstrumDeus;
using CalRD.NPCs.DevourerofGods;
using CalRD.NPCs.Leviathan;
using CalRD.NPCs.NormalNPCs;
using CalRD.NPCs.SlimeGod;
using CalRD.NPCs.StormWeaver;
using CalRD.NPCs.SulphurousSea;
using CalRD.NPCs.TownNPCs;
using CalRD.Tiles.Ores;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.NPCs
{
	public class CalamityGlobalNPCLoot : GlobalNPC
    {
		public static int halibutCannonBaseDropChance = 100000;

        #region Instance Per Entity
        public override bool InstancePerEntity => false;
        protected override bool CloneNewInstances => false;
        #endregion

        #region PreNPCLoot
        public override bool PreKill(NPC npc)
        {
            if (BossRushEvent.BossRushActive)
            {
                return BossRushLootCancel(npc, Mod);
            }

            bool abyssLootCancel = AbyssLootCancel(npc, Mod);
            if (abyssLootCancel)
            {
                return false;
            }

			if (CalamityWorld.death)
			{
				switch (npc.type)
				{
					case NPCID.DiggerHead:
					case NPCID.DiggerBody:
					case NPCID.DiggerTail:
						return SplittingWormLoot(npc, Mod, 0);
					case NPCID.SeekerHead:
					case NPCID.SeekerBody:
					case NPCID.SeekerTail:
						return SplittingWormLoot(npc, Mod, 1);
					case NPCID.DuneSplicerHead:
					case NPCID.DuneSplicerBody:
					case NPCID.DuneSplicerTail:
						return SplittingWormLoot(npc, Mod, 2);
					default:
						break;
				}
			}

            if (CalamityWorld.revenge)
            {
                if (npc.type == NPCID.Probe || npc.type == NPCID.ServantofCthulhu)
                {
                    return false;
                }
            }

            // Determine whether this NPC is the second Twin killed in a fight, regardless of which Twin it is.
            bool lastTwinStanding = false;
            if (npc.type == NPCID.Retinazer)
            {
                lastTwinStanding = !NPC.AnyNPCs(NPCID.Spazmatism);
            }
            else if (npc.type == NPCID.Spazmatism)
            {
                lastTwinStanding = !NPC.AnyNPCs(NPCID.Retinazer);
            }

            // Mechanical Bosses' combined lore item
            bool mechLore = !NPC.downedMechBossAny && (lastTwinStanding || npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime);
            DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeMechs>(), true, mechLore);

            if (npc.type == NPCID.KingSlime)
            {
                // Drop a huge spray of Gel items
                int minGel = Main.expertMode ? 90 : 60;
                int maxGel = Main.expertMode ? 120 : 80;
                DropHelper.DropItemSpray(npc.GetSource_FromThis(), npc, ItemID.Gel, minGel, maxGel, 2);

                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeKingSlime>(), true, !NPC.downedSlimeKing);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedSlimeKing, 2, 0, 0);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Dryad }, NPC.downedSlimeKing);
			}
            else if (npc.type == NPCID.EyeofCthulhu)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeEyeofCthulhu>(), true, !NPC.downedBoss1);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedBoss1, 2, 0, 0);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Merchant, NPCID.Dryad }, NPC.downedBoss1);
			}
            else if ((npc.boss && (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)) || npc.type == NPCID.BrainofCthulhu)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeCorruption>(), true, !WorldGen.crimson && !NPC.downedBoss2);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeEaterofWorlds>(), true, !WorldGen.crimson && !NPC.downedBoss2);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeCrimson>(), true, WorldGen.crimson && !NPC.downedBoss2);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeBrainofCthulhu>(), true, WorldGen.crimson && !NPC.downedBoss2);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedBoss2, 2, 0, 0);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Merchant, NPCID.ArmsDealer, NPCID.Dryad }, NPC.downedBoss2);
			}
            else if (npc.type == NPCID.QueenBee)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeQueenBee>(), true, !NPC.downedQueenBee);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedQueenBee, 2, 0, 0);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.ArmsDealer, NPCID.Dryad }, NPC.downedQueenBee);
			}
            else if (npc.type == NPCID.SkeletronHead)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<ClothiersWrath>(), !Main.expertMode, DropHelper.RareVariantDropRateInt, 1, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeSkeletron>(), true, !NPC.downedBoss3);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedBoss3, 3, 1, 0);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Merchant, NPCID.Dryad }, NPC.downedBoss3);
			}
            else if (npc.type == NPCID.WallofFlesh)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<MLGRune>(), !Main.expertMode && !CalamityWorld.demonMode); // Demon Trophy
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<Meowthrower>(), !Main.expertMode, 5, 1, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<BlackHawkRemote>(), !Main.expertMode, 5, 1, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<BlastBarrel>(), !Main.expertMode, 5, 1, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<RogueEmblem>(), !Main.expertMode, 8, 1, 1);
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<IbarakiBox>(), !Main.hardMode, Main.hardMode ? 0.1f : 1f); // 100% chance on first kill, 10% chance afterwards
                DropHelper.DropItemFromSetCondition(npc.GetSource_FromThis(), npc, !Main.expertMode, 0.2f, ItemID.CorruptionKey, ItemID.CrimsonKey);

                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeUnderworld>(), true, !Main.hardMode);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeWallofFlesh>(), true, !Main.hardMode);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, Main.hardMode, 3, 1, 0);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Merchant, NPCID.ArmsDealer, NPCID.Dryad, NPCID.Painter, NPCID.WitchDoctor, NPCID.Stylist, NPCID.Demolitionist, NPCID.PartyGirl, NPCID.Clothier, NPCID.SkeletonMerchant, ModContent.NPCType<THIEF>() }, Main.hardMode);

				// First kill text (this is not a loot function)
				if (!Main.hardMode)
				{
					string key2 = "The Sunken Sea trembles...";
					Color messageColor2 = Color.Aquamarine;

                    CalamityUtils.DisplayLocalizedText(key2, messageColor2);
                }
            }
            else if (lastTwinStanding)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeTwins>(), true, !NPC.downedMechBoss2);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedMechBoss2, 4, 2, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<MysteriousCircuitry>(), CalamityGlobalNPC.DraedonMayhem, 1f, 8, 16);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<DubiousPlating>(), CalamityGlobalNPC.DraedonMayhem, 1f, 8, 16);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.DD2Bartender, NPCID.Stylist, NPCID.Truffle, ModContent.NPCType<THIEF>() }, NPC.downedMechBossAny);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Stylist, ModContent.NPCType<DILF>(), ModContent.NPCType<FAP>(), ModContent.NPCType<THIEF>() }, !NPC.downedMechBoss1 || NPC.downedMechBoss2 || !NPC.downedMechBoss3);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Steampunker }, NPC.downedMechBoss2 || !CalamityConfig.Instance.SellVanillaSummons);
			}
            else if (npc.type == NPCID.TheDestroyer)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeDestroyer>(), true, !NPC.downedMechBoss1);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedMechBoss1, 4, 2, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<MysteriousCircuitry>(), CalamityGlobalNPC.DraedonMayhem, 1f, 8, 16);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<DubiousPlating>(), CalamityGlobalNPC.DraedonMayhem, 1f, 8, 16);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.DD2Bartender, NPCID.Stylist, NPCID.Truffle, ModContent.NPCType<THIEF>() }, NPC.downedMechBossAny);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Stylist, ModContent.NPCType<DILF>(), ModContent.NPCType<FAP>(), ModContent.NPCType<THIEF>() }, NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Steampunker }, NPC.downedMechBoss1 || !CalamityConfig.Instance.SellVanillaSummons);
			}
            else if (npc.type == NPCID.SkeletronPrime)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeSkeletronPrime>(), true, !NPC.downedMechBoss3);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<GoldBurdenBreaker>(), true, npc.ai[1] == 2f && CalamityWorld.revenge);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedMechBoss3, 4, 2, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<MysteriousCircuitry>(), CalamityGlobalNPC.DraedonMayhem, 1f, 8, 16);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<DubiousPlating>(), CalamityGlobalNPC.DraedonMayhem, 1f, 8, 16);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.DD2Bartender, NPCID.Stylist, NPCID.Truffle, ModContent.NPCType<THIEF>() }, NPC.downedMechBossAny);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Stylist, ModContent.NPCType<DILF>(), ModContent.NPCType<FAP>(), ModContent.NPCType<THIEF>() }, !NPC.downedMechBoss1 || !NPC.downedMechBoss2 || NPC.downedMechBoss3);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Steampunker }, NPC.downedMechBoss3 || !CalamityConfig.Instance.SellVanillaSummons);
            }
            else if (npc.type == NPCID.Plantera)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.JungleKey, !Main.expertMode, 5, 1, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgePlantera>(), true, !NPC.downedPlantBoss);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedPlantBoss, 4, 2, 1);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.WitchDoctor, NPCID.Truffle, ModContent.NPCType<FAP>(), ModContent.NPCType<THIEF>() }, NPC.downedPlantBoss);

				// Spawn Perennial Ore if Plantera has never been killed
				if (!NPC.downedPlantBoss)
                {
                    string key2 = "Energized plant matter has formed in the underground.";
                    Color messageColor2 = Color.GreenYellow;
                    string key3 = "The desert sand shifts intensely!";
                    Color messageColor3 = Color.Goldenrod;

                    WorldGenerationMethods.SpawnOre(ModContent.TileType<PerennialOre>(), 12E-05, .5f, .7f);

                    CalamityUtils.DisplayLocalizedText(key2, messageColor2);
                    CalamityUtils.DisplayLocalizedText(key3, messageColor3);
                }
            }
			else if (npc.type == NPCID.Pumpking)
			{
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Clothier }, NPC.downedHalloweenKing);
			}
			else if (npc.type == NPCID.Everscream)
			{
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<DILF>() }, NPC.downedChristmasTree || !NPC.downedChristmasSantank || !NPC.downedChristmasIceQueen);
			}
			else if (npc.type == NPCID.SantaNK1)
			{
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<DILF>() }, !NPC.downedChristmasTree || NPC.downedChristmasSantank || !NPC.downedChristmasIceQueen);
			}
			else if (npc.type == NPCID.IceQueen)
			{
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Clothier }, NPC.downedChristmasIceQueen);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<DILF>() }, !NPC.downedChristmasTree || !NPC.downedChristmasSantank || NPC.downedChristmasIceQueen);
			}
            else if (npc.type == NPCID.Golem)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<EssenceofCinder>(), !Main.expertMode, 5, 10);
				DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<LeadWizard>(), !Main.expertMode, DropHelper.RareVariantDropRateFloat);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.Picksaw, true, !NPC.downedGolemBoss);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeGolem>(), true, !NPC.downedGolemBoss);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedGolemBoss, 4, 2, 1);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.ArmsDealer, NPCID.Cyborg, NPCID.Steampunker, NPCID.Wizard, NPCID.WitchDoctor, NPCID.DD2Bartender, ModContent.NPCType<FAP>(), ModContent.NPCType<THIEF>() }, NPC.downedGolemBoss);

				// If Golem has never been killed, send messages about PBG
				if (!NPC.downedGolemBoss)
                {
                    string key = "A plague has befallen the Jungle.";
                    Color messageColor = Color.Lime;

                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }
            }
            else if (npc.type == NPCID.DD2Betsy && !CalamityWorld.downedBetsy)
            {
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, CalamityWorld.downedBetsy, 4, 2, 1);

                // Mark Betsy as dead (Vanilla does not keep track of her)
                CalamityWorld.downedBetsy = true;
                CalamityNetcode.SyncWorld();
            }
            else if (npc.type == NPCID.DukeFishron)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<DukesDecapitator>(), !Main.expertMode, 5, 1, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeDukeFishron>(), true, !NPC.downedFishron);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedPlantBoss, 4, 2, 1);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<SEAHOE>() }, NPC.downedFishron || !CalamityConfig.Instance.SellVanillaSummons);
			}
            else if (npc.type == NPCID.CultistBoss)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeLunaticCultist>(), true, !NPC.downedAncientCultist);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedAncientCultist, 4, 2, 1);

                // Blood Moon lore item
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeBloodMoon>(), true, Main.bloodMoon);

                // Deus text (this is not a loot function)
                if (!NPC.downedAncientCultist)
                {
                    string key = "A star-spawned horror tunnels through the astral infection.";
                    Color messageColor = Color.Gold;

                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }
            }
            else if (npc.type == NPCID.MoonLordCore)
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.LunarOre, !Main.expertMode, 50, 50);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<MLGRune2>(), true, !Main.expertMode);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.GravityGlobe, !Main.expertMode);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<UtensilPoker>(), !Main.expertMode, 9, 1, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<GrandDad>(), !Main.expertMode, DropHelper.RareVariantDropRateInt, 1, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<Infinity>(), !Main.expertMode, DropHelper.RareVariantDropRateInt, 1, 1);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<KnowledgeMoonLord>(), true, !NPC.downedMoonlord);
                DropHelper.DropResidentEvilAmmo(npc.GetSource_FromThis(), npc, NPC.downedMoonlord, 5, 2, 1);

				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<THIEF>() }, NPC.downedMoonlord);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Wizard }, NPC.downedMoonlord || !CalamityConfig.Instance.SellVanillaSummons);

				string key = "The profaned flame blazes fiercely!";
                Color messageColor = Color.Orange;
                string key2 = "Cosmic terrors are watching...";
                Color messageColor2 = Color.Violet;
                string key3 = "The bloody moon beckons...";
                Color messageColor3 = Color.Crimson;
                string key4 = "Shrieks are echoing from the dungeon.";
                Color messageColor4 = Color.Cyan;
                string key5 = "A cold and dark energy has materialized in space.";
                Color messageColor5 = Color.LightGray;

                // Spawn Exodium and send messages about Providence, Bloodstone, Phantoplasm, etc. if ML has not been killed yet
                if (!NPC.downedMoonlord)
                {
                    WorldGenerationMethods.SpawnOre(ModContent.TileType<ExodiumOre>(), 12E-05, .01f, .07f);

                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                    CalamityUtils.DisplayLocalizedText(key2, messageColor);
                    CalamityUtils.DisplayLocalizedText(key3, messageColor);
                    CalamityUtils.DisplayLocalizedText(key4, messageColor);
                    CalamityUtils.DisplayLocalizedText(key5, messageColor);
                }
            }
			//Since Calamity makes it spawn in pre-hardmode, don't want to cause other mods to freak out if they use it as a tier gate (like a new weapon or something)
			else if (npc.type == NPCID.GreenJellyfish && !Main.hardMode)
			{
                DropHelper.DropItem(npc.GetSource_FromThis(), npc, ItemID.Glowstick, 1, 4);
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.JellyfishNecklace, 0.1f);
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Megaphone, Main.expertMode ? 0.2f : 0.1f);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.JellyfishNecklace, CalamityWorld.defiled, DropHelper.DefiledDropRateFloat);
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.Megaphone, CalamityWorld.defiled, DropHelper.DefiledDropRateFloat);
				DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<VitalJelly>(), Main.expertMode ? 5 : 7);
				return false;
			}
			else if (npc.type == NPCID.VoodooDemon && Main.player[npc.target].Calamity().underworldLore)
			{
                NPCLoader.blockLoot.Add(ItemID.GuideVoodooDoll);
			}

            return true;
        }
        #endregion

        #region Boss Rush Loot Cancel
        private bool BossRushLootCancel(NPC npc, Mod mod)
        {
            // Eater of Worlds splits in Boss Rush now, so you have to kill every single segment to progress.
            // Vanilla sets npc.boss to true for the last Eater of Worlds segment to die in NPC.checkDead.
            // This means we do not need to manually check for other segments ourselves.
            if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
            {
                if (npc.boss)
				{
                    BossRushEvent.BossRushStage++;
                    CalamityUtils.KillAllHostileProjectiles();
                    CalamityWorld.bossRushHostileProjKillCounter = 3;
				}
            }
            
            // Anahita and Leviathan manually check for each other (this probably isn't necessary).
            else if (npc.type == ModContent.NPCType<Siren>() || npc.type == ModContent.NPCType<Leviathan.Leviathan>())
            {
                int bossType = (npc.type == ModContent.NPCType<Siren>()) ? ModContent.NPCType<Leviathan.Leviathan>() : ModContent.NPCType<Siren>();
                if (!NPC.AnyNPCs(bossType))
                {
                    BossRushEvent.BossRushStage++;
                    CalamityUtils.KillAllHostileProjectiles();
					CalamityWorld.bossRushHostileProjKillCounter = 3;
                }
            }
            
            // Killing any split Deus head ends the fight instantly. You don't need to kill both.
            else if (npc.type == ModContent.NPCType<AstrumDeusHeadSpectral>() && npc.Calamity().newAI[0] != 0f)
            {
                BossRushEvent.BossRushStage++;
                CalamityUtils.KillAllHostileProjectiles();
                CalamityWorld.bossRushHostileProjKillCounter = 3;
            }

            // All Slime God entities must be killed to progress to the next stage.
            else if (npc.type == ModContent.NPCType<SlimeGodCore>() || npc.type == ModContent.NPCType<SlimeGodSplit>() || npc.type == ModContent.NPCType<SlimeGodRunSplit>())
            {
                if (npc.type == ModContent.NPCType<SlimeGodCore>() && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodSplit>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodRunSplit>()) &&
                    !NPC.AnyNPCs(ModContent.NPCType<SlimeGod.SlimeGod>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodRun>()))
                {
                    BossRushEvent.BossRushStage++;
                    CalamityUtils.KillAllHostileProjectiles();
					CalamityWorld.bossRushHostileProjKillCounter = 3;
                }
                else if (npc.type == ModContent.NPCType<SlimeGodSplit>() && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodCore>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodRunSplit>()) &&
                    NPC.CountNPCS(ModContent.NPCType<SlimeGodSplit>()) < 2 && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodRun>()))
                {
                    BossRushEvent.BossRushStage++;
                    CalamityUtils.KillAllHostileProjectiles();
					CalamityWorld.bossRushHostileProjKillCounter = 3;
                }
                else if (npc.type == ModContent.NPCType<SlimeGodRunSplit>() && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodCore>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodSplit>()) &&
                    NPC.CountNPCS(ModContent.NPCType<SlimeGodRunSplit>()) < 2 && !NPC.AnyNPCs(ModContent.NPCType<SlimeGod.SlimeGod>()))
                {
                    BossRushEvent.BossRushStage++;
                    CalamityUtils.KillAllHostileProjectiles();
					CalamityWorld.bossRushHostileProjKillCounter = 3;
                }
            }

            // This is the generic form of "Are there any remaining NPCs on the boss list for this boss rush stage?" check.
            else if ((BossRushEvent.Bosses.Any(boss => boss.EntityID == npc.type) && !BossRushEvent.BossIDsAfterDeath.ContainsKey(npc.type)) ||
                     BossRushEvent.BossIDsAfterDeath.Values.Any(killList => killList.Contains(npc.type)))
            {
                BossRushEvent.BossRushStage++;
                CalamityUtils.KillAllHostileProjectiles();
                CalamityWorld.bossRushHostileProjKillCounter = 3;
                if (BossRushEvent.BossDeathEffects.ContainsKey(npc.type))
                    BossRushEvent.BossDeathEffects[npc.type].Invoke(npc);
            }

            if (Main.netMode == NetmodeID.Server)
            {
                var netMessage = mod.GetPacket();
                netMessage.Write((byte)CalRDMessageType.BossRushStage);
                netMessage.Write(BossRushEvent.BossRushStage);
                netMessage.Send();
				var netMessage2 = mod.GetPacket();
				netMessage2.Write((byte)CalRDMessageType.BRHostileProjKillSync);
				netMessage2.Write(CalamityWorld.bossRushHostileProjKillCounter);
				netMessage2.Send();
            }

            return false;
        }
        #endregion

        #region Abyss Loot Cancel
        private bool AbyssLootCancel(NPC npc, Mod mod)
        {
            int x = Main.maxTilesX;
            int y = Main.maxTilesY;
            int genLimit = x / 2;
            int abyssChasmY = y - 250;
            int abyssChasmX = CalamityWorld.abyssSide ? genLimit - (genLimit - 135) : genLimit + (genLimit - 135);
            bool abyssPosX = false;
            bool abyssPosY = (double)(npc.position.Y / 16f) <= abyssChasmY;

            if (CalamityWorld.abyssSide)
            {
                if ((double)(npc.position.X / 16f) < abyssChasmX + 80)
                {
                    abyssPosX = true;
                }
            }
            else
            {
                if ((double)(npc.position.X / 16f) > abyssChasmX - 80)
                {
                    abyssPosX = true;
                }
            }

            bool hurtByAbyss = npc.wet && npc.damage > 0 && !npc.boss && !npc.friendly && !npc.dontTakeDamage &&
                (((npc.position.Y / 16f > (Main.rockLayer - Main.maxTilesY * 0.05)) &&
                abyssPosY && abyssPosX) || CalamityWorld.abyssTiles > 200) && !npc.buffImmune[ModContent.BuffType<CrushDepth>()];

            return hurtByAbyss;
        }
		#endregion

		#region Splitting Worm Loot
		private bool SplittingWormLoot(NPC npc, Mod mod, int wormType)
		{
			switch (wormType)
			{
				case 0: return CheckSegments(NPCID.DiggerHead, NPCID.DiggerBody, NPCID.DiggerTail);
				case 1: return CheckSegments(NPCID.SeekerHead, NPCID.SeekerBody, NPCID.SeekerTail);
				case 2: return CheckSegments(NPCID.DuneSplicerHead, NPCID.DuneSplicerBody, NPCID.DuneSplicerTail);
				default:
					break;
			}

			bool CheckSegments(int head, int body, int tail)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (i != npc.whoAmI && Main.npc[i].active && (Main.npc[i].type == head || Main.npc[i].type == body || Main.npc[i].type == tail))
					{
						return false;
					}
				}
				return true;
			}

			return true;
		}
		#endregion

		#region NPCLoot
		public override void OnKill(NPC npc)
        {
            ResetAdrenaline(npc);

            // LATER -- keeping bosses alive lets draedon mayhem continue even after killing mechs
            // Reset Draedon Mayhem to false if no bosses are alive
            if (CalamityGlobalNPC.DraedonMayhem)
            {
                if (!CalamityPlayer.areThereAnyDamnBosses)
                {
                    CalamityGlobalNPC.DraedonMayhem = false;
                    CalamityNetcode.SyncWorld();
                }
            }

            if (CalamityWorld.defiled)
                DefiledLoot(npc);
            if (CalamityWorld.armageddon)
                ArmageddonLoot(npc);

            AcidRainProgression(npc);
            CheckBossSpawn(npc);
            ArmorSetLoot(npc);
            RareLoot(npc);
            RareVariants(npc);
            CommonLoot(npc);
            TownNPCLoot(npc);
            EventEnemyLoot(npc, Main.pumpkinMoon, Main.snowMoon, Main.eclipse);
        }
        #endregion

        #region Defiled Loot
        private void DefiledLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Werewolf:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.MoonCharm, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.AdhesiveBandage, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.AnglerFish:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.AdhesiveBandage, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DesertBeast:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.AncientHorn, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ArmoredSkeleton:
                case NPCID.HeavySkeleton:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.BeamSword, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.ArmorPolish, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Clown:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Bananarang, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ToxicSludge:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Bezoar, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.EyeofCthulhu:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Binoculars, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.WanderingEye:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.BlackLens, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.CorruptSlime:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Blindfold, DropHelper.DefiledDropRateInt);
                    break;

                // This is all the random skeletons in the hardmode dungeon
                case NPCID.RustyArmoredBonesAxe:
                case NPCID.RustyArmoredBonesFlail:
                case NPCID.RustyArmoredBonesSword:
                case NPCID.RustyArmoredBonesSwordNoArmor:
                case NPCID.BlueArmoredBones:
                case NPCID.BlueArmoredBonesMace:
                case NPCID.BlueArmoredBonesNoPants:
                case NPCID.BlueArmoredBonesSword:
                case NPCID.HellArmoredBones:
                case NPCID.HellArmoredBonesSpikeShield:
                case NPCID.HellArmoredBonesMace:
                case NPCID.HellArmoredBonesSword:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Keybrand, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.BoneFeather, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.MagnetSphere, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.WispinaBottle, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.UndeadMiner:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.BonePickaxe, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ScutlixRider:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.BrainScrambler, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Vampire:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.BrokenBatWing, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.MoonStone, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.CaveBat:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.ChainKnife, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.DepthMeter, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DarkCaster:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.ClothierVoodooDoll, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.TallyCounter, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.PirateCaptain:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.CoinGun, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.DiscountCard, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Cutlass, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.LuckyCoin, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.PirateStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Reaper:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.DeathSickle, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Demon:
                case NPCID.VoodooDemon:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.DemonScythe, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DesertDjinn:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.DjinnLamp, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.DjinnsCurse, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Shark:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.DivingHelmet, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Pixie:
                case NPCID.Wraith:
                case NPCID.Mummy:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.FastClock, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.RedDevil:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.FireFeather, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.UnholyTrident, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.IceElemental:
                case NPCID.IcyMerman:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.IceSickle, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.FrostStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ArmoredViking:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.IceSickle, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.IceTortoise:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.IceSickle, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.FrozenTurtleShell, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Harpy:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.GiantHarpyFeather, Main.hardMode && !npc.SpawnedFromStatue, DropHelper.DefiledDropRateFloat);
                    break;

                case NPCID.Piranha:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Hook, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.PinkJellyfish:
                case NPCID.BlueJellyfish:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.JellyfishNecklace, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Paladin:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Kraken, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.PaladinsHammer, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.SkeletonArcher:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Marrow, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.MagicQuiver, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Lavabat:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.MagmaStone, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.WalkingAntlion:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.AntlionClaw, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DarkMummy:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Blindfold, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Megaphone, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.GreenJellyfish:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Megaphone, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.JellyfishNecklace, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.CursedSkull:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Nazar, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.FireImp:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.ObsidianRose, DropHelper.DefiledDropRateInt);
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.Cascade, NPC.downedBoss3, DropHelper.DefiledDropRateFloat);
                    break;

                case NPCID.BlackRecluse:
                case NPCID.BlackRecluseWall:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.PoisonStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ChaosElemental:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.RodofDiscord, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.SnowFlinx:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.SnowballLauncher, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Plantera:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.TheAxe, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.GiantBat:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.TrifoldMap, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.AngryTrapper:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Uzi, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Corruptor:
                case NPCID.FloatyGross:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Vitamins, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.GiantTortoise:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.Yelets, NPC.downedMechBossAny, DropHelper.DefiledDropRateFloat);
                    break;

                case NPCID.DeadlySphere:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.DeadlySphereStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DrManFly:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.ToxicFlask, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.CreatureFromTheDeep:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.NeptunesShell, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Butcher:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.ButchersChainsaw, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Psycho:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.PsychoKnife, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Drippler:
                case NPCID.BloodZombie:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.SharkToothNecklace, !npc.SpawnedFromStatue, DropHelper.DefiledDropRateFloat);
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.MoneyTrough, !npc.SpawnedFromStatue, DropHelper.DefiledDropRateFloat);
                    break;

                case NPCID.GoblinWarrior:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Harpoon, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Pinky:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.SlimeStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.FlyingSnake:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.LizardEgg, DropHelper.DefiledDropRateInt);
                    break;

                default:
                    break;
            }

            // Every type of demon eye counts for Black Lenses
            if (CalamityLists.demonEyeList.Contains(npc.type))
            {
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.BlackLens, DropHelper.DefiledDropRateInt);
            }

            // Every type of Skeleton counts for the Bone Sword
            if (CalamityLists.skeletonList.Contains(npc.type) && npc.type != NPCID.ArmoredSkeleton && npc.type != NPCID.HeavySkeleton && npc.type != NPCID.SkeletonArcher && npc.type != NPCID.GreekSkeleton)
            {
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.BoneSword, DropHelper.DefiledDropRateInt);
            }

            // Every type of Angry Bones counts for the Clothier Voodoo Doll
            if (CalamityLists.angryBonesList.Contains(npc.type))
            {
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.ClothierVoodooDoll, DropHelper.DefiledDropRateInt);
            }

            // Every type of hornet AND moss hornet can drop Bezoar
            if (CalamityLists.hornetList.Contains(npc.type) || CalamityLists.mossHornetList.Contains(npc.type))
            {
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Bezoar, DropHelper.DefiledDropRateInt);
            }

            // Every type of moss hornet can drop Tattered Bee Wings
            if (CalamityLists.mossHornetList.Contains(npc.type))
            {
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.TatteredBeeWing, DropHelper.DefiledDropRateInt);
            }

            // Because all switch cases must be constant at compile time, modded NPC IDs (which can change) can't be included.
            if (npc.type == ModContent.NPCType<SunBat>())
            {
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.HelFire, DropHelper.DefiledDropRateInt);
            }
            else if (npc.type == ModContent.NPCType<Cryon>())
            {
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Amarok, DropHelper.DefiledDropRateInt);
            }
        }
        #endregion

        #region Armageddon Loot
        private void ArmageddonLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.KingSlime: 
                    DropHelper.DropArmageddonBags(ItemID.KingSlimeBossBag, npc);
                    break;
                case NPCID.EyeofCthulhu:
                    DropHelper.DropArmageddonBags(ItemID.EyeOfCthulhuBossBag, npc);
                    break;
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    if (npc.boss) // only drop from the 1 "boss" segment (redcode)
                    {
                        DropHelper.DropArmageddonBags(ItemID.EaterOfWorldsBossBag, npc);
                    }

                    break;

                case NPCID.BrainofCthulhu:
                    DropHelper.DropArmageddonBags(ItemID.BrainOfCthulhuBossBag, npc);
                    break;
                case NPCID.QueenBee:
                    DropHelper.DropArmageddonBags(ItemID.QueenBeeBossBag, npc);
                    break;
                case NPCID.SkeletronHead:
                    DropHelper.DropArmageddonBags(ItemID.SkeletronBossBag, npc);
                    break;
                case NPCID.WallofFlesh:
                    DropHelper.DropArmageddonBags(ItemID.WallOfFleshBossBag, npc);
                    break;
                case NPCID.Retinazer: // only drop if spaz is already dead
                    if (!NPC.AnyNPCs(NPCID.Spazmatism))
                    {
                        DropHelper.DropArmageddonBags(ItemID.TwinsBossBag, npc);
                    }

                    break;

                case NPCID.Spazmatism: // only drop if ret is already dead
                    if (!NPC.AnyNPCs(NPCID.Retinazer))
                    {
                        DropHelper.DropArmageddonBags(ItemID.TwinsBossBag, npc);
                    }

                    break;

                case NPCID.TheDestroyer:
                    DropHelper.DropArmageddonBags(ItemID.DestroyerBossBag, npc);
                    break;
                case NPCID.SkeletronPrime:
                    DropHelper.DropArmageddonBags(ItemID.SkeletronPrimeBossBag, npc);
                    break;
                case NPCID.Plantera:
                    DropHelper.DropArmageddonBags(ItemID.PlanteraBossBag, npc);
                    break;
                case NPCID.Golem:
                    DropHelper.DropArmageddonBags(ItemID.GolemBossBag, npc);
                    break;
                case NPCID.DD2Betsy:
                    DropHelper.DropArmageddonBags(ItemID.BossBagBetsy, npc);
                    break;
                case NPCID.DukeFishron:
                    DropHelper.DropArmageddonBags(ItemID.FishronBossBag, npc);
                    break;
                case NPCID.MoonLordCore:
                    DropHelper.DropArmageddonBags(ItemID.MoonLordBossBag, npc);
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Reset Adrenaline
        private void ResetAdrenaline(NPC npc)
        {
            bool revenge = CalamityWorld.revenge;
            if (npc.boss && revenge)
            {
                if (npc.type != ModContent.NPCType<HiveMind.HiveMind>() && npc.type != ModContent.NPCType<Leviathan.Leviathan>() && npc.type != ModContent.NPCType<Siren>() &&
                    npc.type != ModContent.NPCType<StormWeaverHead>() && npc.type != ModContent.NPCType<StormWeaverBody>() &&
                    npc.type != ModContent.NPCType<StormWeaverTail>() && npc.type != ModContent.NPCType<DevourerofGodsHead>() &&
                    npc.type != ModContent.NPCType<DevourerofGodsBody>() && npc.type != ModContent.NPCType<DevourerofGodsTail>() && 
					npc.type != ModContent.NPCType<Calamitas.Calamitas>())
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active)
                        {
                            Main.player[Main.myPlayer].Calamity().adrenaline = 0;
                        }
                    }
                }
            }
        }
        #endregion

        #region Check Boss Spawn
        // not really drop code
        private void CheckBossSpawn(NPC npc)
        {
            if ((npc.type == ModContent.NPCType<PhantomSpirit>() || npc.type == ModContent.NPCType<PhantomSpiritS>() || npc.type == ModContent.NPCType<PhantomSpiritM>() ||
                npc.type == ModContent.NPCType<PhantomSpiritL>()) && !NPC.AnyNPCs(ModContent.NPCType<Polterghast.Polterghast>()) && !CalamityWorld.downedPolterghast)
            {
                CalRD.ghostKillCount++;
                if (CalRD.ghostKillCount == 10)
                {
                    string key = "Wails echo through the dilapidated dungeon halls...";
                    Color messageColor = Color.Cyan;

                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }
                else if (CalRD.ghostKillCount == 20)
                {
                    string key = "Long-dead prisoners seek their zealous revenge...";
                    Color messageColor = Color.Cyan;

                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }

                if (CalRD.ghostKillCount >= 30 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int lastPlayer = npc.lastInteraction;

                    if (!Main.player[lastPlayer].active || Main.player[lastPlayer].dead)
                    {
                        lastPlayer = npc.FindClosestPlayer();
                    }

                    if (lastPlayer >= 0)
                    {
                        NPC.SpawnOnPlayer(lastPlayer, ModContent.NPCType<Polterghast.Polterghast>());
                        CalRD.ghostKillCount = 0;
                    }
                }
            }

            if (NPC.downedPlantBoss && (npc.type == NPCID.SandShark || npc.type == NPCID.SandsharkHallow || npc.type == NPCID.SandsharkCorrupt || npc.type == NPCID.SandsharkCrimson) && !NPC.AnyNPCs(ModContent.NPCType<GreatSandShark.GreatSandShark>()))
            {
                CalRD.sharkKillCount++;
                if (CalRD.sharkKillCount == 4)
                {
                    string key = "Something stirs in the warm desert sands...";
                    Color messageColor = Color.Goldenrod;

                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }
                else if (CalRD.sharkKillCount == 8)
                {
                    string key = "An enormous apex predator approaches...";
                    Color messageColor = Color.Goldenrod;

                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }
                if (CalRD.sharkKillCount >= 10 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active)
                    {
                        SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/MaulerRoar"),
                            Main.player[Main.myPlayer].position);
                    }

                    int lastPlayer = npc.lastInteraction;

                    if (!Main.player[lastPlayer].active || Main.player[lastPlayer].dead)
                    {
                        lastPlayer = npc.FindClosestPlayer();
                    }

                    if (lastPlayer >= 0)
                    {
                        NPC.SpawnOnPlayer(lastPlayer, ModContent.NPCType<GreatSandShark.GreatSandShark>());
                        CalRD.sharkKillCount = -5;
                    }
                }
            }
        }
        #endregion

        #region Armor Set Loot
        private void ArmorSetLoot(NPC npc)
        {
            // Tarragon armor set bonus: 20% chance to drop hearts from all valid enemies
            if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].Calamity().tarraSet)
            {
                if (!npc.SpawnedFromStatue && (npc.damage > 5 || npc.boss) && npc.lifeMax > 100)
                {
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Heart, 5);
                }
            }

            // Blood Orb drops: Valid enemy during a blood moon on the Surface
            if (!npc.SpawnedFromStatue && (npc.damage > 5 || npc.boss) && Main.bloodMoon && npc.HasPlayerTarget && npc.position.Y / 16D < Main.worldSurface)
            {
                if (Main.player[Player.FindClosest(npc.Center, npc.width, npc.height)].Calamity().bloodflareSet)
                {
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<BloodOrb>(), 2); // 50% chance of 1 orb with Bloodflare
                }

                // 1/12 chance to get a Blood Orb with or without Bloodflare
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<BloodOrb>(), 12);
            }
        }
        #endregion

        #region Rare Loot
        private void RareLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Drippler:
                    int eyeballDropRate = CalamityWorld.defiled ? DropHelper.DefiledDropRateInt : 300;
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<BouncingEyeball>(), eyeballDropRate, 1, 1);
                    break;

                case NPCID.FireImp:
                    int stalactiteDropRate = CalamityWorld.defiled ? DropHelper.DefiledDropRateInt : Main.expertMode ? 100 : 150;
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<AshenStalactite>(), stalactiteDropRate, 1, 1);
                    break;

                case NPCID.PossessedArmor:
                    int amuletDropRate = CalamityWorld.defiled ? DropHelper.DefiledDropRateInt : Main.expertMode ? 150 : 200;
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<PsychoticAmulet>(), amuletDropRate, 1, 1);
                    break;

                case NPCID.SeaSnail:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<SeaShell>(), Main.expertMode ? 2 : 3);
                    break;

                case NPCID.GreekSkeleton:
                    int gladiatorDropRate = Main.expertMode ? 15 : 20;
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.GladiatorHelmet, gladiatorDropRate);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.GladiatorBreastplate, gladiatorDropRate);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.GladiatorLeggings, gladiatorDropRate);
                    break;

                case NPCID.GiantTortoise:
					DropHelper.DropItemRIV(npc.GetSource_FromThis(), npc, ModContent.ItemType<GiantTortoiseShell>(), ModContent.ItemType<FabledTortoiseShell>(), Main.expertMode ? 0.2f : 0.142857f, 0.005f);
                    break;

                case NPCID.GiantShelly:
                case NPCID.GiantShelly2:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<GiantShell>(), Main.expertMode ? 5 : 7);
                    break;

                case NPCID.AnomuraFungus:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<FungalCarapace>(), Main.expertMode ? 5 : 7);
                    break;

                case NPCID.Crawdad:
                case NPCID.Crawdad2:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<CrawCarapace>(), Main.expertMode ? 5 : 7);
                    break;

                case NPCID.GreenJellyfish:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<VitalJelly>(), Main.expertMode ? 5 : 7);
                    break;

                case NPCID.PinkJellyfish:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<LifeJelly>(), Main.expertMode ? 20 : 25);
                    break;

                case NPCID.BlueJellyfish:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<ManaJelly>(), Main.expertMode ? 5 : 7);
                    break;

                case NPCID.DarkCaster:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<AncientShiv>(), Main.expertMode ? 20 : 25);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<ShinobiBlade>(), Main.expertMode ? 20 : 25);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<StaffOfNecrosteocytes>(), Main.expertMode ? 20 : 25);
                    break;

                case NPCID.BigMimicHallow:
                case NPCID.BigMimicCorruption:
                case NPCID.BigMimicCrimson:
                case NPCID.BigMimicJungle: // arguably unnecessary
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<CelestialClaymore>(), Main.expertMode ? 5 : 7);
                    break;

                case NPCID.Clinger:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<CursedDagger>(), Main.expertMode ? 20 : 25);
                    break;

                case NPCID.Shark:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.SharkToothNecklace, Main.expertMode ? 20 : 30);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<JoyfulHeart>(), Main.expertMode ? 20 : 30);
                    break;

                case NPCID.PresentMimic:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<HolidayHalberd>(), Main.expertMode ? 5 : 7);
                    break;

                case NPCID.IchorSticker:
					DropHelper.DropItemRIV(npc.GetSource_FromThis(), npc, ModContent.ItemType<IchorSpear>(), ModContent.ItemType<SpearofDestiny>(), Main.expertMode ? 0.05f : 0.04f, 0.005f);
                    break;

                case NPCID.Harpy:
                    int glazeDropRate = CalamityWorld.defiled ? DropHelper.DefiledDropRateInt : Main.expertMode ? 60 : 80;
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<SkyGlaze>(), NPC.downedBoss1, glazeDropRate, 1, 1);
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<EssenceofCinder>(), Main.hardMode && !npc.SpawnedFromStatue, Main.expertMode ? 2 : 3, 1, 1);
                    break;

                case NPCID.Antlion:
                case NPCID.WalkingAntlion:
                case NPCID.FlyingAntlion:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<MandibleClaws>(), Main.expertMode ? 30 : 40);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<MandibleBow>(), Main.expertMode ? 30 : 40);
                    break;

                case NPCID.TombCrawlerHead:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<BurntSienna>(), Main.expertMode ? 15 : 20);
                    break;

                case NPCID.DuneSplicerHead:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<Terracotta>(), NPC.downedPlantBoss, Main.expertMode ? 20 : 30, 1, 1);
                    break;

                case NPCID.MartianSaucerCore:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<NullificationRifle>(), Main.expertMode ? 5 : 7);
                    break;

                case NPCID.Demon:
                case NPCID.VoodooDemon:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<DemonicBoneAsh>(), Main.expertMode ? 2 : 3);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<BladecrestOathsword>(), Main.expertMode ? 20 : 25);
                    break;

                case NPCID.BoneSerpentHead:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<DemonicBoneAsh>(), Main.expertMode ? 2 : 3);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<OldLordOathsword>(), Main.expertMode ? 10 : 15);
                    break;

                case NPCID.Tim:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<PlasmaRod>(), Main.expertMode ? 2 : 3);
                    break;

                case NPCID.GoblinSorcerer:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<PlasmaRod>(), Main.expertMode ? 20 : 25);
                    break;

                case NPCID.PirateDeadeye:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<ProporsePistol>(), Main.expertMode ? 20 : 25);
                    break;

                case NPCID.PirateCrossbower:
					DropHelper.DropItemRIV(npc.GetSource_FromThis(), npc, ModContent.ItemType<RaidersGlory>(), ModContent.ItemType<Arbalest>(), Main.expertMode ? 0.05f : 0.04f, 0.005f);
                    break;

                case NPCID.GoblinSummoner:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<TheFirstShadowflame>(), Main.expertMode ? 5 : 7);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<BurningStrife>(), Main.expertMode ? 3 : 6);
                    break;

                case NPCID.SandElemental:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<WifeinaBottle>(), Main.expertMode ? 5 : 7);
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<WifeinaBottlewithBoobs>(), Main.expertMode, 20, 1, 1);
                    break;

                case NPCID.GoblinWarrior:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<Warblade>(), Main.expertMode ? 15 : 20);
                    break;

                case NPCID.MartianWalker:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<Wingman>(), Main.expertMode ? 5 : 7);
                    break;

                case NPCID.GiantCursedSkull:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<WrathoftheAncients>(), Main.expertMode ? 20 : 25);
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<Keelhaul>(), CalamityWorld.downedLeviathan, 10, 1, 1);
                    break;

                case NPCID.Necromancer:
                case NPCID.NecromancerArmored:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<WrathoftheAncients>(), Main.expertMode ? 20 : 25);
                    break;

                case NPCID.DeadlySphere:
                    float defectiveDropRate = CalamityWorld.defiled ? DropHelper.DefiledDropRateFloat : Main.expertMode ? 0.0375f : 0.025f;
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<DefectiveSphere>(), defectiveDropRate); //same as deadly sphere staff
                    break;

                case NPCID.BloodJelly:
                case NPCID.FungoFish:
                    float necklaceDropRate = CalamityWorld.defiled ? DropHelper.DefiledDropRateFloat : 0.01f;
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.JellyfishNecklace, necklaceDropRate);
                    break;

                default:
                    break;
            }

            // Every type of Moss Hornet counts for the Needler
            if (CalamityLists.mossHornetList.Contains(npc.type))
            {
                int needlerDropRate = Main.expertMode ? 20 : 25;
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<Needler>(), needlerDropRate);
            }

            // Every type of Skeleton counts for the Waraxe and Ancient Bone Dust
            if (CalamityLists.skeletonList.Contains(npc.type))
            {
                DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<Waraxe>(), !Main.hardMode, Main.expertMode ? 15 : 20, 1, 1);
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<AncientBoneDust>(), Main.expertMode ? 4 : 5);
            }
        }
        #endregion

        #region Rare Variants
        private void RareVariants(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.BloodZombie:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<Carnage>(), NPC.downedBoss3 && !npc.SpawnedFromStatue, 200, 1, 1);
                    break;

                case NPCID.VortexRifleman:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<TrueConferenceCall>(), 200);
                    break;

                case NPCID.DesertBeast:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<EvilSmasher>(), 200);
                    break;

                case NPCID.DungeonSpirit:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<PearlGod>(), 200);
                    break;

                case NPCID.RuneWizard:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<EyeofMagnus>(), 10);
                    break;

                case NPCID.Mimic:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<TheBee>(), !npc.SpawnedFromStatue, 100, 1, 1);
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Acid Rain
        private void AcidRainProgression(NPC npc)
        {
            Dictionary<int, AcidRainSpawnData> possibleEnemies = AcidRainEvent.PossibleEnemiesPreHM;

            if (CalamityWorld.downedAquaticScourge)
                possibleEnemies = AcidRainEvent.PossibleEnemiesAS;
            if (CalamityWorld.downedPolterghast)
                possibleEnemies = AcidRainEvent.PossibleEnemiesPolter;

            if (CalamityWorld.rainingAcid)
            {
                if (possibleEnemies.Select(enemy => enemy.Key).Contains(npc.type))
                {
                    CalamityWorld.acidRainPoints -= possibleEnemies[npc.type].InvasionContributionPoints;
                    if (CalamityWorld.downedPolterghast)
                    {
                        CalamityWorld.acidRainPoints = (int)MathHelper.Max(1, CalamityWorld.acidRainPoints); // Cap at 1. The last point is for Old Duke.
                    }

                    // UpdateInvasion incorporates a world sync, so this is indeed synced as a result.
                    Main.rainTime += Main.rand.Next(240, 300 + 1); // Add some time to the rain, so that it doesn't end mid-way.
                }
                Dictionary<int, AcidRainSpawnData> possibleMinibosses = CalamityWorld.downedPolterghast ? AcidRainEvent.PossibleMinibossesPolter : AcidRainEvent.PossibleMinibossesAS;
                if (possibleMinibosses.Select(miniboss => miniboss.Key).Contains(npc.type))
                {
                    CalamityWorld.acidRainPoints -= possibleMinibosses[npc.type].InvasionContributionPoints;
                    if (CalamityWorld.downedPolterghast)
                    {
                        CalamityWorld.acidRainPoints = (int)MathHelper.Max(1, CalamityWorld.acidRainPoints); // Cap at 1. The last point is for Old Duke.
                    }

                    // UpdateInvasion incorporates a world sync, so this is indeed synced as a result.
                    Main.rainTime += Main.rand.Next(1800, 2100 + 1); // Add some time to the rain, so that it doesn't end mid-way.
                }
            }

            CalamityWorld.acidRainPoints = (int)MathHelper.Max(0, CalamityWorld.acidRainPoints); // To prevent negative completion ratios

            if (CalamityWorld.rainingAcid && CalamityWorld.downedPolterghast && 
                npc.type == ModContent.NPCType<OldDuke.OldDuke>() &&
                CalamityWorld.acidRainPoints <= 2f)
            {
                CalamityWorld.triedToSummonOldDuke = false;
                CalamityWorld.acidRainPoints = 0;
            }
            CalamityWorld.timeSinceAcidRainKill = 0;
            AcidRainEvent.UpdateInvasion();
        }
        #endregion

        #region Common Loot
        private void CommonLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Vulture:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<DesertFeather>(), 2, 1, Main.expertMode ? 2 : 1);
                    break;

                case NPCID.RedDevil:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<EssenceofChaos>(), Main.expertMode ? 1f : 0.5f);
                    break;

                case NPCID.WyvernHead:
                    DropHelper.DropItem(npc.GetSource_FromThis(), npc, ModContent.ItemType<EssenceofCinder>(), 1, Main.expertMode ? 2 : 1);
                    break;

                case NPCID.AngryNimbus:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<EssenceofCinder>(), Main.expertMode ? 2 : 3);
                    break;

                case NPCID.IcyMerman:
                case NPCID.IceTortoise:
                case NPCID.IceElemental:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<EssenceofEleum>(), Main.expertMode ? 2 : 3);
                    break;

                case NPCID.IceGolem:
                    DropHelper.DropItem(npc.GetSource_FromThis(), npc, ModContent.ItemType<EssenceofEleum>(), 1, 2);
                    break;

                case NPCID.Plantera:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<LivingShard>(), !Main.expertMode, 12, 18);
                    break;

                case NPCID.SolarSpearman: //Drakanian
                case NPCID.SolarSolenian: //Selenian
                case NPCID.SolarCorite:
                case NPCID.SolarSroller:
                case NPCID.SolarDrakomireRider:
                case NPCID.SolarDrakomire:
                case NPCID.SolarCrawltipedeHead:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<MeldBlob>(), Main.expertMode ? 4 : 5);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.FragmentSolar, Main.expertMode ? 4 : 5);
                    break;

                case NPCID.VortexSoldier: //Vortexian
                case NPCID.VortexLarva: //Alien Larva
                case NPCID.VortexHornet: //Alien Hornet
                case NPCID.VortexHornetQueen: //Alien Queen
                case NPCID.VortexRifleman: //Storm Diver
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<MeldBlob>(), Main.expertMode ? 4 : 5);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.FragmentVortex, Main.expertMode ? 4 : 5);
                    break;

                case NPCID.NebulaBrain: //Nebula Floater
                case NPCID.NebulaSoldier: //Predictor
                case NPCID.NebulaHeadcrab: //Brain Suckler
                case NPCID.NebulaBeast: //Evolution Beast
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<MeldBlob>(), Main.expertMode ? 4 : 5);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.FragmentNebula, Main.expertMode ? 4 : 5);
                    break;

                case NPCID.StardustSoldier: //Stargazer
                case NPCID.StardustSpiderBig: //Twinkle Popper
                case NPCID.StardustJellyfishBig: //Flow Invader
                case NPCID.StardustCellBig: //Star Cell
                case NPCID.StardustWormHead: //Milkyway Weaver
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<MeldBlob>(), Main.expertMode ? 4 : 5);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.FragmentStardust, Main.expertMode ? 4 : 5);
                    break;

                case NPCID.DungeonGuardian:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<GoldBurdenBreaker>(), Main.hardMode);
                    break;

                case NPCID.CultistBoss:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<StardustStaff>(), Main.expertMode ? 3 : 5);
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<ThornBlossom>(), DropHelper.RareVariantDropRateInt);
                    break;

                case NPCID.EyeofCthulhu:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<VictoryShard>(), !Main.expertMode, 2, 4);
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<TeardropCleaver>(), !Main.expertMode, 5, 1, 1);
                    break;

                case NPCID.QueenBee:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.Stinger, !Main.expertMode, 5, 10);
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<HardenedHoneycomb>(), !Main.expertMode, 30, 50);
                    break;

                case NPCID.AngryTrapper:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<TrapperBulb>(), Main.expertMode ? 4 : 5);
                    break;

                case NPCID.MotherSlime:
                case NPCID.CorruptSlime:
                case NPCID.Crimslime:
                case NPCID.BigCrimslime:
                case NPCID.LittleCrimslime:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<MurkySludge>(), Main.expertMode ? 3 : 4);
                    break;

                case NPCID.Derpling:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<BeetleJuice>(), Main.expertMode ? 4 : 5);
                    break;

                case NPCID.SpikedJungleSlime:
                case NPCID.Arapaima:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<MurkyPaste>(), Main.expertMode ? 4 : 5);
                    break;

                case NPCID.Reaper:
                case NPCID.Psycho:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<SolarVeil>(), CalamityWorld.downedCalamitas || NPC.downedPlantBoss, Main.expertMode ? 0.75f : 0.5f, 1, 4);
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<DarksunFragment>(), CalamityWorld.buffedEclipse, Main.expertMode ? 0.06f : 0.04f, 1, 1);
                    break;

				//other solar eclipse creatures
                case NPCID.Eyezor:
                case NPCID.Frankenstein:
                case NPCID.SwampThing:
                case NPCID.Vampire:
                case NPCID.VampireBat:
                case NPCID.CreatureFromTheDeep:
                case NPCID.Fritz:
                case NPCID.ThePossessed:
                case NPCID.Butcher:
                case NPCID.DeadlySphere:
                case NPCID.DrManFly:
                case NPCID.Nailhead:
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ModContent.ItemType<DarksunFragment>(), CalamityWorld.buffedEclipse, Main.expertMode ? 0.06f : 0.04f, 1, 1);
                    break;

                case NPCID.MartianOfficer:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<ShockGrenade>(), Main.expertMode ? 3 : 4, 3, 8);
                    break;
                case NPCID.BrainScrambler:
                case NPCID.GrayGrunt:
                case NPCID.GigaZapper:
                case NPCID.MartianEngineer:
                case NPCID.RayGunner:
                case NPCID.ScutlixRider:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<ShockGrenade>(), Main.expertMode ? 4 : 5, 1, 4);
                    break;

                case NPCID.Gastropod:
                    DropHelper.DropItem(npc.GetSource_FromThis(), npc, ItemID.PinkGel, 5, 10);
                    break;

                default:
                    break;
            }

            // All hardmode dungeon enemies drop Ectoblood
            if (CalamityLists.dungeonEnemyBuffList.Contains(npc.type))
            {
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<Ectoblood>(), 2, 1, Main.expertMode ? 3 : 1);
            }

            // Every type of moss hornet can drop stingers
            if (CalamityLists.mossHornetList.Contains(npc.type))
            {
                DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ItemID.Stinger, Main.expertMode ? 1f : 0.6666f);
            }
        }
        #endregion

        #region Town NPC Loot
        private void TownNPCLoot(NPC npc)
        {
            const float TrasherEatDistance = 48f;

            if (npc.type == NPCID.Angler)
            {
                bool fedToTrasher = false;
                for(int i = 0; i < Main.maxNPCs; ++i)
                {
                    NPC nearby = Main.npc[i];
                    if (!nearby.active || nearby.type != ModContent.NPCType<Trasher>())
                        continue;
                    if (npc.Distance(nearby.Center) < TrasherEatDistance)
                    {
                        fedToTrasher = true;
                        break;
                    }
                }

                if (fedToTrasher)
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.GoldenFishingRod, Main.hardMode);
                else
                    DropHelper.DropItemCondition(npc.GetSource_FromThis(), npc, ItemID.GoldenFishingRod, Main.hardMode, 12, 1, 1);
            }
        }
		#endregion

		#region Boss Loot
		private void EventEnemyLoot(NPC npc, bool pumpkin, bool frost, bool eclipse)
		{
			// Not really loot code, but NPCLoot is the only death hook
			if (npc.boss && !CalamityWorld.downedBossAny)
			{
				CalamityWorld.downedBossAny = true;
				CalamityNetcode.SyncWorld();
			}

			// Nightmare Fuel, Endothermic Energy and Darksun Fragments
			if (!CalamityWorld.downedDoG)
			{
				return;
			}

			if (frost)
			{
				switch (npc.type)
				{
					case NPCID.Nutcracker:
					case NPCID.NutcrackerSpinning:
					case NPCID.ElfCopter:
					case NPCID.Flocko:
						DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<EndothermicEnergy>(), 2);
						break;
					case NPCID.Krampus:
					case NPCID.Yeti:
					case NPCID.PresentMimic:
						DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<EndothermicEnergy>(), 2, 1, 2);
						break;
					case NPCID.Everscream:
						DropHelper.DropItem(npc.GetSource_FromThis(), npc, ModContent.ItemType<EndothermicEnergy>(), 3, 5);
						break;
					case NPCID.SantaNK1:
						DropHelper.DropItem(npc.GetSource_FromThis(), npc, ModContent.ItemType<EndothermicEnergy>(), 5, 10);
						break;
					case NPCID.IceQueen:
						DropHelper.DropItem(npc.GetSource_FromThis(), npc, ModContent.ItemType<EndothermicEnergy>(), 10, 20);
						break;
				}
			}
			else if (pumpkin)
			{
				switch (npc.type)
				{
					case NPCID.Splinterling:
						DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<NightmareFuel>(), 2);
						break;
					case NPCID.Hellhound:
					case NPCID.Poltergeist:
						DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<NightmareFuel>(), 2, 1, 2);
						break;
					case NPCID.HeadlessHorseman:
						DropHelper.DropItem(npc.GetSource_FromThis(), npc, ModContent.ItemType<NightmareFuel>(), 3, 5);
						break;
					case NPCID.MourningWood:
						DropHelper.DropItem(npc.GetSource_FromThis(), npc, ModContent.ItemType<NightmareFuel>(), 5, 10);
						break;
					case NPCID.Pumpking:
						DropHelper.DropItem(npc.GetSource_FromThis(), npc, ModContent.ItemType<NightmareFuel>(), 10, 20);
						break;
				}
			}

			if (!CalamityWorld.buffedEclipse)
			{
				return;
			}

			if (npc.type == NPCID.Mothron)
			{
				DropHelper.DropItem(npc.GetSource_FromThis(), npc, ModContent.ItemType<DarksunFragment>(), 10, 20);

				// Mark a buffed Mothron as killed (allowing access to Yharon P2)
				CalamityWorld.downedBuffedMothron = true;
				CalamityNetcode.SyncWorld();
			}
		}
        #endregion
    }
}
