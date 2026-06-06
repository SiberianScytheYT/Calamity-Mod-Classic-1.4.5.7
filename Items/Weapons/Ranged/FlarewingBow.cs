using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class FlarewingBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flarewing Bow");
/*
            Tooltip.SetDefault("Shoots a spread of arrows\n" +
                "Wooden arrows are converted to bouncing obsidian bats");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 20;
            Item.height = 62;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num117 = 0.314159274f;
            int num118 = 5;
            Vector2 vector7 = new Vector2(velocity.X, velocity.Y);
            vector7.Normalize();
            vector7 *= 50f;
            bool flag11 = Collision.CanHit(vector2, 0, 0, vector2 + vector7, 0, 0);
            for (int num119 = 0; num119 < num118; num119++)
            {
                float num120 = (float)num119 - ((float)num118 - 1f) / 2f;
                Vector2 value9 = vector7.RotatedBy((double)(num117 * num120), default);
                if (!flag11)
                {
                    value9 -= vector7;
                }
                if (type == ProjectileID.WoodenArrowFriendly)
                {
                    int num123 = Projectile.NewProjectile(source, vector2.X + value9.X, vector2.Y + value9.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FlareBat>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[num123].noDropItem = true;
                }
                else
                {
                    int num123 = Projectile.NewProjectile(source, vector2.X + value9.X, vector2.Y + value9.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
                    Main.projectile[num123].noDropItem = true;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellwingBow);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 5);
            recipe.AddIngredient(ItemID.LivingFireBlock, 50);
            recipe.AddIngredient(ItemID.Obsidian, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
