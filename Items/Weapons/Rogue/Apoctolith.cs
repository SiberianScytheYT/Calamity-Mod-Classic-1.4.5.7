using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class Apoctolith : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
/*
			Tooltip.SetDefault("Maybe catching bricks with your face isn't such a hot idea...\n" +
				"Critical hits tear away enemy defense\n" +
				"Stealth strikes shatter and briefly stun enemies");
*/
			//DisplayName.SetDefault("Apoctolith");
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 120;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<ApoctolithProj>();
			Item.width = 66;
			Item.height = 64;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 1;
			Item.knockBack = 10f;
			Item.crit = 20;
			Item.value = CalamityGlobalItem.Rarity7BuyPrice;
			Item.rare = 7;
			Item.UseSound = SoundID.Item1;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.Calamity().rogue = true;
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			//Check if stealth is full
			if (player.Calamity().StealthStrikeAvailable())
			{
				int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI);
				Main.projectile[p].Calamity().stealthStrike = true;
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ThrowingBrick>(), 100);
			recipe.AddIngredient(ModContent.ItemType<Voidstone>(), 20);
			recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Rogue/ApoctolithGlow").Value);
		}
	}
}
