using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Monsoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Monsoon");
/*
            Tooltip.SetDefault("Fires a spread of 5 arrows\n" +
                "Wooden arrows have a chance to be converted to typhoon arrows or sharks");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 142;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 78;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.Calamity().customRarity = (CalamityRarity)13;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
            float piOver10 = MathHelper.Pi * 0.1f;
            int totalProjectiles = 5;
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            velocity1.Normalize();
            velocity1 *= 40f;
            bool canHit = Collision.CanHit(source1, 0, 0, source1 + velocity1, 0, 0);
            for (int p = 0; p < totalProjectiles; p++)
            {
                float offsetAmt = (float)p - ((float)totalProjectiles - 1f) / 2f;
                Vector2 offset = velocity1.RotatedBy((double)(piOver10 * offsetAmt), default);
                if (!canHit)
                {
                    offset -= velocity1;
                }
                if (type == ProjectileID.WoodenArrowFriendly)
                {
                    if (Main.rand.NextBool(5))
                    {
                        type = ModContent.ProjectileType<MiniSharkron>();
                    }
                    if (Main.rand.NextBool(15))
                    {
                        type = ModContent.ProjectileType<TyphoonArrow>();
                    }
                    int arrow = Projectile.NewProjectile(source, source1.X + offset.X, source1.Y + offset.Y, velocity1.X, velocity1.Y, type, (int)(damage * 1.1f), Item.knockBack, player.whoAmI);
                    Main.projectile[arrow].Calamity().forceRanged = true;
                    Main.projectile[arrow].noDropItem = true;
                    Main.projectile[arrow].arrow = true;
                    Main.projectile[arrow].extraUpdates += 1;
                }
                else
                {
                    int arrow = Projectile.NewProjectile(source, source1.X + offset.X, source1.Y + offset.Y, velocity1.X, velocity1.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[arrow].noDropItem = true;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ReaperTooth>(), 6);
            recipe.AddIngredient(ItemID.Tsunami);
            recipe.AddIngredient(ModContent.ItemType<FlarewingBow>());
            recipe.AddIngredient(ItemID.SharkFin, 2);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
