using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Crystalline : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crystalline");
/*
            Tooltip.SetDefault("Splits into several projectiles as it travels \n" +
                               "Stealth strikes make the blade split more and create crystals when destroyed");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 44;
            Item.damage = 16;
            Item.crit += 4;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 44;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<CrystallineProj>();
            Item.shootSpeed = 10f;
            Item.Calamity().rogue = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WulfrumKnife>(), 50);
            recipe.AddIngredient(ItemID.Diamond, 3);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[proj].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
