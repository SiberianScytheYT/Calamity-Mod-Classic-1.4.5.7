using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class DarkSpark : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dark Spark");
/*
            Tooltip.SetDefault("And everything under the sun is in tune,\n" +
                "But the sun is eclipsed by the moon.");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.reuseDelay = 5;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item13;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.knockBack = 0f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<DarkSparkPrism>();
            Item.shootSpeed = 30f;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DarkSparkPrism>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LastPrism);
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>(), 10);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 20);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 30);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
