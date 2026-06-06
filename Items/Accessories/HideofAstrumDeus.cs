using CalRD.Buffs.DamageOverTime;
using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class HideofAstrumDeus : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hide of Astrum Deus");
/*
            Tooltip.SetDefault("Taking damage drops an immense amount of astral stars from the sky and boosts true melee damage by 50% for a time\n" +
                                "Boost duration is based on the amount of damage you took, the higher the damage the longer the boost\n" +
                                "Provides immunity to the astral infection, cursed inferno, on fire, and frostburn debuffs\n" +
                                "Enemies take damage when they hit you and are inflicted with the astral infection debuff");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity9BuyPrice;
            Item.rare = 9;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.aBulwark = true;
            player.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            modPlayer.aBulwarkRare = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.thorns += 0.75f;
        }
    }
}
