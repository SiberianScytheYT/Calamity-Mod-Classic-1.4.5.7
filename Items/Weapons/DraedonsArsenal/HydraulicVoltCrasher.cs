using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
    public class HydraulicVoltCrasher : ModItem
    {
        // This is the amount of charge consumed every frame the holdout projectile is summoned, i.e. the weapon is in use.
        public const float HoldoutChargeUse = 0.002f;
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hydraulic Volt Crasher");
/*
            Tooltip.SetDefault("Good for both stamping metal plates and instantly fusing them, as well as crushing enemies.\n" +
            "An electrically charged jackhammer which shocks all nearby foes on hit");
*/
        }

        public override void SetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 16;
            Item.useTime = 4;
            Item.shootSpeed = 46f;
            Item.knockBack = 12f;
            Item.width = 56;
            Item.height = 24;
            Item.damage = 65;
            Item.hammer = 230;
            Item.UseSound = SoundID.Item23;

            Item.shoot = ModContent.ProjectileType<HydraulicVoltCrasherProjectile>();
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Red;
            modItem.customRarity = CalamityRarity.DraedonRust;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.channel = true;

            modItem.UsesCharge = true;
            modItem.MaxCharge = 85f;
            modItem.ChargePerUse = 0f; // This weapon is a holdout. Charge is consumed by the holdout projectile.
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 && Item.Calamity().Charge > 0;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 8);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 12);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.SoulofSight, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
