using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class MeteorFist : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Meteor Fist");
/*
            Tooltip.SetDefault("Fires a fist that explodes \n" +
                               "Stealth strikes make the fist ricochet between enemies up to 4 times");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 22;
            Item.damage = 15;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.knockBack = 5.75f;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<MeteorFistProj>();
            Item.shootSpeed = 10f;
            Item.Calamity().rogue = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<MeteorFistProj>(), damage, Item.knockBack, player.whoAmI, 0f, 4f);
                Main.projectile[proj].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
