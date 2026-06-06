using CalRD.Items.Materials;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class Mistlestorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mistlestorm");
/*
            Tooltip.SetDefault("Casts a storm of pine needles and leaves");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 66;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 5;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PineNeedleFriendly;
            Item.shootSpeed = 24f;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num106 = 2 + Main.rand.Next(3);
            for (int num107 = 0; num107 < num106; num107++)
            {
                float num110 = 0.025f * (float)num107;
                velocity.X += (float)Main.rand.Next(-35, 36) * num110;
                velocity.Y += (float)Main.rand.Next(-35, 36) * num110;
                float num84 = (float)Math.Sqrt((double)(velocity.X * velocity.X + velocity.Y * velocity.Y));
                num84 = Item.shootSpeed / num84;
                velocity.X *= num84;
                velocity.Y *= num84;
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, (float)Main.rand.Next(0, 10 * (num107 + 1)), 0f);
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.Leaf, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Razorpine);
            recipe.AddIngredient(ItemID.LeafBlower);
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 7);
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
