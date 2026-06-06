using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class EvilSmasher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Evil Smasher");
/*
            Tooltip.SetDefault("EViL! sMaSH eVIl! SmAsh...ER!");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 62;
			Item.height = 62;
			Item.scale = 2f;
			Item.damage = 55;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

		public override float UseTimeMultiplier	(Player player)
		{
			if (player.Calamity().brimlashBusterBoost)
				return 2f;
			return 1f;
		}

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			float damageMult = 0f;
            if (player.Calamity().brimlashBusterBoost)
				damageMult = 0.5f;
			damage.Base += damageMult;
		}

        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
        {
            if (player.Calamity().brimlashBusterBoost)
            {
                knockback *= 1.75f;
            }
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			OnHitEffect(target.Center, player, Item.knockBack);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
			OnHitEffect(target.Center, player, Item.knockBack);
        }

		private void OnHitEffect(Vector2 targetPos, Player player, float knockback)
		{
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), targetPos, Vector2.Zero, ModContent.ProjectileType<FossilSpike>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, Main.myPlayer);
			player.Calamity().brimlashBusterBoost = Main.rand.NextBool(3);
		}
    }
}
