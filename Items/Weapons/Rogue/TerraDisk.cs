using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class TerraDisk : RogueWeapon
    {
        public static int BaseDamage = 100;
        public static float Speed = 12f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra Disk");
/*
            Tooltip.SetDefault("Throws a disk that has a chance to generate several disks if enemies are near it\n"
                               +"A max of three disks can be active at a time");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 46;
            Item.height = 46;
            Item.damage = BaseDamage;
            Item.knockBack = 4f;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;

            Item.Calamity().rogue = true;
            Item.shoot = ModContent.ProjectileType<TerraDiskProjectile>();
            Item.shootSpeed = Speed;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.ownedProjectileCounts[Item.shoot] >= 3)
			{
				return false;
			}
			else
			{
				return true;
			}
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
            Main.projectile[proj].Calamity().forceRogue = true;
			Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeashellBoomerang>());
            recipe.AddIngredient(ModContent.ItemType<Equanimity>());
            recipe.AddIngredient(ItemID.ThornChakram);
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
