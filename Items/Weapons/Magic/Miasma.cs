using CalRD.Projectiles.Magic;
using CalRD.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class Miasma : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Miasma");
/*
            Tooltip.SetDefault("Releases gas clouds that stay still and explode after a while");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 16;
            Item.width = 50;
            Item.height = 64;
            Item.useTime = Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
			Item.rare = 5;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiasmaGas>();
            Item.shootSpeed = 10f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < Main.rand.Next(5, 8 + 1); i++)
            {
                Vector2 velocity1 = new Vector2(velocity.X, velocity.Y) * Main.rand.NextFloat(0.9f, 1.1f);
                float angle = Main.rand.NextFloat(-1f, 1f) * MathHelper.ToRadians(30f);
                Projectile.NewProjectile(source, position, velocity1.RotatedBy(angle), type, damage, Item.knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.NimbusRod);
            recipe.AddIngredient(ModContent.ItemType<AquamarineStaff>());
            recipe.AddIngredient(ModContent.ItemType<CorrodedFossil>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
