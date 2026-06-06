using CalRD.Items.Weapons.Magic;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class MagnaStriker : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magna Striker");
/*
            Tooltip.SetDefault("Fires a string of opal and magna strikes");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 60;
            Item.height = 22;
            Item.useTime = 5;
            Item.reuseDelay = 6;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.25f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/OpalStrike");
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<OpalStrike>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int randomProj = Main.rand.Next(2);
            if (randomProj == 0)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<OpalStrike>(), (int)(damage * 0.75), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<MagnaStrike>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<OpalStriker>());
            recipe.AddIngredient(ModContent.ItemType<MagnaCannon>());
            recipe.AddRecipeGroup("AnyAdamantiteBar", 6);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
