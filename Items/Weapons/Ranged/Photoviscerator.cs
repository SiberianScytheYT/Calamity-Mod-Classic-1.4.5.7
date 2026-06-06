using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Photoviscerator : ModItem
    {
        public const int CooldownTime = 60 * 7; // 7 second cooldown.
		public const double AltFireDamageMult = 4.27;
		
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Photoviscerator");
/*
            Tooltip.SetDefault("90% chance to not consume gel\n" +
                "Fires a stream of exo flames and light that explodes into homing sparks\n" +
                "Right click to fire homing flares which stick to enemies and incinerate them");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 250;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 84;
            Item.height = 30;
            Item.useTime = 2;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item34;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.useAmmo = AmmoID.Gel;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ModContent.ProjectileType<ExoLightBurst>();
                Item.useTime = Item.useAnimation = 27;
            }
            else
            {
                Item.useTime = 2;
                Item.useAnimation = 10;
                Item.shoot = ModContent.ProjectileType<ExoFire>();
            }
            return base.CanUseItem(player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
                position += velocity1.ToRotation().ToRotationVector2() * 80f;
                Projectile.NewProjectile(source, position, velocity1.SafeNormalize(Vector2.Zero) * 17f, ModContent.ProjectileType<ExoLightBurst>(), (int)(damage * AltFireDamageMult), Item.knockBack, player.whoAmI);
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 velocity1 = new Vector2(velocity.X, velocity.Y).RotatedByRandom(0.05f);
                    Projectile.NewProjectile(source, position, velocity1, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                }
                if (Main.rand.NextBool(8))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 velocity1 = new Vector2(velocity.X, velocity.Y) * 2f;
                        position += velocity1.ToRotation().ToRotationVector2() * 64f;
                        int yDirection = (i == 0).ToDirectionInt();
                        velocity1 = velocity1.RotatedBy(0.2f * yDirection);
                        Projectile lightBomb = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, velocity1, ModContent.ProjectileType<ExoLightBomb>(), damage, Item.knockBack, player.whoAmI);

                        lightBomb.localAI[1] = yDirection;
                        lightBomb.netUpdate = true;
                    }
                }
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 90)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ElementalEruption>());
            recipe.AddIngredient(ModContent.ItemType<CleansingBlaze>());
            recipe.AddIngredient(ModContent.ItemType<HalleysInferno>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
