using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class SealedSingularity : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Sealed Singularity");
/*
			Tooltip.SetDefault("Shatters on impact, summoning a black hole that sucks in nearby enemies\n" +
			"Stealth strikes summon a black hole that lasts longer and sucks enemies with stronger force");
*/
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 169;
			Item.knockBack = 5f;
			Item.useAnimation = Item.useTime = 25;
			Item.useStyle = 1;
			Item.Calamity().rogue = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SealedSingularityProj>();
			Item.shootSpeed = 12f;

			Item.noMelee = Item.noUseGraphic = true;
			Item.height = Item.width = 34;
			Item.UseSound = SoundID.Item106;
			Item.value = CalamityGlobalItem.Rarity13BuyPrice;
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
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
			recipe.AddIngredient(ModContent.ItemType<DuststormInABottle>());
			recipe.AddIngredient(ModContent.ItemType<DarkPlasma>(), 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
    }
}
