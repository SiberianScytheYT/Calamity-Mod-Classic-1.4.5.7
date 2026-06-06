using CalRD.Projectiles.Melee;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
	public class Murasama : ModItem
	{
		public int frameCounter = 0;
		public int frame = 0;
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Murasama");
/*
			Tooltip.SetDefault("There will be blood!\n" +
				"ID and power-level locked\n" +
				"Prove your strength or have the correct user ID to wield this sword");
*/
		}

		public override void SetDefaults()
		{
			Item.height = 128;
			Item.width = 56;
			Item.damage = 20001;
			Item.crit += 30;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 5;
			Item.knockBack = 6.5f;
			Item.autoReuse = false;
			Item.value = Item.buyPrice(2, 50, 0, 0);
			Item.rare = 10;
			Item.shoot = ModContent.ProjectileType<MurasamaSlash>();
			Item.shootSpeed = 24f;
			Item.Calamity().customRarity = CalamityRarity.Violet;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 14));
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			//0 = 6 frames, 8 = 3 frames]
			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			spriteBatch.Draw(texture, position, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13), Color.White, 0f, origin, scale, SpriteEffects.None, 0);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Melee/MurasamaGlow").Value;
			spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13, false), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[Item.shoot] > 0)
				return false;
			return CalamityWorld.downedYharon || player.name == "Jetstream Sam" || player.name == "Samuel Rodrigues";
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
