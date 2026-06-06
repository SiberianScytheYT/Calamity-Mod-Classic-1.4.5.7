using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class Nanotech : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nanotech");
/*
            Tooltip.SetDefault("Rogue projectiles create nanoblades as they travel\n" +
                "Stealth strikes summon nanobeams and sparks on enemy hits\n" +
				"Stealth strikes have +20 armor penetration, deal 5% more damage, and heal for 1 HP\n" +
                "12% increased rogue damage and 15% increased rogue velocity\n" +
                "Whenever you crit an enemy with a rogue weapon your rogue damage increases\n" +
                "This effect can stack up to 150 times\n" +
				"Max rogue damage boost is 10%\n" +
				"This line is modified below");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            int critLevel = Main.player[Main.myPlayer].Calamity().raiderStack;
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip8")
                {
                    line2.Text = "Rogue Crit Level: " + critLevel;
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.nanotech = true;
            modPlayer.raiderTalisman = true;
            modPlayer.electricianGlove = true;
            player.Calamity().throwingDamage += 0.12f;
            player.Calamity().throwingVelocity += 0.15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RogueEmblem>());
            recipe.AddIngredient(ModContent.ItemType<RaidersTalisman>());
            recipe.AddIngredient(ModContent.ItemType<MoonstoneCrown>());
            recipe.AddIngredient(ModContent.ItemType<ElectriciansGlove>());
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
