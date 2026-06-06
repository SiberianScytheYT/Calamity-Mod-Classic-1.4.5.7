using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;

namespace CalRD.Items.Weapons.Rogue
{
	public class Sandslasher : RogueWeapon
    {
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Sandslasher");
/*
			Tooltip.SetDefault("Throws a huge shuriken made out of fused sand unaffected by gravity which slowly accelerates horizontally\n"
							  +"It does more damage depending on how fast it goes horizontally and how long it has been flying for\n"
                              +"Stealth strikes periodically release sand clouds");
*/
        }

		public override void SafeSetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.damage = 115;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 20;
			Item.knockBack = 5f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<SandslasherProj>();
			Item.shootSpeed = 7f;
            Item.Calamity().rogue = true;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.Calamity().StealthStrikeAvailable())
            {
                int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI);
                Main.projectile[proj].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
	    {
	        Recipe recipe = CreateRecipe();
	        recipe.AddIngredient(ModContent.ItemType<GrandScale>());
			recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 6);
            recipe.AddRecipeGroup("AnyGoldBar", 10);
            recipe.AddIngredient(ItemID.HardenedSand, 25);
            recipe.AddTile(TileID.MythrilAnvil);
	        recipe.Register();
	    }
	}
}
