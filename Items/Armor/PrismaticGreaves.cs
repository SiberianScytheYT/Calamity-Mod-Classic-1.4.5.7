using CalRD.Items.Materials;
using CalRD.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class PrismaticGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Prismatic Greaves");
/*
			Tooltip.SetDefault("10% increased magic damage and 12% increased magic crit\n" +
				"20% decreased non-magic damage\n" +
				"10% increased flight time and 2% increased jump speed");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.defense = 21;
			Item.value = CalamityGlobalItem.Rarity13BuyPrice;
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
		}

		public override void UpdateEquip(Player player)
		{
			player.Calamity().prismaticGreaves = true;
			player.GetDamage(DamageClass.Magic) += 0.1f;
			player.GetCritChance(DamageClass.Magic) += 12;
			player.jumpSpeedBoost += 0.1f;
			player.GetDamage(DamageClass.Generic) -= 0.2f;
			player.GetDamage(DamageClass.Magic) += 0.2f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ArmoredShell>(), 3);
			recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 5);
			recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 6);
			recipe.AddIngredient(ItemID.Nanites, 300);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
