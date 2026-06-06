using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class HardenedHoneycomb : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hardened Honeycomb");
/*
            Tooltip.SetDefault("Fires a honeycomb that shatters into fragments\n"
                               +"Grants the honey buff to players it touches\n"
                               +"Stealth strikes can bounce off walls and enemies");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 30;
            Item.damage = 25;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 21;
            Item.useTime = 21;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = 300;
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<Honeycomb>();
            Item.shootSpeed = 10f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                Main.projectile[stealth].penetrate = 3;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.Hive);
            recipe.AddIngredient(ItemID.CrispyHoneyBlock);
            recipe.AddIngredient(ItemID.BeeWax);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
