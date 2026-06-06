using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class FrostBlossomStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Blossom Staff");
/*
            Tooltip.SetDefault("Summons a frozen flower over your head");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.mana = 10;
            Item.width = 34;
            Item.height = 24;
            Item.useTime = Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item28;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FrostBlossom>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CalamityUtils.KillShootProjectiles(true, type, player);
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Shiverthorn, 5);
            recipe.AddRecipeGroup("AnySnowBlock", 50);
            recipe.AddRecipeGroup("AnyIceBlock", 50);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
