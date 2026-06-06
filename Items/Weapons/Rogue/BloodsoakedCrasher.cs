using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class BloodsoakedCrasher : RogueWeapon //This weapon has been coded by Ben || Termi
    {
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bloodsoaked Crasher");
/*
			Tooltip.SetDefault("Slows down when hitting an enemy. Speeds up otherwise\n" +
			"Heals on enemy hits\n" +
			"Stealth strikes spawn homing blood on enemy hits");
*/
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 300;
			Item.knockBack = 3f;
			Item.autoReuse = true;
			Item.Calamity().rogue = true;
			Item.useAnimation = Item.useTime = 18;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<BloodsoakedCrashax>();

			Item.width = 66;
			Item.height = 64;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.buyPrice(1, 40, 0, 0);
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.PureGreen;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
			Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable(); //setting the stealth strike
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(); //post-Prov rogue weapon
			recipe.AddIngredient(ModContent.ItemType<CrushsawCrasher>());
			recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
