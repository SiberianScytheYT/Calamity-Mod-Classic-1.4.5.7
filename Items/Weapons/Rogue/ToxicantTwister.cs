using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class ToxicantTwister : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Toxicant Twister");
/*
			Tooltip.SetDefault("Throws a slow moving boomerang\n" +
				"After a few moments, the boomerang chooses a target and rapidly homes in\n" +
				"Stealth strikes home in faster and rapidly release sand");
*/
		}

		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 46;
			Item.damage = 488;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAnimation = Item.useTime = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(1, 40, 0, 0);
			Item.Calamity().postMoonLordRarity = 13;
			Item.rare = 10;
			Item.shoot = ModContent.ProjectileType<ToxicantTwisterTwoPointZero>();
			Item.shootSpeed = 18f;
			Item.Calamity().rogue = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			double stealthMult = player.Calamity().StealthStrikeAvailable() ? 1.3333 : 1D;
			int boomer = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, (int)(damage * stealthMult), Item.knockBack, player.whoAmI, 0f, 0f);
			Main.projectile[boomer].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
			return false;
		}
	}
}
