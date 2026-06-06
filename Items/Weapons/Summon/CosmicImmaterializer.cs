using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class CosmicImmaterializer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cosmic Immaterializer");
/*
            Tooltip.SetDefault("Summons a cosmic energy spiral to fight for you\n" +
                               "The orb will fire swarms of homing energy bolts when enemies are detected by it\n" +
                               "Requires 10 minion slots to use\n" +
                               "There can only be one\n" +
                               "Without a summoner armor set bonus this minion will deal less damage");
*/
        }

        public override void SetDefaults()
        {
            Item.mana = 100;
            Item.damage = 3000;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 74;
            Item.height = 72;
            Item.useTime = Item.useAnimation = 10;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item60;
            Item.shoot = ModContent.ProjectileType<CosmicEnergySpiral>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 && player.maxMinions >= 10;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CalamityUtils.KillShootProjectiles(true, type, player);
            CalamityPlayer modPlayer = player.Calamity();
            bool hasSummonerSet = modPlayer.tarraSummon || modPlayer.bloodflareSummon || modPlayer.godSlayerSummon || modPlayer.silvaSummon || modPlayer.dsSetBonus || modPlayer.omegaBlueSet || modPlayer.fearmongerSet; //demonshade included so summoner isn't forced to use auric for BR
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, (int)(damage * (hasSummonerSet ? 1 : 0.66)), Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CorvidHarbringerStaff>());
            recipe.AddIngredient(ModContent.ItemType<AncientIceChunk>());
            recipe.AddIngredient(ModContent.ItemType<ElementalAxe>());
            recipe.AddIngredient(ModContent.ItemType<EnergyStaff>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
