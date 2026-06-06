using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class YharimsCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Yharim's Crystal");
/*
            Tooltip.SetDefault("Fires draconic beams of total annihilation");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 350;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.reuseDelay = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item13;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.knockBack = 0f;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<YharimsCrystalPrism>();
            Item.shootSpeed = 30f;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
    }
}
