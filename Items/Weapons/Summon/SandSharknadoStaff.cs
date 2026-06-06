using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class SandSharknadoStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sand Sharknado Staff");
/*
            Tooltip.SetDefault("Summons a sandnado to fight for you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.mana = 10;
            Item.width = 48;
            Item.height = 56;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SandnadoMinion>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                position = Main.MouseWorld;
                velocity.X = 0;
                velocity.Y = 0;
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ForgottenApexWand>());
            recipe.AddIngredient(ModContent.ItemType<GrandScale>());
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 10);
            recipe.AddIngredient(ItemID.AncientCloth, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
