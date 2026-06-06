using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class VividClarity : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vivid Clarity");
/*
            Tooltip.SetDefault("Fires five randomized beams of elemental energy at the cursor\n" +
							   "On enemy and tile hits, beams either explode into a big flash,\n" +
							   "summon an additonal laser from the sky,\n" +
							   "or split into energy orbs\n" +
                               "High IQ increases the weapon's potential");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 515;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 42;
            Item.width = 90;
            Item.height = 112;
            Item.useAnimation = 20;
            Item.useTime = 4;
            Item.reuseDelay = Item.useAnimation;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VividBeam>();
            Item.shootSpeed = 6f;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(20, 20);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float speed = Item.shootSpeed;
            float xPos = (float)Main.mouseX + Main.screenPosition.X - playerPos.X;
            float yPos = (float)Main.mouseY + Main.screenPosition.Y - playerPos.Y;
            float f = Main.rand.NextFloat() * MathHelper.TwoPi;
            float source1VariationLow = 20f;
            float source1VariationHigh = 60f;
            Vector2 source1 = playerPos + f.ToRotationVector2() * MathHelper.Lerp(source1VariationLow, source1VariationHigh, Main.rand.NextFloat());
            for (int num202 = 0; num202 < 50; num202++)
            {
                source1 = playerPos + f.ToRotationVector2() * MathHelper.Lerp(source1VariationLow, source1VariationHigh, Main.rand.NextFloat());
                if (Collision.CanHit(playerPos, 0, 0, source1 + (source1 - playerPos).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
                {
                    break;
                }
                f = Main.rand.NextFloat() * MathHelper.TwoPi;
            }
            Vector2 velocity1 = Main.MouseWorld - source1;
            Vector2 velocity1Variation = new Vector2(xPos, yPos).SafeNormalize(Vector2.UnitY) * speed;
            velocity1 = velocity1.SafeNormalize(velocity1Variation) * speed;
            velocity1 = Vector2.Lerp(velocity1, velocity1Variation, 0.25f);
            Projectile.NewProjectile(source, source1, velocity1, type, damage, Item.knockBack, player.whoAmI, 0f, Main.rand.Next(3));
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ElementalRay>());
            recipe.AddIngredient(ModContent.ItemType<ArchAmaryllis>());
            recipe.AddIngredient(ModContent.ItemType<AsteroidStaff>());
            recipe.AddIngredient(ModContent.ItemType<UltraLiquidator>());
            recipe.AddIngredient(ModContent.ItemType<PhantasmalFury>());
            recipe.AddIngredient(ModContent.ItemType<ShadowboltStaff>());
            recipe.AddIngredient(ModContent.ItemType<HeliumFlash>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
