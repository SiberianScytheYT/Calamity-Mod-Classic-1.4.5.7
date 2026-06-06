using CalRD.Buffs.DamageOverTime;
using CalRD.CalPlayer;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class AstralArcanum : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Arcanum");
/*
            Tooltip.SetDefault("Taking damage drops astral stars from the sky\n" +
                               "Provides immunity to the astral infection debuff\n" +
                               "You have a 5% chance to reflect projectiles when they hit you\n" +
                               "If this effect triggers you get healed for the projectile's damage\n" +
                               "Boosts life regen even while under the effects of a damaging debuff\n" +
                               "While under the effects of a damaging debuff you will gain 20 defense\n" +
                               "TOOLTIP LINE HERE");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            string hotkey = CalRD.AstralArcanumUIHotkey.TooltipHotkeyString();
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip6")
                {
                    line2.Text = "Press " + hotkey + " to toggle teleportation UI";
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.astralArcanum = true;
            modPlayer.aBulwark = true;
            modPlayer.projRef = true;
            player.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CelestialJewel>());
            recipe.AddIngredient(ModContent.ItemType<AstralBulwark>());
            recipe.AddIngredient(ModContent.ItemType<ArcanumoftheVoid>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
