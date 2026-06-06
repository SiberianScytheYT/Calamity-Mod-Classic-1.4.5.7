using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalRD.Projectiles.Typeless;

namespace CalRD.Items.Fishing.SunkenSeaCatches
{
	public class SerpentsBite : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Serpent's Bite");
/*
            Tooltip.SetDefault("Reach: 28.125\n"
                               +"Launch Velocity: 18\n"
                               +"Pull Velocity: 14");
*/
		}

		public override void SetDefaults()
		{
			// Instead of copying these values, we can clone and modify the ones we want to copy
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 18f; // how quickly the hook is shot.
			Item.shoot = ProjectileType<SerpentsBiteHook>();
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
			Item.width = 30;
			Item.height = 32;
		}
	}
}
