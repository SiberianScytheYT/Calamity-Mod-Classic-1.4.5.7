using CalRD.Projectiles.Rogue;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Ranged
{
    public class SpectralstormCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectralstorm Cannon");
/*
            Tooltip.SetDefault("70% chance to not consume flares\n" +
                "Fires a storm of lost souls and flares");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 66;
            Item.height = 26;
            Item.useTime = 4;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Flare;
            Item.shootSpeed = 9.5f;
            Item.useAmmo = AmmoID.Flare;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 70)
                return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
			float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
			int flare = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI);
			Main.projectile[flare].timeLeft = 200;
			Main.projectile[flare].Calamity().forceRanged = true;

			float SpeedX2 = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
			float SpeedY2 = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
            int soul = Projectile.NewProjectile(source, position.X, position.Y, SpeedX2, SpeedY2, ModContent.ProjectileType<LostSoulFriendly>(), damage, Item.knockBack, player.whoAmI);
            Main.projectile[soul].timeLeft = 600;
            Main.projectile[soul].Calamity().forceRanged = true;
            Main.projectile[soul].frame = Main.rand.Next(4);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FirestormCannon>());
            recipe.AddIngredient(ItemID.FragmentVortex, 20);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
