using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class HandheldTank : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Handheld Tank");
        }

        public override void SetDefaults()
        {
            Item.width = 110;
            Item.height = 46;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 1000;
            Item.crit += 15;
            Item.knockBack = 16f;
            Item.useTime = 71;
            Item.useAnimation = 71;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/TankCannon");
            Item.noMelee = true;

            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;

            Item.shoot = ModContent.ProjectileType<HandheldTankShell>();
            Item.shootSpeed = 6f;
            Item.useAmmo = AmmoID.Rocket;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<HandheldTankShell>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-33, 0);
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<Shroomer>());
            r.AddRecipeGroup(RecipeGroupID.IronBar, 50);
            r.AddIngredient(ModContent.ItemType<DivineGeode>(), 5);
            r.AddIngredient(ItemID.TigerSkin);
            r.AddTile(TileID.LunarCraftingStation);
            r.Register();
        }
    }
}
