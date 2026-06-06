using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using CalRD.CalPlayer;

namespace CalRD.Items.Weapons.Rogue
{
	public class SkyStabber : RogueWeapon
    {
        private static int damage = 50;
        private static int knockBack = 2;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sky Stabber");
/*
            Tooltip.SetDefault("Shoots a gravity-defying spiky ball. Stacks up to 4.\n" +
                "Stealth strikes make the balls rain feathers onto enemies when they hit\n" +
				"Right click to delete all existing spiky balls");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = damage;
            Item.crit = 4;
            Item.Calamity().rogue = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 1;
            Item.height = 1;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = knockBack;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.maxStack = 4;

            Item.shootSpeed = 2f;
            Item.shoot = ModContent.ProjectileType<SkyStabberProj>();
        }

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.shoot = 0;
				Item.shootSpeed = 0f;
				return player.ownedProjectileCounts[ModContent.ProjectileType<SkyStabberProj>()] > 0;
			}
			else
			{
				Item.shoot = ModContent.ProjectileType<SkyStabberProj>();
				Item.shootSpeed = 2f;
				int UseMax = Item.stack;
				return player.ownedProjectileCounts[ModContent.ProjectileType<SkyStabberProj>()] < UseMax;
			}
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer modPlayer = player.Calamity();
			modPlayer.killSpikyBalls = false;
            if (modPlayer.StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<SkyStabberProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
			modPlayer.killSpikyBalls = true;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ItemID.SpikyBall, 100);
            recipe.AddIngredient(ItemID.Cloud, 10);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 4);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }

    }
}
