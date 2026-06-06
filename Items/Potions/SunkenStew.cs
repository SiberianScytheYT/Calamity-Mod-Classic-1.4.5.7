using CalRD.Items.Placeables;
using CalRD.Items.Fishing.BrimstoneCragCatches;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
	public class SunkenStew : ModItem
	{
		public static int BuffType = BuffID.WellFed;
		public static int BuffDuration = 216000;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Hadal Stew");
/*
            Tooltip.SetDefault("Only gives 50 seconds of Potion Sickness\n" +
               "Grants Well Fed\n" +
               "60 minute duration");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 18;
			Item.useTurn = true;
			Item.maxStack = 30;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.rare = 3;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.potion = true;
			Item.healLife = 120;
			Item.healMana = 150;
			Item.value = Item.buyPrice(0, 2, 0, 0);
		}

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (Main.LocalPlayer.pStone)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip0")
					{
						line2.Text = "Only gives 37 seconds of Potion Sickness";
					}
				}
			}
        }

        public override bool CanUseItem(Player player)
        {
            return player.potionDelay <= 0 && player.Calamity().potionTimer <= 0;
        }

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
            player.AddBuff(BuffType, BuffDuration);
			// fixes hardcoded potion sickness duration from quick heal (see CalamityPlayerMiscEffects.cs)
			player.Calamity().potionTimer = 2;
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(2);
			recipe.AddIngredient(ModContent.ItemType<AbyssGravel>(), 10);
			recipe.AddIngredient(ModContent.ItemType<CoastalDemonfish>());
			recipe.AddIngredient(ItemID.Honeyfin);
			recipe.AddIngredient(ItemID.Bowl, 2);
			recipe.AddTile(TileID.CookingPots);
			recipe.Register();
			recipe = CreateRecipe(2);
			recipe.AddIngredient(ModContent.ItemType<Voidstone>(), 10);
			recipe.AddIngredient(ModContent.ItemType<CoastalDemonfish>());
			recipe.AddIngredient(ItemID.Honeyfin);
			recipe.AddIngredient(ItemID.Bowl, 2);
			recipe.AddTile(TileID.CookingPots);
			recipe.Register();
		}
	}
}
