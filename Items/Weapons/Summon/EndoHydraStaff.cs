using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class EndoHydraStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Endo Hydra Staff");
/*
            Tooltip.SetDefault("Summons a frigid entity with a head\n" +
                               "If the entity already exists, using this item again will cause it to gain more heads");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item60;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 25;
            Item.damage = 450;
            Item.knockBack = 3f;
            Item.autoReuse = true;
            Item.useTime = Item.useAnimation = 10;
            Item.shoot = ModContent.ProjectileType<EndoHydraBody>();
            Item.shootSpeed = 10f;

            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                bool bodyExists = false;
                int bodyIndex = -1;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == type && Main.projectile[i].owner == player.whoAmI)
                    {
                        bodyIndex = i;
                        bodyExists = true;
                        break;
                    }
                }
                if (bodyExists)
                {
                    Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), player.Center, Vector2.One.RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<EndoHydraHead>(), damage, Item.knockBack, player.whoAmI, bodyIndex);
                }
                else
                {
                    bodyIndex = Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
                    Projectile.NewProjectile(source, player.Center, Vector2.One.RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<EndoHydraHead>(), damage, Item.knockBack, player.whoAmI, bodyIndex);
                    for (int i = 0; i < 72; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(Main.projectile[bodyIndex].Center, 113);
                        dust.velocity = (MathHelper.TwoPi * Vector2.Dot((i / 72f * MathHelper.TwoPi).ToRotationVector2(), player.velocity.SafeNormalize(Vector2.UnitY).RotatedBy(i / 72f * -MathHelper.TwoPi))).ToRotationVector2();
                        dust.velocity = dust.velocity.RotatedBy(i / 36f * MathHelper.TwoPi) * 8f;
                        dust.noGravity = true;
                        dust.scale = 1.9f;
                    }
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.StaffoftheFrostHydra);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<EndothermicEnergy>(), 15);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
