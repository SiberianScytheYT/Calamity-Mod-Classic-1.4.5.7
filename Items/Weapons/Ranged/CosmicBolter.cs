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
    public class CosmicBolter : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cosmic Bolter");
/*
            Tooltip.SetDefault("Fires three sliding energy bolts");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 76;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.75f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item75;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LunarBolt2>();
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
            float piOver10 = MathHelper.Pi * 0.1f;
            int projAmt = 3;
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            velocity1.Normalize();
            velocity1 *= 30f;
            bool canHit = Collision.CanHit(source1, 0, 0, source1 + velocity1, 0, 0);
            for (int i = 0; i < projAmt; i++)
            {
                float offsetAmt = i - (projAmt - 1f) / 2f;
                Vector2 offset = velocity1.RotatedBy(piOver10 * offsetAmt, default);
                if (!canHit)
                {
                    offset -= velocity1;
                }
                Projectile.NewProjectile(source, source1 + offset, new Vector2(velocity1.X, velocity1.Y), ModContent.ProjectileType<LunarBolt2>(), damage, Item.knockBack, player.whoAmI);
			}
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<LunarianBow>());
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 5);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
