using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class ElementalEruption : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elemental Eruption");
/*
            Tooltip.SetDefault("90% chance to not consume gel\n" +
                "Fires a spread of rainbow flames");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 56;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 34;
            Item.useTime = 7;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item34;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TerraFireGreen2>();
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Gel;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = Main.rand.Next(3, 5);
            for (int index = 0; index < num6; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 90)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ModContent.ItemType<TerraFlameburster>());
            recipe.AddIngredient(ModContent.ItemType<Meowthrower>());
            recipe.AddIngredient(ModContent.ItemType<MepheticSprayer>());
            recipe.AddIngredient(ModContent.ItemType<BrimstoneFlamesprayer>());
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
