using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class DodusHandcannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dodu's Handcannon");
/*
            Tooltip.SetDefault("The power of the nut rests in your hands");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 42;
            Item.damage = 485;
            Item.crit += 16;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 10f;

            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;

            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LargeWeaponFire");

            Item.shootSpeed = 24f;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 5);
        }

        public override bool CanUseItem(Player player)
        {
            return CalamityGlobalItem.HasEnoughAmmo(player, Item, 5);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 5; i++)
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.BulletHighVelocity, damage, Item.knockBack, player.whoAmI, 0f, 0f);

            // Consume 5 ammo per shot
            CalamityGlobalItem.ConsumeAdditionalAmmo(player, Item, 5);

            return false;
        }

        // Disable vanilla ammo consumption
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Aeries>());
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
