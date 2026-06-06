using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class SomaPrime : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soma Prime");
/*
            Tooltip.SetDefault("Shoots extremely powerful high velocity rounds that inflict a powerful bleed debuff");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 1000;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 94;
            Item.height = 34;
            Item.crit += 40;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SlashRound>();
            Item.shootSpeed = 30f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<P90>());
            recipe.AddIngredient(ModContent.ItemType<Minigun>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<SlashRound>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 50)
                return false;
            return true;
        }
    }
}
