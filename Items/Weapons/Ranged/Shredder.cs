using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Shredder : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shredder");
/*
            Tooltip.SetDefault("The myth, the legend, the weapon that drops more frames than any other\n" +
                "Fires a barrage of energy bolts that split and bounce\n" +
                "Right click to fire a barrage of normal bullets");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 24;
            Item.useTime = 4;
            Item.reuseDelay = 12;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item31;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int bulletAmt = 4;
            if (player.altFunctionUse == 2)
            {
                for (int index = 0; index < bulletAmt; ++index)
                {
                    float SpeedX = velocity.X + (float)Main.rand.Next(-30, 31) * 0.05f;
                    float SpeedY = velocity.Y + (float)Main.rand.Next(-30, 31) * 0.05f;
                    int shot = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[shot].timeLeft = 180;
                }
                return false;
            }
            else
            {
                for (int index = 0; index < bulletAmt; ++index)
                {
                    float SpeedX = velocity.X + (float)Main.rand.Next(-30, 31) * 0.05f;
                    float SpeedY = velocity.Y + (float)Main.rand.Next(-30, 31) * 0.05f;
                    int shot = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<ChargedBlast>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[shot].timeLeft = 180;
                }
                return false;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ModContent.ItemType<ChargedDartRifle>());
            recipe.AddIngredient(ModContent.ItemType<FrostbiteBlaster>());
            recipe.AddIngredient(ItemID.Shotgun);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
