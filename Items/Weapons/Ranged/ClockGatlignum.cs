using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class ClockGatlignum : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Clock Gatlignum");
/*
            Tooltip.SetDefault("33% chance to not consume ammo\n" +
                "Fires a string of high velocity bullets");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 66;
            Item.height = 34;
            Item.useTime = 3;
            Item.reuseDelay = 12;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.75f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item31;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + (float)Main.rand.Next(-15, 16) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-15, 16) * 0.05f;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileID.BulletHighVelocity, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
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
            recipe.AddIngredient(ItemID.Gatligator);
            recipe.AddIngredient(ItemID.VenusMagnum);
            recipe.AddIngredient(ItemID.ClockworkAssaultRifle);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 3);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
