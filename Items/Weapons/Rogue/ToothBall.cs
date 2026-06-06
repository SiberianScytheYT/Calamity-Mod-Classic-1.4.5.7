using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class ToothBall : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tooth Ball");
/*
            Tooltip.SetDefault("Stealth strikes spawn rain clouds on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 30;
            Item.damage = 26;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 13;
            Item.crit = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 2.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = 1000;
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<ToothBallProjectile>();
            Item.shootSpeed = 16f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                Main.projectile[stealth].usesLocalNPCImmunity = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(100);
            recipe.AddIngredient(ModContent.ItemType<BloodSample>());
            recipe.AddIngredient(ItemID.Vertebrae);
            recipe.AddIngredient(ItemID.CrimtaneBar);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
