using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Impaler : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Impaler");
/*
            Tooltip.SetDefault("Fires explosive and flaming stakes\n" +
                "Instantly kills vampires and vampire bats");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.DamageType = DamageClass.Ranged;
            Item.crit += 14;
            Item.width = 40;
            Item.height = 26;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FlamingStake>();
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Stake;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -10);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + (float)Main.rand.Next(-2, 3) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-2, 3) * 0.05f;
            if (Main.rand.NextBool(3))
            {
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<ExplodingStake>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<FlamingStake>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 5);
            recipe.AddIngredient(ItemID.StakeLauncher);
            recipe.AddIngredient(ItemID.ExplosivePowder, 100);
            recipe.AddIngredient(ItemID.LivingFireBlock, 75);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
