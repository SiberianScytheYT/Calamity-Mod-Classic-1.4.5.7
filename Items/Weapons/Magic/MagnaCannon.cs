using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class MagnaCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magna Cannon");
/*
            Tooltip.SetDefault("Fires a concentrated blast of energy");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.width = 56;
            Item.height = 34;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item117;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<MagnaBlast>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 3; ++index)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Granite, 25);
            recipe.AddIngredient(ItemID.Obsidian, 15);
            recipe.AddIngredient(ItemID.Amber, 5);
            recipe.AddIngredient(ItemID.SpaceGun);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
