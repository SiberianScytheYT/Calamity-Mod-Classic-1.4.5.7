using CalRD.Items.Materials;
using CalRD.Items.Potions;
using CalRD.Projectiles.Ranged;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class StarCannonEX : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Cannon EX");
/*
            Tooltip.SetDefault("Fires a mix of normal, starfury, and astral stars");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 95;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 74;
            Item.height = 24;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = 7;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FallenStarProj>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.FallenStar;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = Main.rand.Next(1, 3);
            for (int index = 0; index < num6; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-15, 16) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-15, 16) * 0.05f;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        type = ModContent.ProjectileType<FallenStarProj>();
                        break;
                    case 1:
                        type = ProjectileID.Starfury;
                        break;
                    case 2:
                        type = ModContent.ProjectileType<AstralStar>();
                        break;
                }
                int star = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[star].Calamity().forceRanged = true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.StarCannon);
            recipe.AddIngredient(ModContent.ItemType<AstralJelly>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 25);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
