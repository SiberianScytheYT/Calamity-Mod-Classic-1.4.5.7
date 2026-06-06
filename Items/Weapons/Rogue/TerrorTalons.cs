using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class TerrorTalons : RogueWeapon
    {
        private float sign = 1f;
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terror Talons");
/*
            Tooltip.SetDefault("Fires small wavering claws\n" +
            "Stealth strikes launch a large, high speed claw which pierces");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 40;
            Item.damage = 47;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.shoot = ModContent.ProjectileType<TalonSmallProj>();
            Item.shootSpeed = 10.5f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                float stealthDamageMult = 3.25f;
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<TalonLargeProj>(), (int)(damage * stealthDamageMult), Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
            }
            else
            {
                // flip flop every standard projectile
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<TalonSmallProj>(), damage, Item.knockBack, player.whoAmI, 0f, sign);
                sign = -sign;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FeralClaws);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
