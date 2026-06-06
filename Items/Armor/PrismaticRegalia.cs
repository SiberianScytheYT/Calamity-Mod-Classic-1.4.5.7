using CalRD.Items.Materials;
using CalRD.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class PrismaticRegalia : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Prismatic Regalia");
/*
			Tooltip.SetDefault("12% increased magic damage and 15% increased magic crit\n" +
				"20% decreased non-magic damage\n" +
				"+20 max life and +40 max mana\n" +
				"Magic attacks occasionally fire a pair of homing rockets");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.defense = 33;
			Item.value = CalamityGlobalItem.Rarity13BuyPrice;
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
		}

		public override void UpdateEquip(Player player)
		{
			player.Calamity().prismaticRegalia = true;
			player.statLifeMax2 += 20;
			player.statManaMax2 += 40;
			player.GetDamage(DamageClass.Magic) += 0.12f;
			player.GetCritChance(DamageClass.Magic) += 15;
			player.GetDamage(DamageClass.Generic) -= 0.2f;
			player.GetDamage(DamageClass.Magic) += 0.2f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ArmoredShell>(), 3);
			recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 5);
			recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 8);
			recipe.AddIngredient(ItemID.Nanites, 300);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
