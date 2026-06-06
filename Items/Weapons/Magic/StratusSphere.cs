using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Projectiles.Magic;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Ores;

namespace CalRD.Items.Weapons.Magic
{
	public class StratusSphere : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Stratus Sphere");
/*
			Tooltip.SetDefault("Fires an energy orb containing the essence of our stratosphere\n" +
			"Up to six of these can be active at a time");
*/
		}
		public override void SetDefaults()
		{
			Item.damage = 419;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.width = 22;
			Item.height = 24;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.shoot = ModContent.ProjectileType<StratusSphereProj>();
			Item.shootSpeed = 7f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.mana = 30;
			Item.knockBack = 2;
			Item.UseSound = SoundID.Item20;
			Item.rare = 4;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.holdStyle = 3;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
		}

        public override bool CanUseItem(Player player)
        {
			if (player.ownedProjectileCounts[Item.shoot] >= 6)
			{
				return false;
			}
			else
			{
				return true;
			}
        }

        public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 5);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 4);
            recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 12);
            recipe.AddIngredient(ItemID.NebulaArcanum);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
		}
	}
}

