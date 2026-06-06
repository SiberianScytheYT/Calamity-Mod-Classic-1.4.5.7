using CalRD.Items.Materials;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.Hybrid;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class EmpyreanKnives : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Empyrean Knives");
/*
            Tooltip.SetDefault("Throws a flurry of bouncing knives that can heal the user");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.damage = 600;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;
            Item.height = 20;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<EmpyreanKnife>();
            Item.shootSpeed = 15f;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float num72 = Item.shootSpeed;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 value = Vector2.UnitX.RotatedBy((double)player.fullRotation, default);
            Vector2 vector3 = Main.MouseWorld - vector2;
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
            num78 *= num80;
            num79 *= num80;
            int num146 = 4;
            if (Main.rand.NextBool(2))
            {
                num146++;
            }
            if (Main.rand.NextBool(4))
            {
                num146++;
            }
            if (Main.rand.NextBool(8))
            {
                num146++;
            }
            if (Main.rand.NextBool(16))
            {
                num146++;
            }
            for (int num147 = 0; num147 < num146; num147++)
            {
                float num148 = num78;
                float num149 = num79;
                float num150 = 0.05f * (float)num147;
                num148 += (float)Main.rand.Next(-25, 26) * num150;
                num149 += (float)Main.rand.Next(-25, 26) * num150;
                num80 = (float)Math.Sqrt((double)(num148 * num148 + num149 * num149));
                num80 = num72 / num80;
                num148 *= num80;
                num149 *= num80;
                float x4 = vector2.X;
                float y4 = vector2.Y;
                Projectile.NewProjectile(source, x4, y4, num148, num149, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.VampireKnives);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
            recipe.AddIngredient(ModContent.ItemType<LunarKunai>(), 999);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
