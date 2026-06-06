using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Equanimity : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Equanimity");
/*
            Tooltip.SetDefault("Throws a dark/light boomerang that confuses enemies\n" +
                "The boomerang will create light shards upon hitting enemies when thrown out, and will fire homing dark shards when returning\n" +
                "Stealth strikes cause the boomerang to create both dark and light shards whenever one type would be created");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 40;
            Item.damage = 40;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 36;
            Item.maxStack = 1;
            Item.rare = 5;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.shoot = ModContent.ProjectileType<EquanimityProj>();
            Item.shootSpeed = 30f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
                Main.projectile[p].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Flamarang);
            recipe.AddIngredient(ItemID.IceBoomerang);
            recipe.AddIngredient(ItemID.LightShard);
            recipe.AddIngredient(ItemID.DarkShard);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
