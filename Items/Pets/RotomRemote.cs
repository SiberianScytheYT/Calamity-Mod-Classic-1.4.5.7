using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using CalRD.Items.Placeables;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
	public class RotomRemote : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Triboluminescent Etomer");
/*
			Tooltip.SetDefault("Summons an electric troublemaker\n" +
			"A little note is attached:\n" +
			"Thank you, Aloe! Very much appreciated from Ben");
*/
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.useTime = Item.useAnimation = 20;
			Item.shoot = ModContent.ProjectileType<RotomPet>();
			Item.buffType = ModContent.BuffType<RotomBuff>();

			Item.width = 30;
			Item.height = 34;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item113;
			Item.value = Item.buyPrice(0, 4, 0, 0);
			Item.rare = 3;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Pets/RotomRemoteGlow").Value);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<PrismShard>(), 5);
			recipe.AddRecipeGroup("AnyGoldBar", 8);
			recipe.AddIngredient(ModContent.ItemType<DemonicBoneAsh>());
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
