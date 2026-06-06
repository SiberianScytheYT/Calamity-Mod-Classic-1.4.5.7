using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Aeries : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aeries");
/*
            Tooltip.SetDefault("Fires shockblast rounds that can steal enemy life");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<ShockblastRound>();
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ShockblastRound>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpectreBar, 5);
            recipe.AddIngredient(ModContent.ItemType<CursedCapper>());
            recipe.AddIngredient(ItemID.ShroomiteBar, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
