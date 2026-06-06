using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class AcidGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acid Gun");
/*
            Tooltip.SetDefault("Releases three streams of acid");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 9;
            Item.width = 48;
            Item.height = 30;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item13;
            Item.autoReuse = true;
            Item.shootSpeed = 14f;
            Item.shoot = ModContent.ProjectileType<AcidGunStream>();
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                float angle = MathHelper.Lerp(-0.145f, 0.145f, i / 3f);
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).RotatedBy(angle), type, damage, Item.knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Acidwood>(), 35);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
