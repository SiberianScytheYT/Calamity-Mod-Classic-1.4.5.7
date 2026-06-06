using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class ClaretCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Claret Cannon");
/*
            Tooltip.SetDefault("Fires a string of bloody tears that drain enemy health");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 215;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 48;
            Item.height = 30;
            Item.useTime = 3;
            Item.reuseDelay = 10;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<ClaretCannonProj>();
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ClaretCannonProj>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }
}
