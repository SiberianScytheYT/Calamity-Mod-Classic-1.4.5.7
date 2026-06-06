using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRD.Items.Weapons.Ranged
{
	public class HeavenlyGale : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Heavenly Gale");
/*
			Tooltip.SetDefault("Fires a barrage of 5 random exo arrows\n" +
				"Green exo arrows explode into a tornado on death\n" +
				"Blue exo arrows cause a second group of arrows to fire on enemy hits\n" +
				"Orange exo arrows cause explosions on death\n" +
				"Teal exo arrows ignore enemy immunity frames\n" +
				"66% chance to not consume ammo");
*/
		}

		public override void SetDefaults()
		{
			Item.damage = 600;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 44;
			Item.height = 58;
			Item.useTime = 11;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4f;
			Item.value = Item.buyPrice(2, 50, 0, 0);
			Item.rare = 10;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 17f;
			Item.useAmmo = AmmoID.Arrow;
			Item.Calamity().customRarity = CalamityRarity.Violet;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
			float dmgMult = 1f;
			float piOver10 = MathHelper.Pi * 0.1f;
			int arrowAmt = 5;
			Vector2 speed = new Vector2(velocity.X, velocity.Y);
			speed.Normalize();
			speed *= 40f;
			bool canHit = Collision.CanHit(source1, 0, 0, source1 + speed, 0, 0);
			for (int i = 0; i < arrowAmt; i++)
			{
				float offsetAmt = i - (arrowAmt - 1f) / 2f;
				Vector2 offset = speed.RotatedBy((double)(piOver10 * offsetAmt), default);
				if (!canHit)
				{
					offset -= speed;
				}
				int arrow = Utils.SelectRandom(Main.rand, new int[]
				{
					ProjectileType<TealExoArrow>(),
					ProjectileType<OrangeExoArrow>(),
					ProjectileType<BlueExoArrow>(),
					ProjectileType<GreenExoArrow>()
				});
				if (player.ownedProjectileCounts[ProjectileType<GreenExoArrow>()] + player.ownedProjectileCounts[ProjectileType<ExoTornado>()] > 5)
				{
					arrow = Utils.SelectRandom(Main.rand, new int[]
					{
						ProjectileType<TealExoArrow>(),
						ProjectileType<OrangeExoArrow>(),
						ProjectileType<BlueExoArrow>()
					});
				}
				if (arrow == ProjectileType<TealExoArrow>())
					dmgMult = 0.5f;
				Projectile.NewProjectile(source, source1.X + offset.X, source1.Y + offset.Y, velocity.X, velocity.Y, arrow, (int)(damage * dmgMult), Item.knockBack, player.whoAmI);
			}
			return false;
		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			if (Main.rand.Next(0, 100) < 66)
				return false;
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Alluvion>());
			recipe.AddIngredient(ItemType<AstrealDefeat>());
			recipe.AddIngredient(ItemType<ClockworkBow>());
			recipe.AddIngredient(ItemType<Galeforce>());
			recipe.AddIngredient(ItemType<PlanetaryAnnihilation>());
			recipe.AddIngredient(ItemType<TheBallista>());
			recipe.AddIngredient(ItemType<AuricBar>(), 4);
			recipe.AddTile(TileType<DraedonsForge>());
			recipe.Register();
		}
	}
}
