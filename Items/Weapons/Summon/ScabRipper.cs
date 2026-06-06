using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class ScabRipper : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scab Ripper");
/*
            Tooltip.SetDefault("Summons a baby blood crawler to protect you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.mana = 10;
            Item.width = 66;
            Item.height = 70;
            Item.useTime = Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0.5f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item83;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BabyBloodCrawler>();
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
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI);
            }
            return false;
        }
		
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimtaneBar, 5);
            recipe.AddIngredient(ItemID.TissueSample, 9);
            recipe.AddIngredient(ItemID.Shadewood, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
