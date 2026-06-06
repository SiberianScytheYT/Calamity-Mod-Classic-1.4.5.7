using CalRD.Projectiles.Ranged;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
	public class FlurrystormCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flurrystorm Cannon");
/*
            Tooltip.SetDefault("Fires a chain of snowballs that become faster over time\n" +
			"Has a chance to also fire an ice chunk that shatters into shards\n" +
			"50% chance to not consume snowballs");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.width = 58;
            Item.height = 34;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1.2f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item11;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FlurrystormCannonShooting>();
			Item.useAmmo = AmmoID.Snowball;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
			Item.shootSpeed = 18f;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FlurrystormCannonShooting>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 50)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SnowballCannon);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 10);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddIngredient(ModContent.ItemType<VictoryShard>(), 25);
            recipe.AddIngredient(ItemID.WaterBucket, 3);
            recipe.AddIngredient(ItemID.IllegalGunParts);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
