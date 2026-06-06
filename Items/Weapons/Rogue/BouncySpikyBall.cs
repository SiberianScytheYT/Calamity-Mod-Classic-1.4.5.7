using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class BouncySpikyBall : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bouncy Spiky Ball");
/*
            Tooltip.SetDefault("Throws a very bouncy ball that richochets off walls and enemies\n"
                               +"Receives a small boost in damage and velocity after bouncing off an enemy\n"
                               +"Stealth strikes provide a bigger boost after richocheting");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 14;
            Item.damage = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
			Item.consumable = true;
			Item.maxStack = 999;
            Item.height = 14;
            Item.value = Item.buyPrice(0, 0, 1, 0);
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<BouncyBol>();
            Item.shootSpeed = 8f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.SpikyBall, 3);
            recipe.AddIngredient(ItemID.PinkGel);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
