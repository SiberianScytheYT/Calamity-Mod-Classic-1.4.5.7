using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class EarthenPike : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Earthen Pike");
/*
            Tooltip.SetDefault("Crushes enemy defenses\n" +
                "Sprays fossil shards on use");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 60;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<EarthenPikeSpear>();
            Item.shootSpeed = 8f;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
	}
}
