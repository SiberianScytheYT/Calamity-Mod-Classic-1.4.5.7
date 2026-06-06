using CalRD.Projectiles.Rogue;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace CalRD.Items.Weapons.Rogue
{
	public class FinalDawn : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("The Final Dawn");
/*
			Tooltip.SetDefault("We shall ride into the sunrise once more\n" +
							   "Attack enemies with a giant scythe swing to replenish stealth\n" +
							   "Press up and attack to throw the scythe \n" +
							   "Stealth strikes perform a horizontal swing that leaves a lingering fire aura\n" +
							   "Stealth strikes performed while pressing up fling yourself at the enemy and slice through them, causing homing fireballs to emerge");
*/
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 5000;
			Item.Calamity().rogue = true;
			Item.width = 78;
			Item.height = 66;
			Item.noMelee = true;
			Item.useTime = Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4;
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
			Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
			Item.autoReuse = false;
			Item.shoot = ProjectileType<FinalDawnProjectile>();
			Item.shootSpeed = 1f;
			Item.useTurn = false;
            Item.channel = true;
			Item.noUseGraphic = true;
		}
		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] +
			player.ownedProjectileCounts[ProjectileType<FinalDawnFireSlash>()] +
			player.ownedProjectileCounts[ProjectileType<FinalDawnHorizontalSlash>()] +
			player.ownedProjectileCounts[ProjectileType<FinalDawnThrow>()] +
			player.ownedProjectileCounts[ProjectileType<FinalDawnThrow2>()] <= 0;
    }
}