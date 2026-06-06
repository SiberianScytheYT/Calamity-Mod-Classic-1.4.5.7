using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Tiles.Furniture.CraftingStations;

namespace CalRD.Items.Weapons.Magic
{
    public class AlphaRay : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Alpha Ray");
/*
            Tooltip.SetDefault("Disintegrates everything with a tri-beam of energy and lasers\n" +
                "Right click to fire a Y-shaped beam of destructive energy and a spread of lasers");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }


        public override void SetDefaults()
        {
            Item.damage = 280;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 5;
            Item.width = 84;
            Item.height = 74;
            Item.useTime = 3;
            Item.useAnimation = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<ParticleBeamofDoom>();
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<BigBeamofDeath>(), (int)(damage * 1.75), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            else
            {
                Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                float num117 = 0.314159274f;
                int num118 = 3;
                Vector2 vector7 = new Vector2(velocity.X, velocity.Y);
                vector7.Normalize();
                vector7 *= 80f;
                bool flag11 = Collision.CanHit(vector2, 0, 0, vector2 + vector7, 0, 0);
                for (int num119 = 0; num119 < num118; num119++)
                {
                    float num120 = (float)num119 - ((float)num118 - 1f) / 2f;
                    Vector2 value9 = vector7.RotatedBy((double)(num117 * num120), default);
                    if (!flag11)
                    {
                        value9 -= vector7;
                    }
                    Projectile.NewProjectile(source, vector2.X + value9.X, vector2.Y + value9.Y, velocity.X, velocity.Y, type, (int)(damage * 0.8), Item.knockBack, player.whoAmI, 0.0f, 0.0f);
                    int laser = Projectile.NewProjectile(source, vector2.X + value9.X, vector2.Y + value9.Y, velocity.X * 2f, velocity.Y * 2f, ProjectileID.LaserMachinegunLaser, (int)(damage * 0.4), Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[laser].timeLeft = 120;
                    Main.projectile[laser].tileCollide = false;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Wingman>(), 2);
            recipe.AddIngredient(ModContent.ItemType<Genisis>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
