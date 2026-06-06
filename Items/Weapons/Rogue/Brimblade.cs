using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class Brimblade : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimblade");
/*
            Tooltip.SetDefault("Throws a blade that splits on enemy hits\n" +
			"Stealth strikes split further and cause the player to launch a barrage of brimstone darts");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 26;
            Item.damage = 28;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item1;
            Item.height = 26;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<BrimbladeProj>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.Calamity().StealthStrikeAvailable())
			{
				int blade = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[blade].Calamity().stealthStrike = true;

				for (int i = -6; i <= 6; i += 4)
				{
					Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(i));
					int dart = Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<SeethingDischargeBrimstoneBarrage>(), damage, Item.knockBack * 0.5f, player.whoAmI, 0f, 0f);
					Main.projectile[dart].Calamity().forceRogue = true;
				}
				return false;
			}
			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
