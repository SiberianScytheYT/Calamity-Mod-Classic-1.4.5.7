using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class WebBall : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Web Ball");
/*
            Tooltip.SetDefault("Throws a web-covered ball that covers enemies in cobwebs to slow them down\n"
                               +"Stealth strikes slow enemies down longer");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 20;
            Item.damage = 8;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			Item.maxStack = 999;
			Item.consumable = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 0, 30);
            Item.rare = 0;
            Item.shoot = ModContent.ProjectileType<WebBallBol>();
            Item.shootSpeed = 6.5f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
				int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, Item.damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[proj].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Cobweb, 3);
            recipe.Register();
        }
    }
}
