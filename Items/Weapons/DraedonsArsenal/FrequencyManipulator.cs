using CalRD.Items.Materials;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class FrequencyManipulator : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Frequency Manipulator");
/*
			Tooltip.SetDefault("A long device, used in the tuning of some rather... original machines.\n" +
							   "Swings a spear around and then throws it\n" +
							   "On collision, the spear releases a burst of homing energy\n" +
							   "Stealth strikes release more energy and explode on collision");
*/
		}

		public override void SafeSetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.damage = 80;
			modItem.rogue = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.width = 26;
			Item.height = 44;
			Item.useTime = 56;
			Item.useAnimation = 56;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5f;

			Item.value = CalamityGlobalItem.Rarity5BuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;
			Item.UseSound = SoundID.Item1;

			Item.shootSpeed = 16f;
			Item.shoot = ModContent.ProjectileType<FrequencyManipulatorProjectile>();

			modItem.UsesCharge = true;
			modItem.MaxCharge = 85f;
			modItem.ChargePerUse = 0.04f;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f).Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 8);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 12);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofSight, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
