using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Rogue
{
    public class CobaltKunai : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cobalt Kunai");
/*
            Tooltip.SetDefault("Stealth strikes fire three homing cobalt energy bolts");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 18;
            Item.damage = 36;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 2.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 40;
            Item.maxStack = 999;
            Item.value = 900;
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<CobaltKunaiProjectile>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
			{
				for (int i = -6; i <= 6; i += 6)
				{
					Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(i));
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CobaltEnergy>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
				}
				return false;
			}
			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(30);
            recipe.AddIngredient(ItemID.CobaltBar);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
