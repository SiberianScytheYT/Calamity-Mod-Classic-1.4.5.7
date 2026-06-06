using CalRD.Items.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class BlunderBooster : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blunder Booster");
/*
            Tooltip.SetDefault("12% increased rogue damage and 15% increased rogue projectile velocity\n" +
                "Summons a red lightning aura to surround the player and electrify nearby enemies\n" +
                "TOOLTIP LINE HERE" + 
                "This effect has a 3 second cooldown before it can be used again");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 38;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = 10;
			Item.Calamity().postMoonLordRarity = 12;
            Item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */ => !player.Calamity().hasJetpack;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.Calamity().hasJetpack = true;
            player.Calamity().throwingDamage += 0.12f;
            player.Calamity().throwingVelocity += 0.15f;
            player.Calamity().blunderBooster = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            string hotkey = CalRD.PlaguePackHotKey.TooltipHotkeyString();
            foreach (TooltipLine line in list)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                {
                    line.Text = "Press " + hotkey + " to consume 25% of your maximum stealth to perform a swift upwards/diagonal dash which leaves a trail of lightning bolts";
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PlaguedFuelPack>());
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
