using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Genisis : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Genisis");
/*
            Tooltip.SetDefault("Fires a Y-shaped beam of destructive energy and a spread of lasers");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 74;
            Item.height = 28;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<BigBeamofDeath>();
            Item.Calamity().customRarity = CalamityRarity.Turquoise; //12
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            int num6 = 3;
            float SpeedX = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
            for (int index = 0; index < num6; ++index)
            {
                int projectile = Projectile.NewProjectile(source, position.X, position.Y, SpeedX * 1.05f, SpeedY * 1.05f, ProjectileID.LaserMachinegunLaser, (int)((double)damage * 0.65), Item.knockBack * 0.6f, player.whoAmI, 0f, 0f);
                Main.projectile[projectile].timeLeft = 120;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LaserMachinegun);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
