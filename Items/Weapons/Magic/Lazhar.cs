using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Lazhar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lazhar");
/*
            Tooltip.SetDefault("Fires a bouncing laser that explodes on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 42;
            Item.height = 20;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item12;
            Item.autoReuse = true;
            Item.shootSpeed = 15f;
            Item.shoot = ModContent.ProjectileType<SolarBeam2>();
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + (float)Main.rand.Next(-15, 16) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-15, 16) * 0.05f;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HeatRay);
            recipe.AddIngredient(ModContent.ItemType<Zapper>());
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 6);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
