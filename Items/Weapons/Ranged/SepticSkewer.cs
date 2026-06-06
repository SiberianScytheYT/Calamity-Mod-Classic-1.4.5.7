using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
	public class SepticSkewer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Septic Skewer");
/*
            Tooltip.SetDefault("Launches a spiky harpoon infested with toxins\n" +
				"Releases bacteria when returning to the player");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 501;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 24;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.Calamity().postMoonLordRarity = 13;
			Item.rare = 10;
            Item.UseSound = SoundID.Item10;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<SepticSkewerHarpoon>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}
