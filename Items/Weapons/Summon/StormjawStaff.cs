using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class StormjawStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stormjaw Staff");
/*
            Tooltip.SetDefault("Summons a baby stormlion to fight for you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = Item.useAnimation = 35;
            Item.scale = 0.75f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.NPCDeath34;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StormjawBaby>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
            }
            return false;
        }
    }
}
