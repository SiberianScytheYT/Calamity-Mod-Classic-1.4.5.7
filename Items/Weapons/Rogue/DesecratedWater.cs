using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class DesecratedWater : RogueWeapon
    {
        public const int BaseDamage = 55;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Desecrated Water");
/*
            Tooltip.SetDefault("Throws an unholy flask of water that explodes into an explosion of bubbles on death\n"
                               +"Stealth strikes spawn additional bubbles that inflict Ichor and Cursed Inferno");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = BaseDamage;
            Item.width = 22;
            Item.height = 24;
            Item.useAnimation = 29;
            Item.useTime = 29;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.rare = 6;
            Item.UseSound = SoundID.Item106;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 48); //sell price of 9 gold 60 silver
            Item.shoot = ModContent.ProjectileType<DesecratedWaterProj>();
            Item.shootSpeed = 12f;
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
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnyEvilWater", 100);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddRecipeGroup("CursedFlameIchor", 5);
            recipe.AddIngredient(ItemID.SoulofNight, 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
