using System;
using CalRD.Items.Accessories;
using CalRD.Items.Ammo.FiniteUse;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRD
{
    #region Fraction Struct (thanks Yorai)
    public struct Fraction
    {
        internal readonly int numerator;
        internal readonly int denominator;

        public Fraction(int n, int d)
        {
            numerator = n < 0 ? 0 : n;
            denominator = d <= 0 ? 1 : d;
        }

        public static implicit operator float(Fraction f) => f.numerator / (float)f.denominator;
    }
    #endregion
    
    public static class DropHelper
    {
        #region Lambda Drop Rule Condition
        // This class serves as a vanilla drop rule condition that is based on completely arbitrary code.
        // Create these using the function DropHelper.If as needed.
        internal class LambdaDropRuleCondition : IItemDropRuleCondition
        {
            private readonly Func<DropAttemptInfo, bool> conditionLambda;
            private readonly bool visibleInUI;
            private readonly string description;

            internal LambdaDropRuleCondition(Func<DropAttemptInfo, bool> lambda, bool ui = true, string desc = null)
            {
                conditionLambda = lambda;
                visibleInUI = ui;
                description = desc;
            }

            public bool CanDrop(DropAttemptInfo info) => conditionLambda(info);
            public bool CanShowItemDropInUI() => visibleInUI;
            public string GetConditionDescription() => description;
        }

        internal class LambdaDropRuleCondition2 : IItemDropRuleCondition
        {
            private readonly Func<DropAttemptInfo, bool> conditionLambda;
            private readonly Func<bool> visibleInUI;
            private readonly string description;

            internal LambdaDropRuleCondition2(Func<DropAttemptInfo, bool> lambda, Func<bool> ui, string desc = null)
            {
                conditionLambda = lambda;
                visibleInUI = ui;
                description = desc;
            }

            public bool CanDrop(DropAttemptInfo info) => conditionLambda(info);
            public bool CanShowItemDropInUI() => visibleInUI();
            public string GetConditionDescription() => description;
        }

        internal class LambdaDropRuleCondition3 : IItemDropRuleCondition
        {
            private readonly Func<DropAttemptInfo, bool> conditionLambda;
            private readonly Func<bool> visibleInUI;
            private readonly Func<string> description;

            internal LambdaDropRuleCondition3(Func<DropAttemptInfo, bool> lambda, Func<bool> ui, Func<string> desc)
            {
                conditionLambda = lambda;
                visibleInUI = ui;
                description = desc;
            }

            public bool CanDrop(DropAttemptInfo info) => conditionLambda(info);
            public bool CanShowItemDropInUI() => visibleInUI();
            public string GetConditionDescription() => description();
        }

        /// <summary>
        /// Creates a new LambdaDropRuleCondition which executes the code of your choosing to decide whether this item drop should occur.<br />
        /// This version of "If" does <b>NOT</b> use the DropAttemptInfo struct that is available.<br />
        /// This lets you write simpler lambdas that do not need the context, e.g. just checking if a boss is dead.
        /// </summary>
        /// <param name="lambda">Lambda function which evaluates to true or false, deciding whether the item should drop. <code>() => {CodeHere}</code></param>
        /// <returns>The LambdaDropRuleCondition produced.</returns>
        public static IItemDropRuleCondition If(Func<bool> lambda) => new LambdaDropRuleCondition((_) => lambda());

        /// <summary>
        /// Creates a new LambdaDropRuleCondition which executes the code of your choosing to decide whether this item drop should occur.<br />
        /// This version of "If" does <b>NOT</b> use the DropAttemptInfo struct that is available.<br />
        /// This lets you write simpler lambdas that do not need the context, e.g. just checking if a boss is dead.
        /// </summary>
        /// <param name="lambda">Lambda function which evaluates to true or false, deciding whether the item should drop. <code>() => {CodeHere}</code></param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The LambdaDropRuleCondition produced.</returns>
        public static IItemDropRuleCondition If(Func<bool> lambda, bool ui = true, string desc = null)
        {
            bool LambdaInfoWrapper(DropAttemptInfo _) => lambda();
            return new LambdaDropRuleCondition(LambdaInfoWrapper, ui, desc);
        }
        public static IItemDropRuleCondition If(Func<bool> lambda, Func<bool> ui, string desc = null)
        {
            bool LambdaInfoWrapper(DropAttemptInfo _) => lambda();
            return new LambdaDropRuleCondition2(LambdaInfoWrapper, ui, desc);
        }
        public static IItemDropRuleCondition If(Func<bool> lambda, Func<bool> ui, Func<string> desc)
        {
            bool LambdaInfoWrapper(DropAttemptInfo _) => lambda();
            return new LambdaDropRuleCondition3(LambdaInfoWrapper, ui, desc);
        }

        /// <summary>
        /// Creates a new LambdaDropRuleCondition which executes the code of your choosing to decide whether this item drop should occur.<br />
        /// This version of "If" <b>DOES</b> use the DropAttemptInfo struct, and thus the provided lambda requires 1 argument.
        /// </summary>
        /// <param name="lambda">Lambda function which evaluates to true or false, deciding whether the item should drop. <code>(info) => {CodeHere}</code></param>
        /// <returns>The LambdaDropRuleCondition produced.</returns>
        public static IItemDropRuleCondition If(Func<DropAttemptInfo, bool> lambda) => new LambdaDropRuleCondition(lambda);

        /// <summary>
        /// Creates a new LambdaDropRuleCondition which executes the code of your choosing to decide whether this item drop should occur.<br />
        /// This version of "If" <b>DOES</b> use the DropAttemptInfo struct, and thus the provided lambda requires 1 argument.
        /// </summary>
        /// <param name="lambda">Lambda function which evaluates to true or false, deciding whether the item should drop. <code>(info) => {CodeHere}</code></param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The LambdaDropRuleCondition produced.</returns>
        public static IItemDropRuleCondition If(Func<DropAttemptInfo, bool> lambda, bool ui = true, string desc = null)
        {
            return new LambdaDropRuleCondition(lambda, ui, desc);
        }
        public static IItemDropRuleCondition If(Func<DropAttemptInfo, bool> lambda, Func<bool> ui, string desc = null)
        {
            return new LambdaDropRuleCondition2(lambda, ui, desc);
        }
        public static IItemDropRuleCondition If(Func<DropAttemptInfo, bool> lambda, Func<bool> ui, Func<string> desc)
        {
            return new LambdaDropRuleCondition3(lambda, ui, desc);
        }
        #endregion
        
        #region Leading Condition Rule Extensions
        /// <summary>
        /// Adds any given drop rule as a chained rule to the given LeadingConditionRule.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should have another drop rule registered as one of its chains.</param>
        /// <param name="chainedRule">The drop rule which should occur given this leading condition.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <returns>The LeadingConditionRule (first parameter).</returns>
        public static IItemDropRule Add(this LeadingConditionRule mainRule, IItemDropRule chainedRule, bool hideLootReport = false)
        {
            return mainRule.OnSuccess(chainedRule, hideLootReport);
        }

        /// <summary>
        /// Shorthand to add a simple drop to the given LeadingConditionRule.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRateInt">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <returns>The LeadingConditionRule (first parameter).</returns>
        public static IItemDropRule Add(this LeadingConditionRule mainRule, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1, bool hideLootReport = false)
        {
            return mainRule.OnSuccess(ItemDropRule.Common(itemID, dropRateInt, minQuantity, maxQuantity), hideLootReport);
        }

        /// <summary>
        /// Shorthand to add a simple drop to the given LeadingConditionRule using a Fraction drop rate.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRate">The chance that the item will drop as a DropHelper Fraction.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <returns>The LeadingConditionRule (first parameter).</returns>
        public static IItemDropRule Add(this LeadingConditionRule mainRule, int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1, bool hideLootReport = false)
        {
            return mainRule.OnSuccess(new CommonDrop(itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator), hideLootReport);
        }

        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to the given LeadingConditionRule.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRateInt">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this LeadingConditionRule mainRule, Func<bool> lambda, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1, bool hideLootReport = false, string desc = null)
        {
            return mainRule.OnSuccess(ItemDropRule.ByCondition(If(lambda, true, desc), itemID, dropRateInt, minQuantity, maxQuantity), hideLootReport);
        }

        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to the given LeadingConditionRule using a Fraction drop rate.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRate">The chance that the item will drop as a DropHelper Fraction.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this LeadingConditionRule mainRule, Func<bool> lambda, int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1, bool hideLootReport = false, string desc = null)
        {
            return mainRule.OnSuccess(ItemDropRule.ByCondition(If(lambda, true, desc), itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator), hideLootReport);
        }

        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to the given LeadingConditionRule.<br />
        /// <b>This version requires a lambda which uses DropAttemptInfo.</b>
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="lambda">A lambda which takes a DropAttemptInfo struct and evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRateInt">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this LeadingConditionRule mainRule, Func<DropAttemptInfo, bool> lambda, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1, bool hideLootReport = false, string desc = null)
        {
            return mainRule.OnSuccess(ItemDropRule.ByCondition(If(lambda, true, desc), itemID, dropRateInt, minQuantity, maxQuantity), hideLootReport);
        }

        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to the given LeadingConditionRule using a Fraction drop rate.<br />
        /// <b>This version requires a lambda which uses DropAttemptInfo.</b>
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="lambda">A lambda which takes a DropAttemptInfo struct and evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRate">The chance that the item will drop as a DropHelper Fraction.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this LeadingConditionRule mainRule, Func<DropAttemptInfo, bool> lambda, int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1, bool hideLootReport = false, string desc = null)
        {
            return mainRule.OnSuccess(ItemDropRule.ByCondition(If(lambda, true, desc), itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator), hideLootReport);
        }

        /// <summary>
        /// Adds any given drop rule as a chained rule to the given LeadingConditionRule.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should have another drop rule registered as one of its chains.</param>
        /// <param name="chainedRule">The drop rule which should occur given this leading condition.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <returns>The LeadingConditionRule (first parameter).</returns>
        public static IItemDropRule AddFail(this LeadingConditionRule mainRule, IItemDropRule chainedRule, bool hideLootReport = false)
        {
            return mainRule.OnFailedConditions(chainedRule, hideLootReport);
        }

        /// <summary>
        /// Shorthand to add a simple drop to the given LeadingConditionRule.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRateInt">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <returns>The LeadingConditionRule (first parameter).</returns>
        public static IItemDropRule AddFail(this LeadingConditionRule mainRule, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1, bool hideLootReport = false)
        {
            return mainRule.OnFailedConditions(ItemDropRule.Common(itemID, dropRateInt, minQuantity, maxQuantity), hideLootReport);
        }

        /// <summary>
        /// Shorthand to add a simple drop to the given LeadingConditionRule using a Fraction drop rate.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRate">The chance that the item will drop as a DropHelper Fraction.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="hideLootReport">Set to true for this drop to not appear in the Bestiary.</param>
        /// <returns>The LeadingConditionRule (first parameter).</returns>
        public static IItemDropRule AddFail(this LeadingConditionRule mainRule, int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1, bool hideLootReport = false)
        {
            return mainRule.OnFailedConditions(new CommonDrop(itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator), hideLootReport);
        }
        #endregion
        
        #region Global Drop Chances
        /// <summary>
        /// The Defiled Rune boosts various low drop rates to one in this value.
        /// </summary>
        public const int DefiledDropRateInt = 20;

        /// <summary>
        /// The Defiled Rune boosts various low drop rates to this chance (decimal number out of 1.0).
        /// </summary>
        public const float DefiledDropRateFloat = 0.05f;

        /// <summary>
        /// Legendary drops have a 1 in X chance of dropping, where X is this variable.
        /// </summary>
        public const int LegendaryDropRateInt = 100;

        /// <summary>
        /// Legendary weapons have this chance to drop (decimal number out of 1.0).
        /// </summary>
        public const float LegendaryDropRateFloat = 0.01f;

        /// <summary>
        /// Rare Item Variants have a 1 in X chance of dropping, where X is this variable.
        /// </summary>
        public const int RareVariantDropRateInt = 40;

        /// <summary>
        /// Rare Item Variants have this chance to drop (decimal number out of 1.0).
        /// </summary>
        public const float RareVariantDropRateFloat = 0.025f;

        /// <summary>
        /// Direct weapon drops (straight from the boss in Normal Mode) have a 1 in X chance of dropping, where X is this variable.
        /// </summary>
        public const int DirectWeaponDropRateInt = 4;
        
        /// <summary>
        /// Direct weapon drops (straight from the boss in Normal Mode) have this chance to drop (decimal number out of 1.0).
        /// </summary>
        public const float DirectWeaponDropRateFloat = 0.25f;

        /// <summary>
        /// Weapons in Expert Mode typically have this chance to drop (as a DropHelper Fraction).
        /// </summary>
        public static readonly Fraction DirectWeaponDropRateFraction = new(1, DirectWeaponDropRateInt);
        
        /// <summary>
        /// Bag weapons (Expert Mode and higher) typically have a 1 in X chance of dropping, where X is this variable.
        /// </summary>
        public const int BagWeaponDropRateInt = 3;
        
        /// <summary>
        /// Bag weapon drops (Expert Mode and higher) have this chance to drop (decimal number out of 1.0).
        /// </summary>
        public const float BagWeaponDropRateFloat = 0.3333333f;
            
        /// <summary>
        /// Weapons in Expert Mode typically have this chance to drop (as a DropHelper Fraction).
        /// </summary>
        public static readonly Fraction BagWeaponDropRateFraction = new(1, BagWeaponDropRateInt);
        #endregion

        #region Weighted Item Sets
        public const float DefaultWeight = 1f;
        public const float MinisiculeWeight = 1E-6f;

        // TODO -- DropHelper will need to be fully retooled in 1.4 to utilize this struct for all functions.
        public struct WeightedItemStack
        {
            internal int itemID;
            internal float weight;
            internal int minQuantity;
            internal int maxQuantity;

            internal WeightedItemStack(int id, float w)
            {
                itemID = id;
                weight = w;
                minQuantity = 1;
                maxQuantity = 1;
            }

            internal WeightedItemStack(int id, float w, int quantity)
            {
                itemID = id;
                weight = w;
                minQuantity = quantity;
                maxQuantity = quantity;
            }

            internal WeightedItemStack(int id, float w, int min, int max)
            {
                itemID = id;
                weight = w;
                minQuantity = min;
                maxQuantity = max;
            }
            
            internal int ChooseQuantity(UnifiedRandom rng) => rng.Next(minQuantity, maxQuantity + 1);
            
            // Allow for implicitly casting integer item IDs into weighted item stacks.
            // Stack size is assumed to be 1. Weight is assumed to be default.
            public static implicit operator WeightedItemStack(int id)
            {
                return new WeightedItemStack(id, DefaultWeight, 1);
            }
        }

        // int itemID --> WeightedItemStack
        public static WeightedItemStack WeightStack(this int itemID) => WeightStack(itemID, DefaultWeight);
        public static WeightedItemStack WeightStack(this int itemID, float weight) => new WeightedItemStack(itemID, weight);
        public static WeightedItemStack WeightStack(this int itemID, int quantity) => WeightStack(itemID, DefaultWeight, quantity);
        public static WeightedItemStack WeightStack(this int itemID, float weight, int quantity) => new WeightedItemStack(itemID, weight, quantity);
        public static WeightedItemStack WeightStack(this int itemID, int min, int max) => WeightStack(itemID, DefaultWeight, min, max);
        public static WeightedItemStack WeightStack(this int itemID, float weight, int min, int max) => new WeightedItemStack(itemID, weight, min, max);

        // ModItem generic parameter --> WeightedItemStack
        public static WeightedItemStack WeightStack<T>() where T : ModItem => WeightStack<T>(DefaultWeight);
        public static WeightedItemStack WeightStack<T>(float weight) where T : ModItem => WeightStack(ModContent.ItemType<T>(), weight);
        public static WeightedItemStack WeightStack<T>(int quantity) where T : ModItem => WeightStack<T>(DefaultWeight, quantity);
        public static WeightedItemStack WeightStack<T>(float weight, int quantity) where T : ModItem => WeightStack(ModContent.ItemType<T>(), weight, quantity);
        public static WeightedItemStack WeightStack<T>(int min, int max) where T : ModItem => WeightStack<T>(DefaultWeight, min, max);
        public static WeightedItemStack WeightStack<T>(float weight, int min, int max) where T : ModItem => WeightStack(ModContent.ItemType<T>(), weight, min, max);

        // Separated implementation used so weighted random code isn't duplicated in two places.
        private static WeightedItemStack RollWeightedRandom(WeightedItemStack[] stacks)
        {
            int i;
            float[] breakpoints = new float[stacks.Length];
            float totalWeight = 0f;

            // Assign breakpoints based on the cumulative sum of weights thus far.
            // Error check invalid weights by giving them an unbelievably small drop chance.
            for (i = 0; i < stacks.Length; ++i)
            {
                float w = stacks[i].weight;
                if (w <= 0f || float.IsNaN(w) || float.IsInfinity(w))
                    w = MinisiculeWeight;
                breakpoints[i] = totalWeight += w;
            }

            // Iterate through the breakpoints until you find the first one that is surpassed. Drop that item.
            float needle = Main.rand.NextFloat(totalWeight);
            i = 0;
            while (needle > breakpoints[i])
                ++i;
            return stacks[i];
        }

        /// <summary>
        /// Chooses an item (or stack of items) from an array of drop definitions and drops it from the given NPC.<br></br>
        /// Each item is given a certain weight to spawn. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="stacks">The array of drop definitions to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemFromWeightedSet(IEntitySource src, NPC npc, bool dropPerPlayer, params WeightedItemStack[] stacks)
        {
            // Can't choose anything from an empty array.
            if (stacks is null || stacks.Length == 0)
                return 0;

            WeightedItemStack stk = RollWeightedRandom(stacks);
            return DropItem(src, npc, stk.itemID, dropPerPlayer, stk.minQuantity, stk.maxQuantity);
        }

        /// <summary>
        /// Chooses an item (or stack of items) from an array of drop definitions and drops it from the given NPC.<br></br>
        /// Each item is given a certain weight to spawn.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="stacks">The array of drop definitions to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemFromWeightedSet(IEntitySource src, NPC npc, params WeightedItemStack[] stacks)
        {
            return DropItemFromWeightedSet(src, npc, false, stacks);
        }

        /// <summary>
        /// Chooses an item (or stack of items) from an array of drop definitions and spawns it for the given player.<br></br>
        /// Each item is given a certain weight to spawn.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item(s).</param>
        /// <param name="stacks">The array of drop definitions to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemFromWeightedSet(IEntitySource src, Player p, params WeightedItemStack[] stacks)
        {
            // Can't choose anything from an empty array.
            if (stacks is null || stacks.Length == 0)
                return 0;

            WeightedItemStack stk = RollWeightedRandom(stacks);
            return DropItem(src, p, stk.itemID, stk.minQuantity, stk.maxQuantity);
        }

        /// <summary>
        /// Rolls for each item (or stack of items) in an array of drop definitions to drop at their defined chances.<br></br>
        /// Always drops at least one of the defined stacks. Optionally spawns one copy of these drops per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the items.</param>
        /// <param name="dropPerPlayer">Whether the drops should be "instanced" (each player gets their own copy).</param>
        /// <param name="stacks">The array of drop definitions to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropEntireWeightedSet(IEntitySource src, NPC npc, bool dropPerPlayer, params WeightedItemStack[] stacks)
        {
            int numDrops = 0;

            // Can't choose anything from an empty array.
            if (stacks is null || stacks.Length == 0)
                return numDrops;

            for (int i = 0; i < stacks.Length; ++i)
            {
                WeightedItemStack stk = stacks[i];
                numDrops += DropItemChance(npc.GetSource_FromThis(), npc, stk.itemID, dropPerPlayer, stk.weight, stk.minQuantity, stk.maxQuantity);
            }

            // If nothing at all was dropped, drop one thing at (weighted) random.
            if (numDrops <= 0)
                numDrops += DropItemFromWeightedSet(src, npc, dropPerPlayer, stacks);

            return numDrops;
        }

        /// <summary>
        /// Rolls for each item (or stack of items) in an array of drop definitions to drop at their defined chances.<br></br>
        /// Always drops at least one of the defined stacks.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the items.</param>
        /// <param name="stacks">The array of drop definitions to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropEntireWeightedSet(IEntitySource src, NPC npc, params WeightedItemStack[] stacks)
        {
            return DropEntireWeightedSet(src, npc, false, stacks);
        }

        /// <summary>
        /// Rolls for each item (or stack of items) in an array of drop definitions to drop at their defined chances.<br></br>
        /// Always drops at least one of the defined stacks.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item(s).</param>
        /// <param name="stacks">The array of drop definitions to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropEntireWeightedSet(IEntitySource src, Player p, params WeightedItemStack[] stacks)
        {
            int numDrops = 0;

            // Can't choose anything from an empty array.
            if (stacks is null || stacks.Length == 0)
                return numDrops;

            for (int i = 0; i < stacks.Length; ++i)
            {
                WeightedItemStack stk = stacks[i];
                numDrops += DropItemChance(src, p, stk.itemID, stk.weight, stk.minQuantity, stk.maxQuantity);
            }

            // If nothing at all was dropped, drop one thing at (weighted) random.
            if (numDrops <= 0)
                numDrops += DropItemFromWeightedSet(src, p, stacks);

            return numDrops;
        }
        #endregion

        #region Extra Boss Bags
        /// <summary>
        /// The number of extra loot bags bosses drop when Revengeance Mode is active.<br></br>
        /// This is normally zero; Revengenace Mode provides no extra bags.
        /// </summary>
        public static int RevExtraBags = 0;

        /// <summary>
        /// The number of extra loot bags bosses drop when Death Mode is active.<br></br>
        /// This is normally zero; Death Mode provides no extra bags.
        /// </summary>
        public static int DeathExtraBags = 0;

        /// <summary>
        /// The number of extra loot bags bosses drop when the Defiled Rune is active.<br></br>
        /// This is normally zero; Defiled Rune provides no extra bags.
        /// </summary>
        public static int DefiledExtraBags = 0;

        /// <summary>
        /// The number of extra loot bags bosses drop when Armageddon is active.<br></br>
        /// This is normally 5. Bosses drop 5 bags on normal, and 6 on Expert+.
        /// </summary>
        public static int ArmageddonExtraBags = 5;
        #endregion

        #region Boss Bag Drop Helpers
        /// <summary>
        /// Automatically drops the correct number of boss bags for each difficulty based on constants kept in DropHelper.
        /// </summary>
        /// <param name ="bossBag">The Item type corresponding to the boss bag.</param>
        /// <param name="theBoss">The NPC to drop boss bags for.</param>
        /// <returns>The number of boss bags dropped.</returns>
        public static int DropBags(int bossBag, NPC theBoss)
        {
            int bagsDropped = 0;

            // Don't drop any bags for an invalid NPC.
            if (theBoss is null)
                return bagsDropped;

            // Armageddon's bonus bags drop even on Normal.
            bagsDropped += DropArmageddonBags(bossBag, theBoss);

            // If the difficulty isn't Expert+, no more bags are dropped.
            if (!Main.expertMode)
                return bagsDropped;

            // Drop the 1 vanilla Expert Mode boss bag.
            Item.NewItem(theBoss.GetSource_FromThis(), theBoss.Center, theBoss.Size, bossBag);
            bagsDropped++;

            // If Rev is active, possibly drop extra bags.
            if (CalamityWorld.revenge)
            {
                for (int i = 0; i < RevExtraBags; ++i)
                    Item.NewItem(theBoss.GetSource_FromThis(), theBoss.Center, theBoss.Size, bossBag);

                bagsDropped += RevExtraBags;
            }

            // If Death is active, possibly drop extra bags.
            if (CalamityWorld.death)
            {
                for (int i = 0; i < DeathExtraBags; ++i)
                    Item.NewItem(theBoss.GetSource_FromThis(), theBoss.Center, theBoss.Size, bossBag);

                bagsDropped += DeathExtraBags;
            }

            // If Defiled is active, possibly drop extra bags.
            if (CalamityWorld.defiled)
            {
                for (int i = 0; i < DefiledExtraBags; ++i)
                    Item.NewItem(theBoss.GetSource_FromThis(), theBoss.Center, theBoss.Size, bossBag);

                bagsDropped += DefiledExtraBags;
            }

            return bagsDropped;
        }

        /// <summary>
        /// Drops the correct number of boss bags for Armageddon.
        /// </summary>
        /// <param name ="bossBag">The Item type corresponding to the boss bag.</param>
        /// <param name="theBoss">The NPC to drop boss bags for.</param>
        /// <returns>The number of boss bags dropped.</returns>
        public static int DropArmageddonBags(int bossBag, NPC theBoss)
        {
            if (!CalamityWorld.armageddon)
                return 0;

            for (int i = 0; i < ArmageddonExtraBags; ++i)
                Item.NewItem(theBoss.GetSource_FromThis(), theBoss.Center, theBoss.Size, bossBag);
            return ArmageddonExtraBags;
        }
        #endregion
        
        #region ILoot extensions
        /// <summary>
        /// Drops the correct number of boss bags for Armageddon.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name ="bossBag">The Item type corresponding to the boss bag.</param>
        /// <returns>The number of boss bags dropped.</returns>
        public static IItemDropRule AddArmageddonBags(this LeadingConditionRule mainRule, int bossBag) => mainRule.OnSuccess(ItemDropRule.Common(bossBag, 1, ArmageddonExtraBags, ArmageddonExtraBags));
        
        /// <summary>
        /// Shorthand to add a simple drop to a loot table.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRateInt">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule Add(this ILoot loot, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1)
        {
            return loot.Add(ItemDropRule.Common(itemID, dropRateInt, minQuantity, maxQuantity));
        }

        /// <summary>
        /// Shorthand to add a simple drop to a loot table using a Fraction drop rate.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRate">The chance that the item will drop as a DropHelper Fraction.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule Add(this ILoot loot, int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1)
        {
            return loot.Add(new CommonDrop(itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator));
        }
        /// <summary>
        /// Shorthand for shorthand: Registers a Normal Mode only LeadingConditionRule for a loot table and returns it to you.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <returns>A Normal Mode only LeadingConditionRule.</returns>
        public static LeadingConditionRule DefineNormalOnlyDropSet(this ILoot loot) => loot.DefineConditionalDropSet(new Conditions.NotExpert());
        
        /// <summary>
        /// Registers a LeadingConditionRule for a loot table and returns it so you can add drops to that rule.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="condition">The condition behind which you want to gate several drop rules.</param>
        /// <returns>The LeadingConditionRule which encapsulates the given condition.</returns>
        public static LeadingConditionRule DefineConditionalDropSet(this ILoot loot, IItemDropRuleCondition condition)
        {
            LeadingConditionRule rule = new LeadingConditionRule(condition);
            loot.Add(rule);
            return rule;
        }
        
        /// <summary>
        /// Shorthand for registering a LeadingConditionRule using DropHelper.If.<br />
        /// This version does <b>NOT</b> use the DropAttemptInfo struct that is available.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <returns>The LeadingConditionRule which encapsulates the given lambda.</returns>
        public static LeadingConditionRule DefineConditionalDropSet(this ILoot loot, Func<bool> lambda) => loot.DefineConditionalDropSet(If(lambda));

        /// <summary>
        /// Shorthand for registering a LeadingConditionRule using DropHelper.If.<br />
        /// This version <b>DOES</b> use the DropAttemptInfo struct, and thus the provided lambda requires 1 argument.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <returns>The LeadingConditionRule which encapsulates the given lambda.</returns>
        public static LeadingConditionRule DefineConditionalDropSet(this ILoot loot, Func<DropAttemptInfo, bool> lambda) => loot.DefineConditionalDropSet(If(lambda));
        
        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to a loot table.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="cond">An IItemDropRuleCondition which encapsulates the condition which needs to be checked in real-time.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRateInt">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this ILoot loot, IItemDropRuleCondition cond, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1)
        {
            return loot.Add(ItemDropRule.ByCondition(cond, itemID, dropRateInt, minQuantity, maxQuantity));
        }

        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to a loot table using a Fraction drop rate.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="cond">An IItemDropRuleCondition which encapsulates the condition which needs to be checked in real-time.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRate">The chance that the item will drop as a DropHelper Fraction.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this ILoot loot, IItemDropRuleCondition cond, int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1)
        {
            return loot.Add(ItemDropRule.ByCondition(cond, itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator));
        }

        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to a loot table.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRateInt">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this ILoot loot, Func<bool> lambda, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1, bool ui = true, string desc = null)
        {
            return loot.Add(ItemDropRule.ByCondition(If(lambda, ui, desc), itemID, dropRateInt, minQuantity, maxQuantity));
        }

        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to a loot table using a Fraction drop rate.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRate">The chance that the item will drop as a DropHelper Fraction.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this ILoot loot, Func<bool> lambda, int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1, bool ui = true, string desc = null)
        {
            return loot.Add(ItemDropRule.ByCondition(If(lambda, ui, desc), itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator));
        }

        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to a loot table.<br />
        /// <b>This version requires a lambda which uses DropAttemptInfo.</b>
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which takes a DropAttemptInfo struct and evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRateInt">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this ILoot loot, Func<DropAttemptInfo, bool> lambda, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1, bool ui = true, string desc = null)
        {
            return loot.Add(ItemDropRule.ByCondition(If(lambda, ui, desc), itemID, dropRateInt, minQuantity, maxQuantity));
        }

        /// <summary>
        /// Shorthand to add an arbitrary conditional drop to a loot table using a Fraction drop rate.<br />
        /// <b>This version requires a lambda which uses DropAttemptInfo.</b>
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which takes a DropAttemptInfo struct and evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item to drop.</param>
        /// <param name="dropRate">The chance that the item will drop as a DropHelper Fraction.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>The item drop rule registered.</returns>
        public static IItemDropRule AddIf(this ILoot loot, Func<DropAttemptInfo, bool> lambda, int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1, bool ui = true, string desc = null)
        {
            return loot.Add(ItemDropRule.ByCondition(If(lambda, ui, desc), itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator));
        }
        #endregion

        #region "Calamity Style" Drop Rule
        /// <summary>
        /// Also known as the "Calamity Style" drop rule.<br />
        /// Every item in the list has the given chance to drop individually.<br />
        /// If no items drop, then one of them is forced to drop, chosen at random.
        /// </summary>
        public class AllOptionsAtOnceWithPityDropRule : IItemDropRule
        {
            public WeightedItemStack[] stacks;
            public Fraction dropRate;
            public bool usesLuck;
            public List<IItemDropRuleChainAttempt> ChainedRules { get; set; }

            public AllOptionsAtOnceWithPityDropRule(Fraction dropRate, bool luck, params WeightedItemStack[] stacks)
            {
                this.dropRate = dropRate;
                this.stacks = stacks;
                usesLuck = luck;
                ChainedRules = new List<IItemDropRuleChainAttempt>();
            }

            public AllOptionsAtOnceWithPityDropRule(Fraction dropRate, bool luck, params int[] itemIDs)
            {
                this.dropRate = dropRate;
                stacks = new WeightedItemStack[itemIDs.Length];
                for (int i = 0; i < stacks.Length; ++i)
                    stacks[i] = itemIDs[i]; // implicit conversion operator
                usesLuck = luck;
                ChainedRules = new List<IItemDropRuleChainAttempt>();
            }

            public bool CanDrop(DropAttemptInfo info) => true;

            public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
            {
                bool droppedAnything = false;

                // Roll for each drop individually.
                foreach (WeightedItemStack stack in stacks)
                {
                    bool rngRoll = usesLuck ? info.player.RollLuck(dropRate.denominator) < dropRate.numerator : info.rng.NextFloat() < dropRate;
                    droppedAnything |= rngRoll;
                    if (rngRoll)
                        CommonCode.DropItem(info, stack.itemID, stack.ChooseQuantity(info.rng));
                }

                // If everything fails to drop, force drop one item from the set.
                if (!droppedAnything)
                {
                    WeightedItemStack stack = info.rng.NextFromList(stacks);
                    CommonCode.DropItem(info, stack.itemID, stack.ChooseQuantity(info.rng));
                }

                // Calamity style drops cannot fail. You will always get at least one item.
                ItemDropAttemptResult result = default;
                result.State = ItemDropAttemptResultState.Success;
                return result;
            }

            public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
            {
                int numDrops = stacks.Length;
                float rawDropRate = dropRate;
                // Combinatorics:
                // OPTION 1: [The item drops = Raw Drop Rate]
                // +
                // OPTION 2: [ALL items fail to drop = (1-x)^n] * [This item is chosen as pity = 1/n]
                float dropRateWithPityRoll = rawDropRate + (float)(Math.Pow(1f - rawDropRate, numDrops) * (1f / numDrops));
                float dropRateAdjustedForParent = dropRateWithPityRoll * ratesInfo.parentDroprateChance;

                // Report the drop rate of each individual item. This calculation includes the fact that each individual item can be guaranteed as pity.
                foreach (WeightedItemStack stack in stacks)
                    drops.Add(new DropRateInfo(stack.itemID, stack.minQuantity, stack.maxQuantity, dropRateAdjustedForParent, ratesInfo.conditions));

                Chains.ReportDroprates(ChainedRules, rawDropRate, drops, ratesInfo);
            }
        }

        public static IItemDropRule CalamityStyle(Fraction dropRateForEachItem, params WeightedItemStack[] stacks) => CalamityStyle(dropRateForEachItem, true, stacks);
        public static IItemDropRule CalamityStyle(Fraction dropRateForEachItem, bool luck, params WeightedItemStack[] stacks)
        {
            return new AllOptionsAtOnceWithPityDropRule(dropRateForEachItem, luck, stacks);
        }
        public static IItemDropRule CalamityStyle(Fraction dropRateForEachItem, params int[] itemIDs) => CalamityStyle(dropRateForEachItem, true, itemIDs);
        public static IItemDropRule CalamityStyle(Fraction dropRateForEachItem, bool luck, params int[] itemIDs)
        {
            return new AllOptionsAtOnceWithPityDropRule(dropRateForEachItem, luck, itemIDs);
        }
        #endregion
        
        #region Per Player Drop Rule
        public class PerPlayerDropRule : CommonDrop
        {
            // Calamity Classic 1.4.5 also defaults this to vanilla's 15 minutes, to be period accurate.
            private const int DefaultDropProtectionTime = 54000; // 15 minutes
            private int protectionTime;

            public PerPlayerDropRule(int itemID, int denominator, int minQuantity = 1, int maxQuantity = 1, int numerator = 1, int protectFrames = DefaultDropProtectionTime)
                : base(itemID, denominator, minQuantity, maxQuantity, numerator)
            {
                protectionTime = protectFrames;
            }

            public PerPlayerDropRule(int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1)
                : base(itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator)
            {
                protectionTime = DefaultDropProtectionTime;
            }

            // Overriding CanDrop is unnecessary. This drop rule has no condition.
            // If you want to use a condition with PerPlayerDropRule, use DropHelper.If

            public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
            {
                ItemDropAttemptResult result = default;
                if (info.rng.Next(chanceDenominator) < chanceNumerator)
                {
                    int stack = info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1);
                    TryDropInternal(info, itemId, stack);
                    result.State = ItemDropAttemptResultState.Success;
                    return result;
                }

                result.State = ItemDropAttemptResultState.FailedRandomRoll;
                return result;
            }

            // The contents of this method are more or less copied from CommonCode.DropItemLocalPerClientAndSetNPCMoneyTo0
            private void TryDropInternal(DropAttemptInfo info, int itemId, int stack)
            {
                if (itemId <= 0 || itemId >= ItemLoader.ItemCount)
                    return;

                // If server-side, then the item must be spawned for each client individually.
                if (Main.dedServ)
                {
                    NPC npc = info.npc;
                    int idx = Item.NewItem(npc.GetSource_Loot(), npc.Center, itemId, stack, true, -1);
                    if (idx < Main.maxItems)
                    {
                        Main.timeItemSlotCannotBeReusedFor[idx] = protectionTime;
                        foreach (Player player in Main.ActivePlayers)
                            NetMessage.SendData(MessageID.InstancedItem, player.whoAmI, -1, null, idx);
                        Main.item[idx].active = false;
                    }
                }

                // Otherwise just drop the item.
                else
                    CommonCode.DropItem(info, itemId, stack);
            }
        }

        public static IItemDropRule PerPlayer(int itemID, int denominator = 1, int minQuantity = 1, int maxQuantity = 1, int numerator = 1)
        {
            return new PerPlayerDropRule(itemID, denominator, minQuantity, maxQuantity, numerator);
        }
        public static IItemDropRule PerPlayer(int itemID, Fraction dropRate, int minQuantity = 1, int maxQuantity = 1)
        {
            return PerPlayer(itemID, dropRate.denominator, minQuantity, maxQuantity, dropRate.numerator);
        }
        #endregion
        
        #region Specific Drop Helpers
        // Code copied from Player.QuickSpawnClonedItem, which was added by TML.
        /// <summary>
        /// Clones the given item and spawns it into the world at the given position. You can also customize stack count as necessary.<br></br>
        /// The default stack count of -1 makes it copy the stack count of the given item.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="item">The item to clone and spawn.</param>
        /// <param name="position">Where the item should be spawned.</param>
        /// <param name="stack">The stack count to use. Leave at -1 to use the stack of the <b>item</b> parameter.</param>
        /// <returns>The spawned clone of the item. <b>NEVER</b> equal to the input item.</returns>
        public static Item DropItemClone(IEntitySource src, Item item, Vector2 position, int stack = -1)
        {
            int index = Item.NewItem(src, position, item.type, stack, false, -1, false, false);
            Item theClone = Main.item[index] = item.Clone();
            theClone.whoAmI = index;
            theClone.position = position;
            if (stack != -1)
                theClone.stack = stack;

            // If in multiplayer, broadcast that this item was spawned.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, index, 1f);

            return theClone;
        }

        /// <summary>
        /// Finds the worm segment nearest to an NPC's target by combing the NPC array for the closest NPC that is one of the specified types.<br></br>
        /// Return the specified NPC's index if no matching worm segment was found.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="wormHead">The NPC whose target is used for distance comparisons.</param>
        /// <param name="wormSegmentIDs">An array (or multiple parameters) of NPC IDs which are the worm segments to look for.</param>
        /// <returns>An index in the NPC array of the closest worm segment, or the specified NPC's index.</returns>
        public static int FindClosestWormSegment(IEntitySource src, NPC wormHead, params int[] wormSegmentIDs)
        {
            List<int> idsToCheck = new List<int>(wormSegmentIDs);
            Vector2 playerPos = Main.player[wormHead.target].Center;

            int r = wormHead.whoAmI;
            float minDist = 1E+06f;
            for (int i = 0; i < Main.npc.Length; ++i)
            {
                NPC n = Main.npc[i];
                if (n != null && n.active && idsToCheck.Contains(n.type))
                {
                    float dist = (n.Center - playerPos).Length();
                    if (dist < minDist)
                    {
                        minDist = dist;
                        r = i;
                    }
                }
            }
            return r;
        }
        
        /// <summary>
        /// Shorthand for shorthand: Registers an item to drop per-player.<br />
        /// Intended for lore items, but can be used generally for instanced drops.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="itemID">The item ID to drop.</param>
        /// <returns>A LeadingConditionRule which you can attach more PerPlayer or other rules to as you want.</returns>
        public static IItemDropRule AddPerPlayer(this ILoot loot, int itemID, int denominator = 1, int minQuantity = 1, int maxQuantity = 1, int numerator = 1) => loot.Add(PerPlayer(itemID, denominator, minQuantity, maxQuantity, numerator));
        
        /// <summary>
        /// Shorthand for shorthand: Registers an item to drop per-player.<br />
        /// Intended for lore items, but can be used generally for instanced drops.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="itemID">The item ID to drop.</param>
        /// <returns>A LeadingConditionRule which you can attach more PerPlayer or other rules to as you want.</returns>
        public static IItemDropRule AddPerPlayer(this LeadingConditionRule mainRule, int itemID, int denominator = 1, int minQuantity = 1, int maxQuantity = 1, int numerator = 1) => mainRule.OnSuccess(PerPlayer(itemID, denominator, minQuantity, maxQuantity, numerator));
        
        /// <summary>
        /// Shorthand for shorthand: Registers an item to drop per-player on the specified condition.<br />
        /// Intended for lore items, but can be used generally for instanced drops.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item ID to drop.</param>
        /// <param name="denominator">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>A LeadingConditionRule which you can attach more PerPlayer or other rules to as you want.</returns>
        public static LeadingConditionRule AddConditionalPerPlayer(this LeadingConditionRule mainRule, Func<bool> lambda, int itemID, int denominator = 1, int minQuantity = 1, int maxQuantity = 1, int numerator = 1, bool ui = true, string desc = null)
        {
            LeadingConditionRule lcr = new(If(lambda, ui, desc));
            lcr.Add(PerPlayer(itemID, denominator, minQuantity, maxQuantity, numerator));
            mainRule.OnSuccess(lcr);
            return mainRule;
        }
        
        /// <summary>
        /// Shorthand for shorthand: Registers an item to drop per-player on the specified condition.<br />
        /// Intended for lore items, but can be used generally for instanced drops.
        /// </summary>
        /// <param name="mainRule">The LeadingConditionRule which should drop this item as one of its chains.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item ID to drop.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>A LeadingConditionRule which you can attach more PerPlayer or other rules to as you want.</returns>
        public static LeadingConditionRule AddConditionalPerPlayer(this LeadingConditionRule mainRule, Func<bool> lambda, int itemID, bool ui = true, string desc = null)
        {
            LeadingConditionRule lcr = new(If(lambda, ui, desc));
            lcr.Add(PerPlayer(itemID));
            mainRule.OnSuccess(lcr);
            return mainRule;
        }
        
        /// <summary>
        /// Shorthand for shorthand: Registers an item to drop per-player on the specified condition.<br />
        /// Intended for lore items, but can be used generally for instanced drops.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item ID to drop.</param>
        /// <param name="denominator">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>A LeadingConditionRule which you can attach more PerPlayer or other rules to as you want.</returns>
        public static LeadingConditionRule AddConditionalPerPlayer(this ILoot loot, Func<bool> lambda, int itemID, int denominator = 1, int minQuantity = 1, int maxQuantity = 1, int numerator = 1, bool ui = true, string desc = null)
        {
            LeadingConditionRule lcr = new(If(lambda, ui, desc));
            lcr.Add(PerPlayer(itemID, denominator, minQuantity, maxQuantity, numerator));
            loot.Add(lcr);
            return lcr;
        }
        
        /// <summary>
        /// Shorthand for shorthand: Registers an item to drop per-player on the specified condition.<br />
        /// Intended for lore items, but can be used generally for instanced drops.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item ID to drop.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>A LeadingConditionRule which you can attach more PerPlayer or other rules to as you want.</returns>
        public static LeadingConditionRule AddConditionalPerPlayer(this ILoot loot, Func<bool> lambda, int itemID, bool ui = true, string desc = null)
        {
            LeadingConditionRule lcr = new(If(lambda, ui, desc));
            lcr.Add(PerPlayer(itemID));
            loot.Add(lcr);
            return lcr;
        }

        /// <summary>
        /// Shorthand for shorthand: Registers an item to drop per-player on the specified condition.<br />
        /// Intended for lore items, but can be used generally for instanced drops.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item ID to drop.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>A LeadingConditionRule which you can attach more PerPlayer or other rules to as you want.</returns>
        public static LeadingConditionRule AddConditionalPerPlayer(this ILoot loot, Func<DropAttemptInfo, bool> lambda, int itemID, bool ui = true, string desc = null)
        {
            LeadingConditionRule lcr = new(If(lambda, ui, desc));
            lcr.Add(PerPlayer(itemID));
            loot.Add(lcr);
            return lcr;
        }
        
        /// <summary>
        /// Shorthand for shorthand: Registers an item to drop per-player on the specified condition.<br />
        /// Intended for lore items, but can be used generally for instanced drops.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="lambda">A lambda which evaluates in real-time to the condition that needs to be checked.</param>
        /// <param name="itemID">The item ID to drop.</param>
        /// <param name="dropRateInt">The chance that the item will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 1.</param>
        /// <param name="ui">Whether drops registered with this condition appear in the Bestiary. Defaults to true.</param>
        /// <param name="desc">The description of this condition in the Bestiary. Defaults to null.</param>
        /// <returns>A LeadingConditionRule which you can attach more PerPlayer or other rules to as you want.</returns>
        public static LeadingConditionRule AddConditionalPerPlayer(this ILoot loot, Func<DropAttemptInfo, bool> lambda, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1, bool ui = true, string desc = null)
        {
            LeadingConditionRule lcr = new(If(lambda, ui, desc));
            lcr.Add(PerPlayer(itemID, dropRateInt, minQuantity, maxQuantity));
            loot.Add(lcr);
            return lcr;
        }

        /// <summary>
        /// Adds the Revengeance Mode bag accessories to the given loot table.
        /// </summary>
        /// <param name="loot">The ILoot interface for the loot table.</param>
        public static void AddRevBagAccessories(this ILoot loot)
        {
            var lcr = new LeadingConditionRule(If(() => CalamityWorld.revenge));
            lcr.Add(new OneFromOptionsDropRule(20, 1,  ModContent.ItemType<StressPills>(), ModContent.ItemType<Laudanum>(), ModContent.ItemType<HeartofDarkness>()));
            loot.Add(lcr);
        }
        
        /// <summary>
        /// Adds finite use "Resident Evil" ammunition to the given loot table, if the downed boolean isn't already true.
        /// </summary>
        /// /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="alreadyKilled">A downed boolean corresponding to this NPC. Use "false" to always drop ammo.</param>
        /// <param name="magnum">The number of Magnum Rounds to drop.</param>
        /// <param name="bazooka">The number of Grenade Rounds to drop.</param>
        /// <param name="hydra">The number of Explosive Shells to drop.</param>
        /// <returns>The total amount of ammunition dropped.</returns>
        public static void AddResidentEvilAmmo(this ILoot loot, bool alreadyKilled, int magnum, int bazooka, int hydra)
        {
            var rule = new LeadingConditionRule(If(() => !alreadyKilled));
            if (magnum != 0)
                rule.Add(ItemDropRule.Common(ModContent.ItemType<MagnumRounds>(), 1, magnum, magnum));
            if (bazooka != 0)
                rule.Add(ItemDropRule.Common(ModContent.ItemType<GrenadeRounds>(), 1, bazooka, bazooka));
            if (hydra != 0)
                rule.Add(ItemDropRule.Common(ModContent.ItemType<ExplosiveShells>(), 1, hydra, hydra));
            loot.Add(rule);
        }
        
        /// <summary>
        /// Adds finite use "Resident Evil" ammunition to the given loot table, if the downed boolean isn't already true.
        /// </summary>
        /// /// <param name="loot">The ILoot interface for the loot table.</param>
        /// <param name="alreadyKilled">A downed boolean corresponding to this NPC. Use "false" to always drop ammo.</param>
        /// <param name="magnum">The number of Magnum Rounds to drop.</param>
        /// <param name="bazooka">The number of Grenade Rounds to drop.</param>
        /// <param name="hydra">The number of Explosive Shells to drop.</param>
        /// <returns>The total amount of ammunition dropped.</returns>
        public static void AddResidentEvilAmmo(this LeadingConditionRule mainRule, bool alreadyKilled, int magnum, int bazooka, int hydra)
        {
            var rule = new LeadingConditionRule(If(() => !alreadyKilled));
            if (magnum != 0)
                rule.Add(ItemDropRule.Common(ModContent.ItemType<MagnumRounds>(), 1, magnum, magnum));
            if (bazooka != 0)
                rule.Add(ItemDropRule.Common(ModContent.ItemType<GrenadeRounds>(), 1, bazooka, bazooka));
            if (hydra != 0)
                rule.Add(ItemDropRule.Common(ModContent.ItemType<ExplosiveShells>(), 1, hydra, hydra));
            mainRule.Add(rule);
        }

        /// <summary>
        /// Drops finite use "Resident Evil" ammunition from the given NPC, if the downed boolean isn't already true.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="theBoss">The NPC to drop ammo from.</param>
        /// <param name="alreadyKilled">A downed boolean corresponding to this NPC. Use "false" to always drop ammo.</param>
        /// <param name="magnum">The number of Magnum Rounds to drop.</param>
        /// <param name="bazooka">The number of Grenade Rounds to drop.</param>
        /// <param name="hydra">The number of Explosive Shells to drop.</param>
        /// <returns>The total amount of ammunition dropped.</returns>
        public static int DropResidentEvilAmmo(IEntitySource src, NPC theBoss, bool alreadyKilled, int magnum, int bazooka, int hydra)
        {
            if (alreadyKilled)
                return 0;

            int dropped = 0;
            dropped += DropItem(src, theBoss, ModContent.ItemType<MagnumRounds>(), magnum);
            dropped += DropItem(src,theBoss, ModContent.ItemType<GrenadeRounds>(), bazooka);
            dropped += DropItem(src,theBoss, ModContent.ItemType<ExplosiveShells>(), hydra);
            return dropped;
        }

        /// <summary>
        /// Randomly peppers stacks of 1 of the specified item all across the given NPC's hitbox.<br></br>
        /// Makes it appear as though the NPC "explodes" into a cloud of many identical items. Best used with floating items such as Souls.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <param name="stackSize">The number of items to place in each separate stack.</param>
        /// <returns>The number of items dropped. Not always equal to quantity if stack size isn't 1.</returns>
        public static int DropItemSpray(IEntitySource src, NPC npc, int itemID, int minQuantity = 1, int maxQuantity = 0, int stackSize = 1)
        {
            int quantity;

            // If they're equal (or for some reason max is less??) then just drop the minimum amount.
            if (maxQuantity <= minQuantity)
                quantity = minQuantity;

            // Otherwise pick a random amount to drop, inclusive.
            else
                quantity = Main.rand.Next(minQuantity, maxQuantity + 1);

            int dropped = 0;
            Vector2 pos = Vector2.Zero;
            for (int i = 0; i < quantity; i += stackSize)
            {
                pos.X = Main.rand.NextFloat(npc.Hitbox.Left, npc.Hitbox.Right);
                pos.Y = Main.rand.NextFloat(npc.Hitbox.Top, npc.Hitbox.Bottom);
                Item.NewItem(src, pos, itemID, stackSize);
                dropped += stackSize;
            }

            return dropped;
        }

        /// <summary>
        /// Drops an item that may instead be replaced by a given Rare Item Variant (RIV).
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item.</param>
        /// <param name="itemID">The ID of the normal item to drop.</param>
        /// <param name="rareID">The ID of the rare item to drop.</param>
        /// <param name="itemChance">The chance that one of the two will drop. A decimal number <= 1.0.</param>
        /// <param name="rareChance">The chance that the RIV will drop. A decimal number <= 1.0.</param>
        /// <returns>Whether an item was spawned.</returns>
        public static bool DropItemRIV(IEntitySource src, NPC npc, int itemID, int rareID, float itemChance, float rareChance = RareVariantDropRateFloat)
        {
            float f = Main.rand.NextFloat();
            bool replaceWithRare = f <= rareChance; // 1/X chance overall of getting RIV
            if (f <= itemChance) // 1/X chance of getting original OR the RIV replacing it
            {
                DropItemCondition(src, npc, itemID, !replaceWithRare);
                DropItemCondition(src, npc, rareID, replaceWithRare);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Drops an item that may instead be replaced by a given Rare Item Variant (RIV).
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="player">The player which should receive the item.</param>
        /// <param name="itemID">The ID of the normal item to drop.</param>
        /// <param name="rareID">The ID of the rare item to drop.</param>
        /// <param name="itemChance">The chance that one of the two will drop. A decimal number <= 1.0.</param>
        /// <param name="rareChance">The chance that the RIV will drop. A decimal number <= 1.0.</param>
        /// <returns>Whether an item was spawned.</returns>
        public static bool DropItemRIV(IEntitySource src, Player player, int itemID, int rareID, float itemChance, float rareChance = RareVariantDropRateFloat)
        {
            float f = Main.rand.NextFloat();
            bool replaceWithRare = f <= rareChance; // 1/X chance overall of getting RIV
            if (f <= itemChance) // 1/X chance of getting original OR the RIV replacing it
            {
                DropItemCondition(src, player, itemID, !replaceWithRare);
                DropItemCondition(src, player, rareID, replaceWithRare);
                return true;
            }
            return false;
        }
        #endregion

        #region NPC Item Drops 100% Chance
        /// <summary>
        /// Drops a stack of one or more items from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItem(IEntitySource src, NPC npc, int itemID, bool dropPerPlayer, int minQuantity = 1, int maxQuantity = 0)
        {
            int quantity;

            // If they're equal (or for some reason max is less??) then just drop the minimum amount.
            if (maxQuantity <= minQuantity)
                quantity = minQuantity;

            // Otherwise pick a random amount to drop, inclusive.
            else
                quantity = Main.rand.Next(minQuantity, maxQuantity + 1);

            // If the final quantity is 0 or less, don't bother.
            if (quantity <= 0)
                return 0;

            // If the drop is supposed to be instanced, drop it as such.
            if (dropPerPlayer)
            {
                npc.DropItemInstanced(npc.position, npc.Size, itemID, quantity, true);
            }
            else
            {
                Item.NewItem(src, npc.Hitbox, itemID, quantity);
            }

            return quantity;
        }

        /// <summary>
        /// Drops a stack of one or more items from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItem(IEntitySource src, NPC npc, int itemID, int minQuantity = 1, int maxQuantity = 0)
        {
            return DropItem(src, npc, itemID, false, minQuantity, maxQuantity);
        }
        #endregion

        #region NPC Item Drops Float Chance
        /// <summary>
        /// At a chance, drops a stack of one or more items from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="chance">The chance that the items will drop. A decimal number <= 1.0.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemChance(IEntitySource src, NPC npc, int itemID, bool dropPerPlayer, float chance, int minQuantity = 1, int maxQuantity = 0)
        {
            // If you fail the roll to get the drop, stop immediately.
            if (Main.rand.NextFloat() > chance)
                return 0;

            return DropItem(src, npc, itemID, dropPerPlayer, minQuantity, maxQuantity);
        }

        /// <summary>
        /// At a chance, drops a stack of one or more items from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="chance">The chance that the items will drop. A decimal number <= 1.0.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemChance(IEntitySource src, NPC npc, int itemID, float chance, int minQuantity = 1, int maxQuantity = 0)
        {
            return DropItemChance(src, npc, itemID, false, chance, minQuantity, maxQuantity);
        }
        #endregion

        #region NPC Item Drops Int Chance
        /// <summary>
        /// At a chance, drops a stack of one or more items from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="oneInXChance">The chance that the items will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemChance(IEntitySource src, NPC npc, int itemID, bool dropPerPlayer, int oneInXChance, int minQuantity = 1, int maxQuantity = 0)
        {
            // If you fail the roll to get the drop, stop immediately.
            if (Main.rand.Next(oneInXChance) != 0)
                return 0;

            return DropItem(src, npc, itemID, dropPerPlayer, minQuantity, maxQuantity);
        }

        /// <summary>
        /// At a chance, drops a stack of one or more items from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="oneInXChance">The chance that the items will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemChance(IEntitySource src, NPC npc, int itemID, int oneInXChance, int minQuantity = 1, int maxQuantity = 0)
        {
            return DropItemChance(src, npc, itemID, false, oneInXChance, minQuantity, maxQuantity);
        }
        #endregion

        #region NPC Item Drops Conditional
        /// <summary>
        /// With a condition, drops a stack of one or more items from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemCondition(IEntitySource src, NPC npc, int itemID, bool dropPerPlayer, bool condition, int minQuantity = 1, int maxQuantity = 0)
        {
            return condition ? DropItem(src, npc, itemID, dropPerPlayer, minQuantity, maxQuantity) : 0;
        }

        /// <summary>
        /// With a condition, drops a stack of one or more items from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemCondition(IEntitySource src, NPC npc, int itemID, bool condition, int minQuantity = 1, int maxQuantity = 0)
        {
            return condition ? DropItem(src, npc, itemID, false, minQuantity, maxQuantity) : 0;
        }

        /// <summary>
        /// With a condition and at a chance, drops a stack of one or more items from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="chance">The chance that the items will drop. A decimal number <= 1.0.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemCondition(IEntitySource src, NPC npc, int itemID, bool dropPerPlayer, bool condition, float chance, int minQuantity = 1, int maxQuantity = 0)
        {
            return condition ? DropItemChance(npc.GetSource_FromThis(), npc, itemID, dropPerPlayer, chance, minQuantity, maxQuantity) : 0;
        }

        /// <summary>
        /// With a condition and at a chance, drops a stack of one or more items from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="chance">The chance that the items will drop. A decimal number <= 1.0.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemCondition(IEntitySource src, NPC npc, int itemID, bool condition, float chance, int minQuantity = 1, int maxQuantity = 0)
        {
            return condition ? DropItemChance(npc.GetSource_FromThis(), npc, itemID, false, chance, minQuantity, maxQuantity) : 0;
        }

        /// <summary>
        /// With a condition and at a chance, drops a stack of one or more items from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="oneInXChance">The chance that the items will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemCondition(IEntitySource src, NPC npc, int itemID, bool dropPerPlayer, bool condition, int oneInXChance, int minQuantity = 1, int maxQuantity = 0)
        {
            return condition ? DropItemChance(npc.GetSource_FromThis(), npc, itemID, dropPerPlayer, oneInXChance, minQuantity, maxQuantity) : 0;
        }

        /// <summary>
        /// With a condition and at a chance, drops a stack of one or more items from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to drop.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="oneInXChance">The chance that the items will drop is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to drop. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to drop. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropItemCondition(IEntitySource src, NPC npc, int itemID, bool condition, int oneInXChance, int minQuantity = 1, int maxQuantity = 0)
        {
            return condition ? DropItemChance(npc.GetSource_FromThis(), npc, itemID, false, oneInXChance, minQuantity, maxQuantity) : 0;
        }
        #endregion

        #region NPC Item Set Drops
        /// <summary>
        /// Chooses an item from an array and drops it from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>Whether an item was dropped.</returns>
        public static bool DropItemFromSet(IEntitySource src, NPC npc, bool dropPerPlayer, params int[] itemIDs)
        {
            // Can't choose anything from an empty array.
            if (itemIDs is null || itemIDs.Length == 0)
                return false;

            // Choose which item to drop.
            int itemID = Main.rand.Next(itemIDs);

            // If the drop is supposed to be instanced, drop it as such.
            if (dropPerPlayer)
            {
                npc.DropItemInstanced(npc.position, npc.Size, itemID, 1, true);
            }
            else
            {
                Item.NewItem(src, npc.Hitbox, itemID);
            }

            return true;
        }

        /// <summary>
        /// Chooses an item from an array and drops it from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>Whether an item was dropped.</returns>
        public static bool DropItemFromSet(IEntitySource src, NPC npc, params int[] itemIDs)
        {
            return DropItemFromSet(src, npc, false, itemIDs);
        }

        /// <summary>
        /// At a chance, chooses an item from an array and drops it from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="chance">The chance that the item will drop. A decimal number <= 1.0.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>Whether an item was dropped.</returns>
        public static bool DropItemFromSetChance(IEntitySource src, NPC npc, bool dropPerPlayer, float chance, params int[] itemIDs)
        {
            // If you fail the roll to get the drop, stop immediately.
            if (Main.rand.NextFloat() > chance)
                return false;

            return DropItemFromSet(src, npc, dropPerPlayer, itemIDs);
        }

        /// <summary>
        /// At a chance, chooses an item from an array and drops it from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item.</param>
        /// <param name="chance">The chance that the item will drop. A decimal number <= 1.0.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>Whether an item was dropped.</returns>
        public static bool DropItemFromSetChance(IEntitySource src, NPC npc, float chance, params int[] itemIDs)
        {
            return DropItemFromSetChance(src, npc, false, chance, itemIDs);
        }
        #endregion

        #region NPC Item Set Drops Conditional
        /// <summary>
        /// With a condition, chooses an item from an array and drops it from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        public static bool DropItemFromSetCondition(IEntitySource src, NPC npc, bool dropPerPlayer, bool condition, params int[] itemIDs)
        {
            return condition ? DropItemFromSet(src, npc, dropPerPlayer, itemIDs) : false;
        }

        /// <summary>
        /// With a condition, chooses an item from an array and drops it from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        public static bool DropItemFromSetCondition(IEntitySource src, NPC npc, bool condition, params int[] itemIDs)
        {
            return condition ? DropItemFromSet(src, npc, false, itemIDs) : false;
        }

        /// <summary>
        /// With a condition, chooses an item from an array and drops it from the given NPC. Optionally spawns one copy of this drop per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item.</param>
        /// <param name="dropPerPlayer">Whether the drop should be "instanced" (each player gets their own copy).</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="chance">The chance that the item will drop. A decimal number <= 1.0.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        public static bool DropItemFromSetCondition(IEntitySource src, NPC npc, bool dropPerPlayer, bool condition, float chance, params int[] itemIDs)
        {
            return condition ? DropItemFromSetChance(src, npc, dropPerPlayer, chance, itemIDs) : false;
        }

        /// <summary>
        /// With a condition, chooses an item from an array and drops it from the given NPC.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the item.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this drop. If false, nothing is dropped.</param>
        /// <param name="chance">The chance that the item will drop. A decimal number <= 1.0.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        public static bool DropItemFromSetCondition(IEntitySource src, NPC npc, bool condition, float chance, params int[] itemIDs)
        {
            return condition ? DropItemFromSetChance(src, npc, false, chance, itemIDs) : false;
        }
        #endregion

        #region NPC Entire Set Drops
        /// <summary>
        /// Rolls for each item in an array to drop at a given chance. Always drops at least one item.<br></br>
        /// Optionally spawns one copy of these drops per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the items.</param>
        /// <param name="dropPerPlayer">Whether the drops should be "instanced" (each player gets their own copy).</param>
        /// <param name="chance">The chance that an item will drop. A decimal number <= 1.0.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropEntireSet(IEntitySource src, NPC npc, bool dropPerPlayer, float chance, params int[] itemIDs)
        {
            int numDrops = 0;

            // Can't choose anything from an empty array.
            if (itemIDs is null || itemIDs.Length == 0)
                return numDrops;

            // Tally the total number of items dropped as the drop set is iterated through.
            for (int i = 0; i < itemIDs.Length; ++i)
                numDrops += DropItemChance(npc.GetSource_FromThis(), npc, itemIDs[i], dropPerPlayer, chance);

            // If nothing at all was dropped, drop one thing at random.
            numDrops += DropItemFromSetCondition(src, npc, dropPerPlayer, numDrops <= 0, itemIDs) ? 1 : 0;
            return numDrops;
        }

        /// <summary>
        /// Rolls for each item in an array to drop at a given chance. Always drops at least one item.<br></br>
        /// Optionally spawns one copy of these drops per player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the items.</param>
        /// <param name="dropPerPlayer">Whether the drops should be "instanced" (each player gets their own copy).</param>
        /// <param name="oneInXChance">The chance that the items will spawn is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropEntireSet(IEntitySource src, NPC npc, bool dropPerPlayer, int oneInXChance, params int[] itemIDs)
        {
            int numDrops = 0;

            // Can't choose anything from an empty array.
            if (itemIDs is null || itemIDs.Length == 0)
                return numDrops;

            // Tally the total number of items dropped as the drop set is iterated through.
            for (int i = 0; i < itemIDs.Length; ++i)
                numDrops += DropItemChance(npc.GetSource_FromThis(), npc, itemIDs[i], dropPerPlayer, oneInXChance);

            // If nothing at all was dropped, drop one thing at random.
            numDrops += DropItemFromSetCondition(src, npc, dropPerPlayer, numDrops <= 0, itemIDs) ? 1 : 0;
            return numDrops;
        }

        /// <summary>
        /// Rolls for each item in an array to drop at a given chance. Always drops at least one item.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the items.</param>
        /// <param name="chance">The chance that an item will drop. A decimal number <= 1.0.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropEntireSet(IEntitySource src, NPC npc, float chance, params int[] itemIDs)
        {
            return DropEntireSet(src, npc, false, chance, itemIDs);
        }

        /// <summary>
        /// Rolls for each item in an array to drop at a given chance. Always drops at least one item.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="npc">The NPC which should drop the items.</param>
        /// <param name="oneInXChance">The chance that the items will spawn is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropEntireSet(IEntitySource src, NPC npc, int oneInXChance, params int[] itemIDs)
        {
            return DropEntireSet(src, npc, false, oneInXChance, itemIDs);
        }
        #endregion

        #region Player Item Spawns
        /// <summary>
        /// Spawns a stack of one or more items for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to spawn.</param>
        /// <param name="minQuantity">The minimum number of items to spawn. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to spawn. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items spawned.</returns>
        public static int DropItem(IEntitySource src, Player p, int itemID, int minQuantity = 1, int maxQuantity = 0)
        {
            int quantity;

            // If they're equal (or for some reason max is less??) then just drop the minimum amount.
            if (maxQuantity <= minQuantity)
                quantity = minQuantity;

            // Otherwise pick a random amount to drop, inclusive.
            else
                quantity = Main.rand.Next(minQuantity, maxQuantity + 1);

            // If the final quantity is 0 or less, don't bother.
            if (quantity <= 0)
                return 0;

            p.QuickSpawnItem(src,  itemID, quantity);
            return quantity;
        }

        /// <summary>
        /// At a chance, spawns a stack of one or more items for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to spawn.</param>
        /// <param name="chance">The chance that the items will spawn. A decimal number <= 1.0.</param>
        /// <param name="minQuantity">The minimum number of items to spawn. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to spawn. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items spawned.</returns>
        public static int DropItemChance(IEntitySource src, Player p, int itemID, float chance, int minQuantity = 1, int maxQuantity = 0)
        {
            // If you fail the roll to get the drop, stop immediately.
            if (Main.rand.NextFloat() > chance)
                return 0;

            return DropItem(src, p, itemID, minQuantity, maxQuantity);
        }

        /// <summary>
        /// At a chance, spawns a stack of one or more items for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to spawn.</param>
        /// <param name="oneInXChance">The chance that the items will spawn is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to spawn. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to spawn. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items spawned.</returns>
        public static int DropItemChance(IEntitySource src, Player p, int itemID, int oneInXChance, int minQuantity = 1, int maxQuantity = 0)
        {
            // If you fail the roll to get the drop, stop immediately.
            if (Main.rand.Next(oneInXChance) != 0)
                return 0;

            return DropItem(src, p, itemID, minQuantity, maxQuantity);
        }
        #endregion

        #region Player Item Spawns Conditional
        /// <summary>
        /// With a condition, spawns a stack of one or more items for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to spawn.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this spawn. If false, nothing is spawned.</param>
        /// <param name="minQuantity">The minimum number of items to spawn. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to spawn. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items spawned.</returns>
        public static int DropItemCondition(IEntitySource src, Player p, int itemID, bool condition, int minQuantity = 1, int maxQuantity = 0)
        {
            return condition ? DropItem(src, p, itemID, minQuantity, maxQuantity) : 0;
        }

        /// <summary>
        /// With a condition and at a chance, spawns a stack of one or more items for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to spawn.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this spawn. If false, nothing is spawned.</param>
        /// <param name="chance">The chance that the items will spawn. A decimal number <= 1.0.</param>
        /// <param name="minQuantity">The minimum number of items to spawn. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to spawn. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items spawned.</returns>
        public static int DropItemCondition(IEntitySource src, Player p, int itemID, bool condition, float chance, int minQuantity = 1, int maxQuantity = 0)
        {
            return condition ? DropItemChance(src, p, itemID, chance, minQuantity, maxQuantity) : 0;
        }

        /// <summary>
        /// With a condition and at a chance, spawns a stack of one or more items for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item(s).</param>
        /// <param name="itemID">The ID of the item(s) to spawn.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this spawn. If false, nothing is spawned.</param>
        /// <param name="oneInXChance">The chance that the items will spawn is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="minQuantity">The minimum number of items to spawn. Defaults to 1.</param>
        /// <param name="maxQuantity">The maximum number of items to spawn. Defaults to 0, meaning the minimum quantity is always used.</param>
        /// <returns>The number of items spawned.</returns>
        public static int DropItemCondition(IEntitySource src, Player p, int itemID, bool condition, int oneInXChance, int minQuantity = 1, int maxQuantity = 0)
        {
            return condition ? DropItemChance(src, p, itemID, oneInXChance, minQuantity, maxQuantity) : 0;
        }
        #endregion

        #region Player Item Set Spawns
        /// <summary>
        /// Chooses an item from an array and spawns it for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be spawned.</param>
        /// <returns>Whether an item was spawned.</returns>
        public static bool DropItemFromSet(IEntitySource src, Player p, params int[] itemIDs)
        {
            // Can't choose anything from an empty array.
            if (itemIDs is null || itemIDs.Length == 0)
                return false;

            // Choose which item to drop.
            int itemID = Main.rand.Next(itemIDs);

            p.QuickSpawnItem(src, itemID);
            return true;
        }

        /// <summary>
        /// At a chance, chooses an item from an array and spawns it for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item.</param>
        /// <param name="chance">The chance that the item will spawn. A decimal number <= 1.0.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be spawned.</param>
        /// <returns>Whether an item was spawned.</returns>
        public static bool DropItemFromSetChance(IEntitySource src, Player p, float chance, params int[] itemIDs)
        {
            // If you fail the roll to get the drop, stop immediately.
            if (Main.rand.NextFloat() > chance)
                return false;

            return DropItemFromSet(src, p, itemIDs);
        }
        #endregion

        #region Player Item Set Spawns Conditional
        /// <summary>
        /// With a condition, chooses an item from an array and spawns it for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this spawn. If false, nothing is spawned.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be spawned.</param>
        /// <returns>Whether an item was spawned.</returns>
        public static bool DropItemFromSetCondition(IEntitySource src, Player p, bool condition, params int[] itemIDs)
        {
            return condition ? DropItemFromSet(src, p, itemIDs) : false;
        }

        /// <summary>
        /// With a condition and at a chance, chooses an item from an array and spawns it for the given player.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the item.</param>
        /// <param name="condition">Any arbitrary Boolean condition to gate this spawn. If false, nothing is spawned.</param>
        /// <param name="chance">The chance that the items will spawn. A decimal number <= 1.0.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be spawned.</param>
        /// <returns>Whether an item was spawned.</returns>
        public static bool DropItemFromSetCondition(IEntitySource src, Player p, bool condition, float chance, params int[] itemIDs)
        {
            return condition ? DropItemFromSetChance(src, p, chance, itemIDs) : false;
        }
        #endregion

        #region Player Entire Set Spawns
        /// <summary>
        /// Rolls for each item in an array to drop at a given chance. Always drops at least one item.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the items.</param>
        /// <param name="chance">The chance that an item will drop. A decimal number <= 1.0.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropEntireSet(IEntitySource src, Player p, float chance, params int[] itemIDs)
        {
            int numDrops = 0;

            // Can't choose anything from an empty array.
            if (itemIDs is null || itemIDs.Length == 0)
                return numDrops;

            // Tally the total number of items dropped as the drop set is iterated through.
            for (int i = 0; i < itemIDs.Length; ++i)
                numDrops += DropItemChance(src, p, itemIDs[i], chance);

            // If nothing at all was dropped, drop one thing at random.
            numDrops += DropItemFromSetCondition(src, p, numDrops <= 0, itemIDs) ? 1 : 0;
            return numDrops;
        }

        /// <summary>
        /// Rolls for each item in an array to drop at a given chance. Always drops at least one item.
        /// </summary>
        /// <param name="src">The NPC the item(s) belongs to.</param>
        /// <param name="p">The player which should receive the items.</param>
        /// <param name="oneInXChance">The chance that the items will spawn is 1 in this number. For example, 5 gives a 1 in 5 chance.</param>
        /// <param name="itemIDs">The array of items to choose from. If it's null or empty, nothing will be dropped.</param>
        /// <returns>The number of items dropped.</returns>
        public static int DropEntireSet(IEntitySource src, Player p, int oneInXChance, params int[] itemIDs)
        {
            int numDrops = 0;

            // Can't choose anything from an empty array.
            if (itemIDs is null || itemIDs.Length == 0)
                return numDrops;

            // Tally the total number of items dropped as the drop set is iterated through.
            for (int i = 0; i < itemIDs.Length; ++i)
                numDrops += DropItemChance(src, p, itemIDs[i], oneInXChance);

            // If nothing at all was dropped, drop one thing at random.
            numDrops += DropItemFromSetCondition(src,  p, numDrops <= 0, itemIDs) ? 1 : 0;
            return numDrops;
        }
        #endregion
    }
}
