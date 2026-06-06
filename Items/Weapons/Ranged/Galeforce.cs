using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Galeforce : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Galeforce");
/*
            Tooltip.SetDefault("Fires a spread of low-damage feathers");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 52;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -8; i <= 8; i += 8)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(i));
                Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<FeatherLarge>(), damage / 4, 0f, player.whoAmI, 0f, 0f);
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 8);
            recipe.AddIngredient(ItemID.SunplateBlock, 3);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }
    }
}
