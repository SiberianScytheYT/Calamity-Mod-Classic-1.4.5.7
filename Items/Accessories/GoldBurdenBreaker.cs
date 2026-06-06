using CalRD.CalPlayer;
using CalRD.Projectiles.Typeless;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class GoldBurdenBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Burden Breaker");
/*
            Tooltip.SetDefault("The good time\n" +
				"Go fast\n" +
				"WARNING: May have disastrous results\n" +
				"Increases horizontal movement speed beyond comprehension");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = 10;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (CalamityPlayer.areThereAnyDamnBosses)
            { return; }
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.dashMod = modPlayer.dashMod == 7 ? 0 : modPlayer.dashMod; //statis belt memes for projectile spam :feelsgreat:
            modPlayer.burdenBreakerYeet = true;
            // Completely remove movement restrictions if you're yeeting with the profaned spear
            if (player.ownedProjectileCounts[ModContent.ProjectileType<RelicOfDeliveranceSpear>()] <= 0)
            {
                if (player.velocity.X > 5f)
                {
                    player.velocity.X *= 1.025f;
                    if (player.velocity.X >= 500f)
                    {
                        player.velocity.X = 0f;
                    }
                }
                else if (player.velocity.X < -5f)
                {
                    player.velocity.X *= 1.025f;
                    if (player.velocity.X <= -500f)
                    {
                        player.velocity.X = 0f;
                    }
                }
            }
        }
    }
}
