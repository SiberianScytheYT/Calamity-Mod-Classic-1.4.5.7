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
    public class PlanetaryAnnihilation : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Planetary Annihilation");
/*
            Tooltip.SetDefault("Fires a storm of arrows from the sky\n" +
                "Wooden arrows are converted to homing energy bolts");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 58;
            Item.height = 102;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item75;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PlanetaryAnnihilationProj>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float num72 = Item.shootSpeed;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
            float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
            if (player.gravDir == -1f)
            {
                num79 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
            }
            float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
            if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f))
            {
                num78 = (float)player.direction;
                num79 = 0f;
                num80 = num72;
            }
            else
            {
                num80 = num72 / num80;
            }

            vector2 = new Vector2(player.position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
            vector2.X = (vector2.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
            vector2.Y -= 100f;
            num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
            num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
            if (num79 < 0f)
            {
                num79 *= -1f;
            }
            if (num79 < 20f)
            {
                num79 = 20f;
            }
            num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
            num80 = num72 / num80;
            num78 *= num80;
            num79 *= num80;
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                for (int i = 0; i < 7; i++)
                {
                    float speedX4 = num78 + (float)Main.rand.Next(-120, 121) * 0.02f;
                    float speedY5 = num79 + (float)Main.rand.Next(-120, 121) * 0.02f;
                    Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, ModContent.ProjectileType<PlanetaryAnnihilationProj>(), damage, Item.knockBack, player.whoAmI, 0f, (float)i);
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    float speedX4 = num78 + (float)Main.rand.Next(-120, 121) * 0.02f;
                    float speedY5 = num79 + (float)Main.rand.Next(-120, 121) * 0.02f;
                    int num121 = Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[num121].noDropItem = true;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ModContent.ItemType<CosmicBolter>());
            recipe.AddIngredient(ItemID.DaedalusStormbow);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
