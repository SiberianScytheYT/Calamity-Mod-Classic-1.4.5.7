using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Shroomer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomer");
/*
            Tooltip.SetDefault("Has a chance to fire an extremely powerful homing mushroom");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 90;
            Item.height = 28;
            Item.crit += 35;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 9.75f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            if (Main.rand.NextBool(5))
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Shroom>(), (int)((double)damage * 1.5), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SniperRifle);
            recipe.AddIngredient(ItemID.ShroomiteBar, 11);
            recipe.AddIngredient(ItemID.FragmentVortex, 15);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
