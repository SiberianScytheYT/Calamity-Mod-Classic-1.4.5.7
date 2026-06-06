using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class DreadmineStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dreadmine Staff");
/*
            Tooltip.SetDefault("Summons a dreadmine turret to fight for you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.mana = 10;
            Item.width = 44;
            Item.height = 44;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item113;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DreadmineTurret>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//CalamityUtils.OnlyOneSentry(player, type);
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
            player.UpdateMaxTurrets();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 30);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
