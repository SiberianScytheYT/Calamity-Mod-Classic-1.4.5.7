using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class AmbrosialAmpoule : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ambrosial Ampoule");
/*
            Tooltip.SetDefault("25% increased mining speed\n" +
                "You emit light\n" +
                "5% increased damage reduction and increased life regen\n" +
                "Poison, Freeze, Chill, Frostburn, and Venom immunity\n" +
                "Honey-like life regen with no speed penalty\n" +
                "Most bee/hornet enemies and projectiles do 75% damage to you");
*/
        }

        public override void SetDefaults()
        {
            Item.defense = 4;
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip5")
					{
						line2.Text = "Most bee/hornet enemies and projectiles do 75% damage to you\n" +
						"Provides cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.beeResist = true;
            modPlayer.aAmpoule = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CorruptFlask>());
            recipe.AddIngredient(ModContent.ItemType<ArchaicPowder>());
            recipe.AddIngredient(ModContent.ItemType<RadiantOoze>());
            recipe.AddIngredient(ModContent.ItemType<HoneyDew>());
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 15);
            recipe.AddIngredient(ModContent.ItemType<CryoBar>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CrimsonFlask>());
            recipe.AddIngredient(ModContent.ItemType<ArchaicPowder>());
            recipe.AddIngredient(ModContent.ItemType<RadiantOoze>());
            recipe.AddIngredient(ModContent.ItemType<HoneyDew>());
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 15);
            recipe.AddIngredient(ModContent.ItemType<CryoBar>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
