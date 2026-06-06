using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class GildedProboscis : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gilded Proboscis");
/*
            Tooltip.SetDefault("Ignores immunity frames\n" +
                "Heals the player on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.damage = 160;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 19;
            Item.knockBack = 8.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<GildedProboscisProj>();
            Item.shootSpeed = 13f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
	}
}
