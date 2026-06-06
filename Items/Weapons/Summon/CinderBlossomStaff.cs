using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class CinderBlossomStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cinder Blossom Staff");
/*
            Tooltip.SetDefault("Summons a really hot flower over your head");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.mana = 10;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CinderBlossom>();
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
            recipe.AddIngredient(ItemID.Fireblossom, 5);
            recipe.AddIngredient(ItemID.LavaBucket, 2);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
