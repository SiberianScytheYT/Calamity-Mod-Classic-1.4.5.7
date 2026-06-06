using CalRD.CalPlayer;
using CalRD.Items.Potions;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class CelestialJewel : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Jewel");
/*
            Tooltip.SetDefault("Boosts life regen even while under the effects of a damaging debuff\n" +
                "While under the effects of a damaging debuff you will gain 20 defense\n" +
                "TOOLTIP LINE HERE");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            string hotkey = CalRD.AstralTeleportHotKey.TooltipHotkeyString();
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip2")
                {
                    line2.Text = "Press " + hotkey + " to teleport to a random location";
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.celestialJewel = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CrownJewel>());
            recipe.AddIngredient(ModContent.ItemType<AstralJelly>(), 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
