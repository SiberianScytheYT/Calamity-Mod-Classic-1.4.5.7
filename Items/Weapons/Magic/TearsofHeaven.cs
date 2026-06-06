using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class TearsofHeaven : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tears of Heaven");
/*
            Tooltip.SetDefault("Casts slow-moving homing balls of heavenly light");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 64;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 18;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TearsofHeavenProjectile>();
            Item.shootSpeed = 5.5f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = Main.rand.Next(2, 3);
            for (int index = 0; index < num6; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)(double)damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FrigidflashBolt>());
            recipe.AddIngredient(ItemID.WaterBolt);
            recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 5);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
