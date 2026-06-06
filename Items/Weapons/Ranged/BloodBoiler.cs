using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class BloodBoiler : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Boiler");
/*
            Tooltip.SetDefault("Fires a stream of lifestealing bloodfire\n" +
				"Must be used in 10 second bursts\n" +
                "Uses your life as ammo");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 415;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 60;
            Item.height = 30;
            Item.useTime = 5;
            Item.useAnimation = 600;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<BloodBoilerFire>();
            Item.Calamity().postMoonLordRarity = 13;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//using this weapon once will subtract 120 health in total
            player.statLife -= 1;
            if (player.statLife <= 0)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(Main.rand.NextBool(2) ? player.name + " suffered from severe anemia." : player.name + " was unable to obtain a blood transfusion."), 1000.0, 0, false);
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 6);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
