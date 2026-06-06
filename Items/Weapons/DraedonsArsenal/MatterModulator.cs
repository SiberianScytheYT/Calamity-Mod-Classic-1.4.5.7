using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
	public class MatterModulator : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Matter Modulator");
/*
			Tooltip.SetDefault("Using extra mass gained from collision with solid materials, it causes extra damage.\n" +
			"Fires a burst of unstable matter that does significant damage after striking a tile.\n" +
			"Before striking a tile, the matter pierces infinitely but deals little damage.\n" +
			"Deals more damage against enemies with high defenses");
*/
		}

		public override void SetDefaults()
		{
			CalamityGlobalItem modItem = Item.Calamity();

			Item.width = 40;
			Item.height = 22;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 84;
			Item.knockBack = 11f;
			Item.useTime = Item.useAnimation = 33;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaBolt");
			Item.noMelee = true;

			Item.value = CalamityGlobalItem.Rarity5BuyPrice;
			Item.rare = ItemRarityID.Red;
			modItem.customRarity = CalamityRarity.DraedonRust;

			Item.shoot = ModContent.ProjectileType<UnstableMatter>();
			Item.shootSpeed = 7f;

			modItem.UsesCharge = true;
			modItem.MaxCharge = 85f;
			modItem.ChargePerUse = 0.075f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < Main.rand.Next(3, 5 + 1); i++)
			{
				Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).RotatedByRandom(0.4f) * Main.rand.NextFloat(0.8f, 1.3f), type, damage, Item.knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 12);
			recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 8);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
