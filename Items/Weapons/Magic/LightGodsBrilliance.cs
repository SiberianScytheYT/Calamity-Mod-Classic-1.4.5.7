using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class LightGodsBrilliance : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Light God's Brilliance");
/*
            Tooltip.SetDefault("Casts small, homing light beads along with explosive light balls");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 34;
            Item.height = 36;
            Item.useTime = 3;
            Item.useAnimation = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LightBead>();
            Item.shootSpeed = 25f;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = Main.rand.Next(2, 5);
            for (int index = 0; index < num6; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-50, 51) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-50, 51) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)(double)damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }
            if (Main.rand.NextBool(3))
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<LightBall>(), (int)((double)damage * 2.0), Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ShadecrystalTome>());
            recipe.AddIngredient(ModContent.ItemType<AbyssalTome>());
            recipe.AddIngredient(ItemID.HolyWater, 10);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddIngredient(ItemID.SoulofLight, 30);
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 5);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
