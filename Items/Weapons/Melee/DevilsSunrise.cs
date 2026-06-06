using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class DevilsSunrise : ModItem
    {
        public static int BaseDamage = 360;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Devil's Sunrise");
/*
            Tooltip.SetDefault("Balls? Smalls.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 66;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.damage = BaseDamage;
            Item.crit += 10;
            Item.knockBack = 4f;
            Item.useAnimation = 25;
            Item.useTime = 5;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(1, 40, 0, 0);

            Item.shoot = ModContent.ProjectileType<DevilsSunriseProj>();
            Item.shootSpeed = 24f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DevilsSunriseProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DevilsSunriseCyclone>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ItemID.Arkhalis);
            r.AddIngredient(ModContent.ItemType<DemonicBoneAsh>(), 10);
            r.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 25);
            r.AddTile(TileID.LunarCraftingStation);
            r.Register();
        }
    }
}
