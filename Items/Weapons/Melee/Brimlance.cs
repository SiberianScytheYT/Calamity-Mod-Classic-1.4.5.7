using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Brimlance : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimlance");
/*
            Tooltip.SetDefault("Enemies killed by the spear explode into brimstone flames");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.damage = 100;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 19;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 56;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<BrimlanceProj>();
            Item.shootSpeed = 12f;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
	}
}
