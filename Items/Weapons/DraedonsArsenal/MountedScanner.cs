using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class MountedScanner : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mounted Scanner");
/*
			Tooltip.SetDefault("Laser technology used in this case for both targeting and defense.\n" +
			"Summons a powerful weapon above your head that fires lasers at nearby enemies\n" +
			"Deals less damage against enemies with high defense");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 26;
			Item.height = 26;
			Item.DamageType = DamageClass.Summon;
			Item.damage = 41;
			Item.knockBack = 2f;
			Item.mana = 10;
			Item.useTime = Item.useAnimation = 25;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item15;
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.Rarity5BuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<MountedScannerSummon>();
			Item.shootSpeed = 1f;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 85f;
			modItem.ChargePerUse = 0.85f;
			modItem.ChargePerAltUse = 0f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
			int totalOwnedScanners = player.ownedProjectileCounts[type];
			int currentScannerIndex = 0;
			foreach (Projectile projectile in Main.projectile)
			{
				if (!projectile.active)
					continue;
				if (projectile.type != type)
					continue;
				if (projectile.owner != player.whoAmI)
					continue;
				float completionRatio = currentScannerIndex / (float)totalOwnedScanners;
				
				// ai[0] is the angular offset relative to the projectile's owner.
				// For the first 15 summons, wrap around the player angularly, but not at a perfect angle, a bit like the Dazzling Stabbers when idle.
				// But once the total summon count is greater than 15, just create a perfect circle depending on the total amount of summons.
				if (totalOwnedScanners <= 14)
				{
					projectile.ai[0] = Utils.AngleLerp(0f, MathHelper.Pi, currentScannerIndex / 15f);
					if (currentScannerIndex % 2f == 1f)
						projectile.ai[0] = -Utils.AngleLerp(0f, MathHelper.Pi, (currentScannerIndex + 1) / 15f);
				}
				else
				{
					projectile.ai[0] = MathHelper.TwoPi / totalOwnedScanners * currentScannerIndex;
				}

				// Add a specific offset so that the scanners spawn above the player at first and not to the side.
				projectile.ai[0] -= MathHelper.PiOver2;
				projectile.netUpdate = true;
				currentScannerIndex++;
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 15);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 5);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
