using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Spyker : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spyker");
/*
            Tooltip.SetDefault("Fires spikes that stick to enemies, tiles, and explode into shrapnel");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 240;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 44;
            Item.height = 26;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item108;
            Item.autoReuse = true;
            Item.shootSpeed = 9f;
            Item.shoot = ModContent.ProjectileType<SpykerProj>();
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SpykerProj>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Needler>());
            recipe.AddIngredient(ItemID.Stynger);
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
