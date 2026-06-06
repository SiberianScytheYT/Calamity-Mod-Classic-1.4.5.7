using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class ShadecrystalTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shadecrystal Barrage");
/*
            Tooltip.SetDefault("Summons rapid fire shadecrystals, can shoot two crystals at once");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ShadecrystalProjectile>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = Main.rand.Next(1, 3);
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
            recipe.AddIngredient(ItemID.CrystalStorm);
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 6);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
