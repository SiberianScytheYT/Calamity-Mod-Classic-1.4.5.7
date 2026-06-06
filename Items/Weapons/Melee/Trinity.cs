using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Trinity : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Trinity");
/*
            Tooltip.SetDefault("Fires a spread of energy bolts");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 54;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ProjectileID.RubyBolt;
            Item.shootSpeed = 11f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    type = ProjectileID.RubyBolt;
                    break;
                case 1:
                    type = ProjectileID.SapphireBolt;
                    break;
                case 2:
                    type = ProjectileID.AmethystBolt;
                    break;
                default:
                    break;
            }
            for (int projectiles = 0; projectiles <= 3; projectiles++)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-30, 31) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-30, 31) * 0.05f;
                int proj = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)((double)damage * 0.6), Item.knockBack, Main.myPlayer, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 9);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 73);
            }
        }
    }
}
