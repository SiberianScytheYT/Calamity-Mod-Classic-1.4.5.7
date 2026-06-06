using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Alluvion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Alluvion");
/*
            Tooltip.SetDefault("Moderate chance to convert wooden arrows to sharks\n" +
                       "Low chance to convert wooden arrows to typhoon arrows\n" +
                        "Fires a torrent of ten arrows at once");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 60;
            Item.height = 90;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 17f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num117 = MathHelper.Pi * 0.1f;
            int totalProjectiles = 10;
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            velocity1.Normalize();
            velocity1 *= 20f;
            bool canHit = Collision.CanHit(source1, 0, 0, source1 + velocity1, 0, 0);
            for (int i = 0; i < totalProjectiles; i++)
            {
                float num120 = (float)i - ((float)totalProjectiles - 1f) / 2f;
                Vector2 offset = velocity1.RotatedBy((double)(num117 * num120), default);
                if (!canHit)
                {
                    offset -= velocity1;
                }
                if (type == ProjectileID.WoodenArrowFriendly)
                {
                    if (Main.rand.NextBool(12))
                    {
                        type = ModContent.ProjectileType<TorrentialArrow>();
                    }
                    if (Main.rand.NextBool(25))
                    {
                        type = ModContent.ProjectileType<MiniSharkron>();
                    }
                    if (Main.rand.NextBool(100))
                    {
                        type = ModContent.ProjectileType<TyphoonArrow>();
                    }
                    int proj = Projectile.NewProjectile(source, source1.X + offset.X, source1.Y + offset.Y, velocity1.X, velocity1.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[proj].Calamity().forceRanged = true;
                    Main.projectile[proj].noDropItem = true;
                    Main.projectile[proj].arrow = true;
                }
                else
                {
                    int proj = Projectile.NewProjectile(source, source1.X + offset.X, source1.Y + offset.Y, velocity1.X, velocity1.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[proj].noDropItem = true;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Monsoon>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 20);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
