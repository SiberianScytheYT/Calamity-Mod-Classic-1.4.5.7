using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Pumpler : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pumpler");
/*
            Tooltip.SetDefault("33% chance to not consume ammo\n" +
                "Right click to fire an explosive pumpkin");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 62;
            Item.height = 36;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.25f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 11f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shootSpeed = 4f;
            }
            else
            {
                Item.shootSpeed = 11f;
            }
            return base.CanUseItem(player);
        }

		public override float UseSpeedMultiplier(Player player)
        {
            if (player.altFunctionUse == 2)
				return (5f/18f);
			return 1f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileID.JackOLantern, (int)((double)damage * 1.65), Item.knockBack * 4f, player.whoAmI, 0f, 0f);
            }
            else
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 33)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Pumpkin, 30);
            recipe.AddIngredient(ItemID.PumpkinSeed, 5);
            recipe.AddIngredient(ItemID.IllegalGunParts);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
