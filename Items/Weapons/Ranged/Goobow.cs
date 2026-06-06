using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Goobow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Goobow");
/*
            Tooltip.SetDefault("Fires two streams of slime");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 50;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 4;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
            float piOver10 = 0.1f * MathHelper.Pi;
            int projAmt = 2;
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            velocity1.Normalize();
            velocity1 *= 20f;
            bool canHit = Collision.CanHit(source1, 0, 0, source1 + velocity1, 0, 0);
            for (int i = 0; i < projAmt; i++)
            {
                float offsetAmt = i - (projAmt - 1f) / 2f;
                Vector2 offset = velocity1.RotatedBy((double)(piOver10 * offsetAmt), default);
                if (!canHit)
                {
                    offset -= velocity1;
                }
                int index = Projectile.NewProjectile(source, source1 + offset, new Vector2(velocity1.X, velocity1.Y) * 0.6f, ProjectileID.SlimeGun, damage / 4, 0f, player.whoAmI);
                Main.projectile[index].Calamity().forceRanged = true;
                Main.projectile[index].usesLocalNPCImmunity = true;
                Main.projectile[index].localNPCHitCooldown = 10;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 18);
            recipe.AddIngredient(ItemID.Gel, 30);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();
        }
    }
}
