using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class P90 : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("P90");
/*
            Tooltip.SetDefault("33% chance to not consume ammo\n" +
                "It's a bullet hose");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 60;
            Item.height = 28;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-14, -1);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; ++i)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-15, 16) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-15, 16) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 33)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
