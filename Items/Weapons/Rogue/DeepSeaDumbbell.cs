using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class DeepSeaDumbbell : RogueWeapon
    {
        private static int BaseDamage = 900;
        private static float MeleeFlexMult = 25f;
        private float flexBonusDamageMult = 0f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Deep Sea Dumbbell");
/*
            Tooltip.SetDefault("Throws a dumbbell that bounces and flings weights with each bounce\n" +
                "Right click to flex with it, increasing the power of your next throw with a stealth strike\n" +
                "Can boost the projectile damage up to 7.5 times its original damage");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.width = 38;
            Item.damage = BaseDamage;
            Item.crit -= 2;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.knockBack = 16f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.height = 24;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<DeepSeaDumbbell1>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.noMelee = false;
                Item.noUseGraphic = false;
                Item.autoReuse = false;
                Item.UseSound = SoundID.Item1;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.autoReuse = true;
                Item.UseSound = SoundID.Item1;
            }
            return base.CanUseItem(player);
        }

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse == 2)
				return (5f/9f);
			return 1f;
		}

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.Calamity().StealthStrikeAvailable() && player.altFunctionUse != 2)
                damage *= flexBonusDamageMult;
            // base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        // Flexes deal 25x damage if you actually hit with them directly.
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= MeleeFlexMult;
        }

        public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= MeleeFlexMult;
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Alt fire doesn't actually shoot anything. It flexes, increasing the damage of the next stealth strike
            if (player.altFunctionUse == 2)
            {
                flexBonusDamageMult += 1f;
                if (flexBonusDamageMult > 6.5f)
                    flexBonusDamageMult = 6.5f;
                return false;
            }

            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            if (player.Calamity().StealthStrikeAvailable())
            {
                Main.projectile[proj].Calamity().stealthStrike = true;
                flexBonusDamageMult = 0f;
            }
            return false;
        }
    }
}
