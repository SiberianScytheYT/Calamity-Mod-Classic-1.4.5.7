using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class BorealisBomber : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Borealis Bomber");
/*
            Tooltip.SetDefault("Summons aureus bombers to fight for you\n" +
			"Aureus bombers explode on enemy impact\n" +
			"Does not consume minion slots");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.mana = 15;
            Item.width = 48;
            Item.height = 56;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item44;
            //item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AureusBomber>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                position = Main.MouseWorld;
                velocity.X = 0;
                velocity.Y = 0;
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, Main.myPlayer);
            }
            return false;
        }
    }
}
