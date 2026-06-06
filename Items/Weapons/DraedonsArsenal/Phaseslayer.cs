using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class Phaseslayer : ModItem
	{
		public const int Damage = 10200;
		// When below this percentage of charge, the sword is small instead of big.
		public const float SizeChargeThreshold = 0.25f;
		// The small sword barely affects damage on its own because damage is already dropping significantly at low charge.
		public const float SmallDamageMultiplier = 0.9f;
		// This is the amount of charge consumed every frame the holdout projectile is summoned, i.e. the weapon is in use.
		public const float HoldoutChargeUse = 0.005f;
		// This is the amount of charge consumed every time a sword beam is fired.
		public const float SwordBeamChargeUse = 0.1f;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Phaseslayer");
/*
			Tooltip.SetDefault("A rough prototype of the Murasama blade, it is formed entirely from laser energy.\n" +
							   "Wield a colossal laser blade which is controlled by the cursor\n" +
							   "Faster swings deal more damage and release sword beams\n" +
							   "When at low charge, the blade is smaller and weaker\n" +
							   "Deals less damage against enemies with high defense");
*/
		}
		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.damage = Damage;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.width = 26;
			Item.height = 26;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTurn = false;
			Item.knockBack = 7f;

			Item.noUseGraphic = true;

			Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.shoot = ModContent.ProjectileType<PhaseslayerProjectile>();
			Item.channel = true;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 250f;
			modItem.ChargePerUse = 0f; // This weapon is a holdout. Charge is consumed by the holdout projectile.
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 && Item.Calamity().Charge > 0;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile blade = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
			blade.rotation = blade.AngleTo(Main.MouseWorld);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 15);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 25);
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
			recipe.Register();
		}
	}
}
