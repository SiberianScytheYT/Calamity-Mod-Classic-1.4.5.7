using CalRD.Items;
using CalRD.Items.Accessories;
using CalRD.Items.Accessories.Wings;
using CalRD.Items.Critters;
using CalRD.Items.Fishing.AstralCatches;
using CalRD.Items.Fishing.BrimstoneCragCatches;
using CalRD.Items.Fishing.SunkenSeaCatches;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Items.Potions;
using CalRD.Items.Weapons.Melee;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD
{
    internal class CalamityRecipes
    {
        private static Recipe GetNewRecipe(int itemType, int amount)
        {
            return /* ModContent.GetInstance<CalRD>() */Recipe.Create(itemType, amount);
        }
        
        private static Recipe GetNewRecipe(int itemType)
        {
            return /* ModContent.GetInstance<CalRD>() */Recipe.Create(itemType);
        }

        public static void AddRecipes()
        {
            //EditTerraBladeRecipe();
            //EditFireGauntletRecipe();
            AstralAlternatives();

            AddPotionRecipes();
            AddCookedFood();
            AddToolRecipes();
            AddProgressionRecipes();
            AddEarlyGameWeaponRecipes();
            AddEarlyGameAccessoryRecipes();
            AddArmorRecipes();
            AddAnkhShieldRecipes();
            AddAlternateHardmodeRecipes();

            // Leather from Vertebrae, for Crimson worlds
            Recipe r = GetNewRecipe(ItemID.Leather);
            r.AddIngredient(ItemID.Vertebrae, 5);
            r.AddTile(TileID.WorkBenches);
            r.Register();

            // Black Lens
            r = GetNewRecipe(ItemID.BlackLens);
            r.AddIngredient(ItemID.Lens);
            r.AddIngredient(ItemID.BlackDye);
            r.AddTile(TileID.DyeVat);
            r.Register();

            // Fallen Star
            r = GetNewRecipe(ItemID.FallenStar);
            r.AddIngredient(ModContent.ItemType<Stardust>(), 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Ectoplasm from Ectoblood
            r = GetNewRecipe(ItemID.Ectoplasm);
            r.AddIngredient(ModContent.ItemType<Ectoblood>(), 3);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Rocket I from Empty Bullet
            r = GetNewRecipe(ItemID.RocketI, 20);
            r.AddIngredient(ItemID.EmptyBullet, 20);
            r.AddIngredient(ItemID.ExplosivePowder, 1);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Life Crystal
            r = GetNewRecipe(ItemID.LifeCrystal);
            r.AddIngredient(ItemID.Bone, 5);
            r.AddIngredient(ItemID.PinkGel);
            r.AddIngredient(ItemID.HealingPotion);
            r.AddIngredient(ItemID.Ruby);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Life Fruit
            r = GetNewRecipe(ItemID.LifeFruit);
            r.AddIngredient(ModContent.ItemType<PlantyMush>(), 10);
            r.AddIngredient(ModContent.ItemType<LivingShard>());
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Target Dummy Reverse Compatibility
            r = GetNewRecipe(ItemID.TargetDummy);
            r.AddIngredient(ModContent.ItemType<SuperDummy>());
            r.AddTile(TileID.Anvils);
            r.Register();
        }

        /*
        // Change Terra Blade's recipe to require 7 Living Shards (forces the Blade to be post-Plantera)
        private static void EditTerraBladeRecipe()
        {
            List<Recipe> rec = Main.recipe.ToList();
            rec.Where(x => x.createItem.type == ItemID.TerraBlade).ToList().ForEach(s =>
            {
                for (int i = 0; i < s.requiredItem.Length; i++)
                {
                    s.requiredItem[i] = new Item();
                }
                s.requiredItem[0].SetDefaults(ItemID.TrueNightsEdge, false);
                s.requiredItem[0].stack = 1;
                s.requiredItem[1].SetDefaults(ItemID.TrueExcalibur, false);
                s.requiredItem[1].stack = 1;
                s.requiredItem[2].SetDefaults(ModContent.ItemType<LivingShard>(), false);
                s.requiredItem[2].stack = 7;

                s.createItem.SetDefaults(ItemID.TerraBlade, false);
                s.createItem.stack = 1;
            });
        }
        */

        /*
        // Change Fire Gauntlet's recipe to require 5 Chaotic Bars (forces the item to be post-Golem)
        private static void EditFireGauntletRecipe()
        {
            List<Recipe> rec = Main.recipe.ToList();
            rec.Where(x => x.createItem.type == ItemID.FireGauntlet).ToList().ForEach(s =>
            {
                for (int i = 0; i < s.requiredItem.Length; i++)
                {
                    s.requiredItem[i] = new Item();
                }
                s.requiredItem[0].SetDefaults(ItemID.MagmaStone, false);
                s.requiredItem[0].stack = 1;
                s.requiredItem[1].SetDefaults(ItemID.MechanicalGlove, false);
                s.requiredItem[1].stack = 1;
                s.requiredItem[2].SetDefaults(ModContent.ItemType<CruptixBar>(), false);
                s.requiredItem[2].stack = 5;

                s.createItem.SetDefaults(ItemID.FireGauntlet, false);
                s.createItem.stack = 1;
            });
        }
        */

        #region Astral Alternatives
        private static void AstralAlternatives()
        {
            //Bowl
            Recipe r = GetNewRecipe(ItemID.Bowl);
            r.AddIngredient(ModContent.ItemType<AstralClay>(), 2);
            r.AddTile(TileID.Furnaces);
            r.Register();

            //Clay Pot
            r = GetNewRecipe(ItemID.ClayPot);
            r.AddIngredient(ModContent.ItemType<AstralClay>(), 2);
            r.AddTile(TileID.Furnaces);
            r.Register();

            //Pink Vase
            r = GetNewRecipe(ItemID.PinkVase);
            r.AddIngredient(ModContent.ItemType<AstralClay>(), 2);
            r.AddTile(TileID.Furnaces);
            r.Register();
        }
        #endregion

        #region Potions
        // Equivalent Blood Orb recipes for almost all vanilla potions
        private static void AddPotionRecipes()
        {
            short[] potions = new[]
            {
                ItemID.WormholePotion,
                ItemID.TeleportationPotion,
                ItemID.SwiftnessPotion,
                ItemID.FeatherfallPotion,
                ItemID.GravitationPotion,
                ItemID.ShinePotion,
                ItemID.InvisibilityPotion,
                ItemID.NightOwlPotion,
                ItemID.SpelunkerPotion,
                ItemID.HunterPotion,
                ItemID.TrapsightPotion,
                ItemID.BattlePotion,
                ItemID.CalmingPotion,
                ItemID.WrathPotion,
                ItemID.RagePotion,
                ItemID.ThornsPotion,
                ItemID.IronskinPotion,
                ItemID.EndurancePotion,
                ItemID.RegenerationPotion,
                ItemID.LifeforcePotion,
                ItemID.HeartreachPotion,
                ItemID.TitanPotion,
                ItemID.ArcheryPotion,
                ItemID.AmmoReservationPotion,
                ItemID.MagicPowerPotion,
                ItemID.ManaRegenerationPotion,
                ItemID.SummoningPotion,
                ItemID.InfernoPotion,
                ItemID.WarmthPotion,
                ItemID.ObsidianSkinPotion,
                ItemID.GillsPotion,
                ItemID.WaterWalkingPotion,
                ItemID.FlipperPotion,
                ItemID.BuilderPotion,
                ItemID.MiningPotion,
                ItemID.FishingPotion,
                ItemID.CratePotion,
                ItemID.SonarPotion,
                ItemID.GenderChangePotion,
                ItemID.LovePotion,
                ItemID.StinkPotion,
                ItemID.RecallPotion
            };
            Recipe r;

            foreach (var potion in potions)
            {
                r = GetNewRecipe(potion);
                r.AddIngredient(ModContent.ItemType<BloodOrb>(), 10);
                r.AddIngredient(ItemID.BottledWater);
                r.AddTile(TileID.AlchemyTable);
                r.Register();
            }

            r = GetNewRecipe(ItemID.ArcheryPotion);
            r.AddIngredient(ItemID.BottledWater);
            r.AddIngredient(ItemID.Daybloom);
            r.AddIngredient(ModContent.ItemType<BlightedLens>());
            r.AddTile(TileID.Bottles);
            r.Register();
        }
        #endregion

        #region Cooked Food
        private static void AddCookedFood()
        {
            Recipe r = GetNewRecipe(ItemID.CookedFish);
            r.AddIngredient(ModContent.ItemType<TwinklingPollox>());
            r.AddTile(TileID.CookingPots);
            r.Register();

            r = GetNewRecipe(ItemID.CookedFish);
            r.AddIngredient(ModContent.ItemType<PrismaticGuppy>());
            r.AddTile(TileID.CookingPots);
            r.Register();

            r = GetNewRecipe(ItemID.CookedFish);
            r.AddIngredient(ModContent.ItemType<CragBullhead>());
            r.AddTile(TileID.CookingPots);
            r.Register();

            r = GetNewRecipe(ItemID.CookedShrimp);
            r.AddIngredient(ModContent.ItemType<ProcyonidPrawn>());
            r.AddTile(TileID.CookingPots);
            r.Register();

            r = GetNewRecipe(ItemID.Bacon);
            r.AddIngredient(ModContent.ItemType<PiggyItem>());
            r.AddTile(TileID.CookingPots);
            r.Register();
        }
        #endregion

        #region Tools
        // Essential tools such as the Magic Mirror and Rod of Discord
        private static void AddToolRecipes()
        {
            // Magic Mirror
            Recipe r = GetNewRecipe(ItemID.MagicMirror);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 10);
            r.AddIngredient(ItemID.Glass, 10);
            r.AddIngredient(ItemID.FallenStar, 10);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Ice Mirror
            r = GetNewRecipe(ItemID.IceMirror);
            r.AddRecipeGroup("AnyIceBlock", 20);
            r.AddIngredient(ItemID.Glass, 10);
            r.AddIngredient(ItemID.FallenStar, 10);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 5);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Shadow Key
            r = GetNewRecipe(ItemID.ShadowKey);
            r.AddIngredient(ItemID.GoldenKey);
            r.AddIngredient(ItemID.Obsidian, 20);
            r.AddIngredient(ItemID.Bone, 5);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Rod of Discord
            r = GetNewRecipe(ItemID.RodofDiscord);
            r.AddIngredient(ItemID.SoulofLight, 30);
            r.AddIngredient(ItemID.ChaosFish, 5);
            r.AddIngredient(ItemID.PixieDust, 50);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Sky Mill
            r = GetNewRecipe(ItemID.SkyMill);
            r.AddIngredient(ItemID.SunplateBlock, 10);
            r.AddIngredient(ItemID.Cloud, 5);
            r.AddIngredient(ItemID.RainCloud, 3);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Ice Machine
            r = GetNewRecipe(ItemID.IceMachine);
            r.AddRecipeGroup("AnyIceBlock", 25);
            r.AddRecipeGroup("AnySnowBlock", 15);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 3);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Bug Net
            r = GetNewRecipe(ItemID.BugNet);
            r.AddIngredient(ItemID.Cobweb, 30);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 3);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Umbrella
            r = GetNewRecipe(ItemID.Umbrella);
            r.AddIngredient(ItemID.Silk, 5);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 2);
            r.AddTile(TileID.Loom);
            r.Register();
        }
        #endregion

        #region ProgressionItems
        // Boss summon and progression items
        private static void AddProgressionRecipes()
        {
            // Guide Voodoo Doll
            Recipe r = GetNewRecipe(ItemID.GuideVoodooDoll);
            r.AddIngredient(ItemID.Leather, 2);
            r.AddRecipeGroup("EvilPowder", 10);
            r.AddTile(TileID.Hellforge);
            r.Register();

            // Temple Key
            r = GetNewRecipe(ItemID.TempleKey);
            r.AddIngredient(ItemID.JungleSpores, 15);
            r.AddIngredient(ItemID.RichMahogany, 15);
            r.AddIngredient(ItemID.SoulofNight, 15);
            r.AddIngredient(ItemID.SoulofLight, 15);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Lihzahrd Power Cell (NOT Calamity's Old Power Cell)
            r = GetNewRecipe(ItemID.LihzahrdPowerCell);
            r.AddIngredient(ItemID.LihzahrdBrick, 15);
            r.AddIngredient(ModContent.ItemType<CoreofCinder>());
            r.AddTile(TileID.LihzahrdFurnace);
            r.Register();

            // Truffle Worm
            r = GetNewRecipe(ItemID.TruffleWorm);
            r.AddIngredient(ItemID.GlowingMushroom, 15);
            r.AddIngredient(ItemID.Worm);
            r.AddTile(TileID.Autohammer);
            r.Register();
        }
        #endregion

        #region EarlyGameWeapons
        // Early game weapons such as Enchanted Sword
        private static void AddEarlyGameWeaponRecipes()
        {
            // Shuriken
            Recipe r = GetNewRecipe(ItemID.Shuriken, 50);
            r.AddRecipeGroup(RecipeGroupID.IronBar);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Throwing Knife
            r = GetNewRecipe(ItemID.ThrowingKnife, 50);
            r.AddRecipeGroup(RecipeGroupID.IronBar);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Wand of Sparking
            r = GetNewRecipe(ItemID.WandofSparking);
            r.AddIngredient(ItemID.Wood, 5);
            r.AddIngredient(ItemID.Torch, 3);
            r.AddIngredient(ItemID.FallenStar);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Starfury w/ Gold Broadsword
            r = GetNewRecipe(ItemID.Starfury);
            r.AddIngredient(ItemID.GoldBroadsword);
            r.AddIngredient(ItemID.FallenStar, 10);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 3);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Starfury w/ Platinum Broadsword
            r = GetNewRecipe(ItemID.Starfury);
            r.AddIngredient(ItemID.PlatinumBroadsword);
            r.AddIngredient(ItemID.FallenStar, 10);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 3);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Enchanted Sword (requires Hardmode materials)
            r = GetNewRecipe(ItemID.EnchantedSword);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.AddIngredient(ItemID.SoulofLight, 15);
            r.AddIngredient(ItemID.UnicornHorn, 3);
            r.AddIngredient(ItemID.LightShard);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Muramasa
            r = GetNewRecipe(ItemID.Muramasa);
            r.AddRecipeGroup("AnyCobaltBar", 15);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Water Bolt w/ Hardmode Spell Tome
            r = GetNewRecipe(ItemID.WaterBolt);
            r.AddIngredient(ItemID.SpellTome);
            r.AddIngredient(ItemID.Waterleaf, 3);
            r.AddIngredient(ItemID.WaterCandle);
            r.AddTile(TileID.Bookcases);
            r.Register();

            //Slime Staff
            r = GetNewRecipe(ItemID.SlimeStaff);
            r.AddRecipeGroup(RecipeGroupID.Wood, 6);
            r.AddIngredient(ItemID.Gel, 40);
            r.AddIngredient(ItemID.PinkGel, 10);
            r.AddTile(TileID.Anvils);
            r.Register();

            //Ice Boomerang
            r = GetNewRecipe(ItemID.IceBoomerang);
            r.AddRecipeGroup("AnyIceBlock", 20);
            r.AddRecipeGroup("AnySnowBlock", 10);
            r.AddIngredient(ItemID.Shiverthorn);
            r.AddTile(TileID.IceMachine);
            r.Register();
        }
        #endregion

        #region EarlyGameAccessories
        // Early game accessories such as Cloud in a Bottle
        private static void AddEarlyGameAccessoryRecipes()
        {
            // Cloud in a Bottle
            Recipe r = GetNewRecipe(ItemID.CloudinaBottle);
            r.AddIngredient(ItemID.Feather, 2);
            r.AddIngredient(ItemID.Bottle);
            r.AddIngredient(ItemID.Cloud, 25);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Hermes Boots
            r = GetNewRecipe(ItemID.HermesBoots);
            r.AddIngredient(ItemID.Silk, 10);
            r.AddIngredient(ItemID.SwiftnessPotion, 2);
            r.AddTile(TileID.Loom);
            r.Register();

            // Blizzard in a Bottle
            r = GetNewRecipe(ItemID.BlizzardinaBottle);
            r.AddIngredient(ItemID.Feather, 4);
            r.AddIngredient(ItemID.Bottle);
            r.AddRecipeGroup("AnySnowBlock", 50);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Sandstorm in a Bottle
            r = GetNewRecipe(ItemID.SandstorminaBottle);
            r.AddIngredient(ModContent.ItemType<DesertFeather>(), 10);
            r.AddIngredient(ItemID.Feather, 6);
            r.AddIngredient(ItemID.Bottle);
            r.AddIngredient(ItemID.SandBlock, 70);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Frog Leg
            r = GetNewRecipe(ItemID.FrogLeg);
            r.AddIngredient(ItemID.Frog, 10);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Flying Carpet
            r = GetNewRecipe(ItemID.FlyingCarpet);
            r.AddIngredient(ItemID.AncientCloth, 10);
            r.AddIngredient(ItemID.SoulofLight, 10);
            r.AddIngredient(ItemID.SoulofNight, 10);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Aglet
            r = GetNewRecipe(ItemID.Aglet);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 5);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Anklet of the Wind
            r = GetNewRecipe(ItemID.AnkletoftheWind);
            r.AddIngredient(ItemID.JungleSpores, 15);
            r.AddIngredient(ItemID.Cloud, 15);
            r.AddIngredient(ItemID.PinkGel, 5);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Water Walking Boots
            r = GetNewRecipe(ItemID.WaterWalkingBoots);
            r.AddIngredient(ItemID.Leather, 5);
            r.AddIngredient(ItemID.WaterWalkingPotion, 8);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Ice Skates
            r = GetNewRecipe(ItemID.IceSkates);
            r.AddRecipeGroup("AnyIceBlock", 20);
            r.AddIngredient(ItemID.Leather, 5);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 5);
            r.AddTile(TileID.IceMachine);
            r.Register();

            // Lucky Horseshoe
            r = GetNewRecipe(ItemID.LuckyHorseshoe);
            r.AddIngredient(ItemID.SunplateBlock, 10);
            r.AddIngredient(ItemID.Cloud, 10);
            r.AddRecipeGroup("AnyGoldBar", 5);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Shiny Red Balloon
            r = GetNewRecipe(ItemID.ShinyRedBalloon);
            r.AddIngredient(ItemID.WhiteString);
            r.AddIngredient(ItemID.Gel, 80);
            r.AddIngredient(ItemID.Cloud, 40);
            r.AddTile(TileID.Solidifier);
            r.Register();

            // Lava Charm
            r = GetNewRecipe(ItemID.LavaCharm);
            r.AddIngredient(ItemID.LavaBucket, 5);
            r.AddIngredient(ItemID.Obsidian, 25);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 5);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Obsidian Rose
            r = GetNewRecipe(ItemID.ObsidianRose);
            r.AddIngredient(ItemID.JungleRose);
            r.AddIngredient(ItemID.Obsidian, 10);
            r.AddIngredient(ItemID.Hellstone, 10);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Feral Claws
            r = GetNewRecipe(ItemID.FeralClaws);
            r.AddIngredient(ItemID.Leather, 10);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Radar
            r = GetNewRecipe(ItemID.Radar);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 5);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Metal Detector
            r = GetNewRecipe(ItemID.MetalDetector);
            r.AddIngredient(ItemID.Wire, 10);
            r.AddIngredient(ItemID.GoldDust, 5);
            r.AddIngredient(ItemID.SpelunkerGlowstick, 5);
            r.AddRecipeGroup(RecipeGroupID.IronBar, 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Hand Warmer
            r = GetNewRecipe(ItemID.HandWarmer);
            r.AddIngredient(ItemID.Silk, 5);
            r.AddIngredient(ItemID.Shiverthorn);
            r.AddRecipeGroup("AnySnowBlock", 10);
            r.AddTile(TileID.Loom);
            r.Register();

            // Flower Boots
            r = GetNewRecipe(ItemID.FlowerBoots);
            r.AddIngredient(ItemID.Silk, 7);
            r.AddIngredient(ItemID.JungleRose);
            r.AddIngredient(ItemID.JungleGrassSeeds, 5);
            r.AddTile(TileID.Loom);
            r.Register();
        }
        #endregion

        #region Armor
        // Rare uncraftable armors like Eskimo armor
        private static void AddArmorRecipes()
        {
            // Eskimo armor
            Recipe r = GetNewRecipe(ItemID.EskimoHood);
            r.AddIngredient(ItemID.Silk, 4);
            r.AddIngredient(ItemID.Leather);
            r.AddIngredient(ItemID.BorealWood, 12);
            r.AddTile(TileID.Loom);
            r.Register();

            r = GetNewRecipe(ItemID.EskimoCoat);
            r.AddIngredient(ItemID.Silk, 8);
            r.AddIngredient(ItemID.Leather);
            r.AddIngredient(ItemID.BorealWood, 18);
            r.AddTile(TileID.Loom);
            r.Register();

            r = GetNewRecipe(ItemID.EskimoPants);
            r.AddIngredient(ItemID.Silk, 6);
            r.AddIngredient(ItemID.Leather);
            r.AddIngredient(ItemID.BorealWood, 15);
            r.AddTile(TileID.Loom);
            r.Register();
        }
        #endregion

        #region AnkhShield
        // Every base component for the Ankh Shield
        private static void AddAnkhShieldRecipes()
        {
            // Cobalt Shield
            Recipe r = GetNewRecipe(ItemID.CobaltShield);
            r.AddRecipeGroup("AnyCobaltBar", 10);
            r.AddTile(TileID.Anvils);
            r.Register();

            // Armor Polish (broken armor)
            r = GetNewRecipe(ItemID.ArmorPolish);
            r.AddIngredient(ItemID.Bone, 50);
            r.AddIngredient(ModContent.ItemType<AncientBoneDust>(), 3);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Adhesive Bandage (bleeding)
            r = GetNewRecipe(ItemID.AdhesiveBandage);
            r.AddIngredient(ItemID.Silk, 10);
            r.AddIngredient(ItemID.Gel, 50);
            r.AddIngredient(ItemID.GreaterHealingPotion);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Bezoar (poison)
            r = GetNewRecipe(ItemID.Bezoar);
            r.AddIngredient(ItemID.Stinger, 15);
            r.AddIngredient(ModContent.ItemType<MurkyPaste>());
            r.AddTile(TileID.Anvils);
            r.Register();

            // Nazar (curse)
            r = GetNewRecipe(ItemID.Nazar);
            r.AddIngredient(ItemID.SoulofNight, 20);
            r.AddIngredient(ItemID.Lens, 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Vitamins (weakness)
            r = GetNewRecipe(ItemID.Vitamins);
            r.AddIngredient(ItemID.BottledWater);
            r.AddIngredient(ItemID.Waterleaf, 5);
            r.AddIngredient(ItemID.Blinkroot, 5);
            r.AddIngredient(ItemID.Daybloom, 5);
            r.AddIngredient(ModContent.ItemType<BeetleJuice>(), 3);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Blindfold (darkness)
            r = GetNewRecipe(ItemID.Blindfold);
            r.AddIngredient(ItemID.Silk, 30);
            r.AddIngredient(ItemID.SoulofNight, 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Trifold Map (confusion)
            r = GetNewRecipe(ItemID.TrifoldMap);
            r.AddIngredient(ItemID.Silk, 20);
            r.AddIngredient(ItemID.SoulofLight, 3);
            r.AddIngredient(ItemID.SoulofNight, 3);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Fast Clock (slow)
            r = GetNewRecipe(ItemID.FastClock);
            r.AddIngredient(ItemID.Timer1Second);
            r.AddIngredient(ItemID.PixieDust, 15);
            r.AddIngredient(ItemID.SoulofLight, 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Megaphone (silence)
            r = GetNewRecipe(ItemID.Megaphone);
            r.AddIngredient(ItemID.Wire, 10);
            r.AddIngredient(ItemID.HallowedBar, 5);
            r.AddIngredient(ItemID.Ruby, 3);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
        #endregion

        #region HardmodeEquipment
        // Alternate recipes for vanilla Hardmode equipment
        private static void AddAlternateHardmodeRecipes()
        {
            // Avenger Emblem made with Rogue Emblem
            Recipe r = GetNewRecipe(ItemID.AvengerEmblem);
            r.AddIngredient(ModContent.ItemType<RogueEmblem>());
            r.AddIngredient(ItemID.SoulofMight, 5);
            r.AddIngredient(ItemID.SoulofSight, 5);
            r.AddIngredient(ItemID.SoulofFright, 5);
            r.AddTile(TileID.TinkerersWorkbench);
            r.Register();

            // Celestial Magnet
            r = GetNewRecipe(ItemID.CelestialMagnet);
            r.AddIngredient(ItemID.FallenStar, 20);
            r.AddIngredient(ItemID.SoulofMight, 10);
            r.AddIngredient(ItemID.SoulofLight, 5);
            r.AddIngredient(ItemID.SoulofNight, 5);
            r.AddIngredient(ModContent.ItemType<CryoBar>(), 3);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Frozen Turtle Shell
            r = GetNewRecipe(ItemID.FrozenTurtleShell);
            r.AddIngredient(ItemID.TurtleShell, 3);
            r.AddIngredient(ModContent.ItemType<EssenceofEleum>(), 9);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Magic Quiver
            r = GetNewRecipe(ItemID.MagicQuiver);
            r.AddIngredient(ItemID.EndlessQuiver);
            r.AddIngredient(ItemID.PixieDust, 10);
            r.AddIngredient(ModContent.ItemType<BlightedLens>(), 5);
            r.AddIngredient(ItemID.SoulofLight, 8);
            r.AddTile(TileID.CrystalBall);
            r.Register();

            // Frost Helmet w/ Frigid Bars
            r = GetNewRecipe(ItemID.FrostHelmet);
            r.AddIngredient(ModContent.ItemType<CryoBar>(), 6);
            r.AddIngredient(ItemID.FrostCore);
            r.AddTile(TileID.IceMachine);
            r.Register();

            // Frost Breastplate w/ Frigid Bars
            r = GetNewRecipe(ItemID.FrostBreastplate);
            r.AddIngredient(ModContent.ItemType<CryoBar>(), 10);
            r.AddIngredient(ItemID.FrostCore);
            r.AddTile(TileID.IceMachine);
            r.Register();

            // Frost Leggings w/ Frigid Bars
            r = GetNewRecipe(ItemID.FrostLeggings);
            r.AddIngredient(ModContent.ItemType<CryoBar>(), 8);
            r.AddIngredient(ItemID.FrostCore);
            r.AddTile(TileID.IceMachine);
            r.Register();

            // Terra Blade w/ True Bloody Edge
            r = GetNewRecipe(ItemID.TerraBlade);
            r.AddIngredient(ModContent.ItemType<TrueBloodyEdge>());
            r.AddIngredient(ItemID.TrueExcalibur);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();

            // Turtle Shell with Giant Tortoise Shell
            r = GetNewRecipe(ItemID.TurtleShell);
            r.AddIngredient(ModContent.ItemType<GiantTortoiseShell>());
            r.Register();
        }
        #endregion

        public static void AddRecipeGroups()
        {
            //Modify Vanilla Recipe Groups
            RecipeGroup firefly = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Fireflies"]];
            firefly.ValidItems.Add(ModContent.ItemType<TwinklerItem>());

            RecipeGroup sand = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Sand"]];
            sand.ValidItems.Add(ModContent.ItemType<AstralSand>());

            RecipeGroup wood = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            wood.ValidItems.Add(ModContent.ItemType<Acidwood>()); //Astral Monolith was decidedly not wood-like enough

            //New Groups
            RecipeGroup group = new RecipeGroup(() => "Any Copper Bar", new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            });
            RecipeGroup.RegisterGroup("AnyCopperBar", group);

            group = new RecipeGroup(() => "Any Gold Ore", new int[]
            {
                ItemID.GoldOre,
                ItemID.PlatinumOre
            });
            RecipeGroup.RegisterGroup("AnyGoldOre", group);

            group = new RecipeGroup(() => "Any Gold Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            RecipeGroup.RegisterGroup("AnyGoldBar", group);

            group = new RecipeGroup(() => "Any Evil Block", new int[]
            {
                ItemID.EbonstoneBlock,
                ItemID.CrimstoneBlock
            });
            RecipeGroup.RegisterGroup("AnyEvilBlock", group);

            group = new RecipeGroup(() => "Any Evil Bar", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });
            RecipeGroup.RegisterGroup("AnyEvilBar", group);

            group = new RecipeGroup(() => "Any Cobalt Bar", new int[]
            {
                ItemID.CobaltBar,
                ItemID.PalladiumBar
            });
            RecipeGroup.RegisterGroup("AnyCobaltBar", group);

            group = new RecipeGroup(() => "Any Adamantite Bar", new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup("AnyAdamantiteBar", group);

            group = new RecipeGroup(() => "Nightmare Fuel or Endothermic Energy", new int[]
            {
                ModContent.ItemType<NightmareFuel>(),
                ModContent.ItemType<EndothermicEnergy>()
            });
            RecipeGroup.RegisterGroup("NForEE", group);

            group = new RecipeGroup(() => "Any Evil Powder", new int[]
            {
                ItemID.VilePowder,
                ItemID.ViciousPowder
            });
            RecipeGroup.RegisterGroup("EvilPowder", group);

            group = new RecipeGroup(() => "Shadow Scale or Tissue Sample", new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });
            RecipeGroup.RegisterGroup("Boss2Material", group);

            group = new RecipeGroup(() => "Cursed Flame or Ichor", new int[]
            {
                ItemID.CursedFlame,
                ItemID.Ichor
            });
            RecipeGroup.RegisterGroup("CursedFlameIchor", group);

            group = new RecipeGroup(() => "Any Evil Flask", new int[]
            {
                ItemID.FlaskofCursedFlames,
                ItemID.FlaskofIchor
            });
            RecipeGroup.RegisterGroup("AnyEvilFlask", group);

            group = new RecipeGroup(() => "Any Evil Water", new int[]
            {
                ItemID.UnholyWater,
                ItemID.BloodWater
            });
            RecipeGroup.RegisterGroup("AnyEvilWater", group);

            group = new RecipeGroup(() => "Any Ice Block", new int[]
            {
                ItemID.IceBlock,
                ItemID.PurpleIceBlock,
                ItemID.RedIceBlock,
                ItemID.PinkIceBlock,
                ModContent.ItemType<AstralIce>()
            });
            RecipeGroup.RegisterGroup("AnyIceBlock", group);

            group = new RecipeGroup(() => "Any Snow Block", new int[]
            {
                ItemID.SnowBlock,
                ModContent.ItemType<AstralSnow>()
            });
            RecipeGroup.RegisterGroup("AnySnowBlock", group);

            group = new RecipeGroup(() => "Any Silt", new int[]
            {
                ItemID.SiltBlock,
                ItemID.SlushBlock,
                ModContent.ItemType<AstralSilt>()
            });
            RecipeGroup.RegisterGroup("SiltGroup", group);

            group = new RecipeGroup(() => "Any Hardmode Anvil", new int[]
            {
                ItemID.MythrilAnvil,
                ItemID.OrichalcumAnvil
            });
            RecipeGroup.RegisterGroup("HardmodeAnvil", group);

            group = new RecipeGroup(() => "Any Hardmode Forge", new int[]
            {
                ItemID.AdamantiteForge,
                ItemID.TitaniumForge
            });
            RecipeGroup.RegisterGroup("HardmodeForge", group);

            group = new RecipeGroup(() => "Any Lunar Pickaxe", new int[]
            {
                ItemID.SolarFlarePickaxe,
                ItemID.VortexPickaxe,
                ItemID.NebulaPickaxe,
                ItemID.StardustPickaxe
            });
            RecipeGroup.RegisterGroup("LunarPickaxe", group);

            group = new RecipeGroup(() => "Any Lunar Hamaxe", new int[]
            {
                ItemID.LunarHamaxeSolar,
                ItemID.LunarHamaxeVortex,
                ItemID.LunarHamaxeNebula,
                ItemID.LunarHamaxeStardust
            });
            RecipeGroup.RegisterGroup("LunarHamaxe", group);

            group = new RecipeGroup(() => "Any Food Item", new int[]
            {
                ItemID.CookedFish,
                ItemID.CookedMarshmallow,
                ItemID.PadThai,
                ItemID.Pho,
                ItemID.CookedShrimp,
                ItemID.Sashimi,
                ItemID.Bacon,
                ItemID.BowlofSoup,
                ItemID.GrubSoup,
                ItemID.GingerbreadCookie,
                ItemID.SugarCookie,
                ItemID.ChristmasPudding,
                ItemID.PumpkinPie,
                ModContent.ItemType<Baguette>(),
                ModContent.ItemType<DeliciousMeat>(),
                ModContent.ItemType<SunkenStew>()
            });
            RecipeGroup.RegisterGroup("AnyFood", group);

            group = new RecipeGroup(() => "Any Wings", new int[]
            {
                ItemID.DemonWings,
                ItemID.AngelWings,
                ItemID.RedsWings,
                ItemID.ButterflyWings,
                ItemID.FairyWings,
                ItemID.HarpyWings,
                ItemID.BoneWings,
                ItemID.FlameWings,
                ItemID.FrozenWings,
                ItemID.GhostWings,
                ItemID.SteampunkWings,
                ItemID.LeafWings,
                ItemID.BatWings,
                ItemID.BeeWings,
                ItemID.DTownsWings,
                ItemID.WillsWings,
                ItemID.CrownosWings,
                ItemID.CenxsWings,
                ItemID.TatteredFairyWings,
                ItemID.SpookyWings,
                ItemID.Hoverboard,
                ItemID.FestiveWings,
                ItemID.BeetleWings,
                ItemID.FinWings,
                ItemID.FishronWings,
                ItemID.MothronWings,
                ItemID.WingsSolar,
                ItemID.WingsVortex,
                ItemID.WingsNebula,
                ItemID.WingsStardust,
                ItemID.Yoraiz0rWings,
                ItemID.JimsWings,
                ItemID.SkiphsWings,
                ItemID.LokisWings,
                ItemID.BetsyWings,
                ItemID.ArkhalisWings,
                ItemID.LeinforsWings,
                ModContent.ItemType<SkylineWings>(),
                ModContent.ItemType<StarlightWings>(),
                ModContent.ItemType<AureateWings>(),
                ModContent.ItemType<DiscordianWings>(),
                ModContent.ItemType<TarragonWings>(),
                ModContent.ItemType<XerocWings>(),
                ModContent.ItemType<HadarianWings>(),
                ModContent.ItemType<SilvaWings>()
            });
            RecipeGroup.RegisterGroup("WingsGroup", group);
        }
    }
}
