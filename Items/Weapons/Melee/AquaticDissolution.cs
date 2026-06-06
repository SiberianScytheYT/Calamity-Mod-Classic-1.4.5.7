using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class AquaticDissolution : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquatic Dissolution");
/*
            Tooltip.SetDefault("Fires whaling spears from the sky that bounce off tiles");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.damage = 200;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<OceanBeam>();
            Item.shootSpeed = 12f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            for (int x = 0; x < 3; x++)
            {
                Projectile.NewProjectile(source, position.X + (float)Main.rand.Next(-30, 31), position.Y - 600f, 0f, 8f, type, damage, Item.knockBack, Main.myPlayer, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Mariana>());
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 7);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 2);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 20);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int num250 = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 33, (float)(player.direction * 2), 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
                Main.dust[num250].velocity *= 0.2f;
                Main.dust[num250].noGravity = true;
            }
        }
    }
}
