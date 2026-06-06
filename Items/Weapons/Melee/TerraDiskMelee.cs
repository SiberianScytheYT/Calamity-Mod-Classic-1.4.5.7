using CalRD.Items.Materials;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TerraDiskMelee : ModItem
    {
        public static int BaseDamage = 100;
        public static float Speed = 12f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra Disk");
/*
            Tooltip.SetDefault("Throws a disk that has a chance to generate several disks if enemies are near it\nA max of three disks can be active at a time");
*/
        }

        public override void SetDefaults()
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

            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
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
            Main.projectile[proj].Calamity().forceMelee = true;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeashellBoomerangMelee>());
            recipe.AddIngredient(ModContent.ItemType<Equanimity>());
            recipe.AddIngredient(ItemID.ThornChakram);
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
