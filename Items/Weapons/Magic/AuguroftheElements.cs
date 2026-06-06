using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class AuguroftheElements : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Augur of the Elements");
/*
            Tooltip.SetDefault("Casts a burst of elemental tentacles to spear your enemies");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 180;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.width = 28;
            Item.crit = 3;
            Item.height = 30;
            Item.useTime = 2;
            Item.reuseDelay = 5;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item103;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ElementTentacle>();
            Item.shootSpeed = 30f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EldritchTome>());
            recipe.AddIngredient(ModContent.ItemType<TomeofFates>());
            recipe.AddIngredient(ItemID.ShadowFlameHexDoll);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
            float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
            Vector2 value2 = new Vector2(num78, num79);
            value2.Normalize();
            Vector2 value3 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
            value3.Normalize();
            value2 = value2 * 6f + value3;
            value2.Normalize();
            value2 *= Item.shootSpeed;
            float num91 = (float)Main.rand.Next(10, 50) * 0.001f;
            if (Main.rand.NextBool(2))
            {
                num91 *= -1f;
            }
            float num92 = (float)Main.rand.Next(10, 50) * 0.001f;
            if (Main.rand.NextBool(2))
            {
                num92 *= -1f;
            }
            Projectile.NewProjectile(source, vector2.X, vector2.Y, value2.X, value2.Y, type, damage, Item.knockBack, player.whoAmI, num92, num91);
            return false;
        }
    }
}
