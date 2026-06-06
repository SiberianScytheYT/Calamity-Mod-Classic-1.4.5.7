using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Zapper : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lazinator");
/*
            Tooltip.SetDefault("Zap");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 46;
            Item.height = 22;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item12;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurpleLaser;
            Item.shootSpeed = 20f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(2))
			{
				type = ProjectileID.GreenLaser;
			}
            int laser = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			Main.projectile[laser].usesLocalNPCImmunity = true;
			Main.projectile[laser].localNPCHitCooldown = 10;
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpaceGun);
            recipe.AddIngredient(ItemID.LaserRifle);
            recipe.AddIngredient(ModContent.ItemType<VictoryShard>(), 5);
            recipe.AddIngredient(ItemID.SoulofSight);
            recipe.AddIngredient(ItemID.SoulofMight);
            recipe.AddIngredient(ItemID.SoulofFright);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
