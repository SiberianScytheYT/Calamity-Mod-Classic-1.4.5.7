using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class RelicofRuin : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Relic of Ruin");
/*
            Tooltip.SetDefault("Casts a spread of sand blades");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 16;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.25f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item84;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ForbiddenAxeBlade>();
            Item.shootSpeed = 5f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int totalProjectiles = 12;
			float radians = MathHelper.TwoPi / totalProjectiles;
            for (int i = 0; i < totalProjectiles; i++)
            {
				Vector2 vector = new Vector2(0f, -Item.shootSpeed).RotatedBy(radians * i);
				Projectile.NewProjectile(source, position, vector, type, damage, Item.knockBack, Main.myPlayer);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 2);
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
