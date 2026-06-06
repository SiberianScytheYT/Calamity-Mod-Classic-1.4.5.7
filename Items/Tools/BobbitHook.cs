using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalRD.Projectiles.Typeless;

namespace CalRD.Items.Tools
{
	public class BobbitHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bobbit Hook");
/*
            Tooltip.SetDefault("Retracts upon attaching to a tile with extreme speeds\n"
                               +"Reach: 40\n"
                               +"Launch Velocity: 25\n"
                               +"Pull Velocity: 28");
*/
		}

		public override void SetDefaults()
		{
			// Instead of copying these values, we can clone and modify the ones we want to copy
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 25f; // how quickly the hook is shot.
			Item.shoot = ProjectileType<BobbitHead>();
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = (CalamityRarity)13;
			Item.width = 30;
			Item.height = 32;
		}
	}
}
