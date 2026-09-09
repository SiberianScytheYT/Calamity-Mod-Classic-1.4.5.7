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
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.NPCs
{
	public class CalamityGlobalNPCLoot : GlobalNPC
    {
		public static int halibutCannonBaseDropChance = 100000;
        
        // Internal function to determine whether this NPC is the second Twin killed in a fight, regardless of which Twin it is.
        public static bool IsLastTwinStanding(DropAttemptInfo info)
        {
            NPC npc = info.npc;
            if (npc is null)
                return false;
            if (npc.type == NPCID.Retinazer)
                return !NPC.AnyNPCs(NPCID.Spazmatism);
            else if (npc.type == NPCID.Spazmatism)
                return !NPC.AnyNPCs(NPCID.Retinazer);
            return false;
        }

        // Internal function to determine whether this NPC should drop the Mechanical Bosses combined lore item
        // Drops on the first mech boss killed (so the 2nd twin, Destroyer, or Skeletron Prime)
        public static bool ShouldDropMechLore(DropAttemptInfo info)
        {
            NPC npc = info.npc;
            if (npc is null)
                return false;
            bool lastTwinStanding = IsLastTwinStanding(info);
            return !NPC.downedMechBossAny && (lastTwinStanding || npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime);
        }

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
            
            if (npc.type == NPCID.KingSlime)
            {
                CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Dryad }, NPC.downedSlimeKing);
			}
            else if (npc.type == NPCID.EyeofCthulhu)
            {
               CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Merchant, NPCID.Dryad }, NPC.downedBoss1);
			}
            else if ((npc.boss && (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)) || npc.type == NPCID.BrainofCthulhu)
            {
                CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Merchant, NPCID.ArmsDealer, NPCID.Dryad }, NPC.downedBoss2);
			}
            else if (npc.type == NPCID.QueenBee)
            {
                CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.ArmsDealer, NPCID.Dryad }, NPC.downedQueenBee);
			}
            else if (npc.type == NPCID.SkeletronHead)
            {
               CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Merchant, NPCID.Dryad }, NPC.downedBoss3);
			}
            else if (npc.type == NPCID.WallofFlesh)
            {
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
                CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.DD2Bartender, NPCID.Stylist, NPCID.Truffle, ModContent.NPCType<THIEF>() }, NPC.downedMechBossAny);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Stylist, ModContent.NPCType<DILF>(), ModContent.NPCType<FAP>(), ModContent.NPCType<THIEF>() }, !NPC.downedMechBoss1 || NPC.downedMechBoss2 || !NPC.downedMechBoss3);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Steampunker }, NPC.downedMechBoss2 || !CalamityConfig.Instance.SellVanillaSummons);
			}
            else if (npc.type == NPCID.TheDestroyer)
            {
                CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.DD2Bartender, NPCID.Stylist, NPCID.Truffle, ModContent.NPCType<THIEF>() }, NPC.downedMechBossAny);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Stylist, ModContent.NPCType<DILF>(), ModContent.NPCType<FAP>(), ModContent.NPCType<THIEF>() }, NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Steampunker }, NPC.downedMechBoss1 || !CalamityConfig.Instance.SellVanillaSummons);
			}
            else if (npc.type == NPCID.SkeletronPrime)
            {
                CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.DD2Bartender, NPCID.Stylist, NPCID.Truffle, ModContent.NPCType<THIEF>() }, NPC.downedMechBossAny);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Stylist, ModContent.NPCType<DILF>(), ModContent.NPCType<FAP>(), ModContent.NPCType<THIEF>() }, !NPC.downedMechBoss1 || !NPC.downedMechBoss2 || NPC.downedMechBoss3);
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Steampunker }, NPC.downedMechBoss3 || !CalamityConfig.Instance.SellVanillaSummons);
            }
            else if (npc.type == NPCID.Plantera)
            {
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
                // Mark Betsy as dead (Vanilla does not keep track of her)
                CalamityWorld.downedBetsy = true;
                CalamityNetcode.SyncWorld();
            }
            else if (npc.type == NPCID.DukeFishron)
            {
				CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<SEAHOE>() }, NPC.downedFishron || !CalamityConfig.Instance.SellVanillaSummons);
			}
            else if (npc.type == NPCID.CultistBoss)
            {
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
			else if (npc.type == NPCID.VoodooDemon && Main.player[npc.target].Calamity().underworldLore)
			{
                NPCLoader.blockLoot.Add(ItemID.GuideVoodooDoll);
			}

            return true;
        }
        
        public static bool PreKillModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.KingSlime)
            {
                npcLoot.AddIf(() => Main.expertMode, ItemID.Gel, 1, 90, 120);
                npcLoot.AddIf(() => !Main.expertMode, ItemID.Gel, 1, 60, 80);

                npcLoot.AddConditionalPerPlayer(() => !NPC.downedSlimeKing, ModContent.ItemType<KnowledgeKingSlime>(), 1);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedSlimeKing, 2, 0, 0);
			}
            else if (npc.type == NPCID.EyeofCthulhu)
            {
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedBoss1, ModContent.ItemType<KnowledgeEyeofCthulhu>(), 1);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedBoss1, 2, 0, 0);
			}
            else if ((npc.boss && (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)) || npc.type == NPCID.BrainofCthulhu)
            {
                npcLoot.AddConditionalPerPlayer(() => !WorldGen.crimson && !NPC.downedBoss2, ModContent.ItemType<KnowledgeCorruption>(), true);
                npcLoot.AddConditionalPerPlayer(() => !WorldGen.crimson && !NPC.downedBoss2, ModContent.ItemType<KnowledgeEaterofWorlds>(), true);
                npcLoot.AddConditionalPerPlayer(() => WorldGen.crimson && !NPC.downedBoss2, ModContent.ItemType<KnowledgeCrimson>(), true);
                npcLoot.AddConditionalPerPlayer(() => WorldGen.crimson && !NPC.downedBoss2, ModContent.ItemType<KnowledgeBrainofCthulhu>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedBoss2, 2, 0, 0);
			}
            else if (npc.type == NPCID.QueenBee)
            {
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedQueenBee, ModContent.ItemType<KnowledgeQueenBee>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedQueenBee, 2, 0, 0);
			}
            else if (npc.type == NPCID.SkeletronHead)
            {
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<ClothiersWrath>(), DropHelper.RareVariantDropRateInt);
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedBoss3, ModContent.ItemType<KnowledgeSkeletron>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedBoss3, 3, 1, 0);
			}
            else if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.AddConditionalPerPlayer(() => !Main.expertMode && !CalamityWorld.demonMode, ModContent.ItemType<MLGRune>(), true); // Demon Trophy
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<Meowthrower>(), 5);
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<BlackHawkRemote>(), 5);
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<BlastBarrel>(), 5);
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<RogueEmblem>(), 8);
                npcLoot.AddIf(() => !Main.hardMode, ModContent.ItemType<IbarakiBox>(), 1); // 100% chance on first kill, 10% chance afterwards
                npcLoot.AddIf(() => Main.hardMode, ModContent.ItemType<IbarakiBox>(), 10);
                npcLoot.AddIf(() => !Main.expertMode, ItemID.CorruptionKey, 5);
                npcLoot.AddIf(() => !Main.expertMode, ItemID.CrimsonKey, 5);

                npcLoot.AddConditionalPerPlayer(() => !Main.hardMode, ModContent.ItemType<KnowledgeUnderworld>(), true);
                npcLoot.AddConditionalPerPlayer(() => !Main.hardMode, ModContent.ItemType<KnowledgeWallofFlesh>(), true);
                npcLoot.AddResidentEvilAmmo(info => Main.hardMode, 3, 1, 0);
            }
            else if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
            {
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedMechBoss2, ModContent.ItemType<KnowledgeTwins>(), 1);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedMechBoss2, 4, 2, 1);
                npcLoot.AddIf(() => CalamityGlobalNPC.DraedonMayhem, ModContent.ItemType<MysteriousCircuitry>(), 1, 8, 16);
                npcLoot.AddIf(() => CalamityGlobalNPC.DraedonMayhem, ModContent.ItemType<DubiousPlating>(), 1, 8, 16);
                npcLoot.AddConditionalPerPlayer(ShouldDropMechLore, ModContent.ItemType<KnowledgeMechs>(), 1);
			}
            else if (npc.type == NPCID.TheDestroyer)
            {
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedMechBoss1, ModContent.ItemType<KnowledgeDestroyer>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedMechBoss1, 4, 2, 1);
                npcLoot.AddIf(() => CalamityGlobalNPC.DraedonMayhem, ModContent.ItemType<MysteriousCircuitry>(), 1, 8, 16);
                npcLoot.AddIf(() => CalamityGlobalNPC.DraedonMayhem,  ModContent.ItemType<DubiousPlating>(), 1, 8, 16);
                npcLoot.AddConditionalPerPlayer(ShouldDropMechLore, ModContent.ItemType<KnowledgeMechs>(), 1);
			}
            else if (npc.type == NPCID.SkeletronPrime)
            {
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedMechBoss3, ModContent.ItemType<KnowledgeSkeletronPrime>(), true);
                npcLoot.AddConditionalPerPlayer(info => info.npc.ai[1] == 2f && CalamityWorld.revenge, ModContent.ItemType<GoldBurdenBreaker>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedMechBoss3, 4, 2, 1);
                npcLoot.AddIf(() => CalamityGlobalNPC.DraedonMayhem, ModContent.ItemType<MysteriousCircuitry>(), 1, 8, 16);
                npcLoot.AddIf(() => CalamityGlobalNPC.DraedonMayhem, ModContent.ItemType<DubiousPlating>(), 1, 8, 16);
                npcLoot.AddConditionalPerPlayer(ShouldDropMechLore, ModContent.ItemType<KnowledgeMechs>(), 1);
            }
            else if (npc.type == NPCID.Plantera)
            {
                npcLoot.AddIf(() => !Main.expertMode, ItemID.JungleKey, 5);
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedPlantBoss, ModContent.ItemType<KnowledgePlantera>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedPlantBoss, 4, 2, 1);
            }
            else if (npc.type == NPCID.Golem)
            {
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<EssenceofCinder>(), 1, 5, 10);
				npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<LeadWizard>(), DropHelper.RareVariantDropRateInt);
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedGolemBoss, ItemID.Picksaw, true);
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedGolemBoss, ModContent.ItemType<KnowledgeGolem>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedGolemBoss, 4, 2, 1);
            }
            else if (npc.type == NPCID.DD2Betsy && !CalamityWorld.downedBetsy)
            {
                npcLoot.AddResidentEvilAmmo(info => !CalamityWorld.downedBetsy, 4, 2, 1);
            }
            else if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<DukesDecapitator>(), 5);
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedFishron, ModContent.ItemType<KnowledgeDukeFishron>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedPlantBoss, 4, 2, 1);
			}
            else if (npc.type == NPCID.CultistBoss)
            {
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedAncientCultist, ModContent.ItemType<KnowledgeLunaticCultist>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedAncientCultist, 4, 2, 1);

                // Blood Moon lore item
                npcLoot.AddConditionalPerPlayer(() => Main.bloodMoon, ModContent.ItemType<KnowledgeBloodMoon>(), true);
            }
            else if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.AddIf(() => !Main.expertMode, ItemID.LunarOre, 1, 50, 50);
                npcLoot.AddConditionalPerPlayer(() => !Main.expertMode, ModContent.ItemType<MLGRune2>(), true);
                npcLoot.AddIf(() => !Main.expertMode, ItemID.GravityGlobe);
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<UtensilPoker>(), 9);
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<GrandDad>(), DropHelper.RareVariantDropRateInt);
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<Infinity>(), DropHelper.RareVariantDropRateInt);
                npcLoot.AddConditionalPerPlayer(() => !NPC.downedMoonlord, ModContent.ItemType<KnowledgeMoonLord>(), true);
                npcLoot.AddResidentEvilAmmo(info => !NPC.downedMoonlord, 5, 2, 1);
            }
			//Since Calamity makes it spawn in pre-hardmode, don't want to cause other mods to freak out if they use it as a tier gate (like a new weapon or something)
			else if (npc.type == NPCID.GreenJellyfish)
			{
                npcLoot.AddIf(() => !Main.hardMode, ItemID.Glowstick, 1, 1, 4);
                npcLoot.AddIf(() => !Main.hardMode, ItemID.JellyfishNecklace, 10);
                npcLoot.AddIf(() => Main.expertMode && !Main.hardMode, ItemID.Megaphone, 5);
                npcLoot.AddIf(() => !Main.expertMode && !Main.hardMode, ItemID.Megaphone, 10);
                npcLoot.AddIf(() => CalamityWorld.defiled && !Main.hardMode,  ItemID.JellyfishNecklace, DropHelper.DefiledDropRateInt);
                npcLoot.AddIf(() => CalamityWorld.defiled && !Main.hardMode,  ItemID.Megaphone, DropHelper.DefiledDropRateInt);
                npcLoot.AddIf(() => Main.expertMode && !Main.hardMode, ModContent.ItemType<VitalJelly>(), 5);
                npcLoot.AddIf(() => !Main.expertMode && !Main.hardMode, ModContent.ItemType<VitalJelly>(), 7);
				return false;
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

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // not actually ran in PreKill, but it's named this way for consistency's sake.
            PreKillModifyNPCLoot(npc, npcLoot);
            DefiledLoot(npc, npcLoot);
            ArmageddonLoot(npc, npcLoot);
            RareLoot(npc, npcLoot);
            RareVariants(npc, npcLoot);
            CommonLoot(npc, npcLoot);
            TownNPCLoot(npc, npcLoot);
            EventEnemyLoot(npc, npcLoot);
        }

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

            AcidRainProgression(npc);
            CheckBossSpawn(npc);
            ArmorSetLoot(npc);
            EventEnemyKill(npc);
        }
        #endregion

        #region Defiled Loot
        private static void DefiledLoot(NPC npc, NPCLoot npcLoot)
        {
            var defiled = new LeadingConditionRule(DropHelper.If(() => CalamityWorld.defiled));
            switch (npc.type)
            {
                case NPCID.Werewolf:
                    defiled.Add(ItemID.MoonCharm, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.AdhesiveBandage, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.AnglerFish:
                    defiled.Add(ItemID.AdhesiveBandage, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DesertBeast:
                    defiled.Add(ItemID.AncientHorn, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ArmoredSkeleton:
                case NPCID.HeavySkeleton:
                    defiled.Add(ItemID.BeamSword, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.ArmorPolish, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Clown:
                    defiled.Add(ItemID.Bananarang, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ToxicSludge:
                    defiled.Add(ItemID.Bezoar, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.EyeofCthulhu:
                    defiled.Add(ItemID.Binoculars, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.WanderingEye:
                    defiled.Add(ItemID.BlackLens, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.CorruptSlime:
                    defiled.Add(ItemID.Blindfold, DropHelper.DefiledDropRateInt);
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
                    defiled.Add(ItemID.Keybrand, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.BoneFeather, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.MagnetSphere, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.WispinaBottle, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.UndeadMiner:
                    defiled.Add(ItemID.BonePickaxe, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ScutlixRider:
                    defiled.Add(ItemID.BrainScrambler, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Vampire:
                    defiled.Add(ItemID.BrokenBatWing, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.MoonStone, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.CaveBat:
                    defiled.Add(ItemID.ChainKnife, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.DepthMeter, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DarkCaster:
                    defiled.Add(ItemID.ClothierVoodooDoll, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.TallyCounter, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.PirateCaptain:
                    defiled.Add(ItemID.CoinGun, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.DiscountCard, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.Cutlass, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.LuckyCoin, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.PirateStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Reaper:
                    defiled.Add(ItemID.DeathSickle, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Demon:
                case NPCID.VoodooDemon:
                    defiled.Add(ItemID.DemonScythe, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DesertDjinn:
                    defiled.Add(ItemID.DjinnLamp, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.DjinnsCurse, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Shark:
                    defiled.Add(ItemID.DivingHelmet, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Pixie:
                case NPCID.Wraith:
                case NPCID.Mummy:
                    defiled.Add(ItemID.FastClock, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.RedDevil:
                    defiled.Add(ItemID.FireFeather, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.UnholyTrident, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.IceElemental:
                case NPCID.IcyMerman:
                    defiled.Add(ItemID.IceSickle, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.FrostStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ArmoredViking:
                    defiled.Add(ItemID.IceSickle, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.IceTortoise:
                    defiled.Add(ItemID.IceSickle, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.FrozenTurtleShell, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Harpy:
                    defiled.AddIf(() => Main.hardMode && !npc.SpawnedFromStatue, ItemID.GiantHarpyFeather, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Piranha:
                    defiled.Add(ItemID.Hook, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.PinkJellyfish:
                case NPCID.BlueJellyfish:
                    defiled.Add(ItemID.JellyfishNecklace, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Paladin:
                    defiled.Add(ItemID.Kraken, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.PaladinsHammer, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.SkeletonArcher:
                    defiled.Add(ItemID.Marrow, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.MagicQuiver, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Lavabat:
                    defiled.Add(ItemID.MagmaStone, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.WalkingAntlion:
                    defiled.Add(ItemID.AntlionClaw, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DarkMummy:
                    defiled.Add(ItemID.Blindfold, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.Megaphone, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.GreenJellyfish:
                    defiled.Add(ItemID.Megaphone, DropHelper.DefiledDropRateInt);
                    defiled.Add(ItemID.JellyfishNecklace, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.CursedSkull:
                    defiled.Add(ItemID.Nazar, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.FireImp:
                    defiled.Add(ItemID.ObsidianRose, DropHelper.DefiledDropRateInt);
                    defiled.AddIf(() => NPC.downedBoss3,  ItemID.Cascade, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.BlackRecluse:
                case NPCID.BlackRecluseWall:
                    defiled.Add(ItemID.PoisonStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.ChaosElemental:
                    defiled.Add(ItemID.RodofDiscord, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.SnowFlinx:
                    defiled.Add(ItemID.SnowballLauncher, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Plantera:
                    defiled.Add(ItemID.TheAxe, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.GiantBat:
                    defiled.Add(ItemID.TrifoldMap, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.AngryTrapper:
                    defiled.Add(ItemID.Uzi, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Corruptor:
                case NPCID.FloatyGross:
                    defiled.Add(ItemID.Vitamins, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.GiantTortoise:
                    defiled.AddIf(() => NPC.downedMechBossAny, ItemID.Yelets, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DeadlySphere:
                    defiled.Add(ItemID.DeadlySphereStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.DrManFly:
                    defiled.Add(ItemID.ToxicFlask, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.CreatureFromTheDeep:
                    defiled.Add(ItemID.NeptunesShell, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Butcher:
                    defiled.Add(ItemID.ButchersChainsaw, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Psycho:
                    defiled.Add(ItemID.PsychoKnife, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Drippler:
                case NPCID.BloodZombie:
                    defiled.AddIf(() => !npc.SpawnedFromStatue,  ItemID.SharkToothNecklace, DropHelper.DefiledDropRateInt);
                    defiled.AddIf(() => !npc.SpawnedFromStatue,  ItemID.MoneyTrough, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.GoblinWarrior:
                    defiled.Add(ItemID.Harpoon, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.Pinky:
                    defiled.Add(ItemID.SlimeStaff, DropHelper.DefiledDropRateInt);
                    break;

                case NPCID.FlyingSnake:
                    defiled.Add(ItemID.LizardEgg, DropHelper.DefiledDropRateInt);
                    break;

                default:
                    break;
            }

            // Every type of demon eye counts for Black Lenses
            if (CalamityLists.demonEyeList.Contains(npc.type))
            {
                defiled.Add(ItemID.BlackLens, DropHelper.DefiledDropRateInt);
            }

            // Every type of Skeleton counts for the Bone Sword
            if (CalamityLists.skeletonList.Contains(npc.type) && npc.type != NPCID.ArmoredSkeleton && npc.type != NPCID.HeavySkeleton && npc.type != NPCID.SkeletonArcher && npc.type != NPCID.GreekSkeleton)
            {
                defiled.Add(ItemID.BoneSword, DropHelper.DefiledDropRateInt);
            }

            // Every type of Angry Bones counts for the Clothier Voodoo Doll
            if (CalamityLists.angryBonesList.Contains(npc.type))
            {
                defiled.Add(ItemID.ClothierVoodooDoll, DropHelper.DefiledDropRateInt);
            }

            // Every type of hornet AND moss hornet can drop Bezoar
            if (CalamityLists.hornetList.Contains(npc.type) || CalamityLists.mossHornetList.Contains(npc.type))
            {
                defiled.Add(ItemID.Bezoar, DropHelper.DefiledDropRateInt);
            }

            // Every type of moss hornet can drop Tattered Bee Wings
            if (CalamityLists.mossHornetList.Contains(npc.type))
            {
                defiled.Add(ItemID.TatteredBeeWing, DropHelper.DefiledDropRateInt);
            }

            // Because all switch cases must be constant at compile time, modded NPC IDs (which can change) can't be included.
            if (npc.type == ModContent.NPCType<SunBat>())
            {
                defiled.Add(ItemID.HelFire, DropHelper.DefiledDropRateInt);
            }
            else if (npc.type == ModContent.NPCType<Cryon>())
            {
                defiled.Add(ItemID.Amarok, DropHelper.DefiledDropRateInt);
            }
            npcLoot.Add(defiled);
        }
        #endregion

        #region Armageddon Loot
        private void ArmageddonLoot(NPC npc, NPCLoot npcLoot)
        {
            var armageddon = new LeadingConditionRule(DropHelper.If(() => CalamityWorld.armageddon));
            switch (npc.type)
            {
                case NPCID.KingSlime: 
                    armageddon.AddArmageddonBags(ItemID.KingSlimeBossBag);
                    break;
                case NPCID.EyeofCthulhu:
                    armageddon.AddArmageddonBags(ItemID.EyeOfCthulhuBossBag);
                    break;
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    armageddon.AddIf(info => info.npc.boss, ItemID.EaterOfWorldsBossBag, 1, DropHelper.ArmageddonExtraBags, DropHelper.ArmageddonExtraBags);

                    break;

                case NPCID.BrainofCthulhu:
                    armageddon.AddArmageddonBags(ItemID.BrainOfCthulhuBossBag);
                    break;
                case NPCID.QueenBee:
                    armageddon.AddArmageddonBags(ItemID.QueenBeeBossBag);
                    break;
                case NPCID.SkeletronHead:
                    armageddon.AddArmageddonBags(ItemID.SkeletronBossBag);
                    break;
                case NPCID.WallofFlesh:
                    armageddon.AddArmageddonBags(ItemID.WallOfFleshBossBag);
                    break;
                case NPCID.Retinazer: // only drop if spaz is already dead
                    armageddon.AddIf(IsLastTwinStanding, ItemID.TwinsBossBag, 1, DropHelper.ArmageddonExtraBags, DropHelper.ArmageddonExtraBags);
                    break;

                case NPCID.Spazmatism: // only drop if ret is already dead
                    armageddon.AddIf(IsLastTwinStanding, ItemID.TwinsBossBag, 1, DropHelper.ArmageddonExtraBags, DropHelper.ArmageddonExtraBags);
                    break;

                case NPCID.TheDestroyer:
                    armageddon.AddArmageddonBags(ItemID.DestroyerBossBag);
                    break;
                case NPCID.SkeletronPrime:
                    armageddon.AddArmageddonBags(ItemID.SkeletronPrimeBossBag);
                    break;
                case NPCID.Plantera:
                    armageddon.AddArmageddonBags(ItemID.PlanteraBossBag);
                    break;
                case NPCID.Golem:
                    armageddon.AddArmageddonBags(ItemID.GolemBossBag);
                    break;
                case NPCID.DD2Betsy:
                    armageddon.AddArmageddonBags(ItemID.BossBagBetsy);
                    break;
                case NPCID.DukeFishron:
                    armageddon.AddArmageddonBags(ItemID.FishronBossBag);
                    break;
                case NPCID.MoonLordCore:
                    armageddon.AddArmageddonBags(ItemID.MoonLordBossBag);
                    break;

                default:
                    break;
            }
            npcLoot.Add(armageddon);
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
        private void RareLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.Drippler:
                    npcLoot.AddIf(() => CalamityWorld.defiled, ModContent.ItemType<BouncingEyeball>(), DropHelper.DefiledDropRateInt);
                    npcLoot.AddIf(() => !CalamityWorld.defiled, ModContent.ItemType<BouncingEyeball>(), 300);
                    break;

                case NPCID.FireImp:
                    npcLoot.AddIf(() => CalamityWorld.defiled, ModContent.ItemType<AshenStalactite>(), DropHelper.DefiledDropRateInt);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<AshenStalactite>(), 100);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<AshenStalactite>(), 150);
                    break;

                case NPCID.PossessedArmor:
                    npcLoot.AddIf(() => CalamityWorld.defiled, ModContent.ItemType<PsychoticAmulet>(), DropHelper.DefiledDropRateInt);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<PsychoticAmulet>(), 150);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<PsychoticAmulet>(), 200);
                    break;

                case NPCID.SeaSnail:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<SeaShell>(), 2);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<SeaShell>(), 3);
                    break;

                case NPCID.GreekSkeleton:
                    npcLoot.AddIf(() => Main.expertMode, ItemID.GladiatorHelmet, 15);
                    npcLoot.AddIf(() => Main.expertMode, ItemID.GladiatorBreastplate, 15);
                    npcLoot.AddIf(() => Main.expertMode, ItemID.GladiatorLeggings, 15);
                    npcLoot.AddIf(() => !Main.expertMode, ItemID.GladiatorHelmet, 20);
                    npcLoot.AddIf(() => !Main.expertMode, ItemID.GladiatorBreastplate, 20);
                    npcLoot.AddIf(() => !Main.expertMode, ItemID.GladiatorLeggings, 20);
                    break;

                case NPCID.GiantTortoise:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<GiantTortoiseShell>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<GiantTortoiseShell>(), 7);
                    npcLoot.Add(ModContent.ItemType<FabledTortoiseShell>(), 200);
                    break;

                case NPCID.GiantShelly:
                case NPCID.GiantShelly2:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<GiantShell>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<GiantShell>(), 7);
                    break;

                case NPCID.AnomuraFungus:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<FungalCarapace>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<FungalCarapace>(), 7);
                    break;

                case NPCID.Crawdad:
                case NPCID.Crawdad2:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<CrawCarapace>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<CrawCarapace>(), 7);
                    break;

                case NPCID.GreenJellyfish:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<VitalJelly>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<VitalJelly>(), 7);
                    break;

                case NPCID.PinkJellyfish:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<LifeJelly>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<LifeJelly>(), 25);
                    break;

                case NPCID.BlueJellyfish:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<ManaJelly>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<ManaJelly>(), 7);
                    break;

                case NPCID.DarkCaster:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<AncientShiv>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<AncientShiv>(), 25);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<ShinobiBlade>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<ShinobiBlade>(), 25);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<StaffOfNecrosteocytes>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<StaffOfNecrosteocytes>(), 25);
                    break;

                case NPCID.BigMimicHallow:
                case NPCID.BigMimicCorruption:
                case NPCID.BigMimicCrimson:
                case NPCID.BigMimicJungle: // arguably unnecessary
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<CelestialClaymore>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<CelestialClaymore>(), 7);
                    break;

                case NPCID.Clinger:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<CursedDagger>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<CursedDagger>(), 25);
                    break;

                case NPCID.Shark:
                    npcLoot.AddIf(() => Main.expertMode, ItemID.SharkToothNecklace, 20);
                    npcLoot.AddIf(() => !Main.expertMode, ItemID.SharkToothNecklace, 30);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<JoyfulHeart>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<JoyfulHeart>(), 30);
                    break;

                case NPCID.PresentMimic:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<HolidayHalberd>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<HolidayHalberd>(), 7);
                    break;

                case NPCID.IchorSticker:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<IchorSpear>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<IchorSpear>(), 25);
                    npcLoot.Add(ModContent.ItemType<SpearofDestiny>(), 200);
                    break;

                case NPCID.Harpy:
                    npcLoot.AddIf(() => CalamityWorld.defiled && NPC.downedBoss1, ModContent.ItemType<SkyGlaze>(), DropHelper.DefiledDropRateInt);
                    npcLoot.AddIf(() => Main.expertMode && NPC.downedBoss1, ModContent.ItemType<SkyGlaze>(), 60);
                    npcLoot.AddIf(() => !Main.expertMode && NPC.downedBoss1, ModContent.ItemType<SkyGlaze>(), 80);
                    npcLoot.AddIf(() => Main.expertMode && Main.hardMode && !npc.SpawnedFromStatue,  ModContent.ItemType<EssenceofCinder>(), 2);
                    npcLoot.AddIf(() => !Main.expertMode && Main.hardMode && !npc.SpawnedFromStatue,  ModContent.ItemType<EssenceofCinder>(), 3);
                    break;

                case NPCID.Antlion:
                case NPCID.WalkingAntlion:
                case NPCID.FlyingAntlion:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<MandibleClaws>(), 30);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<MandibleClaws>(), 40);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<MandibleBow>(), 30);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<MandibleBow>(), 40);
                    break;

                case NPCID.TombCrawlerHead:
                    DropHelper.DropItemChance(npc.GetSource_FromThis(), npc, ModContent.ItemType<BurntSienna>(), Main.expertMode ? 15 : 20);
                    break;

                case NPCID.DuneSplicerHead:
                    npcLoot.AddIf(() => NPC.downedPlantBoss && Main.expertMode, ModContent.ItemType<Terracotta>(), 20);
                    npcLoot.AddIf(() => NPC.downedPlantBoss && !Main.expertMode, ModContent.ItemType<Terracotta>(), 30);
                    break;

                case NPCID.MartianSaucerCore:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<NullificationRifle>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<NullificationRifle>(), 7);
                    break;

                case NPCID.Demon:
                case NPCID.VoodooDemon:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<DemonicBoneAsh>(), 2);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<DemonicBoneAsh>(), 3);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<BladecrestOathsword>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<BladecrestOathsword>(), 25);
                    break;

                case NPCID.BoneSerpentHead:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<DemonicBoneAsh>(), 2);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<DemonicBoneAsh>(), 3);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<OldLordOathsword>(), 10);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<OldLordOathsword>(), 15);
                    break;

                case NPCID.Tim:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<PlasmaRod>(), 2);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<PlasmaRod>(), 3);
                    break;

                case NPCID.GoblinSorcerer:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<PlasmaRod>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<PlasmaRod>(), 25);
                    break;

                case NPCID.PirateDeadeye:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<ProporsePistol>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<ProporsePistol>(), 25);
                    break;

                case NPCID.PirateCrossbower:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<RaidersGlory>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<RaidersGlory>(), 25);
                    npcLoot.Add(ModContent.ItemType<Arbalest>(), 200);
                    break;

                case NPCID.GoblinSummoner:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<TheFirstShadowflame>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<TheFirstShadowflame>(), 7);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<BurningStrife>(), 3);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<BurningStrife>(), 6);
                    break;

                case NPCID.SandElemental:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<WifeinaBottle>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<WifeinaBottle>(), 7);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<WifeinaBottlewithBoobs>(), 20);
                    break;

                case NPCID.GoblinWarrior:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<Warblade>(), 15);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<Warblade>(), 20);
                    break;

                case NPCID.MartianWalker:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<Wingman>(), 5);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<Wingman>(), 7);
                    break;

                case NPCID.GiantCursedSkull:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<WrathoftheAncients>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<WrathoftheAncients>(), 25);
                    npcLoot.AddIf(() => CalamityWorld.downedLeviathan, ModContent.ItemType<Keelhaul>(), 10);
                    break;

                case NPCID.Necromancer:
                case NPCID.NecromancerArmored:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<WrathoftheAncients>(), 20);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<WrathoftheAncients>(), 25);
                    break;

                case NPCID.DeadlySphere:
                    npcLoot.AddIf(() => CalamityWorld.defiled, ModContent.ItemType<DefectiveSphere>(), DropHelper.DefiledDropRateInt); //same as deadly sphere staff
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<DefectiveSphere>(), 26); 
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<DefectiveSphere>(), 40); 
                    break;

                case NPCID.BloodJelly:
                case NPCID.FungoFish:
                    npcLoot.AddIf(() => CalamityWorld.defiled, ItemID.JellyfishNecklace, DropHelper.DefiledDropRateInt);
                    npcLoot.Add(ItemID.JellyfishNecklace, 100);
                    break;

                default:
                    break;
            }

            // Every type of Moss Hornet counts for the Needler
            if (CalamityLists.mossHornetList.Contains(npc.type))
            {
                npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<Needler>(), 20);
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<Needler>(), 25);
            }

            // Every type of Skeleton counts for the Waraxe and Ancient Bone Dust
            if (CalamityLists.skeletonList.Contains(npc.type))
            {
                npcLoot.AddIf(() => !Main.hardMode && Main.expertMode, ModContent.ItemType<Waraxe>(), 15);
                npcLoot.AddIf(() => !Main.hardMode && !Main.expertMode, ModContent.ItemType<Waraxe>(), 20);
                npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<AncientBoneDust>(), 4);
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<AncientBoneDust>(), 5);
            }
        }
        #endregion

        #region Rare Variants
        private void RareVariants(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.BloodZombie:
                    npcLoot.AddIf(() => NPC.downedBoss3 && !npc.SpawnedFromStatue,  ModContent.ItemType<Carnage>(), 200);
                    break;

                case NPCID.VortexRifleman:
                    npcLoot.Add(ModContent.ItemType<TrueConferenceCall>(), 200);
                    break;

                case NPCID.DesertBeast:
                    npcLoot.Add(ModContent.ItemType<EvilSmasher>(), 200);
                    break;

                case NPCID.DungeonSpirit:
                    npcLoot.Add(ModContent.ItemType<PearlGod>(), 200);
                    break;

                case NPCID.RuneWizard:
                    npcLoot.Add(ModContent.ItemType<EyeofMagnus>(), 10);
                    break;

                case NPCID.Mimic:
                    npcLoot.AddIf(() => !npc.SpawnedFromStatue, ModContent.ItemType<TheBee>(), 100);
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
        private void CommonLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.Vulture:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<DesertFeather>(), 2, 1,  2);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<DesertFeather>(), 2);
                    break;

                case NPCID.RedDevil:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<EssenceofChaos>());
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<EssenceofChaos>(), 2);
                    break;

                case NPCID.WyvernHead:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<EssenceofCinder>(), 1, 1,  2);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<EssenceofCinder>());
                    break;

                case NPCID.AngryNimbus:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<EssenceofCinder>(), 2);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<EssenceofCinder>(), 3);
                    break;

                case NPCID.IcyMerman:
                case NPCID.IceTortoise:
                case NPCID.IceElemental:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<EssenceofEleum>(), 2);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<EssenceofEleum>(), 3);
                    break;

                case NPCID.IceGolem:
                    npcLoot.Add(ModContent.ItemType<EssenceofEleum>(), 1, 1, 2);
                    break;

                case NPCID.Plantera:
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<LivingShard>(), 1, 12, 18);
                    break;

                case NPCID.SolarSpearman: //Drakanian
                case NPCID.SolarSolenian: //Selenian
                case NPCID.SolarCorite:
                case NPCID.SolarSroller:
                case NPCID.SolarDrakomireRider:
                case NPCID.SolarDrakomire:
                case NPCID.SolarCrawltipedeHead:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<MeldBlob>(), 4);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<MeldBlob>(), 5);
                    npcLoot.AddIf(() => Main.expertMode, ItemID.FragmentSolar, 4);
                    npcLoot.AddIf(() => !Main.expertMode, ItemID.FragmentSolar, 5);
                    break;

                case NPCID.VortexSoldier: //Vortexian
                case NPCID.VortexLarva: //Alien Larva
                case NPCID.VortexHornet: //Alien Hornet
                case NPCID.VortexHornetQueen: //Alien Queen
                case NPCID.VortexRifleman: //Storm Diver
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<MeldBlob>(), 4);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<MeldBlob>(), 5);
                    npcLoot.AddIf(() => Main.expertMode, ItemID.FragmentVortex, 4);
                    npcLoot.AddIf(() => !Main.expertMode, ItemID.FragmentVortex, 5);
                    break;

                case NPCID.NebulaBrain: //Nebula Floater
                case NPCID.NebulaSoldier: //Predictor
                case NPCID.NebulaHeadcrab: //Brain Suckler
                case NPCID.NebulaBeast: //Evolution Beast
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<MeldBlob>(), 4);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<MeldBlob>(), 5);
                    npcLoot.AddIf(() => Main.expertMode, ItemID.FragmentNebula, 4);
                    npcLoot.AddIf(() => !Main.expertMode, ItemID.FragmentNebula, 5);
                    break;

                case NPCID.StardustSoldier: //Stargazer
                case NPCID.StardustSpiderBig: //Twinkle Popper
                case NPCID.StardustJellyfishBig: //Flow Invader
                case NPCID.StardustCellBig: //Star Cell
                case NPCID.StardustWormHead: //Milkyway Weaver
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<MeldBlob>(), 4);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<MeldBlob>(), 5);
                    npcLoot.AddIf(() => Main.expertMode, ItemID.FragmentStardust, 4);
                    npcLoot.AddIf(() => !Main.expertMode, ItemID.FragmentStardust, 5);
                    break;

                case NPCID.DungeonGuardian:
                    npcLoot.AddIf(() => Main.hardMode, ModContent.ItemType<GoldBurdenBreaker>());
                    break;

                case NPCID.CultistBoss:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<StardustStaff>(), 3);
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<StardustStaff>(), 5);
                    npcLoot.Add(ModContent.ItemType<ThornBlossom>(), DropHelper.RareVariantDropRateInt);
                    break;

                case NPCID.EyeofCthulhu:
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<VictoryShard>(), 1, 2, 4);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<TeardropCleaver>(), 5);
                    break;

                case NPCID.QueenBee:
                    npcLoot.AddIf(() => !Main.expertMode, ItemID.Stinger, 1, 5, 10);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<HardenedHoneycomb>(), 1, 30, 50);
                    break;

                case NPCID.AngryTrapper:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<TrapperBulb>(), 4);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<TrapperBulb>(), 5);
                    break;

                case NPCID.MotherSlime:
                case NPCID.CorruptSlime:
                case NPCID.Crimslime:
                case NPCID.BigCrimslime:
                case NPCID.LittleCrimslime:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<MurkySludge>(), 3);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<MurkySludge>(), 4);
                    break;

                case NPCID.Derpling:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<BeetleJuice>(), 4);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<BeetleJuice>(), 5);
                    break;

                case NPCID.SpikedJungleSlime:
                case NPCID.Arapaima:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<MurkyPaste>(), 4);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<MurkyPaste>(), 5);
                    break;

                case NPCID.Reaper:
                case NPCID.Psycho:
                    npcLoot.AddIf(() => (CalamityWorld.downedCalamitas || NPC.downedPlantBoss) && Main.expertMode, ModContent.ItemType<SolarVeil>(), 1, 1, 4);
                    npcLoot.AddIf(() => (CalamityWorld.downedCalamitas || NPC.downedPlantBoss) && !Main.expertMode, ModContent.ItemType<SolarVeil>(), 2, 1, 4);
                    npcLoot.AddIf(() => CalamityWorld.buffedEclipse && Main.expertMode, ModContent.ItemType<DarksunFragment>(), 17);
                    npcLoot.AddIf(() => CalamityWorld.buffedEclipse && !Main.expertMode, ModContent.ItemType<DarksunFragment>(), 25);
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
                    npcLoot.AddIf(() => CalamityWorld.buffedEclipse && Main.expertMode, ModContent.ItemType<DarksunFragment>(), 16);
                    npcLoot.AddIf(() => CalamityWorld.buffedEclipse && !Main.expertMode, ModContent.ItemType<DarksunFragment>(), 25); 
                    break;

                case NPCID.MartianOfficer:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<ShockGrenade>(), 3, 3, 8);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<ShockGrenade>(), 4, 3, 8);
                    break;
                case NPCID.BrainScrambler:
                case NPCID.GrayGrunt:
                case NPCID.GigaZapper:
                case NPCID.MartianEngineer:
                case NPCID.RayGunner:
                case NPCID.ScutlixRider:
                    npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<ShockGrenade>(), 4, 1, 4);
                    npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<ShockGrenade>(), 5, 1, 4);
                    break;

                case NPCID.Gastropod:
                    npcLoot.Add(ItemID.PinkGel, 1, 5, 10);
                    break;

                default:
                    break;
            }

            // All hardmode dungeon enemies drop Ectoblood
            if (CalamityLists.dungeonEnemyBuffList.Contains(npc.type))
            {
                npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<Ectoblood>(), 2, 1, 3);
                npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<Ectoblood>(), 2);
            }

            // Every type of moss hornet can drop stingers
            if (CalamityLists.mossHornetList.Contains(npc.type))
            {
                npcLoot.AddIf(() => Main.expertMode, ItemID.Stinger, 1);
                npcLoot.AddIf(() => !Main.expertMode, ItemID.Stinger, 2);
            }
        }
        #endregion

        #region Town NPC Loot
        private void TownNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Angler)
            {
                LeadingConditionRule trasherLCR = new LeadingConditionRule(AnglerFedToTrasherCondition);
                trasherLCR.Add(ItemDropRule.ByCondition(TrasherText, ItemID.GoldenFishingRod));
                trasherLCR.OnFailedConditions(ItemDropRule.ByCondition(DropHelper.If(() => Main.hardMode), ItemID.GoldenFishingRod, 12));
                npcLoot.Add(trasherLCR);
            }
        }
        
        public static IItemDropRuleCondition TrasherText = DropHelper.If(() => true, true);
        
        public static IItemDropRuleCondition AnglerFedToTrasherCondition = DropHelper.If(info =>
        {
            const float TrasherEatDistance = 48f;
            
            bool trasherNearby = false;
            for (int i = 0; i < Main.maxNPCs; ++i)
            {
                NPC nearby = Main.npc[i];
                if (nearby is null || !nearby.active || nearby.type != ModContent.NPCType<Trasher>())
                    continue;
                if (info.npc.Distance(nearby.Center) < TrasherEatDistance)
                {
                    trasherNearby = true;
                    break;
                }
            }
            return trasherNearby;
        });
		#endregion

		#region Boss Loot
		private void EventEnemyLoot(NPC npc, NPCLoot npcLoot)
        {
            var downedDoG = new LeadingConditionRule(DropHelper.If(() => CalamityWorld.downedDoG));
			switch (npc.type)
            {
                case NPCID.Nutcracker:
                case NPCID.NutcrackerSpinning:
                case NPCID.ElfCopter:
                case NPCID.Flocko:
                    downedDoG.Add(ModContent.ItemType<EndothermicEnergy>(), 2);
                    break;
                case NPCID.Krampus:
                case NPCID.Yeti:
                case NPCID.PresentMimic:
                    downedDoG.Add(ModContent.ItemType<EndothermicEnergy>(), 2, 1, 2);
                    break;
                case NPCID.Everscream:
                    downedDoG.Add(ModContent.ItemType<EndothermicEnergy>(), 1, 3, 5);
                    break;
                case NPCID.SantaNK1:
                    downedDoG.Add(ModContent.ItemType<EndothermicEnergy>(), 1, 5, 10);
                    break;
                case NPCID.IceQueen:
                    downedDoG.Add(ModContent.ItemType<EndothermicEnergy>(), 1, 10, 20);
                    break;
                case NPCID.Splinterling:
                    downedDoG.Add(ModContent.ItemType<NightmareFuel>(), 2);
                    break;
                case NPCID.Hellhound:
                case NPCID.Poltergeist:
                    downedDoG.Add(ModContent.ItemType<NightmareFuel>(), 2, 1, 2);
                    break;
                case NPCID.HeadlessHorseman:
                    downedDoG.Add(ModContent.ItemType<NightmareFuel>(), 1, 3, 5);
                    break;
                case NPCID.MourningWood:
                    downedDoG.Add(ModContent.ItemType<NightmareFuel>(), 1, 5, 10);
                    break;
                case NPCID.Pumpking:
                    downedDoG.Add(ModContent.ItemType<NightmareFuel>(), 1, 10, 20);
                    break;
                case NPCID.Mothron:
                    npcLoot.AddIf(() => CalamityWorld.buffedEclipse, ModContent.ItemType<DarksunFragment>(), 1, 10, 20);
                    break;
            }
            npcLoot.Add(downedDoG);
        }
        
        private void EventEnemyKill(NPC npc)
		{
			// Not really loot code, but NPCLoot is the only death hook
			if (npc.boss && !CalamityWorld.downedBossAny)
			{
				CalamityWorld.downedBossAny = true;
				CalamityNetcode.SyncWorld();
			}

			if (!CalamityWorld.buffedEclipse)
			{
				return;
			}

			if (npc.type == NPCID.Mothron)
			{
				// Mark a buffed Mothron as killed (allowing access to Yharon P2)
				CalamityWorld.downedBuffedMothron = true;
				CalamityNetcode.SyncWorld();
			}
		}
        #endregion
    }
}
