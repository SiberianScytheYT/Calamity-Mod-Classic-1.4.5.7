using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Ranged;
using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Typeless
{
	public class AethersWhisper : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Aether's Whisper");
/*
			Tooltip.SetDefault("Inflicts several long-lasting debuffs and splits on tile hits\n" +
				"Projectiles gain damage as they travel\n" +
				"Right click to change from magic to ranged damage\n" +
				"Right click consumes no mana");
*/
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 1150;
			Item.knockBack = 5.5f;
			Item.useTime = Item.useAnimation = 24;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<AetherBeam>();
			Item.mana = 30;
			Item.DamageType = DamageClass.Magic;
			Item.autoReuse = true;

			Item.width = 134;
			Item.height = 44;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LaserCannon");
			Item.value = Item.buyPrice(1, 40, 0, 0);
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.PureGreen;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.DamageType = DamageClass.Ranged;
				//Item.magic = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			}
			else
			{
				//Item.ranged = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
				Item.DamageType = DamageClass.Magic;
			}
			return base.CanUseItem(player);
		}

		public override bool OnPickup(Player player)
		{
			//Item.ranged = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			Item.DamageType = DamageClass.Magic;
			return true;
		}

		public override void UpdateInventory(Player player)
		{
			// Reset to magic if not using an item to prevent sorting bugs.
			if (player.itemAnimation <= 0)
			{
				//Item.ranged = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
				Item.DamageType = DamageClass.Magic;
			}
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.altFunctionUse == 2)
				mult *= 0f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// This is coded weirdly because it defaults to using magic boosts without this.
			int dmg = player.GetWeaponDamage(player.ActiveItem());
			float ai0 = player.altFunctionUse == 2 ? 1f : 0f;
			Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, dmg, Item.knockBack, player.whoAmI, ai0, 0f);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<PlasmaRod>());
			recipe.AddIngredient(ModContent.ItemType<Lazhar>());
			recipe.AddIngredient(ModContent.ItemType<SpectreRifle>());
			recipe.AddIngredient(ModContent.ItemType<TwistingNether>(), 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
