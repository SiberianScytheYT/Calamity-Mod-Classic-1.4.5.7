using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class SHPC : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("SHPC");
/*
            Tooltip.SetDefault("Legendary Drop\n" +
                "Fires plasma orbs that linger and emit massive explosions\n" +
                "Right click to fire powerful energy beams\n" +
                "Revengeance drop");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 96;
            Item.height = 34;
            Item.useTime = Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.rare = 5;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SHPB>();
            Item.shootSpeed = 20f;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LaserCannon");
            }
            else
            {
                Item.UseSound = SoundID.Item92;
            }
            return base.CanUseItem(player);
        }

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.altFunctionUse == 2)
				mult *= 0.3f;
		}

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse == 2)
				return 1f;
			return 1 / 7.14f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                for (int shootAmt = 0; shootAmt < 3; shootAmt++)
                {
                    float SpeedX = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
                    float SpeedY = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<SHPL>(), damage, Item.knockBack * 0.5f, player.whoAmI, 0f, 0f);
                }
                return false;
            }
            else
            {
                for (int shootAmt = 0; shootAmt < 3; shootAmt++)
                {
                    float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
                    float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<SHPB>(), (int)(damage * 1.1), Item.knockBack, player.whoAmI, 0f, 0f);
                }
                return false;
            }
        }
    }
}
