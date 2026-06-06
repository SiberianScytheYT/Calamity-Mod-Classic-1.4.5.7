using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class SpectreRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectre Rifle");
/*
            Tooltip.SetDefault("Fires a powerful homing soul");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 88;
            Item.height = 30;
            Item.crit += 22;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.LostSoulFriendly;
            Item.shootSpeed = 24f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.LostSoulFriendly, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            Main.projectile[proj].Calamity().forceRanged = true;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpectreBar, 7);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
