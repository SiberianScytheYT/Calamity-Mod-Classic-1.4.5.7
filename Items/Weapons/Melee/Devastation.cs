using CalRD.Items.Placeables;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Devastation : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Devastation");
/*
            Tooltip.SetDefault("Fires galaxy blasts that explode");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 72;
            Item.damage = 160;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<GalaxyBlast>();
            Item.shootSpeed = 16f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch (Main.rand.Next(6))
            {
                case 1:
                    type = ModContent.ProjectileType<GalaxyBlast>();
                    break;
                case 2:
                    type = ModContent.ProjectileType<GalaxyBlastType2>();
                    break;
                case 3:
                    type = ModContent.ProjectileType<GalaxyBlastType3>();
                    break;
                default:
                    break;
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, Main.myPlayer);
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

            int num107 = 4;
            for (int num108 = 0; num108 < num107; num108++)
            {
                vector2 = new Vector2(player.position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                vector2.X = (vector2.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                vector2.Y -= (float)(100 * num108);
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
                float speedX4 = num78 + (float)Main.rand.Next(-40, 41) * 0.02f;
                float speedY5 = num79 + (float)Main.rand.Next(-40, 41) * 0.02f;
                Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, ModContent.ProjectileType<GalaxyBlast>(), damage / 2, Item.knockBack, player.whoAmI, 0f, (float)Main.rand.Next(5));
                Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, ModContent.ProjectileType<GalaxyBlastType2>(), damage / 2, Item.knockBack, player.whoAmI, 0f, (float)Main.rand.Next(3));
                Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, ModContent.ProjectileType<GalaxyBlastType3>(), damage / 2, Item.knockBack, player.whoAmI, 0f, (float)Main.rand.Next(1));
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CatastropheClaymore>());
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 10);
            recipe.AddIngredient(ItemID.MeteoriteBar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 73);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 120);
            target.AddBuff(BuffID.OnFire, 600);
            target.AddBuff(BuffID.Frostburn, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Ichor, 120);
            target.AddBuff(BuffID.OnFire, 600);
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }
}
