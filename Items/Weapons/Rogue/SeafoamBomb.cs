using CalRD.Items.Placeables;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class SeafoamBomb : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seafoam Bomb");
/*
            Tooltip.SetDefault("Throws a bomb that explodes into a bubble which deals extra damage to enemies\n"
                               +"Stealth strikes are faster and explode into 5 bubbles");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 26;
            Item.height = 44;
            Item.damage = 12;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<SeafoamBombProj>();
            Item.shootSpeed = 8f;
            Item.Calamity().rogue = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Bomb, 25);
            recipe.AddIngredient(ModContent.ItemType<SeaPrism>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X + velocity.X / 3, velocity.Y + velocity.Y / 3), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
