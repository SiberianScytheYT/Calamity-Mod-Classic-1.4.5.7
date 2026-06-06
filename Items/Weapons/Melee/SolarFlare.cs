using CalRD.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class SolarFlare : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Solar Flare");
/*
            Tooltip.SetDefault("Emits large holy explosions on hit\n" +
			"A very agile yoyo");
*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 38;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 60;
            Item.knockBack = 7.5f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<SolarFlareYoyo>();
            Item.shootSpeed = 16f;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.value = Item.buyPrice(platinum: 1, gold: 20);
        }
    }
}
