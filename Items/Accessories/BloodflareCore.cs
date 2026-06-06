using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class BloodflareCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bloodflare Core");
/*
			Tooltip.SetDefault("You lose up to half your defense after taking damage\n" + "Lost defense regenerates over time\n" + "You gain 1 health for every 1 defense gained as it regenerates");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.value = CalamityGlobalItem.Rarity13BuyPrice;
			Item.expert = true;
			Item.rare = ItemRarityID.Red;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			CalamityPlayer modPlayer = player.Calamity();
			modPlayer.bloodflareCore = true;
		}
	}
}
