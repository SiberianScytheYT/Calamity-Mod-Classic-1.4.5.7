using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
	public class CorpusAvertorMelee : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Corpus Avertor");
/*
			Tooltip.SetDefault("Seems like it has worn down over time\n" +
				"Attacks grant lifesteal based on damage dealt\n" +
				"The lower your HP the more damage this weapon does and heals the player on enemy hits");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 32;
			Item.damage = 98;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.knockBack = 3f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(gold: 80);
			Item.rare = 8;
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
			Item.shoot = ModContent.ProjectileType<CorpusAvertorProj>();
			Item.shootSpeed = 5f;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
		}

		// Gains 10% of missing health as base damage.
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			int lifeAmount = player.statLifeMax2 - player.statLife;
			damage.Base += lifeAmount * 0.1f; // * player.MeleeDamage();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
