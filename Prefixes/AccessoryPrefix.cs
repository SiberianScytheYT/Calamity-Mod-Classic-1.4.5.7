using System;
using System.Collections.Generic;
using CalRD;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Prefixes
{
    public class Quiet : RogueAccessoryPrefix
    {
        public override float stealthGenBonus => 0.02f;
    }

    public class Cloaked : RogueAccessoryPrefix
    {
        public override float stealthGenBonus => 0.04f;
    }

    public class Camouflaged : RogueAccessoryPrefix
    {
        public override float stealthGenBonus => 0.06f;
    }

    public class Silent : RogueAccessoryPrefix
    {
        public override float stealthGenBonus => 0.08f;
    }

    public abstract class RogueAccessoryPrefix : ModPrefix, ILocalizedModType
    {
        public new string LocalizationCategory => "Prefixes.Accessory";

        // Stats
        public virtual float stealthGenBonus => 0f;

        // Prefix roll logic
        public override PrefixCategory Category => PrefixCategory.Accessory;
        public override bool CanRoll(Item item) => GetType() != typeof(RogueAccessoryPrefix);

        // Applying stealth generation
        public override void ApplyAccessoryEffects(Player player)
        {
            player.Calamity().accStealthGenBoost += stealthGenBonus;
        }

        // Changing value based on prefix tier (rarity is set automatically around value multiplier)
        public override void ModifyValue(ref float valueMult)
        {
            float extraValue = 1f + (2.5f * stealthGenBonus);
            valueMult *= extraValue;
        }
    }
}
