using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class BrimstoneFury : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Fury");
/*
            Tooltip.SetDefault("Fires a spread of brimstone bolts");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 58;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.75f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BrimstoneBolt>();
            Item.shootSpeed = 13f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numProj = 2;
            float rotation = MathHelper.ToRadians(2);
            for (int i = 0; i < numProj + 1; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<BrimstoneBolt>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
