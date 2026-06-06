using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class GildedDagger : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gilded Dagger");
/*
            Tooltip.SetDefault("Throws a shiny blade that ricochets towards another enemy on hit\n" +
                "Stealth strikes cause the blade to home in after ricocheting, with each ricochet dealing 20% more damage\n" +
				"Stealth strikes also have increased piercing");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 40;
            Item.damage = 14;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 36;
            Item.maxStack = 1;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<GildedDaggerProj>();
            Item.shootSpeed = 15f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
                Main.projectile[p].Calamity().stealthStrike = true;
                Main.projectile[p].penetrate = 4;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.GoldBar, 12);
            recipe.AddIngredient(ItemID.ThrowingKnife, 250);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
