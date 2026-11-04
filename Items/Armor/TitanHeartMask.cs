using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class TitanHeartMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Titan Heart Mask");
/*
			Tooltip.SetDefault("5% increased rogue damage and knockback\n" +
			"Rogue weapons spread the astral infection");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 12, 0, 0);
			Item.rare = 5;
			Item.defense = 12; //43
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<TitanHeartMantle>() && legs.type == ModContent.ItemType<TitanHeartBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.TitanHeartMask");
			CalamityPlayer modPlayer = player.Calamity();
			modPlayer.titanHeartSet = true;
			modPlayer.throwingDamage += 0.2f;
			modPlayer.rogueStealthMax += 1f;
			modPlayer.wearingRogueArmor = true;
		}

		public override void UpdateEquip(Player player)
		{
			CalamityPlayer modPlayer = player.Calamity();
			modPlayer.titanHeartMask = true;
			modPlayer.throwingDamage += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AstralMonolith>(), 10);
			recipe.AddIngredient(ModContent.ItemType<TitanHeart>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
