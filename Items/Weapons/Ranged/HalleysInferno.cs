using CalRD.Items.Materials;
using CalRD.Items.Placeables.Ores;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
	public class HalleysInferno : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Halley's Inferno");
/*
			Tooltip.SetDefault("Halley came sooner than expected\n" +
			"Fires a flaming comet\n" +
			"50% chance to not consume gel\n" +
			"Right click to zoom out");
*/
		}

		public override void SetDefaults()
		{
			Item.damage = 1666;
			Item.crit += 20;
			Item.knockBack = 5f;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = Item.useAnimation = 30;
			Item.autoReuse = true;
			Item.useAmmo = AmmoID.Gel;
			Item.shootSpeed = 14.6f;
			Item.shoot = ModContent.ProjectileType<HalleysComet>();

			Item.width = 84;
			Item.height = 34;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item34;
			Item.value = Item.buyPrice(1, 40, 0, 0);
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.PureGreen;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-15, 0);

		public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.Next(100) >= 50;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 6);
			recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 4);
			recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 12);
			recipe.AddIngredient(ItemID.SniperScope);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
