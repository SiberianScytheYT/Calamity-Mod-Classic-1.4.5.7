using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Rogue
{
    public class AdamantiteThrowingAxe : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Adamantite Throwing Axe");
/*
            Tooltip.SetDefault("Stealth strikes summon lightning bolts on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 26;
            Item.damage = 44;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 3.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = 1600;
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<AdamantiteThrowingAxeProjectile>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.Calamity().StealthStrikeAvailable())
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
            Recipe recipe = CreateRecipe(25);
            recipe.AddIngredient(ItemID.AdamantiteBar);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
