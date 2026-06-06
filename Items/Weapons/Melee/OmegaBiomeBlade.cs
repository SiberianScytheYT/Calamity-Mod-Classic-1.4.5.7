using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class OmegaBiomeBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Omega Biome Blade");
/*
            Tooltip.SetDefault("Fires different homing projectiles based on what biome you're in\n" +
                "Projectiles also change based on moon events");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.damage = 220;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 62;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<OmegaBiomeOrb>();
            Item.shootSpeed = 15f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int projectiles = 0; projectiles <= 2; projectiles++)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<OmegaBiomeOrb>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TrueBiomeBlade>());
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>());
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 3);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 3);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 0);
            }
        }
    }
}
