using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class Taser : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Taser");
/*
			Tooltip.SetDefault("A slow, simple electric weapon, meant only for low ranking guards.\n" +
			"Shoots a hook that attaches to enemies and electrocutes them before returning");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 50;
			Item.height = 26;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 22;
			Item.knockBack = 0f;
			Item.useTime = Item.useAnimation = 28;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaBolt");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.Rarity3BuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<TaserHook>();
			Item.shootSpeed = 15f;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 50f;
			modItem.ChargePerUse = 0.05f;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 7);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 5);
			recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 4);
			recipe.AddIngredient(ItemID.MeteoriteBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
