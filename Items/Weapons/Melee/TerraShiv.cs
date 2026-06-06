using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TerraShiv : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra Shiv");
/*
            Tooltip.SetDefault("Don't underestimate the power of shivs");
*/
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useTurn = false;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.width = 42;
            Item.height = 42;
            Item.damage = 140;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ShortTerraBeam>();
            Item.shootSpeed = 12f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, Item.shootSpeed * player.direction, 0f, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TrueNightsStabber>());
            recipe.AddIngredient(ModContent.ItemType<TrueExcaliburShortsword>());
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 107);
            }
        }
    }
}
