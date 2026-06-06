using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class LunarianBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lunarian Bow");
/*
            Tooltip.SetDefault("Fires two sliding energy bolts");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 29;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 62;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 4;
            Item.UseSound = SoundID.Item75;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LunarBolt>();
            Item.shootSpeed = 8f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
            float piOver10 = MathHelper.Pi * 0.1f;
            int projAmt = 2;
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            velocity1.Normalize();
            velocity1 *= 15f;
            bool canHit = Collision.CanHit(source1, 0, 0, source1 + velocity1, 0, 0);
            for (int i = 0; i < projAmt; i++)
            {
                float offsetAmt = i - (projAmt - 1f) / 2f;
                Vector2 offset = velocity1.RotatedBy(piOver10 * offsetAmt, default);
                if (!canHit)
                {
                    offset -= velocity1;
                }
                Projectile.NewProjectile(source, source1 + offset, new Vector2(velocity1.X, velocity1.Y), ModContent.ProjectileType<LunarBolt>(), damage, Item.knockBack, player.whoAmI);
			}
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemonBow);
            recipe.AddIngredient(ItemID.MoltenFury);
            recipe.AddIngredient(ItemID.BeesKnees);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TendonBow);
            recipe.AddIngredient(ItemID.MoltenFury);
            recipe.AddIngredient(ItemID.BeesKnees);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
