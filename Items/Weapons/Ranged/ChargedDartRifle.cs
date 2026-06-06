using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class ChargedDartRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Charged Dart Blaster");
/*
            Tooltip.SetDefault("Fires a shotgun spread of darts and a splitting energy blast\n" +
			"Right click to fire a more powerful exploding energy blast that bounces");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 60;
            Item.height = 24;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LaserCannon");
            Item.autoReuse = true;
            Item.shootSpeed = 22f;
            Item.shoot = ModContent.ProjectileType<ChargedBlast>();
            Item.useAmmo = AmmoID.Dart;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargedBlast3>(), (int)((double)damage * 2), Item.knockBack, player.whoAmI, 0f, 0f);
                return false;
            }
            else
            {
				int num6 = Main.rand.Next(2, 5);
				for (int index = 0; index < num6; ++index)
				{
					float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
					float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
					int projectile = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage / 2, Item.knockBack, player.whoAmI, 0f, 0f);
				}
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargedBlast>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
                return false;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DartRifle);
            recipe.AddIngredient(ItemID.MartianConduitPlating, 25);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 3);
            recipe.AddIngredient(ItemID.FragmentVortex, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DartPistol);
            recipe.AddIngredient(ItemID.MartianConduitPlating, 25);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 3);
            recipe.AddIngredient(ItemID.FragmentVortex, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
