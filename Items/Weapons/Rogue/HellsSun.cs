using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using CalRD.CalPlayer;

namespace CalRD.Items.Weapons.Rogue
{
	public class HellsSun : RogueWeapon
    {
        private static int damage = 84;
        private static int knockBack = 5;
        private static float SdamageMult = 0.6f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hell's Sun");
/*
            Tooltip.SetDefault("The Subterranean Sun in the palm of your hand.\n" +
				"Shoots a gravity-defying spiky ball. Stacks up to 10.\n" +
                "Once stationary, periodically emits small suns that explode on hit\n" +
                "Stealth strikes emit suns at a faster rate and last for a longer amount of time\n" +
				"Right click to delete all existing spiky balls");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = damage;
            Item.Calamity().rogue = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 1;
            Item.height = 1;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = knockBack;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 10;

            Item.shootSpeed = 5f;
            Item.shoot = ModContent.ProjectileType<HellsSunProj>();
        }

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.shoot = 0;
				Item.shootSpeed = 0f;
				return player.ownedProjectileCounts[ModContent.ProjectileType<HellsSunProj>()] > 0;
			}
			else
			{
				Item.shoot = ModContent.ProjectileType<HellsSunProj>();
				Item.shootSpeed = 5f;
				int UseMax = Item.stack;
				return player.ownedProjectileCounts[ModContent.ProjectileType<HellsSunProj>()] < UseMax;
			}
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer modPlayer = player.Calamity();
			modPlayer.killSpikyBalls = false;
            if (modPlayer.StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<HellsSunProj>(), (int)(damage * SdamageMult), Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                Main.projectile[stealth].penetrate = -1;
                Main.projectile[stealth].timeLeft = 2400;
                return false;
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
			modPlayer.killSpikyBalls = true;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ItemID.SpikyBall, 100);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>(), 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
