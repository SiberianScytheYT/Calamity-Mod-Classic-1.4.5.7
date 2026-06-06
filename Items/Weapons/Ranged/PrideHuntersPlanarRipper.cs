using CalRD.Projectiles.Ranged;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class PrideHuntersPlanarRipper : ModItem
    {
		private int counter = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Prideful Hunter's Planar Ripper");
/*
            Tooltip.SetDefault("Converts musket balls into lightning bolts\n" +
			"Lightning bolts travel extremely fast and explode on enemy kills\n" +
			"Every fourth lightning bolt fired will deal 35 percent more damage.\n" +
			"Additionally, lightning bolt crits grant a stacking speed boost to the player\n" +
			"This stacks up to 20 percent bonus movement speed and acceleration\n" +
			"The boost will reset if the player holds a different item");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 77;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 68;
            Item.height = 32;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, -6);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 33)
                return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.Bullet)
            {
				counter++;
				float damageMult = 1f;
				if (counter == 4)
					damageMult = 1.35f;

                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PlanarRipperBolt>(), (int)(damage * damageMult), Item.knockBack, player.whoAmI);
				if (counter >= 4)
                counter = 0;
				return false;
            }
			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ItemID.FragmentVortex, 10);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 6);
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>(), 3);
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 25);
            recipe.AddIngredient(ModContent.ItemType<P90>());
            recipe.AddIngredient(ItemID.Uzi);
            recipe.AddIngredient(ModContent.ItemType<Aeries>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
