using CalRD.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Ranged
{
    public class FirestormCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Firestorm Cannon");
/*
            Tooltip.SetDefault("70% chance to not consume flares\n" +
                "Right click to fire a spread of flares");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 28;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Flare;
            Item.shootSpeed = 5.5f;
            Item.useAmmo = AmmoID.Flare;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 70)
                return false;
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse == 2)
                return 1 / 3f;
				//return 0.3333f;
			return 1f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                int num6 = Main.rand.Next(4, 6);
                for (int index = 0; index < num6; ++index)
                {
                    float SpeedX = velocity.X + (float)Main.rand.Next(-50, 51) * 0.05f;
                    float SpeedY = velocity.Y + (float)Main.rand.Next(-50, 51) * 0.05f;
                    int flare = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)((double)damage * 1.4), Item.knockBack, player.whoAmI, 0.0f, 0.0f);
                    Main.projectile[flare].penetrate = 1;
                    Main.projectile[flare].timeLeft = 600;
                    Main.projectile[flare].Calamity().forceRanged = true;
                }
                return false;
            }
            else
            {
                int num6 = Main.rand.Next(1, 3);
                for (int index = 0; index < num6; ++index)
                {
                    float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
                    float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
                    int projectile = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
                    Main.projectile[projectile].Calamity().forceRanged = true;
                    Main.projectile[projectile].timeLeft = 200;
                    Main.projectile[projectile].penetrate = 3;
                }
                return false;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FlareGun);
            recipe.AddIngredient(ItemID.Boomstick);
            recipe.AddRecipeGroup("AnyGoldBar", 10);
            recipe.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
