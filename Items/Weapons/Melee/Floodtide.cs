using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Floodtide : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Floodtide");
/*
            Tooltip.SetDefault("Launches sharks, because sharks are awesome!");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 89;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 60;
            Item.height = 64;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiniSharkron>();
            Item.shootSpeed = 7f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
                int proj = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
            }
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 217);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 5);
            recipe.AddIngredient(ItemID.SharkFin, 2);
            recipe.AddRecipeGroup("AnyAdamantiteBar", 5);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
