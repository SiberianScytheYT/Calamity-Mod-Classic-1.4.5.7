using CalRD.Items.Placeables;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class AquashardShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquashard Shotgun");
/*
            Tooltip.SetDefault("Shoots aquashards which split upon hitting an enemy");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 62;
            Item.height = 26;
            Item.crit += 6;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Aquashard>();
            Item.shootSpeed = 22f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = Main.rand.Next(2, 4);
            for (int index = 0; index < num6; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
                int projectile = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
                Main.projectile[projectile].timeLeft = 200;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Boomstick);
            recipe.AddIngredient(ModContent.ItemType<SeaPrism>(), 5);
            recipe.AddIngredient(ModContent.ItemType<PrismShard>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Navystone>(), 25);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
