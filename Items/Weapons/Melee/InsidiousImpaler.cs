using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
	public class InsidiousImpaler : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Insidious Impaler");
/*
            Tooltip.SetDefault("Fires a harpoon that sticks to enemies and explodes");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.damage = 350;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 70;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.Calamity().postMoonLordRarity = 13;
			Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<InsidiousImpalerProj>();
            Item.shootSpeed = 5f;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
	}
}
