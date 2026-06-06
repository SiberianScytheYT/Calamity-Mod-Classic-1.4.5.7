using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class LeonidProgenitor : RogueWeapon
	{
        public static readonly Color blueColor = new Color(48, 208, 255);
        public static readonly Color purpleColor = new Color(208, 125, 218);
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Leonid Progenitor");
/*
			Tooltip.SetDefault("Legendary Drop\n" +
				"Throws a bombshell that explodes, summoning a meteor to impact the site\n" +
				"Right click to throw a spread of gravity affected comets that explode, leaving behind a star\n" +
				"Stealth strikes lob a bombshell that additionally splits into comets on hit\n" +
				"Revengeance drop");
*/
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 80;
			Item.Calamity().rogue = true;
			Item.knockBack = 3f;
			Item.useTime = Item.useAnimation = 15;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<LeonidProgenitorBombshell>();
			Item.shootSpeed = 12f;

			Item.width = 32;
			Item.height = 48;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item61;
			Item.value = CalamityGlobalItem.Rarity7BuyPrice;
			Item.rare = 7;
			Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.Calamity().StealthStrikeAvailable() || player.altFunctionUse != 2)
			{
				Item.UseSound = SoundID.Item61;
				Item.shoot = ModContent.ProjectileType<LeonidProgenitorBombshell>();
			}
			else
			{
				Item.UseSound = SoundID.Item88;
				Item.shoot = ModContent.ProjectileType<LeonidCometSmall>();
			}
			return base.CanUseItem(player);
		}

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.Calamity().StealthStrikeAvailable() || player.altFunctionUse != 2)
				return 1f;
			//return 0.75f;
			return 0.8f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.Calamity().StealthStrikeAvailable() || player.altFunctionUse != 2)
			{
				int bomb = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[bomb].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
				return false;
			}
			else
			{
				float dmgMult = 0.5f;
				for (float i = -2.5f; i < 3f; ++i)
				{
					Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(i));
					Projectile.NewProjectile(source, position, perturbedSpeed, type, (int)(damage * dmgMult), Item.knockBack, player.whoAmI, 0f, 0f);
				}
			}
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Rogue/LeonidProgenitorGlow").Value);
		}
	}
}
